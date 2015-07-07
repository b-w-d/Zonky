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

namespace PegView.View
{
    using PegView.ViewModel;

    /// <summary>
    /// Interaction logic for DisplayItemView.xaml
    /// </summary>
    public partial class DisplayItemView : UserControl
    {
        public DisplayItemView()
        {
            InitializeComponent();
            /*
            string itemPath = @"C:\Users\Brian\Pictures\FkhLTHo.jpg";
            ImageCatalog.Catalog catalog = new ImageCatalog.Catalog();
            catalog.TagFile(itemPath, "cat");
            catalog.TagFile(itemPath, "chinese");
            DisplayItemViewModel dispVM = new DisplayItemViewModel(itemPath, catalog);
            ImageCatalog.DisplayItemRuntimeProperties runtime = new ImageCatalog.DisplayItemRuntimeProperties(itemPath);
            dispVM.ItemReference.RunTimeProperties = runtime;           
            this.DataContext = dispVM;
             */
        }
    }
}
