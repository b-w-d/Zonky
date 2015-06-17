using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Threading.Tasks;

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
        /// List holding regex patterns. Each pattern is used to search valid file name patterns. 
        /// Recommend keeping short and simple.
        /// </summary>
        protected List<string> searchPatterns;
        
        /// <summary>
        /// Construct a new NavTreeFolder, on the given folderRoot
        /// </summary>
        /// <param name="folderRoot">A folder to use as the NavItem. This folder should exist on disk</param>
        public NavTreeFolder(string folderRoot) : base()
        {
            this.FileMatchPattern = new List<string>();
            this.ItemFullPath = folderRoot;
            this.FileMatchPattern = this.searchPatterns;
            this.dirInfo = new DirectoryInfo(this.ItemFullPath);
        }

        /// <summary>
        /// Construct a new NavTreeFolder, on the given folderRoot and with a single fileSearchPattern.
        /// </summary>
        /// <param name="folderRoot">A folder to use as the NavItem. This folder should exist on disk</param>
        /// <param name="searchPattern">A search regex used to check child files in this folder</param>
        public NavTreeFolder(string folderRoot, string searchPattern) : this(folderRoot)
        {
            this.searchPatterns.Add(searchPattern);
        }

        /// <summary>
        /// Regex patterns used to check 'ChildItem' files in this folder. Matching follows
        /// 'OR' logic, any file in the folder that matches will be included
        /// </summary>
        public List<string> FileMatchPattern
        {
            get
            {
                return this.searchPatterns;
            }
            
            protected set
            {
                this.searchPatterns = value;
            }
        }

        /// <summary>
        /// The name of the folder
        /// </summary>
        public new string ItemName
        {
            get
            {
                return dirInfo.Name;
            }            
        }

        /// <summary>
        /// Child Folders in this folder. Child folders will have
        /// identical searchPattern logic to the parent.
        /// </summary>
        public new List<NavTreeItemBase> ChildContainers
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
                    NavTreeFolder newItem = new NavTreeFolder(dir.FullName);
                    newItem.searchPatterns = this.searchPatterns;
                    toReturn.Add(newItem);
                }

                return toReturn;
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// The items contained in this NavItem. here just 
        /// display all files found matching searchPatterns.
        /// </summary>
        public new List<DisplayItem> ChildItems
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
                    foreach(string regexPattern in this.searchPatterns)
                    {
                        if(Regex.IsMatch(file.Name, regexPattern))
                        {
                            toReturn.Add(new DisplayItem(file.FullName));
                            goto nextOuterLoop; // :P
                        }
                    }

                nextOuterLoop: ; 
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
