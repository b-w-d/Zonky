using System;
using System.Text;
using System.IO;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ImageCatalogTests
{
    using ImageCatalog;
    using PegView.ViewModel;

    /// <summary>
    /// Summary description for NavFolderViewModel
    /// </summary>
    [TestClass]
    public class NavFolderViewModelTests
    {
        private TestContext testContextInstance;

        public List<DirectoryInfo> testDirs;

        public JsonSaveLoad jsonSaveLoad;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

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
            if (Directory.Exists("TestDir"))
            {
                Directory.Delete("TestDir", true);
            }
        }

        [TestMethod]
        public void CheckChildContainers()
        {
            NavTreeFolder folderBase = new NavTreeFolder(@"TestDir\Dir1");
            NavFolderViewModel navVm = new NavFolderViewModel(folderBase);

            Assert.IsTrue(navVm.ChildItems.Count == 1);
            Assert.IsTrue(navVm.ChildItems[0].ItemName == "example.jpg");
            Assert.IsTrue(navVm.ChildItems[0].ItemFullPath == Path.Combine(Environment.CurrentDirectory, @"TestDir\Dir1\example.jpg"));
        }
    }
}
