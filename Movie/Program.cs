using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Reflection;
using System.IO;

namespace Movie
{
    class Program
    {
        private static int ShowMenu()
        {

            int choice;

            Console.WriteLine("My watched movies");
            Console.WriteLine("1 . Enter a movie");
            Console.WriteLine("2 . List all movies");
            Console.WriteLine("3 . Export");
            Console.WriteLine("4 . Exit");
            Console.WriteLine("Enter your choice and press enter key");

            string input = Console.ReadLine();
            Int32.TryParse(input, out choice);

            return choice;

        }

        private static int ShowMenuAndGetChoice()
        {
            int choice = ShowMenu();
            while (choice < 1 || choice > 4)
            {
                Console.WriteLine("Invalid choice");
                choice = ShowMenu();
            }
            return choice;
        }

        private static XElement AddMovie(MovieDetailsSpec spec)
        {
            return spec.EnterAMovie();
        }

        private static void DisplayAll(List<XElement> movies)
        {
            movies.Select(Helper.parse).ForEach(Console.WriteLine);
        }


        private static void PromptAndExport(List<XElement> movies)
        {
            string rootPath = ConfigurationManager.AppSettings["ExportRootFolderPath"];

                 #region debug
            StringBuilder sb = new StringBuilder();
            movies.ForEach(t => sb.AppendLine(t.ToString()));
            File.WriteAllText(Path.Combine(rootPath, "sample.txt"),sb.ToString());


            #endregion

            ExportMovieBase.RegisterAllExportHandlers();
            IEnumerable<IExportMovie> handlers = ExportMovieBase.GetAllHandlers().Select(t => t());
            int choice;

            choice = ShowExportMenu(handlers);
            while (choice < 1 || choice > handlers.Count() + 1)
            {
                Console.WriteLine("Invalid choice");
                choice = ShowExportMenu(handlers);
            }
            bool bValid = true;
            
            string path;
            do
            {
                bValid = true;
                Console.WriteLine("Enter file path");
                
                Console.WriteLine(string.Format( "Configured root folder is {0} . if you want to save relative to it start with '/'",rootPath));
                 path = Console.ReadLine();
                if (path.IndexOf('/') == 0)
                    path = Path.Combine(rootPath, path.Substring(1));

                System.IO.FileInfo fi = null;
                try
                {
                    fi = new System.IO.FileInfo(path);
                }
                catch (Exception e)
                {
                    Console.WriteLine("path threw Exception " + e.ToString());
                    bValid = false;
                }

                if (bValid && ReferenceEquals(fi, null))
                {

                    Console.WriteLine(" file name is not valid");
                    bValid = false;
                }
            } while (!bValid);
            try
            {
                handlers.ElementAt(choice - 1).Export(movies, path);
                Console.WriteLine("Exported successfully to " + path);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }



        }

        private static int ShowExportMenu(IEnumerable<IExportMovie> handlers)
        {
            int choice;
            Console.WriteLine("Choose how you want to export");
            handlers.Select((t, i) => string.Format("{0} . {1}", i + 1, t.DisplayText)).ForEach(Console.WriteLine);
            string input = Console.ReadLine();
            Int32.TryParse(input, out choice);

            return choice;
        }

        private static void TakeActionBasedOnChoice(int choice, MovieDetailsSpec spec, List<XElement> movies)
        {
            switch (choice)
            {
                case 1: movies.Add(AddMovie(spec)); break;
                case 2: DisplayAll(movies); break;
                case 3: PromptAndExport(movies); break;
            }
        }


        private static MovieDetailsSpec LoadMovieDetailsSpec()
        {

            return MovieDetailsSpec.LoadMovieDetailsSpecFromXml(ConfigurationManager.AppSettings["MovieDetailsFilePath"]);
        }
        static void Main(string[] args)
        {
            MovieDetailsSpec spec = LoadMovieDetailsSpec();
            List<XElement> movies = new List<XElement>();
            int choice;
            do
            {
                choice = ShowMenuAndGetChoice();
                TakeActionBasedOnChoice(choice, spec, movies);


            } while (choice != 4);

        }
    }
}
