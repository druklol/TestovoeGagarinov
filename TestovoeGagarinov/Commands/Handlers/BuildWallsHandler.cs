using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using TestovoeGagarinov.Utils;

namespace TestovoeGagarinov.Handlers
{
    public class BuildWallsHandler : IExternalEventHandler
    {
        public IList<Room> Rooms { get; set; }

        public void Execute(UIApplication app)
        {
            Document document = RevitAPI.Document;
            WallType baseWallType = new FilteredElementCollector(RevitAPI.Document)
                .OfClass(typeof(WallType))
                .FirstOrDefault() as WallType;



            using (Transaction transaction = new Transaction(document, "Создание стены"))
            {
                transaction.Start();
                foreach (Room room in Rooms)
                {
                    SpatialElementBoundaryOptions options = new SpatialElementBoundaryOptions();
                    var boundaries = room.GetBoundarySegments(options);

                    foreach (var boundary in boundaries)
                    {
                        foreach (var segment in boundary)
                        {
                            Curve curve = segment.GetCurve();

                            Wall wall = document.GetElement(segment.ElementId) as Wall;
                            if (wall == null) continue;

                            double offset = wall.Width / 2;
                            Curve curveWithOffset = GetCurveInRoom(curve, offset, room);
                            if (curveWithOffset is null) continue;

                            Wall.Create(document, curveWithOffset, baseWallType.Id, room.LevelId, room.UnboundedHeight, 0, false, false);
                        }
                    }
                }
                transaction.Commit();

            }
        }

        public string GetName() => "Build Walls Handler";

        private Curve GetCurveInRoom(Curve curve, double offset, Room room)
        {
            Curve curvePlusOffset = curve.CreateOffset(offset, XYZ.BasisZ);
            Curve curveMinusOffset = curve.CreateOffset(-offset, XYZ.BasisZ);

            XYZ midCoordPlus = curvePlusOffset.Evaluate(0.5, true);
            XYZ midCoordMinus = curveMinusOffset.Evaluate(0.5, true);

            if (room.IsPointInRoom(midCoordPlus)) return curvePlusOffset;
            if (room.IsPointInRoom(midCoordMinus)) return curveMinusOffset;
            return null;
        }
    }
}
