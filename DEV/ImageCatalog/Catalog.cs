using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ImageCatalog
{
    /// <summary>
    /// Container class mapping files (given as full path strings) to DisplayItemProperties.
    /// Absence of a file in this container should result in default properties (no rating, 
    /// no tags, etc) not a nullRefException.
    /// Modification of properties should route through this class.
    /// </summary>
    public class Catalog
    {
        /// <summary>
        /// Structure containing the info
        /// </summary>
        [JsonProperty]
        protected Dictionary<string, DisplayItemUserProperties> internalItemMap;

        /// <summary>
        /// TagRegister to help map tags to files
        /// </summary>
        [JsonProperty]
        internal TagRegister tagRegister;

        /// <summary>
        /// Create a new, empty ImageCatalog
        /// </summary>
        public Catalog()
        {
            this.internalItemMap = new Dictionary<string, DisplayItemUserProperties>();
            this.tagRegister = new TagRegister();
        }

        /// <summary>
        /// Retrieve a default DisplayItemUserProperties
        /// </summary>
        /// <returns></returns>
        public static DisplayItemUserProperties DefaultDisplayItemProperties()
        {
            return new DisplayItemUserProperties();
        }

        /// <summary>
        /// Retrieve the DisplayItemUserProperties on a file
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public DisplayItemUserProperties GetDisplayItemProperties(string file)
        {
            if (!this.internalItemMap.ContainsKey(file))
            {
                return Catalog.DefaultDisplayItemProperties();
            }

            return this.internalItemMap[file];
        }
        
        /// <summary>
        /// Retrieves the tags on a given file
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public IEnumerable<string> GetTagsOnFile(string file)
        {
            DisplayItemUserProperties item = GetDisplayItemProperties(file);
            return new List<string>(item.Tags);
        }

        /// <summary>
        /// Retrieve the items with a certain tag
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public IEnumerable<DisplayItemUserProperties> GetItemsWithTag(string tag)
        {
            return this.tagRegister.Get(tag);
        }

        /// <summary>
        /// Retrieve the files with a certain tag
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public IEnumerable<string> GetFilesWithTag(string tag)
        {
            return this.GetItemsWithTag(tag).Select(n => n.ItemFullPath);
        }

        /// <summary>
        /// Tag a file
        /// </summary>
        /// <param name="file"></param>
        /// <param name="tag"></param>
        public void TagFile(string file, string tag)
        {            
            if(string.IsNullOrWhiteSpace(tag))
            {
                return;
            }
            
            DisplayItemUserProperties item = GetOrAddDisplayItemProp(file);

            item.AddTag(tag);
            this.tagRegister.Register(tag, item);
        }

        /// <summary>
        /// Remove a tag from a file
        /// </summary>
        /// <param name="file"></param>
        /// <param name="tag"></param>
        public void UnTagFile(string file, string tag)
        {
            if (string.IsNullOrWhiteSpace(tag))
            {
                return;
            }

            if (!this.internalItemMap.ContainsKey(file))
            {
                return;                
            }

            DisplayItemUserProperties item = this.internalItemMap[file];

            item.RemoveTag(tag);
            this.tagRegister.DeList(tag, item);
        }

        /// <summary>
        /// Sets tags on a file using a simple semi-colon separated string
        /// </summary>
        /// <param name="file">The file to add the properties to</param>
        /// <param name="semiColonSeparatedTags">The string of tags to add</param>
        public void SetTagsOnFile(string file, string semiColonSeparatedTags)
        {
            List<string> tags = new List<string>(semiColonSeparatedTags.Split(';').Select(n => n.Trim()));
            this.SetTagsOnFile(file, tags);
        }
        
        /// <summary>
        /// Sets tags on a file using an IEnumerable List
        /// </summary>
        /// <param name="file">The file to add the properties to</param>
        /// <param name="newTags">List of new tags to add</param>
        public void SetTagsOnFile(string file, IEnumerable<string> newTags)
        {
            DisplayItemUserProperties userProps = GetDisplayItemProperties(file);
            HashSet<string> previousTags = new HashSet<string>(userProps.Tags);
                        
            foreach(string newTag in newTags)
            {
                if(previousTags.Contains(newTag))
                {
                    previousTags.Remove(newTag);                    
                    continue;
                }
                else
                {
                    this.TagFile(file, newTag);
                }
            }

            foreach(string oldTag in previousTags)
            {
                this.UnTagFile(file, oldTag);                
            }
        }

        /// <summary>
        /// Give a file a rating
        /// </summary>
        /// <param name="file"></param>
        /// <param name="rating"></param>
        public void RateFile(string file, int rating)
        {
            DisplayItemUserProperties item = GetOrAddDisplayItemProp(file);
            item.Rating = rating;
        }

        /// <summary>
        /// Clear a rating for a file
        /// </summary>
        /// <param name="file"></param>
        public void ClearRating(string file)
        {
            DisplayItemUserProperties item = GetOrAddDisplayItemProp(file);
            item.Rating = null;
        }

        /// <summary>
        /// Gets or adds a display item. If the item already exists and is mapped
        /// to a string, return it. Otherwise, create a new one, add it, and return.
        /// </summary>
        /// <param name="file">The string to search on</param>
        /// <returns>A displayItemProperty of the string</returns>
        protected DisplayItemUserProperties GetOrAddDisplayItemProp(string file)
        {
            DisplayItemUserProperties item;

            if (!this.internalItemMap.ContainsKey(file))
            {
                item = new DisplayItemUserProperties(file);
                this.internalItemMap.Add(file, item);
            }
            else
            {
                item = this.internalItemMap[file];
            }

            return item;
        }
    }
}
