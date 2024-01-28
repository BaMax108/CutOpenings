using Autodesk.Revit.DB;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace CutOpeningsPlugin.Inserting.Data
{
    /// <summary></summary>
    public class InitialData
    {
        /// <summary></summary>
        public string OpeningsName { get; } = "OpeningsFamilyName";

        /// <summary></summary>
        public FamilySymbol CurrentFamily { get; set; }
        /// <summary></summary>
        public List<RevitLinkInstance> LinkInstances { get; set; }
        /// <summary></summary>
        public List<Element> OpeningsFromLink { get; private set; }
        /// <summary></summary>
        public Dictionary<string, Dictionary<string, Bitmap>> Dict { get; private set; }
        /// <summary></summary>
        public List<FamilyInstance> NewOpenings { get; private set; } = new List<FamilyInstance>();

        private readonly Document Doc;
        
        /// <summary>Массив категорий, с которыми проверяются пересечения.</summary>
        public BuiltInCategory[] CategoriesForTestingCollisionsLink { get; } = new BuiltInCategory[]
        {
            BuiltInCategory.OST_Windows,
            BuiltInCategory.OST_Doors
        };

        /// <summary></summary>
        public InitialData(Document _doc)
        {
            Doc = _doc;
            LinkInstances = new List<RevitLinkInstance>();
            OpeningsFromLink = new List<Element>();
            Dict = new Dictionary<string, Dictionary<string, Bitmap>>();
        }

        /// <summary></summary>
        public void AddOpenings(FamilyInstance fi) => NewOpenings.Add(fi);

        /// <summary></summary>
        public List<string> Getlinks()
        {
            var collectorLinks = new FilteredElementCollector(Doc)
                .OfCategory(BuiltInCategory.OST_RvtLinks)
                .OfClass(typeof(RevitLinkType))
                .ToElements()
                .Cast<RevitLinkType>();

            List<string> linksNames = new List<string>();
            foreach (RevitLinkType rvtLink in collectorLinks)
            {
                if (rvtLink.GetLinkedFileStatus() == LinkedFileStatus.Loaded & rvtLink.IsNestedLink != true)
                {
                    linksNames.Add(rvtLink.Name);
                }
            }
            return linksNames;
        }

        /// <summary>
        /// Получение списка связанных файлов.
        /// </summary>
        public List<RevitLinkInstance> GetLinkInstances(string linkName)
        {
            List<RevitLinkInstance>  linkInstances = new List<RevitLinkInstance>();
            var collector = new FilteredElementCollector(Doc)
                .OfClass(typeof(RevitLinkInstance))
                .WhereElementIsNotElementType()
                .ToElementIds();

            foreach (var col in collector)
            {
                RevitLinkInstance instance = Doc.GetElement(col) as RevitLinkInstance;
                var fullName = instance.Name.Split(':');
                if (instance == null | fullName[0].Trim() != linkName) continue;

                linkInstances.Add(instance);
            }

            return linkInstances;
        }

        /// <summary>
        /// Получение типа семейства для вставки в пространство модели.
        /// </summary>
        public FamilySymbol GetFamilyInstance()
        {
            return new FilteredElementCollector(Doc)
                .OfCategory(BuiltInCategory.OST_TelephoneDevices)
                .WhereElementIsElementType()
                .FirstOrDefault(el => el.Name == OpeningsName) as FamilySymbol;
        }

        /// <summary>
        /// Получение экземпляров семейств, указанных категорий, из связанного файла.
        /// </summary>
        public void GetOpeningsFromLink(Document docLink)
        {
            OpeningsFromLink = new List<Element>();

            List<Element> tempList = new List<Element>();
            tempList.AddRange(new FilteredElementCollector(docLink)
                .WherePasses(new ElementClassFilter(typeof(FamilyInstance)))
                .ToList()
                .Where(z => z.Category.Id == new ElementId(-2000014) ||     // -2000014 Окна
                            z.Category.Id == new ElementId(-2000151) ||     // -2000151 Обобщенные модели
                            z.Category.Id == new ElementId(-2001300) ||     // -2001300 Фундамент несущей конструкции
                            z.Category.Id == new ElementId(-2008075)));     // -2008075 Телефонные устройства


            OpeningsFromLink.AddRange(tempList);
        }
    }
}
