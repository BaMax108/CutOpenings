using System.Collections.Generic;
using System.Reflection;
using System;
using System.Linq;

namespace CutOpeningsPlugin.Inserting.Models
{
    /// <summary></summary>
    public class Builder
    {
        /// <summary>
        /// Метод возвращает коллекцию проинициализированных типов, принадлежащих указанному пространству имен.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="nameSpace"></param>
        /// <returns></returns>
        public List<T> CreateListOfTypes<T>(string nameSpace)
        {
            List<T> result= new List<T>();
            foreach (Type type in Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.Namespace == nameSpace)
                .ToArray())
            {
                if (type == null) continue;
                if (Activator.CreateInstance(Type.GetType(type.FullName)) is T targetObject) result.Add(targetObject);
            }

            return result;
        }
    }
}
