using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ImageCatalogTests
{
    using ImageCatalog;

    [TestClass]
    public class JsonSaveLoadTests
    {
        public List<DirectoryInfo> testDirs;

        public JsonSaveLoad jsonSaveLoad;
        
        [TestInitialize]
        public void Init()
        {
            CleanTestDirs();
            testDirs = new List<DirectoryInfo>();
            testDirs.Add(Directory.CreateDirectory(@"TestDir"));
            testDirs.Add(Directory.CreateDirectory(@"TestDir\Dir1"));
            testDirs.Add(Directory.CreateDirectory(@"TestDir\Dir2\SubDir"));

            File.Create(@"TestDir\Dir1\example.jpg").Close();
            File.Create(@"TestDir\Dir1\example.txt").Close();
            File.Create(@"TestDir\Dir2\SubDir\example.bmp").Close();

             jsonSaveLoad = new JsonSaveLoad();
        }

        [TestCleanup]
        public void CleanUp()
        {
            CleanTestDirs();
        }

        private void CleanTestDirs()
        {
            if(Directory.Exists("TestDir"))
            {
                Directory.Delete("TestDir", true);
            }
        }

        [TestMethod]
        public void SaveLoadNavTreeFolder()
        {            
            NavTreeFolder navTreeOriginal = new NavTreeFolder(@"TestDir");
            
            jsonSaveLoad.Save<NavTreeFolder>("path.json", navTreeOriginal);
            navTreeOriginal = null;
            NavTreeFolder navTreeLoaded = jsonSaveLoad.Load<NavTreeFolder>("path.json");

            List<NavTreeItemBase> subFolders = navTreeLoaded.ChildContainers;
            Assert.IsTrue(subFolders.Count == 2);
            List<DisplayItem> items = navTreeLoaded.ChildItems;
            Assert.IsTrue(items.Count == 0);
            
            Assert.IsTrue(subFolders[0].ChildContainers.Count == 0);
            Assert.IsTrue(subFolders[0].ChildItems.Count == 1);
            Assert.IsTrue(subFolders[0].ChildItems[0].ItemName.Equals("example.jpg"));
            
            Assert.IsTrue(subFolders[1].ChildContainers.Count == 1);
            Assert.IsTrue(subFolders[1].ChildItems.Count == 0);

            Assert.IsTrue(subFolders[1].ChildContainers[0].ChildContainers.Count == 0);
            Assert.IsTrue(subFolders[1].ChildContainers[0].ChildItems.Count == 1);
            Assert.IsTrue(subFolders[1].ChildContainers[0].ChildItems[0].ItemName.Equals("example.bmp"));
        }

        [TestMethod]
        public void SaveLoadNavTreeFolderSearchPatterns()
        {
            string searchPattern = "z/x";

            NavTreeFolder navTreeOriginal = new NavTreeFolder(@"TestDir", searchPattern);

            jsonSaveLoad.Save<NavTreeFolder>("path.json", navTreeOriginal);
            navTreeOriginal = null;
            NavTreeFolder navTreeLoadedFirst = jsonSaveLoad.Load<NavTreeFolder>("path.json");

            Assert.IsTrue(navTreeLoadedFirst.FileMatchPattern == searchPattern);
        }

        [TestMethod]
        public void SaveLoadCatalog()
        {
            Catalog originalCatalog = new Catalog();
            originalCatalog.TagFile("file1", "cat");
            originalCatalog.TagFile("file1", "dog");
            originalCatalog.TagFile("file1", "cat");
            originalCatalog.TagFile("file2", "cat");
            originalCatalog.TagFile("file2", "hippo");
            
            jsonSaveLoad.Save<Catalog>("path.json", originalCatalog);
            originalCatalog = null;

            Catalog loadedCatalog = jsonSaveLoad.Load<Catalog>("path.json");

            List<string> tags = new List<string>(loadedCatalog.GetTagsOnFile("file1"));
            Assert.IsTrue(tags.Count == 2);
            Assert.IsTrue(tags.Contains("cat"));
            Assert.IsTrue(tags.Contains("dog"));

            List<string> files = new List<string>(loadedCatalog.GetFilesWithTag("cat"));
            Assert.IsTrue(files.Count == 2);
            Assert.IsTrue(files.Contains("file1"));
            Assert.IsTrue(files.Contains("file2"));

            files = new List<string>(loadedCatalog.GetFilesWithTag("hippo"));
            Assert.IsTrue(files.Count == 1);
            Assert.IsTrue(files.Contains("file2"));            
        }

        [TestMethod]
        public void SaveLoadTagRegister()
        {
            TagRegister original = new TagRegister();
            original.Register("Cat", new DisplayItemProperties("Item1"));
            original.Register("Dog", new DisplayItemProperties("Item1"));
            original.Register("Dog", new DisplayItemProperties("Item2"));

            jsonSaveLoad.Save<TagRegister>("TagRegister.Json", original);

            original = null;

            TagRegister loaded = jsonSaveLoad.Load<TagRegister>("TagRegister.Json");
            List<DisplayItemProperties> propsCat = new List<DisplayItemProperties>(loaded.Get("Cat"));
            List<DisplayItemProperties> propsDog = new List<DisplayItemProperties>(loaded.Get("Dog"));
            Assert.IsTrue(propsCat.Count == 1);
            Assert.IsTrue(propsCat[0].ItemFullPath == "Item1");
            
            Assert.IsTrue(propsDog.Count == 2);
            Assert.IsTrue(propsDog[0].ItemFullPath == "Item1");
            Assert.IsTrue(propsDog[1].ItemFullPath == "Item2");
        }

        [TestMethod]
        public void SaveLoadDisplayItemProperty()
        {
            DisplayItemProperties propsOriginal = new DisplayItemProperties("FooPath");
            propsOriginal.Rating = 2;
            propsOriginal.AddTag("Cat");
            propsOriginal.AddTag("Dog");

            jsonSaveLoad.Save<DisplayItemProperties>("DisplayItemProperties.Json", propsOriginal);

            propsOriginal = null;
            DisplayItemProperties propsLoaded = jsonSaveLoad.Load<DisplayItemProperties>("DisplayItemProperties.Json");

            Assert.IsTrue(propsLoaded.Rating == 2);
            List<string> loadedTags = new List<string>(propsLoaded.Tags);
            Assert.IsTrue(loadedTags.Count == 2);
            Assert.IsTrue(loadedTags.Contains("Cat"));
            Assert.IsTrue(loadedTags.Contains("Dog"));
            Assert.IsTrue(propsLoaded.ItemFullPath == "FooPath");
        }
    }
}
