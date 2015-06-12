using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ImageCatalog;

namespace ImageCatalogTests
{
    [TestClass]
    public class ImageCatalogUnitTests
    {
        [TestMethod]
        public void CheckGetTags()
        {
            Catalog cat = new Catalog();
            cat.TagFile("file1", "cat");
            cat.TagFile("file1", "dog");
            cat.TagFile("file1", "cat");
            cat.TagFile("file2", "cat");
            cat.TagFile("file2", "hippo");

            List<string> tags = new List<string>(cat.GetTagsOnFile("file1"));
            Assert.IsTrue(tags.Count == 2);
            Assert.IsTrue(tags.Contains("cat"));
            Assert.IsTrue(tags.Contains("dog"));

            List<string> files = new List<string>(cat.GetFilesWithTag("cat"));
            Assert.IsTrue(files.Count == 2);
            Assert.IsTrue(files.Contains("file1"));
            Assert.IsTrue(files.Contains("file2"));

            files = new List<string>(cat.GetFilesWithTag("hippo"));
            Assert.IsTrue(files.Count == 1);            
            Assert.IsTrue(files.Contains("file2"));
        }

        [TestMethod]
        public void CheckRemoveTags()
        {
            Catalog cat = new Catalog();
            cat.TagFile("file1", "cat");
            cat.TagFile("file1", "dog");
            cat.TagFile("file1", "cat");
            cat.TagFile("file2", "cat");
            cat.TagFile("file2", "hippo");
            cat.UnTagFile("file1", "cat");

            List<string> tags = new List<string>(cat.GetTagsOnFile("file1"));
            Assert.IsTrue(tags.Count == 1);            
            Assert.IsTrue(tags.Contains("dog"));

            List<string> files = new List<string>(cat.GetFilesWithTag("cat"));
            Assert.IsTrue(files.Count == 1);            
            Assert.IsTrue(files.Contains("file2"));            
        }

        [TestMethod]
        public void SetGetRemoveRating()
        {
            Catalog cat = new Catalog();

            Assert.IsTrue(cat.GetDisplayItemProperties("file1").Rating == null);
            
            cat.RateFile("file1", 3);
            Assert.IsTrue(cat.GetDisplayItemProperties("file1").Rating == 3);

            cat.ClearRating("file1");
            Assert.IsTrue(cat.GetDisplayItemProperties("file1").Rating == null);

            cat.RateFile("file1", 6);
            Assert.IsTrue(cat.GetDisplayItemProperties("file1").Rating == null);
        }
    }
}
