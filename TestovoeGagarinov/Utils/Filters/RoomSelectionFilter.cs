using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestovoeGagarinov.Filters
{
    public class RoomSelectionFilter : ISelectionFilter
    {
        public bool AllowElement(Element elem) => elem is Room;

        public bool AllowReference(Reference reference, XYZ position) => false;
    }
}
