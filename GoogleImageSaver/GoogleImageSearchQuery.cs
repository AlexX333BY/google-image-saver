using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HtmlAgilityPack;

namespace GoogleImageSaver
{
    public class GoogleImageSearchQuery : IImageSearchQuery
    {
        public GoogleImageSearchQuery()
        {
            desiredResultsCount = 0;
            SaveFolderPath = string.Empty;
            searchQuery = null;
            imageUrls = null;
        }

        public string SearchQuery 
        { 
            get => searchQuery; 
            set {
                string trimmedQuery = value?.Trim();
                if ((trimmedQuery != null) && (trimmedQuery.Length != 0))
                {
                    searchQuery = trimmedQuery;
                }
                else
                {
                    throw new ArgumentException("Query shouldn't be empty", nameof(value));
                }
            } 
        }

        public int DesiredResultsCount
        { 
            get => desiredResultsCount;
            set {
                if (value > 0)
                {
                    desiredResultsCount = value;
                }
                else
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }
            }
        }

        public string SaveFolderPath { 
            get => saveFolderPath; 
            set {
                string absolutePath = Path.GetFullPath(((value == null) || (value.Trim().Length == 0)) ? "." : value.Trim());
                
                if (!File.Exists(absolutePath))
                {
                    Directory.CreateDirectory(absolutePath);
                    saveFolderPath = absolutePath;
                }
                else
                {
                    throw new ArgumentException(string.Format("File with name {0} already exists", absolutePath), nameof(value));
                }
            } 
        }

        public IList<string> ImagesUrls => imageUrls?.ToList();

        public void ProcessQuery()
        {
            throw new System.NotImplementedException();
        }

        public void SaveImages()
        {
            throw new System.NotImplementedException();
        }

        protected string searchQuery, saveFolderPath;
        protected int desiredResultsCount;
        protected IEnumerable<string> imageUrls;
    }
}

