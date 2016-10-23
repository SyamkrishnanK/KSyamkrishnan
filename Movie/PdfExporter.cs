using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movie
{
    public class PdfExporter : ExportMovieBase
    {
        private PdfExporter()
        { }
        [RegisterExportMovie]
        public static void RegisterPlainTextExporter()
        {
            RegisterExportMovieHandler("pdf", () => new PdfExporter());
        }


        public override string DisplayText
        {
            get { return "Export as pdf"; }
        }

        public override void Export(IEnumerable<System.Xml.Linq.XElement> movies, string path)
        {
       


            IEnumerable<string> strMovies = movies.Select(Helper.parse);
           
           

            PdfDocument pdf = new PdfDocument();
            pdf.Info.Title = "My Watched Movie List";

            foreach (var sb in strMovies)
            {
                IEnumerable<string> lines = sb.ToString().Split(new string[] { Environment.NewLine }, StringSplitOptions.None).Where(t => !string.IsNullOrEmpty(t));

                PdfPage pdfPage = pdf.AddPage();


                int yPoint = 0;

                XGraphics graph = XGraphics.FromPdfPage(pdfPage);
                XFont font = new XFont("Verdana", 20, XFontStyle.Regular);
                foreach (var v in lines)
                {
                    graph.DrawString(v, font, XBrushes.Black, new XRect(40, yPoint, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    yPoint = yPoint + 40;
                }
            }
            //string pdfFilename = "txttopdf2.pdf";
            pdf.Save(path);


        }
    }
}
