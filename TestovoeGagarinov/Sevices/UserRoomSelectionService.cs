using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestovoeGagarinov.Filters;
using TestovoeGagarinov.Utils;

namespace TestovoeGagarinov.Sevices
{
    public class UserRoomSelectionService : SelectionServiceBase<Room>
    {
        public override IList<Room> PickElements()
        {
            IList<Room> rooms = new List<Room>();
            ISelectionFilter selectionFilter = new RoomSelectionFilter();

            IList<Reference> selected = RevitAPI.UiDocument.Selection
                .PickObjects(ObjectType.Element, selectionFilter, "Выберите помещения");

            foreach (var room in selected.Select(r => RevitAPI.Document.GetElement(r)).OfType<Room>())
                rooms.Add(room);

            return rooms;
        }
    }
    }
}
