using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageCatalog
{
    public interface IFileSaveLoad
    {
        /// <summary>
        /// Save an object to the provided path
        /// </summary>
        /// <typeparam name="T">The type of the object to save</typeparam>
        /// <param name="path">The path to save the object to</param>
        /// <param name="objectToSave">The object to save</param>
        void Save<T>(string path, T objectToSave);

        /// <summary>
        /// Load an object of type T from a path
        /// </summary>
        /// <typeparam name="T">The type of the object to load</typeparam>
        /// <param name="path">The path to load from</param>
        /// <returns>An object of type T, loaded from the file</returns>
        T Load<T>(string path);
    }
}