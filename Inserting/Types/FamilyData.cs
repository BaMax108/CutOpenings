using CutOpeningsPlugin.Inserting.Data.Enums;

namespace CutOpeningsPlugin.Inserting.Types
{
    /// <summary>
    /// Данные об экземпляре семейства.
    /// </summary>
    public class FamilyData
    {
        public FamilyData() { }

        /// <summary></summary>
        public FamilyData(string _name,
            string _heigh,
            bool _isInstanceHeigh,
            string _width,
            bool _isInstancWidth,
            string _thickness,
            bool _isInstancThickness,
            ShapeType _shapeType)
        {
            Name = _name;
            Heigh = _heigh;
            IsInstanceHeigh = _isInstanceHeigh;
            Width = _width;
            IsInstanceWidth = _isInstancWidth;
            Thickness = _thickness;
            IsInstanceThickness = _isInstancThickness;
            ShapeType = _shapeType;
        }
        /// <summary></summary>
        public FamilyData(string _name,
            string _diameter,
            bool _isInstanceDiameter,
            string _thickness,
            bool _isInstancThickness,
            ShapeType _shapeType)
        {
            Name = _name;
            Diameter = _diameter;
            IsInstancDiameter = _isInstanceDiameter;
            Thickness = _thickness;
            IsInstanceThickness = _isInstancThickness;
            ShapeType = _shapeType;
        }

        /// <summary>
        /// Название семейства.
        /// </summary>
        public string Name { get; }
        
        /// <summary>
        /// Название параметра, отвечающего за высоту экземпляра.
        /// </summary>
        public string Heigh { get; }

        /// <summary>
        /// Является ли параметр параметром экземпляра.
        /// </summary>
        public bool IsInstanceHeigh { get; }

        /// <summary>
        /// Название параметра, отвечающего за ширину экземпляра.
        /// </summary>
        public string Width { get; }

        /// <summary>
        /// Является ли параметр параметром экземпляра.
        /// </summary>
        public bool IsInstanceWidth { get; }

        /// <summary>
        /// Название параметра, отвечающего за толщину основы экземпляра.
        /// </summary>
        public string Thickness { get; }

        /// <summary>
        /// Является ли параметр параметром экземпляра.
        /// </summary>
        public bool IsInstanceThickness { get; }

        /// <summary>
        /// Название параметра, отвечающего за диаметр экземпляра.
        /// </summary>
        public string Diameter { get; }

        /// <summary>
        /// Является ли параметр параметром экземпляра.
        /// </summary>
        public bool IsInstancDiameter { get; }

        /// <summary>
        /// Геометрия сечения.
        /// </summary>
        public ShapeType ShapeType { get; }
    }
}

/*
 
string Name
string Heigh
bool IsInstanceHeigh
string Width
bool IsInstancWidth
string Thickness
bool IsInstancThickness
string Diameter
bool IsInstancDiameter
ShapeType ShapeType

 */