using System.Collections.Generic;

namespace GoogleImageSaver
{
    public interface IImageSearchQuery
    {
        string SearchQuery
        { get; set; }

        int DesiredResultsCount
        { get; set; }

        string SaveFolderPath
        { get; set; }

        IList<string> ImagesUrls
        { get; }

        void ProcessQuery();

        void SaveImages();
    }
}

