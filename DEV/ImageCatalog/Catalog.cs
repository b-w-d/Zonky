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
        protected Dictionary<string, DisplayItemProperties> internalItemMap;

        /// <summary>
        /// TagRegister to help map tags to files
        /// </summary>
        [JsonProperty]
        protected TagRegister tagRegister;

        /// <summary>
        /// Create a new, empty ImageCatalog
        /// </summary>
        public Catalog()
        {
            this.internalItemMap = new Dictionary<string, DisplayItemProperties>();
            this.tagRegister = new TagRegister();
        }

        public DisplayItemProperties GetDisplayItemProperties(string file)
        {
            if (!this.internalItemMap.ContainsKey(file))
            {
                return new DisplayItemProperties();
            }

            return new DisplayItemProperties(this.internalItemMap[file]);
        }
        
        public IEnumerable<string> GetTagsOnFile(string file)
        {
            DisplayItemProperties item = GetDisplayItemProperties(file);
            return new List<string>(item.Tags);
        }

        public IEnumerable<DisplayItemProperties> GetItemsWithTag(string tag)
        {
            return this.tagRegister.Get(tag);
        }

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
            DisplayItemProperties item = GetOrAddDisplayItemProp(file);

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
            if (!this.internalItemMap.ContainsKey(file))
            {
                return;                
            }

            DisplayItemProperties item = this.internalItemMap[file];

            item.RemoveTag(tag);
            this.tagRegister.DeList(tag, item);
        }

        /// <summary>
        /// Give a file a rating
        /// </summary>
        /// <param name="file"></param>
        /// <param name="rating"></param>
        public void RateFile(string file, int rating)
        {
            DisplayItemProperties item = GetOrAddDisplayItemProp(file);
            item.Rating = rating;
        }

        /// <summary>
        /// Clear a rating for a file
        /// </summary>
        /// <param name="file"></param>
        public void ClearRating(string file)
        {
            DisplayItemProperties item = GetOrAddDisplayItemProp(file);
            item.Rating = null;
        }

        /// <summary>
        /// Gets or adds a display item. If the item already exists and is mapped
        /// to a string, return it. Otherwise, create a new one, add it, and return.
        /// </summary>
        /// <param name="file">The string to search on</param>
        /// <returns>A displayItemProperty of the string</returns>
        protected DisplayItemProperties GetOrAddDisplayItemProp(string file)
        {
            DisplayItemProperties item;

            if (!this.internalItemMap.ContainsKey(file))
            {
                item = new DisplayItemProperties(file);
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
