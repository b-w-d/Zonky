﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ImageCatalog
{
    /// <summary>
    /// Class for tracking how tags map to various CatalogItems. Given a set of items, it is
    /// easy to find the tags associated with each. The reverse is not so simple. This class
    /// handles that case.
    /// </summary>
    internal class TagRegister
    {
        /// <summary>
        /// Map of references tags --> CatalogItems
        /// </summary>
        [JsonProperty]
        protected Dictionary<string, List<DisplayItemUserProperties>> tagBase;

        /// <summary>
        /// Construct a new, empty, TagRegister
        /// </summary>
        public TagRegister()
        {
            this.tagBase = new Dictionary<string, List<DisplayItemUserProperties>>();
        }

        /// <summary>
        /// Register a Tag with a specific item
        /// </summary>
        /// <param name="tagName">The name of the tag</param>
        /// <param name="item">A reference to the item to tag</param>
        public void Register(string tagName, DisplayItemUserProperties item)
        {
            ValidateTag(tagName);

            if(!this.tagBase.ContainsKey(tagName))
            {
                this.tagBase.Add(tagName, new List<DisplayItemUserProperties>());
            }

            if(this.tagBase[tagName].Contains(item))
            {
                // no duplicates allowed
                return;
            }

            this.tagBase[tagName].Add(item);            
        }

        /// <summary>
        /// Remove tag from a DisplayItem.
        /// If the tag isn't used, do nothing. 
        /// If the tag maps to the same item more than once, throw InvalidProgramException (the design should prevent this).
        /// </summary>
        /// <param name="tagName">the tag name</param>
        /// <param name="item">reference to the DisplayItemProperties to remove</param>
        public void DeList(string tagName, DisplayItemUserProperties item)
        {
            ValidateTag(tagName);
            
            if(!this.tagBase.ContainsKey(tagName))
            {
                return;
            }

            List<DisplayItemUserProperties> itemCollection = this.tagBase[tagName];

            if (itemCollection.Where(props => props.Equals(item)).ToList().Count > 1)
            {
                throw new InvalidProgramException(string.Format("Bad program state - tag {0} maps to an item more than once", tagName));
            }

            itemCollection.Remove(item);
        }

        /// <summary>
        /// Get the CatalogItems associated with the tag. If the tag
        /// does not exist, return an empty set.
        /// </summary>
        /// <param name="tagName">The tag to search for</param>
        /// <returns>The items matching that tagname</returns>
        public IEnumerable<DisplayItemUserProperties> Get(string tagName)
        {
            if(this.tagBase.ContainsKey(tagName))
            {
                return this.tagBase[tagName].AsEnumerable<DisplayItemUserProperties>();
            }

            return new List<DisplayItemUserProperties>();
        }

        public IEnumerable<string> Tags
        {
            get
            {
                return tagBase.Keys.OrderBy(s => s);
            }
        }

        private bool IsTagValid(string tag)
        {
            return(!string.IsNullOrWhiteSpace(tag));
        }

        private void ValidateTag(string tag)
        {
            if(!IsTagValid(tag))
            {
                throw new ArgumentException(string.Format("Tag '{0}' Failed Validation ", tag));
            }
        }
    }
}
