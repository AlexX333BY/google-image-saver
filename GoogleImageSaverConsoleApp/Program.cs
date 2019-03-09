using System;

namespace GoogleImageSaver
{
    class Program
    {
        static void PrintUsage()
        {
            Console.WriteLine("usage: {0} <search query> <desired results count> [<save folder path>]", AppDomain.CurrentDomain.FriendlyName);
        }

        static void Main(string[] args)
        {
            if (args.Length < 2) 
            {
                PrintUsage();
                return;
            }

            IImageSearchQuery query = new GoogleImageSearchQuery();
            try
            {
                query.SearchQuery = args[0];
            }
            catch (Exception e)
            {
                Console.WriteLine("Search query error: " + e.Message);
                return;
            }

            try
            {
                query.DesiredResultsCount = int.Parse(args[1]);
            }
            catch (Exception e)
            {
                Console.WriteLine("Results count error: " + e.Message);
                return;
            }

            if (args.Length > 2)
            {
                try
                {
                    query.SaveFolderPath = args[2];
                }
                catch (Exception e)
                {
                    Console.WriteLine("Save path error: " + e.Message);
                    return;
                }
            }

            try
            {
                query.ProcessQuery();
                query.SaveImages();
            }
            catch (AggregateException aggregate)
            {
                Console.WriteLine("Errors saving files:");
                foreach (Exception inner in aggregate.InnerExceptions)
                {
                    Console.WriteLine(inner);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Query processing error: " + e.Message);
            }
        }
    }
}
