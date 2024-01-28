namespace CutOpeningsPlugin.Inserting.Data.Interfaces
{
    interface IOpeningRechtangular : IOpening
    {
        /// <summary>
        /// Название параметра, отвечающего за высоту экземпляра.
        /// </summary>
        string Heigh { get; }

        /// <summary>
        /// Является ли параметр параметром экземпляра.
        /// </summary>
        bool IsInstanceHeigh { get; }

        /// <summary>
        /// Название параметра, отвечающего за ширину экземпляра.
        /// </summary>
        string Width { get; }

        /// <summary>
        /// Является ли параметр параметром экземпляра.
        /// </summary>
        bool IsInstanceWidth { get; }

        /// <summary>
        /// Название параметра, отвечающего за толщину основы экземпляра.
        /// </summary>
        string Thickness { get; }

        /// <summary>
        /// Является ли параметр параметром экземпляра.
        /// </summary>
        bool IsInstanceThickness { get; }
    }
}
