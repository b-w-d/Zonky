using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Collections.ObjectModel;

namespace PegView.ViewModel
{
    using ImageCatalog;

    /// <summary>
    /// ViewModel on DisplayItem
    /// </summary>
    public class DisplayItemViewModel : ViewModelBase
    {        
        /// <summary>
        /// Relay Command to bind to
        /// </summary>
        protected RelayCommand addTagCommand;

        /// <summary>
        /// Internal DisplayItem model
        /// </summary>
        protected DisplayItem internalDisplayItem;

        /// <summary>
        /// Construct a new DisplayItemViewModel based on the provided path and Catalog
        /// </summary>
        /// <param name="path">The path to the item</param>
        /// <param name="catalogReference">The Catalog to use to store item metadata</param>
        public DisplayItemViewModel(string path, Catalog catalogReference)
        {
            this.internalDisplayItem = new DisplayItem(path, catalogReference);            
            this.CatalogReference = catalogReference;
        }

        /// <summary>
        /// Construct a new DisplayItemViewModel based on the provided DisplayItem and Catalog
        /// </summary>
        /// <param name="displayItemRef">A reference to a display item</param>
        /// <param name="catalogRef">A reference to a catalog</param>
        public DisplayItemViewModel(DisplayItem displayItemRef, Catalog catalogRef)
        {
            this.internalDisplayItem = displayItemRef;
            this.CatalogReference = catalogRef;
        }

        /// <summary>
        /// Reference to a catalog instance, where we can add user properties. Don't bind on this!!
        /// </summary>
        public Catalog CatalogReference
        {
            get;
            set;
        }

        /// <summary>
        /// The DisplayItem represented by the viewmodel. Including this now in case 
        /// the caller wants to bind to it : this is just to save typing of various properties that
        /// go directly to model. however Dont bind to a method, make a command here, and call it from there.
        /// 
        /// If this is problematic, expose the various properties through VM and consider removing this.
        /// </summary>
        public DisplayItem ItemReference
        {
            get
            {
                return this.internalDisplayItem;
            }
            set
            {
                this.internalDisplayItem = value;
                this.RaisePropertyChangedEvent("ItemReference");
            }
        }

        /// <summary>
        /// The full path to the item
        /// </summary>
        public string ItemFullPath
        {
            get
            {
                return this.internalDisplayItem.ItemFullPath;
            }
        }

        /// <summary>
        /// The name of the item
        /// </summary>
        public string ItemName
        {
            get
            {
                return this.internalDisplayItem.ItemName;
            }
        }

        /// <summary>
        /// Semicolon joined list of tags on this display item
        /// </summary>
        public string TagsConcatenated
        {
            get
            {
                return string.Join("; ", this.internalDisplayItem.UserProperties.Tags);
            }
        }

        /// <summary>
        /// Get the tags on this item
        /// </summary>
        public ObservableCollection<string> Tags
        {
            get
            {
                return new ObservableCollection<string>(ItemReference.UserProperties.Tags);
            }
        }
 
        /// <summary>
        /// Command to add a new tag
        /// </summary>
        public ICommand AddTag
        {
            get
            {
                if (this.addTagCommand == null)
                {
                    /// TODO : This will need to be much more advanced, allow error checking, etc. 
                    /// For now, tags are just Semi-Colon Separated words. They SHOULD NOT have spaces.
                    /// We will just iterate over the words, and add=remove as necessary. 
                    /// Will need to be much, much more advanced later on.
                    this.addTagCommand = new RelayCommand(                        
                        (parameter) =>
                        {                            
                            CatalogReference.SetTagsOnFile(this.ItemFullPath, parameter as string);
                            this.RaisePropertyChangedEvent("ItemReference");
                            this.RaisePropertyChangedEvent("Tags");
                            
                        });
                }
                return this.addTagCommand;
            }
        }
    }
}
