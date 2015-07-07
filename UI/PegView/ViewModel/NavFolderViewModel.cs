using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace PegView.ViewModel
{
    using ImageCatalog;

    public class NavFolderViewModel : ViewModelBase
    {
        protected internal NavTreeFolder internalNavFolderReference;

        public NavFolderViewModel(NavTreeItemBase other)
        {
            ItemReference = other as NavTreeFolder;
        }

        public NavTreeFolder ItemReference
        {
            get
            {
                return this.internalNavFolderReference;
            }

            protected set
            {
                this.internalNavFolderReference = value;
            }
        }

        /// <summary>
        /// The name of the folder
        /// </summary>        
        public string ItemName
        {
            get
            {
                return this.ItemReference.ItemName;
            }            
        }

        /// <summary>
        /// The full path to the item.
        /// </summary>
        public string ItemFullPath
        {
            get
            {
                return this.ItemReference.ItemFullPath;
            }            
        }

        public ObservableCollection<NavFolderViewModel> ChildContainers
        {
            get
            {
                return new ObservableCollection<NavFolderViewModel>(
                    this.internalNavFolderReference.ChildContainers.Select(modelFolder => new NavFolderViewModel(modelFolder)));
            }
        }

        public ObservableCollection<DisplayItemViewModel> ChildItems
        {
            get
            {
                return new ObservableCollection<DisplayItemViewModel>(
                    this.internalNavFolderReference.ChildItems.Select(
                        displayItem => 
                        new DisplayItemViewModel(displayItem, this.internalNavFolderReference.CatalogRef)));                
            }
        }
    }
}
