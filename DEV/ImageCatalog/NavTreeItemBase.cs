using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageCatalog
{
    /// <summary>
    /// Base class for items in the Navigation Tree
    /// </summary>
    public class NavTreeItemBase
    {
        /// <summary>
        /// The full path to the item.
        /// </summary>
        public virtual string ItemFullPath
        {
            get;
            set;
        }

        /// <summary>
        /// The display name in the UI
        /// </summary>
        public virtual string ItemName
        {
            get;
            protected set;
        }

        /// <summary>
        /// Child NavTree items, containing hierarchal data
        /// </summary>
        public virtual List<NavTreeItemBase> ChildContainers
        {
            get;
            protected set;
        }

        /// <summary>
        /// The items contained in this NavItem, probably
        /// images/files to display
        /// </summary>
        public virtual List<DisplayItem> ChildItems
        {
            get;
            protected set;
        }

        public virtual Catalog CatalogRef
        {
            get;
            set;
        }
    }
}
