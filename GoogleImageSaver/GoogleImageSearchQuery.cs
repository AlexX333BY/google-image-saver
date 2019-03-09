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
            if (SearchQuery == null)
            {
                throw new TypeInitializationException(nameof(GoogleImageSearchQuery), null);
            }
            imageUrls = new List<string>();

            if (DesiredResultsCount == 0)
            {
                return;
            }

            string pageUrl;
            bool hadPreviousSearchResult = true;
            HtmlWeb web = new HtmlWeb();
            HtmlDocument document;
            IEnumerable<string> singlePageResult;

            for (int curPage = 0; hadPreviousSearchResult && ((curPage * pageResultsCount) < DesiredResultsCount); ++curPage)
            {
                pageUrl = string.Format(@"https://www.google.com/search?q={0}&tbm=isch&start={1}", SearchQuery, curPage * pageResultsCount);
                document = web.Load(pageUrl);
                singlePageResult = document.DocumentNode.SelectNodes(@"//table[@class='images_table']//img")
                    .Select((node) => node.Attributes["src"].Value);
                if (singlePageResult.Count() > 0)
                {
                    hadPreviousSearchResult = true;
                    imageUrls = imageUrls.Concat(singlePageResult);
                }
                else
                {
                    hadPreviousSearchResult = false;
                }
            }
            imageUrls = imageUrls.Take(DesiredResultsCount);
        }

        public void SaveImages()
        {
            throw new System.NotImplementedException();
        }

        protected string searchQuery, saveFolderPath;
        protected int desiredResultsCount;
        protected IEnumerable<string> imageUrls;
        protected const byte pageResultsCount = 20;
    }
}

