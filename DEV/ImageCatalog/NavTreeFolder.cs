using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ImageCatalog
{
    /// <summary>
    /// CatalogItem representing a folder
    /// </summary>
    public class NavTreeFolder : NavTreeItemBase
    {
        /// <summary>
        /// information about the folder
        /// </summary>
        protected DirectoryInfo dirInfo;

        /// <summary>
        /// The default extensions to match for child files
        /// </summary>
        protected const string defaultSearchPatterns = @"jpg$|jpeg$|bmp$|gif$|png$";
        
        /// <summary>
        /// Construct a new NavTreeFolder, on the given folderRoot
        /// </summary>
        /// <param name="folderRoot">A folder to use as the NavItem. This folder should exist on disk</param>        
        public NavTreeFolder(string ItemFullPath)
            : base()
        {
            this.FileMatchPattern = defaultSearchPatterns;
            this.ItemFullPath = ItemFullPath;
            this.dirInfo = new DirectoryInfo(this.ItemFullPath);            
        }        

        /// <summary>
        /// Construct a new NavTreeFolder, on the given folderRoot and with a single fileSearchPattern.
        /// </summary>
        /// <param name="folderRoot">A folder to use as the NavItem. This folder should exist on disk</param>
        /// <param name="searchPattern">A search regex used to check child files in this folder</param>
        [JsonConstructor]
        public NavTreeFolder(string ItemFullPath, string FileMatchPattern)
            : this(ItemFullPath)
        {
            this.FileMatchPattern = FileMatchPattern;
        }

        /// <summary>
        /// Regex patterns used to check 'ChildItem' files in this folder. Matching follows
        /// 'OR' logic, any file in the folder that matches will be included
        /// </summary>
        public string FileMatchPattern
        {
            get;

            protected set;
        }

        /// <summary>
        /// The name of the folder
        /// </summary>        
        [JsonIgnore]
        public override string ItemName
        {
            get
            {
                return dirInfo.Name;
            }            
        }

        /// <summary>
        /// The full path to the item.
        /// </summary>
        public override string ItemFullPath
        {
            get;
            set;
        }
        
        [JsonIgnore]
        public Catalog CatalogRef
        {
            get;
            set;
        }

        /// <summary>
        /// Child Folders in this folder. Child folders will have
        /// identical searchPattern logic to the parent.
        /// </summary>
        [JsonIgnore]
        public override List<NavTreeItemBase> ChildContainers
        {
            get
            {
                List<NavTreeItemBase> toReturn = new List<NavTreeItemBase>();
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
                    if(!this.IsAccessAuthorized(dir))
                    {
                        continue;
                    }
                    NavTreeFolder newItem = new NavTreeFolder(dir.FullName, this.FileMatchPattern);
                    toReturn.Add(newItem);
                }

                return toReturn;
            }

            protected set
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// The items contained in this NavItem. here just 
        /// display all files found matching searchPatterns.
        /// </summary>
        [JsonIgnore]
        public override List<DisplayItem> ChildItems
        {
            get
            {
                List<DisplayItem> toReturn = new List<DisplayItem>();
                FileInfo[] files = new FileInfo[] { };
                
                try
                {
                    files = this.dirInfo.GetFiles();
                }
                catch (Exception)
                {
                    throw;
                }

                foreach (FileInfo file in files)
                {
                    if(Regex.IsMatch(file.Name, this.FileMatchPattern))
                    {
                        DisplayItem newItem = new DisplayItem(file.FullName);

                        newItem.Properties = this.CatalogRef != null ? 
                            this.CatalogRef.GetDisplayItemProperties(file.FullName) : 
                            Catalog.DefaultDisplayItemProperties();

                        newItem.RunTimeProperties = new DisplayItemRuntimeProperties(file.FullName);
                            
                        toReturn.Add(newItem);
                    }
                }

                return toReturn;
            }
        }

        private bool IsAccessAuthorized(DirectoryInfo folderPath)
        {
            try
            {
                // Attempt to get a list of security permissions from the folder. 
                // This will raise an exception if the path is read only or do not have access to view the permissions. 
                DirectoryInfo[] directories = folderPath.GetDirectories();
                return true;
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }
        }
    }
}
