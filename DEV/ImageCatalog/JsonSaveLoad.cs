using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ImageCatalog
{
    public class JsonSaveLoad : IFileSaveLoad
    {
        /// <summary>
        /// Save an object to the provided path
        /// </summary>
        /// <typeparam name="T">The type of the object to save</typeparam>
        /// <param name="path">The path to save the object to</param>
        /// <param name="objectToSave">The object to save</param>
        public void Save<T>(string path, T objectToSave)
        {
            string jsonString = JsonConvert.SerializeObject(objectToSave);

            File.WriteAllText(path, jsonString);
        }

        /// <summary>
        /// Load an object of type T from a path
        /// </summary>
        /// <typeparam name="T">The type of the object to load</typeparam>
        /// <param name="path">The path to load from</param>
        /// <returns>An object of type T, loaded from the file</returns>
        public T Load<T>(string path)
        {
            if(!File.Exists(path))
            {
                return default(T);
            }

            // TODO - try catch block for unreadable file, non-json compliant file. 
            // Should just return default(T) in these cases

            string objectToLoad = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<T>(objectToLoad);
        }
    }
}
