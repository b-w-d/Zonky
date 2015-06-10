﻿using System;
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

            // TODO : Don't use our local paths
            catalog.AddItem(new FolderCatalogItem(@"C:\Users\Brian\Pictures\Pictures"));
            catalog.AddItem(new FolderCatalogItem(@"C:\Users\Brian\Pictures\"));
            catalog.AddItem(new FolderCatalogItem(@"C:\Users\Brian\Pictures\wallpapers"));
            catalog.AddItem(new FolderCatalogItem(@"C:\Users\Brian\Pictures\From Asteroid\Camera roll"));

            DataContext = catalog;
        }
    }
}