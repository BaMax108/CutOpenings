using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Collections.Generic;
using System.Text;

namespace CutOpeningsPlugin.Inserting.Data
{
    /// <summary></summary>
    public class RepositoryErrors
    {
        /// <summary>
        /// Проверка является ли view.ViewType видом 3D.
        /// </summary>
        public bool IsValidView(View view)
        {
            if (view.ViewType != ViewType.ThreeD) 
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("Перед запуском плагина перейдите на 3D вид (настройки видимости/графики значения не имеют.\n");
                sb.Append($"Активный вид \"{view.Title}\"");

                TaskDialog.Show("Предупреждение", sb.ToString());
                return true;
            }
            return false;
        }

        /// <summary>
        /// Проверка наличия значения в переменной.
        /// </summary>
        public bool IsNull(FamilySymbol arg)
        {
            if (arg == null)
            {
                TaskDialog.Show("Предупреждение", "В модели отсутствует подходящее семейство.");
                return true;
            }
            return false;
        }
        /// <summary>
        /// Проверка наличия объектов в коллекции.
        /// </summary>
        public bool IsNull(List<RevitLinkInstance> arg)
        {
            if (arg.Count == 0)
            {
                TaskDialog.Show("Предупреждение", "В пространстве модели отсутствуют экземпляры связанных файлов.");
                return true;
            }
            return false;
        }
    }
}
