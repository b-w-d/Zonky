using System;
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
    using Newtonsoft.Json;

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
        /// Command to add folder to NavTree
        /// </summary>
        protected RelayCommand addFolderCommand;

        /// <summary>
        /// holder for new folder input
        /// </summary>
        protected string userInputNewFolder;

        /// <summary>
        /// private object for the NavTreeTagitems Property
        /// </summary>
        protected ObservableCollection<NavFolderViewModel> navTreeTagItems;

        /// <summary>
        /// private object the for SelectedNavItems Property. 
        /// 
        /// Contains selected items in the nav tree, whether from tags or folders
        /// </summary>
        protected ObservableCollection<NavFolderViewModel> selectedNavItems;

        /// <summary>
        /// The NavFolderViewModel selected from the tree. This has to be added in by me, 
        /// because the tree doesn't see this by default.
        /// </summary>
        private NavFolderViewModel selectedFolderItemInTree;

        /// <summary>
        /// Create a new ImageCatalogViewModel.
        /// </summary>
        public ImageCatalogViewModel()
        {
            this.ImageCatalog = new Catalog();
            this.items = new ObservableCollection<NavTreeFolder>();
            this.selectedNavItems = new ObservableCollection<NavFolderViewModel>();            
            this.navTreeTagItems = new ObservableCollection<NavFolderViewModel>();
            this.userInputNewFolder = null;            
        }

        /// <summary>
        /// Command to run when new folder is added to navtree.
        /// </summary>
        public ICommand CommandAddNewFile
        {
            get
            {
                if(this.addFolderCommand == null)
                {
                    this.addFolderCommand = new RelayCommand(
                        // function to run when a new item is added to the folder list. 
                        // for now, hardcoding child files (jpgs, etc). 
                        (parameter) =>
                        {
                            string path = parameter as string;
                            NavTreeFolder itemToAdd = new NavTreeFolder(path);
                            itemToAdd.CatalogRef = this.ImageCatalog;
                            this.items.Add(itemToAdd);
                            this.UserInputNewFolderPath = string.Empty;
                            this.RaisePropertyChangedEvent("UserInputNewFolderPath");
                            this.RaisePropertyChangedEvent("Catalog");
                        }, 
                        param => this.AddFolderValid);
                }
                return this.addFolderCommand;
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
        /// Nav Tree Folders, read from disk
        /// </summary>
        [JsonIgnore]
        public ObservableCollection<NavFolderViewModel> NavTreeFolderItems
        {
            get
            {
                return new ObservableCollection<NavFolderViewModel>(this.items.Select(n => new NavFolderViewModel(n)));
            }

            set
            {
                this.RaisePropertyChangedEvent("NavTreeFolderItems");
            }
        }

        /// <summary>
        /// TagList, and files attached to those tags
        /// </summary>
        [JsonIgnore]
        public ObservableCollection<NavFolderViewModel> NavTreeTagItems
        {
            get
            {
                // what a mess... need to sync items here with tags removed/added. 
                // In addition, when tags are added (in displayItems) we don't 
                // have a good way of retrieving them from here. 
                // 
                // TagViewModel?
                foreach(string tag in this.ImageCatalog.GetTags())
                {
                    if(this.navTreeTagItems.Any(n => n.ItemName == tag))
                    {
                        continue;
                    }

                    this.navTreeTagItems.Add(new NavFolderViewModel(new NavTreeTagGroups(tag, this.ImageCatalog)));
                }

                return this.navTreeTagItems;
            }

            set
            {
                this.RaisePropertyChangedEvent("NavTreeTagItems");
            }
        }
        
        /// <summary>
        /// The selected items, whether they be the selected Tag Items or Selected Folder Items
        /// </summary>
        [JsonIgnore]
        public ObservableCollection<NavFolderViewModel> SelectedNavigationItems
        {
            get
            {
                return this.selectedNavItems;
            }

            set
            {
                this.RaisePropertyChangedEvent("SelectedNavigationItems");
                this.RaisePropertyChangedEvent("SelectedSubNavItems");
            }
        }
        
        /// <summary>
        /// Returns the DisplayItemViewModel of selected items in nav tree.
        /// Basically, take the NavFolders returned by SelectedNavigationItems, and 
        /// returns in a single collection.
        /// </summary>
        [JsonIgnore]
        public ObservableCollection<DisplayItemViewModel> SelectedSubNavItems
        {
            get
            {                               
                return new ObservableCollection<DisplayItemViewModel>(this.SelectedNavigationItems.SelectMany(n => n.ChildItems).Where(m => 1 == 1));
            }
        }

        /// <summary>
        /// An additional property for holding the selected item in the nav tree....
        /// .. because the treeview doesn't come with this out of the box. 
        /// </summary>
        [JsonIgnore]
        public NavFolderViewModel SelectedFolderItem
        {
            get
            {
                return selectedFolderItemInTree;
            }
            set
            {
                this.selectedFolderItemInTree = value;

                foreach (NavFolderViewModel item in NavTreeTagItems)
                {
                    item.NavItemIsSelected = false;
                }

                this.selectedNavItems.Clear();

                this.selectedNavItems.Add(this.SelectedFolderItem);

                this.RaisePropertyChangedEvent("SelectedSubNavItems");
            }
        }

        /// <summary>
        /// The catalog. Initially the idea was this viewmodel would just be a wrapper for a
        /// catalog. Now, it is some awful monolithic god object.
        /// </summary>
        public Catalog ImageCatalog
        {
            get;
            set;
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

        /// <summary>
        /// Hacky way to clear the selected items, called when an selection in TagSelector is changed
        /// </summary>
        public void ClearSelectedFolders()
        {
            this.selectedNavItems.Clear();

            foreach (NavFolderViewModel aViewModel in this.NavTreeTagItems.Where(n => n.NavItemIsSelected))
            {
                this.selectedNavItems.Add(aViewModel);
            }
        }
    }
}
