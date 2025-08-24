using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.UI.Selection;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TestovoeGagarinov.Filters;
using TestovoeGagarinov.Utils;

namespace TestovoeGagarinov.Services
{
    public abstract class SelectionServiceBase<T> where T : Element
    {
        public abstract IList<T> PickElements();
    }
}