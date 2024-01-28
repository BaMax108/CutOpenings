using CutOpeningsPlugin.Inserting.Data.Enums;
using CutOpeningsPlugin.Inserting.Data.Interfaces;
using CutOpeningsPlugin.Inserting.Models;
using System.Collections.Generic;

namespace CutOpeningsPlugin.Inserting.Data
{
    /// <summary>
    /// Коллекция семейств отверстий и проемов.
    /// </summary>
    public class RepositoryFSymbols
    {
        /// <summary>
        /// Коллекция отверстий.
        public Dictionary<ShapeType, List<IOpening>> OpeningsDictionary { get; }

        /// <summary></summary>
        public RepositoryFSymbols()
        {
            Builder b = new Builder();
            OpeningsDictionary = new Dictionary<ShapeType, List<IOpening>>()
            {
                {
                    ShapeType.Circle, b.CreateListOfTypes<IOpening>("CutOpeningsPlugin.Inserting.Data.OpeningsCircular")
                },
                {
                    ShapeType.Rechtangle, b.CreateListOfTypes<IOpening>("CutOpeningsPlugin.Inserting.Data.OpeningsRechtangular")
                }
            };

        }
    }
}
