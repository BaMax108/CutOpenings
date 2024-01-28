using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace CutOpeningsPlugin.Cutting.Views
{
    /// <summary>
    /// Логика взаимодействия для WindowCutSettings.xaml
    /// </summary>
    public partial class WindowCutSettings : Window
    {
        private readonly UIDocument UIDoc;
        private readonly Document Doc;

        private Dictionary<string, Category> CurrentCategories { get; set; }

        /// <summary></summary>
        public List<ElementId> SelectionElementIds { get; private set; }

        /// <summary></summary>
        public Dictionary<string, Category> SelectionCategories { get; private set; }

        /// <summary></summary>
        public WindowCutSettings(UIDocument _uIDoc, Document _doc)
        {
            UIDoc = _uIDoc;
            Doc = _doc;

            SettingsBeforeInitialize();
            InitializeComponent();
            SettingsAfterInitialize();
        }

        private void SettingsBeforeInitialize()
        {
            List<Element> tempList = new List<Element>();

            tempList.AddRange(AddingCategories());
            tempList.AddRange(AddingCategories(BuiltInCategory.OST_Walls));
            tempList.AddRange(AddingCategories(BuiltInCategory.OST_Floors));
            tempList.AddRange(AddingCategories(BuiltInCategory.OST_Roofs));
            tempList.AddRange(AddingCategories(BuiltInCategory.OST_Columns));

            CurrentCategories = new Dictionary<string, Category>();

            foreach (Element e in tempList)
            {
                if (CurrentCategories.ContainsKey(e.Category.Name)) continue;
                else if (e.Category.CategoryType == CategoryType.Model)
                    CurrentCategories.Add(e.Category.Name, e.Category);
            }
        }

        private void SettingsAfterInitialize()
        {
            if (CurrentCategories.Count > 0)
            {
                foreach (var item in CurrentCategories)
                {
                    StPanCategories.Children.Add(new CheckBox { Content = item.Key });
                }
            }
        }

        private List<Element> AddingCategories()
        {
            return new FilteredElementCollector(Doc)
                .WherePasses(new ElementClassFilter(typeof(FamilyInstance)))
                .ToList()
                .Where(z => z.Category.Name != "Окна" &
                            z.Category.Name != "Обобщенные модели" &
                            z.Category.Name != "Телефонные устройства")
                .ToList();
        }
        private List<Element> AddingCategories(BuiltInCategory bc)
        {
            return new FilteredElementCollector(Doc)
                    .OfCategory(bc).WhereElementIsNotElementType()
                    .ToList();
        }

        private void BtnCutAll_Click(object sender, RoutedEventArgs e)
        {
            SelectionCategories = new Dictionary<string, Category>();
            foreach (var i in StPanCategories.Children)
            {
                var cb = i as CheckBox;

                if (cb.IsChecked == true)
                {
                    SelectionCategories.Add(cb.Content.ToString(), CurrentCategories[cb.Content.ToString()]);
                }
            }
            this.Close();
        }

        private void BtnCutSelect_Click(object sender, RoutedEventArgs e)
        {
            SelectionElementIds = UIDoc.Selection.GetElementIds().ToList();
            this.Close();
        }
    }
}
