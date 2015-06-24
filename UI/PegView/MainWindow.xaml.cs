using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PegView
{
    using PegView.View;
    using PegView.ViewModel;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ImageCatalogViewModel catalogViewModel;
                
        public MainWindow()
        {
            InitializeComponent();
           
            CatalogTreeView catalogTreeView = this.FindName("CatalogView") as CatalogTreeView;

            ImageCatalog.IFileSaveLoad loader = new ImageCatalog.JsonSaveLoad();

            ImageCatalogViewModel loaded = loader.Load<ImageCatalogViewModel>(this.FileSavePath);
            if(loaded == null)
            {
                loaded = new ImageCatalogViewModel();
            }

            catalogTreeView.DataContext = this.catalogViewModel = loaded;
            this.Closing += OnClosing;
        }

        public string FileSavePath
        {
            get
            {
                return "Catalog.data";
            }
        }

        private void OnClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ImageCatalog.IFileSaveLoad saver = new ImageCatalog.JsonSaveLoad();
            saver.Save<ImageCatalogViewModel>(this.FileSavePath, catalogViewModel);            
        }
    }
}
