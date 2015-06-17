﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace PegView.ViewModel
{
    using ImageCatalog;

    /// <summary>
    /// ViewmModel for ImageCatalog.
    /// </summary>
    public class ImageCatalogViewModel : ViewModelBase
    {
        /// <summary>
        /// Collection of items in the folder view
        /// </summary>
        protected ObservableCollection<NavTreeFolder> items;

        /// <summary>
        /// Catalog, maps metadata about files
        /// </summary>
        protected Catalog catalog;

        /// <summary>
        /// Command to add folder to NavTree
        /// </summary>
        protected ICommand addFolderCommand;

        /// <summary>
        /// holder for new folder input
        /// </summary>
        protected string userInputNewFolder;

        /// <summary>
        /// Create a new ImageCatalogViewModel.
        /// </summary>
        public ImageCatalogViewModel()
        {
            this.items = new ObservableCollection<NavTreeFolder>();
            this.userInputNewFolder = null;
            
            // function to run when a new item is added to the folder list. 
            // for now, hardcoding child files (jpgs, etc). 
            this.addFolderCommand = new DelegateCommand((parameter) =>
            {
                string path = parameter as string;

                this.items.Add(new NavTreeFolder(path, @"jpg$|jpeg$|bmp$|gif$|png$"));
                this.UserInputNewFolderPath = string.Empty;
                this.RaisePropertyChangedEvent("UserInputNewFolderPath");
                this.RaisePropertyChangedEvent("Catalog");
            });
        }

        /// <summary>
        /// Command to run when new folder is added to navtree.
        /// </summary>
        public ICommand CommandAddNewFile
        {
            get
            {
                return addFolderCommand;
            }
        }

        /// <summary>
        /// Observable collection of items in navigation tree
        /// </summary>
        public ObservableCollection<NavTreeFolder> NavTreeRootItems
        {
            get
            {
                return this.items;
            }
        }
     
        /// <summary>
        /// The user input for the new path to add.
        /// </summary>
        public string UserInputNewFolderPath
        {
            get
            {
                return this.userInputNewFolder;
            }

            set
            {
                string oldVal = this.userInputNewFolder;
                this.userInputNewFolder = value;

                if (null == OnValidate("UserInputNewFolderPath"))
                {
                    this.RaisePropertyChangedEvent("UserInputNewFolderPath");
                }

                this.RaisePropertyChangedEvent("AddFolderValid");
            }
        }

        /// <summary>
        /// Returns true if the folder in UserInputNewFolderPath is valid.
        /// Valid folders must 1.) exist and 2.) not already be added
        /// </summary>
        public bool AddFolderValid
        {
            get
            {                
                return string.IsNullOrEmpty(this["UserInputNewFolderPath"]);
            }
        }

        /// <summary>
        /// Validate the property, the error message gets assigned 
        /// in base class ViewModelBase
        /// </summary>
        /// <param name="propertyName">The property to validate for</param>
        /// <returns>string.empty if valid, the error message otherwise</returns>
        protected override string OnValidate(string propertyName)
        {            
            switch(propertyName)
            {
                case "UserInputNewFolderPath":
                    {
                        if(!Directory.Exists(this.userInputNewFolder))
                        {
                            return string.Format("Directory {0} does not exist", this.userInputNewFolder);                            
                        }
                        if(this.items.Any(n => n.ItemFullPath == this.userInputNewFolder))
                        {
                            return string.Format("Directory {0} already exists in the collection", this.userInputNewFolder);                            
                        }
                        return null;
                    }
                default:
                    return null;
            }
        }
    }
}
