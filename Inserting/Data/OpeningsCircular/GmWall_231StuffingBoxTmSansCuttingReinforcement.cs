using Autodesk.Revit.DB;
using CutOpeningsPlugin.Inserting.Data.Enums;
using CutOpeningsPlugin.Inserting.Data.Interfaces;

namespace CutOpeningsPlugin.Inserting.Data.OpeningsCircular
{
    /// <summary>
    /// 231_Сальник набивной ТМ без вырезания арм (ОбщМод_Стена)
    /// </summary>
    internal class GmWall_231StuffingBoxTmSansCuttingReinforcement : IOpeningCircular
    {
        #region Свойства

        public ValidOpenings ValidOpenings { get; } = ValidOpenings.StuffingBoxTmSansCuttingReinforcement_GenericModelWall_231;

        /// <summary>
        /// Название семейства.
        /// </summary>
        public string Name { get; } = "231_Сальник набивной ТМ без вырезания арм (ОбщМод_Стена)";

        /// <summary>
        /// Название параметра, отвечающего за толщину основы экземпляра.
        /// </summary>
        public string Thickness { get; } = "Рзм.ТолщинаОсновы";

        /// <summary>
        /// Является ли параметр параметром экземпляра.
        /// </summary>
        public bool IsInstanceThickness { get; } = true;

        /// <summary>
        /// Название параметра, отвечающего за диаметр экземпляра.
        /// </summary>
        public string Diameter { get; } = "Диаметр наружный";

        /// <summary>
        /// Является ли параметр параметром экземпляра.
        /// </summary>
        public bool IsInstancDiameter { get; } = false;

        /// <summary>
        /// Геометрия сечения.
        /// </summary>
        public ShapeType ShapeType { get; } = ShapeType.Circle;

        #endregion

        /// <summary>
        /// Вычисление точки вставки для нового экземпляра семейства.
        /// </summary>
        public XYZ SetLocationPoint(Document doc, XYZ basePoint, Element element, XYZ linkPosition)
        {
            BoundingBoxXYZ bb = element.get_BoundingBox(doc.ActiveView);

            XYZ newInstanceLocation = basePoint + linkPosition;

            if ((element as FamilyInstance).Location == null)
            {
                newInstanceLocation += (bb.Max + bb.Min) / 2;
            }
            else
            {
                newInstanceLocation += (element.Location as LocationPoint).Point;
            }

            return newInstanceLocation;
        }
    }
}
