using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.ObjectModel;
using ImageCatalog;

namespace PegView.ViewModel
{
    public class ImageCatalogViewModel : ViewModelBase
    {
        protected ObservableCollection<FolderCatalogItem> items;

        public ImageCatalogViewModel()
        {
            this.items = new ObservableCollection<FolderCatalogItem>();
        }

        public ObservableCollection<FolderCatalogItem> Catalog
        {
            get
            {
                return this.items;
            }
        }

        public void AddItem(FolderCatalogItem itemToAdd)
        {
            this.items.Add(itemToAdd);
            this.RaisePropertyChangedEvent("Catalog");
        }

        public void RemoveItem(FolderCatalogItem itemToRemove)
        {
            if(this.items.Contains(itemToRemove))
            {
                this.items.Remove(itemToRemove);
            }

            this.RaisePropertyChangedEvent("Catalog");
        }
    }
}
