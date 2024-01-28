namespace CutOpeningsPlugin.Inserting.Data.Interfaces
{
    interface IOpeningCircular : IOpening
    {
        /// <summary>
        /// Название параметра, отвечающего за толщину основы экземпляра.
        /// </summary>
        string Thickness { get; }

        /// <summary>
        /// Является ли параметр параметром экземпляра.
        /// </summary>
        bool IsInstanceThickness { get; }

        /// <summary>
        /// Название параметра, отвечающего за диаметр экземпляра.
        /// </summary>
        string Diameter { get; }

        /// <summary>
        /// Является ли параметр параметром экземпляра.
        /// </summary>
        bool IsInstancDiameter { get; }
    }
}
