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

using PegView.ViewModel;
using ImageCatalog;

namespace PegView.View
{
    /// <summary>
    /// Interaction logic for CatalogTreeView.xaml
    /// </summary>
    public partial class CatalogTreeView : UserControl
    {
        public CatalogTreeView()
        {
            InitializeComponent();
            ImageCatalogViewModel catalog = new ImageCatalogViewModel();

            string searchPattern = @"jpg$|jpeg$|bmp$|gif$|png$";

            // TODO : Don't use our local paths
            catalog.AddItem(new NavTreeFolder(@"C:\Users\Brian\Pictures\Pictures", searchPattern));
            catalog.AddItem(new NavTreeFolder(@"C:\Users\Brian\Pictures\", searchPattern));
            catalog.AddItem(new NavTreeFolder(@"C:\Users\Brian\Pictures\wallpapers", searchPattern));
            catalog.AddItem(new NavTreeFolder(@"C:\Users\Brian\Pictures\From Asteroid\Camera roll", searchPattern));

            DataContext = catalog;
        }
    }
}
