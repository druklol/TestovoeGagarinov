using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.UI;
using System.Collections.Generic;
using System.Linq;
using System.Xaml;
using TestovoeGagarinov.Utils;

namespace TestovoeGagarinov.Handlers
{
    public class BuildWallsHandler : IExternalEventHandler
    {
        public IList<Room> Rooms { get; set; }

        public void Execute(UIApplication app)
        {
            Document document = RevitAPI.Document;
            WallType baseWallType = GetBaseWallType(document);

            using (Transaction transaction = new Transaction(document, "Создание стены"))
            {
                transaction.Start();
                foreach (Room room in Rooms)
                {
                    CreateWallsInRoom(room, document, baseWallType);
                }
                transaction.Commit();
            }
        }
        public string GetName() => "Build Walls Handler";

        private WallType GetBaseWallType(Document document)
        {
            return new FilteredElementCollector(document)
                .OfClass(typeof(WallType))
                .FirstOrDefault() as WallType;
        }

        private void CreateWallsInRoom(Room room, Document document, WallType wallType)
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

                    Wall.Create(document, curveWithOffset, wallType.Id, room.LevelId, room.UnboundedHeight, 0, false, false);
                }
            }
        }

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
