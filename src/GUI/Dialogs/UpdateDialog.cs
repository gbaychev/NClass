using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using NClass.Translations;

namespace NClass.GUI.Dialogs
{
    public partial class UpdateDialog : Form
    {
        public UpdateDialog(string caption, string releaseName, string releaseDescription)
        {
            InitializeComponent();
            var defaulHtmlStyle = PrepareDefaultHtmlStyle();

            this.Text = caption;
            this.webBrowser.DocumentText = defaulHtmlStyle + " " + releaseDescription;
            this.webBrowser.Navigating += WebBrowserOnNavigating;
            this.lblNewUpdate.Text = $"{Strings.NewVersion}: {releaseName}";
            this.lblDownload.Text = Strings.ProgramDownload;
        }

        private static void WebBrowserOnNavigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            //cancel the current event
            e.Cancel = true;

            //this opens the URL in the user's default browser
            Process.Start(e.Url.ToString());
        }

        private string PrepareDefaultHtmlStyle()
        {
            var defaultFont = SystemFonts.DefaultFont;
            var defaultColor = SystemColors.Control;

            return $"<style> body {{background-color: #{defaultColor.R:X}{defaultColor.G:X}{defaultColor.B:X};font-family: {defaultFont.FontFamily.Name} }} </style>";
        }
    }
}
