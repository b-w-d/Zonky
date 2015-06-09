using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageCatalog
{
    /// <summary>
    /// Class for tracking how tags map to various CatalogItems. Given a set of items, it is
    /// easy to find the tags associated with each. The reverse is not so simple. This class
    /// handles that case.
    /// </summary>
    public class TagRegister
    {
        /// <summary>
        /// Map of references tags --> CatalogItems
        /// </summary>
        protected Dictionary<string, List<CatalogItemBase>> tagBase;

        /// <summary>
        /// Construct a new, empty, TagRegister
        /// </summary>
        public TagRegister()
        {
            this.tagBase = new Dictionary<string, List<CatalogItemBase>>();
        }

        /// <summary>
        /// Register a Tag with a specific item
        /// </summary>
        /// <param name="tagName">The name of the tag</param>
        /// <param name="item">A reference to the item to tag</param>
        public void Register(string tagName, CatalogItemBase item)
        {
            if(this.tagBase.ContainsKey(tagName))
            {
                this.tagBase.Add(tagName, new List<CatalogItemBase>());
            }

            if(this.tagBase[tagName].Contains(item))
            {
                // no duplicates allowed
                return;
            }

            this.tagBase[tagName].Add(item);
            return;
        }

        /// <summary>
        /// Get the CatalogItems associated with the tag. If the tag
        /// does not exist, return an empty set.
        /// </summary>
        /// <param name="tagName">The tag to search for</param>
        /// <returns>The items matching that tagname</returns>
        public IEnumerable<CatalogItemBase> Get(string tagName)
        {
            if(this.tagBase.ContainsKey(tagName))
            {
                return this.tagBase[tagName].AsEnumerable<CatalogItemBase>();
            }

            return new List<CatalogItemBase>();
        }
    }
}
