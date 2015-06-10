using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageCatalog
{
    /// <summary>
    /// CatalogItem representing a folder
    /// </summary>
    public class FolderCatalogItem : CatalogItemBase
    {
        protected DirectoryInfo dirInfo;
        
        public FolderCatalogItem(string folderRoot) : base()
        {
            this.ItemFullPath = folderRoot;
            dirInfo = new DirectoryInfo(this.ItemFullPath);
        }

        public new string ItemName
        {
            get
            {
                return dirInfo.Name;
            }            
        }

        public List<CatalogItemBase> ChildContainers
        {
            get
            {
                List<CatalogItemBase> toReturn = new List<CatalogItemBase>();
                DirectoryInfo[] directories = new DirectoryInfo[] { };

                try
                {
                    directories = this.dirInfo.GetDirectories();
                }
                catch(Exception)
                {
                    throw;
                }

                foreach (DirectoryInfo dir in directories)
                {
                    toReturn.Add(new FolderCatalogItem(dir.FullName));
                }

                return toReturn;
            }

            set
            {
                throw new NotImplementedException();
            }
        }        
    }
}
