using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace TestovoeGagarinov
{
    public class App : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication application)
        {
            string assemblyPath = Assembly.GetExecutingAssembly().Location;
            string iconPath = Path.Combine(Path.GetDirectoryName(assemblyPath), "Resources","Images", "Lev.png");

            string tabName = "Gagarinov Testovoe";
            application.CreateRibbonTab(tabName);

            RibbonPanel panel = application.CreateRibbonPanel(tabName, "Тестовое задание");

            PushButtonData btnData = new PushButtonData("testovoeBtn", "Построить стены по помещениям", assemblyPath, "TestovoeGagarinov.CreateWallsCommand")
            {
                LargeImage = new BitmapImage(new Uri(iconPath))
            };

            panel.AddItem(btnData);

            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }
    }
}
