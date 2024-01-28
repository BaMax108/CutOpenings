using System;
using Autodesk.Revit.UI;
using System.Reflection;
using System.IO;
using System.Windows.Media.Imaging;
using Autodesk.Revit.Attributes;
using static System.Environment;

namespace CutOpeningsPlugin
{
    /// <summary>
    /// Implements the Revit add-in interface IExternalApplication
    /// </summary>
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class Application : IExternalApplication
    {
        /// <summary>
        /// Implements the on Shutdown event
        /// </summary>
        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        /// <summary>
        /// Implements the OnStartup event
        /// </summary>
        public Result OnStartup(UIControlledApplication application)
        {
            RibbonPanel panel = RibbonPanel(application);
            string thisAssemblyPath = Assembly.GetExecutingAssembly().Location;

            AddButton(panel, thisAssemblyPath,
                "InsertOpeningsBtn",
                "Скопировать отверстия",
                "CutOpeningsPlugin.InsertingCommand",
                "В выбранном связанном файле осуществляется поиск отверстий и проемов, на основе найденных элементов создаются отверстия с пустотелой геометрией.",
                new BitmapImage(
                    new Uri(Path.Combine(GetFolderPath(
                        SpecialFolder.ApplicationData) + @"\Autodesk\Revit\Addins\2021\CGN_AddIns\ICO", "32x32_copy.ico"))),
                ContextualHelpType.Url,
                @"ContextualHelpLink1"
                );

            AddButton(panel, thisAssemblyPath,
                "CutOpeningsBtn",
                "Вырезать геометрию",
                "CutOpeningsPlugin.CuttingCommand",
                "Вырезание объемов из элементов выбранных категорий.",
                new BitmapImage(
                    new Uri(Path.Combine(GetFolderPath(
                        SpecialFolder.ApplicationData) + @"\Autodesk\Revit\Addins\2021\CGN_AddIns\ICO", "32x32_cutting.ico"))),
                ContextualHelpType.Url,
                @"ContextualHelpLink2"
                );

            return Result.Succeeded;    
        }

        /// <summary></summary>
        private void AddButton(RibbonPanel panel, string thisAssemblyPath, string btnName, string btnTxt, string btnClassName, string tooltip, BitmapImage img, 
            ContextualHelpType contextualHelpType, string path)
        {
            PushButton button = panel.AddItem(new PushButtonData(btnName,btnTxt,thisAssemblyPath,btnClassName)) as PushButton;
            button.ToolTip = tooltip;
            button.LargeImage = img;
            
            if (contextualHelpType != ContextualHelpType.None)
                button.SetContextualHelp(new ContextualHelp(contextualHelpType, path));
        }

        /// <summary></summary>
        public RibbonPanel RibbonPanel(UIControlledApplication a)
        {
            string tab = "Testing Pannel";
            a.CreateRibbonTab(tab);
            return a.CreateRibbonPanel(tab, "Копирование отверстий");
        }
    }
}