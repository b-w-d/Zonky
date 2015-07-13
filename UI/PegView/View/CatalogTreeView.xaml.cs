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
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {            
            ImageCatalogViewModel catalogVM = this.DataContext as ImageCatalogViewModel;
            catalogVM.ClearSelectedFolders();
            catalogVM.SelectedNavigationItems = null; // fire the setter to             
        }
    }
}
