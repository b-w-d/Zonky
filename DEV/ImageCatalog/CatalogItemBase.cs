using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageCatalog
{
    /// <summary>
    /// Base class for CatalogItems. Folders, Files, etc. Contains basic properties for
    /// tags, ratings, anything else. 
    /// </summary>
    public class CatalogItemBase
    {
        /// <summary>
        /// Internal set of tags on this item
        /// </summary>
        protected HashSet<string> internalTags;
        
        /// <summary>
        /// Construct a new CatalogItem
        /// </summary>
        public CatalogItemBase()
        {
            this.Rating = null;
            this.internalTags = new HashSet<string>();
        }
        
        /// <summary>
        /// the rating of the item
        /// </summary>
        public int? Rating
        {
            get;
            set;
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
