using System.Windows.Forms;
using PDFExport.Lang;

namespace PDFExport
{
  /// <summary>
  /// A form which gets displayed after the PDF-Export has finished.
  /// </summary>
  public partial class PDFExportFinished : Form
  {
    /// <summary>
    /// Initializes a new instance of PDFExportFinished.
    /// </summary>
    public PDFExportFinished()
    {
      InitializeComponent();

      LocalizeComponents();
    }

    /// <summary>
    /// Displays the text for the current culture.
    /// </summary>
    private void LocalizeComponents()
    {
      Text = Strings.FinishedDialog_Title;
      lblFinished.Text = Strings.FinishedDialog_Text;
      cmdOpen.Text = Strings.FinishedDialog_Open;
      cmdClose.Text = Strings.FinishedDialog_Close;
    }
  }
}
