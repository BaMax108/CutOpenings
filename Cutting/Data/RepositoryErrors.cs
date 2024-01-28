using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Collections.Generic;

namespace CutOpeningsPlugin.Cutting.Data
{
    /// <summary>
    /// Репозиторий с ошибками.
    /// </summary>
    public class RepositoryErrors
    {
        /// <summary>
        /// Проверка корректности значения переменнтой UIDocument.
        /// </summary>
        public bool IsNull(UIDocument arg)
        {
            if (arg == null)
            {
                TaskDialog.Show("Error", "Current UI document was NULL.");
                return true;
            }
            return false;
        }
        /// <summary>
        /// Проверка корректности значения переменнтой Document.
        /// </summary>
        public bool IsNull(Document arg)
        {
            if (arg == null)
            {
                TaskDialog.Show("Error", "Current Autodesk Revit project was NULL.");
                return true;
            }
            return false;
        }
        /// <summary>
        /// Проверка корректности коллекции ICollection.
        /// </summary>
        public bool IsNull(int count, string familyName)
        {
            if (count == 0)
            {
                TaskDialog.Show("Error", $"В пространстве модели отсутствуют экземпляры семейства {familyName}.");
                return true;
            }
            return false;
        }
        /// <summary>
        /// Проверка корректности коллекции ICollection.
        /// </summary>
        public bool IsNull(ICollection<ElementId> arg)
        {
            if (arg.Count == 0)
            {
                TaskDialog.Show("Error", "Элементы не выбраны.");
                return true;
            }
            return false;
        }
        
    }
}
