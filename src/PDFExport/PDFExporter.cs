using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using NClass.DiagramEditor;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PDFExport.Lang;

namespace PDFExport
{
  public class PDFExporter
  {
    // ========================================================================
    // Attributes

    #region === Attributes

    /// <summary>
    /// The location where to save the created PDF.
    /// </summary>
    private readonly string fileName;

    /// <summary>
    /// The diagram to export to the PDF.
    /// </summary>
    private readonly IDocument nclassDocument;

    /// <summary>
    /// True if only the selected elements should be exported.
    /// </summary>
    private readonly bool selectedOnly;

    /// <summary>
    /// The size of the border arround the diagram.
    /// </summary>
    private readonly Padding padding;

    #endregion

    // ========================================================================
    // Con- / Destruction

    #region === Con- / Destruction

    /// <summary>
    /// Initializes a new instance of <c>PDFExporter</c>.
    /// </summary>
    /// <param name="fileName">The location where to save the created PDF.</param>
    /// <param name="nclassDocument">The diagram to export to the PDF.</param>
    /// <param name="selectedOnly">True if only the selected elements should be exported.</param>
    /// <param name="padding">The size of the border arround the diagram.</param>
    public PDFExporter(string fileName, IDocument nclassDocument, bool selectedOnly, Padding padding)
    {
      this.fileName = fileName;
      this.nclassDocument = nclassDocument;
      this.selectedOnly = selectedOnly;
      this.padding = padding;
      Successful = false;
    }

    #endregion

    // ========================================================================
    // Properties

    #region === Properties

    /// <summary>
    /// Gets a value indicating whether the export was done successfuly.
    /// </summary>
    public bool Successful { get; set; }

    #endregion

    // ========================================================================
    // Methods

    #region === Methods

    /// <summary>
    /// Exports the given diagram into a PDF. The PDF is saved at the location
    /// given in <see cref="fileName"/>. If selectedOnly is true, only the selected
    /// elements of the diagram get exported.
    /// </summary>
    public void Export()
    {
      PdfDocument document = new PdfDocument();
      PdfPage page = document.AddPage();
      RectangleF diagramSize = nclassDocument.GetPrintingArea(selectedOnly);
      page.Width = new XUnit(diagramSize.Width, XGraphicsUnit.Presentation) + new XUnit(padding.Right*2);
      page.Height = new XUnit(diagramSize.Height, XGraphicsUnit.Presentation) + new XUnit(padding.Bottom*2);

      XGraphics gfx = XGraphics.FromPdfPage(page);
      PDFGraphics graphics = new PDFGraphics(gfx);

      //Translate because of the padding.
      graphics.TranslateTransform(padding.Left, padding.Top);

      //Do some scaling to get from pixels to dots...
      Graphics gdiGraphics = (new Control()).CreateGraphics();
      graphics.ScaleTransform(72.0f/gdiGraphics.DpiX, 72.0f/gdiGraphics.DpiY);
      //Fonts are already mesured in dots but the size of them is also scaled by the above
      //transformation. So we have to applay an opposite transformation on the fonts first.
      graphics.ScaleFont = gdiGraphics.DpiY/72.0f;
      //PDF-Sharp also does the scaling for the images itself. So we have to revert this as
      //we did it for the fonts.
      graphics.ScaleImage = gdiGraphics.DpiX/72.0f;

      //Move everything to the top left corner
      graphics.TranslateTransform(-diagramSize.X, -diagramSize.Y);

      graphics.TakeInitialTransform();

      nclassDocument.Print(graphics, selectedOnly, Style.CurrentStyle);

      //Clean up...
      while(gfx.GraphicsStateLevel > 0)
      {
        gfx.Restore();
      }

      document.Options.CompressContentStreams = true;
      try
      {
        document.Save(fileName);
        Successful = true;
      }
      catch(IOException e)
      {
        MessageBox.Show(String.Format(Strings.Error_CoulNotWritePDF, e.Message), Strings.ErrorDialog_Title,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    #endregion
  }
}