using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ImageCatalog
{
    /// <summary>
    /// Properties contained about a displayItem. These are identified via FullPath (string). 
    /// These can be folders or files. These should be stored in a higher-level container class (supporting constant
    /// time lookup, probably in item FullPath, probably using a dictionary).
    /// Not every DisplayItem will have properties - if properties are not found, appropriate default values 
    /// should be available (example empty internalTags, null rating, etc).
    /// </summary>
    public class DisplayItemProperties
    {
        /// <summary>
        /// Internal set of tags on this item
        /// </summary>
        [JsonProperty]
        protected HashSet<string> internalTags;

        /// <summary>
        /// Internal storage of item rating
        /// </summary>
        [JsonProperty]
        protected int? rating;
        
        /// <summary>
        /// Construct a new CatalogItem
        /// </summary>
        public DisplayItemProperties()
        {
            this.rating = null;
            this.internalTags = new HashSet<string>();
        }

        /// <summary>
        /// Copy Constructor
        /// </summary>
        /// <param name="other"></param>
        public DisplayItemProperties(DisplayItemProperties other)
        {
            this.internalTags = new HashSet<string>(other.internalTags);
            this.rating = other.rating;
        }

        /// <summary>
        /// Constructor taking a path
        /// </summary>
        /// <param name="fullPath"></param>
        [JsonConstructor]
        public DisplayItemProperties(string ItemFullPath)
            : this()
        {
            this.ItemFullPath = ItemFullPath;
        }
        
        /// <summary>
        /// the rating of the item
        /// </summary>
        public int? Rating
        {
            get
            {
                return this.rating;
            }
            
            set
            {
                if(value == null || (value < 5 && value > 0))
                {
                    this.rating = value;
                }
                else
                {
                    this.rating = null;
                }
            }
        }

        /// <summary>
        /// The tags on the item, simple string identifiers
        /// so you can label your cat photos and find them fast.
        /// </summary>
        public IEnumerable<string> Tags
        {
            get
            {
                return this.internalTags.AsEnumerable<string>();
            }
        }

        /// <summary>
        /// The path to the item.
        /// </summary>
        public string ItemFullPath
        {
            get;
            protected set;
        }

        /// <summary>
        /// Item Name
        /// </summary>
        public string ItemName
        {
            get;
            protected set;
        }

        /// <summary>
        /// Add a descriptive tag to the item
        /// </summary>
        /// <param name="tag">The tag to add</param>
        public void AddTag(string tag)
        {
            if(this.internalTags.Contains(tag))
            {
                return;
            }

            this.internalTags.Add(tag);

            // TODO : possible event hookup with TagRegister ?
            // if nothing else, be sure the caller handles it manually;
        }

        /// <summary>
        /// Remove a tag from an item
        /// </summary>
        /// <param name="tag"></param>
        public void RemoveTag(string tag)
        {
            if (this.internalTags.Contains(tag))
            {
                this.internalTags.Remove(tag);
            }
            
            // TODO : possible event hookup with TagRegister ?
            // if nothing else, be sure the caller handles it manually;
        }
    }
}
