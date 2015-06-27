using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Drawing;

namespace ImageCatalog
{
    /// <summary>
    /// Class that holds information about files determined at runtime. 
    /// This can be the image size, resolution, or the image itself.
    /// 
    /// Idea is to 
    ///     1.) prevent any unnecessary saving of objects
    ///     2.) load run time info asynchrous
    /// </summary>
    public class DisplayItemRuntimeProperties
    {
        /// <summary>
        /// The image that is loaded
        /// </summary>
        protected Image imageInternal;

        /// <summary>
        /// The task to assign the image. This wraps a number of other tasks,
        /// including the disk read, Image.Create. On completion, these tasks are complete
        /// and the imageInternal variable is assigned to
        /// </summary>
        protected Task imageLoadTask;

        /// <summary>
        /// Full path to the image
        /// </summary>
        protected string itemFullPath;

        /// <summary>
        /// The number of times a byte[] has been read from disk
        /// </summary>
        protected static int byteCount;

        /// <summary>
        /// The number of times an Image has been created from a byte[].
        /// </summary>
        protected static int imageLoadCount;
        
        /// <summary>
        /// Initialize a new DisplayItemRunTimeProperty, and begin the process of 
        /// loading the image asynchronously.
        /// </summary>
        /// <param name="ItemFullPath">The path to the item</param>
        public DisplayItemRuntimeProperties(string ItemFullPath)
        {
            this.ItemFullPath = ItemFullPath;
                       
            this.imageLoadTask = this.GetFileStreamAsync(ItemFullPath)
            .ContinueWith((antecedent) => 
            {
                Interlocked.Increment(ref imageLoadCount);                
                return this.GetImageLoadTask(antecedent.Result);
            })
            .ContinueWith((antecedent) =>
            {
                this.imageInternal = antecedent.Result.Result;
            });
        }

        /// <summary>
        /// The path to the item
        /// </summary>
        public string ItemFullPath
        {
            get
            {
                return this.itemFullPath;
            }

            set
            {
                this.itemFullPath = value;
            }
        }

        /// <summary>
        /// The number of times a byte[] has been read from disk
        /// </summary>
        public int ByteArrayTaskAccessCount
        {
            get
            {
                return byteCount;
            }
        }

        /// <summary>
        /// The number of times an Image has been created from a byte[].
        /// </summary>
        public static int ImageLoadTaskAccessCount
        {
            get
            {
                return imageLoadCount;
            }
        }

        /// <summary>
        /// Returns true if an item is found, false otherwise
        /// </summary>
        public bool ItemFound
        {
            get
            {
                return (File.Exists(this.ItemFullPath));
            }
        }

        /// <summary>
        /// The number of bytes read from the item
        /// </summary>
        public int BitsRead
        {
            get;
            set;
        }

        /// <summary>
        /// The Image loaded
        /// </summary>
        public Image ImageProperties
        {
            get
            {
                if(this.imageInternal == null)
                {
                    this.imageLoadTask.Wait();
                }
                
                return this.imageInternal;
            }
        }
        
        /// <summary>
        /// Loading a file into UI has 2 parts :
        ///     1. Reading the file from disk (IO Bound, returns byte[])
        ///     2. Creating a stream from byte[] and creating an Image from the stream (CPU bound)
        ///     
        /// This method handles part 1 and should efficiently handle the IO portion of accessing the files. 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private async Task<byte[]> GetFileStreamAsync(string path)
        {
            byte[] result;

            using (FileStream source = File.Open(path, FileMode.Open))
            {
                result = new byte[source.Length];
                
                // ReadAsync returns a Task<int>. That means that when you await the task
                // you'll get an int. 
                Task<int> getBitsTask = source.ReadAsync(result, 0, (int)source.Length);                

                // You can do work here that doesn't rely on the Task<int>
                Interlocked.Increment(ref byteCount);

                // The await operation suspends this method.
                // - The method can't continue until getBitsTask completes
                // - Meanwhile, control returns to the caller of GetFileStreamAsync
                // - Control resumes here when getBitsTask is done
                // - The await operator then retrieve the bits result from getBitsTask
                int bits = await getBitsTask;
                
                this.BitsRead = bits;
            }

            // The return statement specifies a byte array
            // Any methods that are awaiting GetFileStreamAsync retrieve the byte[] value
            return result;
        }
        
        /// <summary>
        /// Loading a file into UI has 2 parts :
        ///     1. Reading the file from disk (IO Bound, returns byte[])
        ///     2. Creating a stream from byte[] and creating an Image from the stream (CPU bound)
        ///            
        /// This handles part 2 : CPU intensive part of loading an image, which seems to be creating a memory
        /// stream from Byte[], and loading the Image from a stream. Async/Await is not necessary here.
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        private Task<Image> GetImageLoadTask(Byte[] array)
        {            
            Task<Image> loadTask = Task<Image>.Run(() =>
            {
                MemoryStream stream = new MemoryStream(array);
                return Image.FromStream(stream);
            });

            return loadTask;
        }
    }         
}
