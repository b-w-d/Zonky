﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageCatalog
{
    /// <summary>
    /// Very simple container class for showing files to be displayed
    /// </summary>
    public class DisplayItem
    {
        /// <summary>
        /// Create a new DisplayItem 
        /// </summary>
        /// <param name="Path">The full path to the item</param>
        public DisplayItem(string Path)
        {
            this.ItemFullPath = Path;            
        }

        /// <summary>
        /// Create a new DisplayItem 
        /// </summary>
        /// <param name="Path">The full path to the item</param>
        public DisplayItem(string Path, Catalog catalogRef)
        {
            this.ItemFullPath = Path;
            this.CatalogRef = catalogRef;
        }

        /// <summary>
        /// The full path to the item
        /// </summary>
        public string ItemFullPath
        {
            get;
            set;
        }

        /// <summary>
        /// The name of the item
        /// </summary>
        public string ItemName
        {
            get
            {
                FileInfo fileInfo = new FileInfo(this.ItemFullPath);
                return fileInfo.Name;
            }
        }

        /// <summary>
        /// Reference to the catalog, so we can add tags and user properties
        /// </summary>
        public Catalog CatalogRef
        {
            get;
            set;
        }

        /// <summary>
        /// User added properties about the item
        /// </summary>
        public DisplayItemUserProperties UserProperties
        {
            get
            {
                DisplayItemUserProperties UserProperties = this.CatalogRef != null ?
                    this.CatalogRef.GetDisplayItemProperties(ItemFullPath) :
                    Catalog.DefaultDisplayItemProperties();

                return UserProperties;
            }           
        }

        /// <summary>
        /// Run time properties about the item
        /// </summary>
        public DisplayItemRuntimeProperties RunTimeProperties
        {
            get;
            set;
        }
    }
}
