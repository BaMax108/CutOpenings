using Autodesk.Revit.DB;
using CutOpeningsPlugin.Inserting.Data.Enums;
using CutOpeningsPlugin.Inserting.Data.Interfaces;

namespace CutOpeningsPlugin.Inserting.Data.OpeningsCircular
{
    /// <summary>
    /// 232_Круглое отверстие в плите без вырезания арм (ОбщМод_Грань)
    /// </summary>
    internal class GmEdg_232CircleOpeningInFloorSansCuttingReinforcement : IOpeningCircular
    {
        #region Свойства

        public ValidOpenings ValidOpenings { get; } = ValidOpenings.CircleOpeningInFloorSansCuttingReinforcement_GenericModelEdge_232;

        /// <summary>
        /// Название семейства.
        /// </summary>
        public string Name { get; } = "232_Круглое отверстие в плите без вырезания арм (ОбщМод_Грань)";

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
        public string Diameter { get; } = "Рзм.Диаметр";

        /// <summary>
        /// Является ли параметр параметром экземпляра.
        /// </summary>
        public bool IsInstancDiameter { get; } = true;

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
