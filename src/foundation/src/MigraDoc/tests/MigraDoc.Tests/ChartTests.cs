// MigraDoc - Creating Documents on the Fly
// See the LICENSE file in the solution root for more information.

using System.Diagnostics;
using PdfSharp.Pdf;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Fields;
using MigraDoc.DocumentObjectModel.Shapes.Charts;
using MigraDoc.Rendering;
using PdfSharp.Fonts;
using PdfSharp.Snippets.Font;
using PdfSharp.TestHelper;
using Xunit;

namespace MigraDoc.Tests
{
    [Collection("MGD")]
    public class ChartTests
    {
        [Fact]
        public void Create_MigraDoc_Chart_Test()
        {
#if CORE
            GlobalFontSettings.FontResolver = SnippetsFontResolver.Get();
#endif

            // Create a MigraDoc document.
            var document = CreateDocument();

            // ----- Unicode encoding in MigraDoc is demonstrated here. -----

            //// A flag indicating whether to create a Unicode PDF or a WinAnsi PDF file.
            //// This setting applies to all fonts used in the PDF document.
            //// This setting has no effect on the RTF renderer.
            //const bool unicode = false;

            // Create a renderer for the MigraDoc document.
            var pdfRenderer = new PdfDocumentRenderer()
            {
                // Associate the MigraDoc document with a renderer.
                Document = document
            };

            // Layout and render document to PDF.
            pdfRenderer.RenderDocument();

            // Save the document...
            var filename = PdfFileHelper.CreateTempFileName("HelloWorld");
            pdfRenderer.PdfDocument.Save(filename);
            // ...and start a viewer.
            PdfFileHelper.StartPdfViewerIfDebugging(filename);

#if DEBUG___
            MigraDoc.DocumentObjectModel.IO.DdlWriter dw = new MigraDoc.DocumentObjectModel.IO.DdlWriter(filename + "_2.mdddl");
            dw.WriteDocument(document);
            dw.Close();
#endif
        }

        /// <summary>
        /// Creates an absolutely minimalistic document.
        /// </summary>
        static Document CreateDocument()
        {
            // Create a new MigraDoc document.
            var document = new Document();

            // Add a section to the document.
            var section = document.AddSection();

            // Add a paragraph to the section.
            var paragraph = section.AddParagraph();

            // Set font color.
            //paragraph.Format.Font.Color = Color.FromCmyk(100, 30, 20, 50);
            paragraph.Format.Font.Color = Colors.DarkBlue;

            // Add some text to the paragraph.
            paragraph.AddFormattedText("Hello, World!", TextFormat.Bold);

            // Line Chart from the Chart-Layout GBE sample.
            var chart = new Chart(ChartType.Line);
            chart.DisplayBlanksAs = BlankType.NotPlotted;
            chart.Height = 185;
            chart.Width = 485;
            chart.FillFormat.Color = Colors.Lavender;
            chart.PlotArea.FillFormat.Color = Colors.Ivory;
            chart.XAxis.LineFormat.Width = 0.25;
            chart.YAxis.LineFormat.Width = 0.25;
            var series = chart.SeriesCollection.AddSeries();
            series.ChartType = ChartType.Line;
            series.Add(5838);
            series.Add(1681);
            series.Add(9136);
            series.Add(2586);
            series.Add(6125);
            series.AddBlank();
            series.AddBlank();
            series.Add(5123);
            series.Add(6486);
            series.Add(1235);

            series = chart.SeriesCollection.AddSeries();
            series.ChartType = ChartType.Line;
            series.Add(9874);
            series.Add(1894);
            series.Add(1895);
            series.Add(2188);
            series.Add(1776);
            series.Add(2189);
            series.Add(9813);
            series.Add(7321);
            series.Add(5874);
            series.Add(8564);

            section.Add(chart);

            // Create the primary footer.
            var footer = section.Footers.Primary;

            // Add content to footer.
            paragraph = footer.AddParagraph();
            paragraph.Add(new DateField { Format = "yyyy/MM/dd HH:mm:ss" });
            paragraph.Format.Alignment = ParagraphAlignment.Center;

            return document;
        }
    }
}
