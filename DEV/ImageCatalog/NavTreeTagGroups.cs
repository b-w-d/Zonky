using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageCatalog
{
    using Newtonsoft.Json;

    /// <summary>
    /// Navigation item for finding items tagged a certain tag
    /// </summary>
    public class NavTreeTagGroups : NavTreeItemBase
    {
        private string TagName;

        public NavTreeTagGroups(string tagName, Catalog CatalogRef)
        {
            this.TagName = tagName;
            this.CatalogRef = CatalogRef;
        }

        /// <summary>
        /// The name of the folder
        /// </summary>        
        [JsonIgnore]
        public override string ItemName
        {
            get
            {
                return TagName;
            }
        }

        /// <summary>
        /// The full path to the item.
        /// </summary>
        public override string ItemFullPath
        {
            get
            {
                return string.Empty;
            }
            
            set
            {
                throw new NotImplementedException();
            }
        }

        [JsonIgnore]
        public override Catalog CatalogRef
        {
            get;
            set;
        }

        /// <summary>
        /// Child Folders in this folder. Child folders will have
        /// identical searchPattern logic to the parent.
        /// </summary>
        [JsonIgnore]
        public override List<NavTreeItemBase> ChildContainers
        {
            get
            {
                // empty collection - no hierarchy in tags
                return new List<NavTreeItemBase>();
            }

            protected set
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// The items contained in this NavItem. here just 
        /// display all files found matching searchPatterns.
        /// </summary>
        [JsonIgnore]
        public override List<DisplayItem> ChildItems
        {
            get
            {
                List<DisplayItemUserProperties> taggedItems = new List<DisplayItemUserProperties>(CatalogRef.GetItemsWithTag(this.TagName));
                List<DisplayItem> toReturn = new List<DisplayItem>();

                foreach(DisplayItemUserProperties props in taggedItems)
                {
                    DisplayItem newItem = new DisplayItem(props.ItemFullPath, this.CatalogRef);

                    newItem.CatalogRef = this.CatalogRef;

                    newItem.RunTimeProperties = new DisplayItemRuntimeProperties(props.ItemFullPath);

                    toReturn.Add(newItem);
                }

                return toReturn;
            }
        }
    }
}
