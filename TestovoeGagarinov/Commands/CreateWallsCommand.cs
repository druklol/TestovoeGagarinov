using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using TestovoeGagarinov.Utils;
using TestovoeGagarinov.View;
using TestovoeGagarinov.ViewModel;

namespace TestovoeGagarinov
{
    [Transaction(TransactionMode.Manual)]
    public class CreateWallsCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            if (RevitAPI.UiApplication == null)
                RevitAPI.Initialize(commandData);
            WallsViewModel viewModel = new WallsViewModel();

            CreateWallsView ui = new CreateWallsView()
            {
                DataContext = viewModel
            };

            ui.Show();

            return Result.Succeeded;
        }
    }
}
