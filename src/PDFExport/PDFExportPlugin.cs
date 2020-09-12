using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using NClass.DiagramEditor;
using NClass.DiagramEditor.Diagrams;
using NClass.GUI;
using NClass.GUI.Dialogs;
using PDFExport.Properties;
using PdfSharp.Drawing;
using Strings = PDFExport.Lang.Strings;

namespace PDFExport
{
    /// <summary>
    /// A plugin to export a diagram to a pdf.
    /// </summary>
    public class PDFExportPlugin : Plugin
    {
        // ========================================================================
        // Attributes

        #region === Attributes

        /// <summary>
        /// The menu item used to start the export.
        /// </summary>
        private readonly ToolStripMenuItem menuItem;

        #endregion

        // ========================================================================
        // Con- / Destruction

        #region === Con- / Destruction

        /// <summary>
        /// Set up the current culture for the strings.
        /// </summary>
        static PDFExportPlugin()
        {
            try
            {
                Strings.Culture = CultureInfo.GetCultureInfo(NClass.GUI.Settings.Default.UILanguage);
            }
            catch (ArgumentException)
            {
                //Culture is not supported, maybe the setting is "default".
            }
        }

        /// <summary>
        /// Constructs a new instance of PDFExportPlugin.
        /// </summary>
        /// <param name="environment">An instance of NClassEnvironment.</param>
        public PDFExportPlugin(NClassEnvironment environment)
          : base(environment)
        {
            menuItem = new ToolStripMenuItem
            {
                Text = Strings.Menu_Title,
                Image = Resources.Document_pdf_16,
                ToolTipText = Strings.Menu_ToolTip
            };
            menuItem.Click += menuItem_Click;
        }

        #endregion

        // ========================================================================
        // Event handling

        #region === Event handling

        /// <summary>
        /// Starts the export.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">Additional information.</param>
        private void menuItem_Click(object sender, EventArgs e)
        {
            Launch();
        }

        #endregion

        // ========================================================================
        // Properties

        #region === Properties

        /// <summary>
        /// Gets a value indicating whether the plugin can be executed at the moment.
        /// </summary>
        public override bool IsAvailable
        {
            get { return DocumentManager.HasDocument; }
        }

        /// <summary>
        /// Gets the menu item used to start the plugin.
        /// </summary>
        public override ToolStripItem MenuItem
        {
            get { return menuItem; }
        }

        #endregion

        // ========================================================================
        // Methods

        #region === Methods

        /// <summary>
        /// Starts the functionality of the plugin.
        /// </summary>
        protected void Launch()
        {
            if (!DocumentManager.HasDocument)
            {
                return;
            }

            string fileName;
            using (SaveFileDialog dialog = new SaveFileDialog())
            {
                dialog.Filter = Strings.SaveDialogFilter;
                dialog.RestoreDirectory = true;
                if (dialog.ShowDialog() == DialogResult.Cancel)
                {
                    return;
                }
                fileName = dialog.FileName;
            }

            PDFExportOptions optionsForm = new PDFExportOptions();
            if (optionsForm.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }
            Padding padding = new Padding((int)new XUnit(optionsForm.PDFPadding.Left, optionsForm.Unit).Point,
                                          (int)new XUnit(optionsForm.PDFPadding.Top, optionsForm.Unit).Point,
                                          (int)new XUnit(optionsForm.PDFPadding.Right, optionsForm.Unit).Point,
                                          (int)new XUnit(optionsForm.PDFPadding.Bottom, optionsForm.Unit).Point);

            MainForm mainForm = Application.OpenForms.OfType<MainForm>().FirstOrDefault();
            var g = mainForm.CreateGraphics();

            PDFExportProgress.ShowAsync(mainForm);

            PDFExporter exporter = new PDFExporter(fileName, DocumentManager.ActiveDocument, optionsForm.SelectedOnly, padding);
            Application.DoEvents();
            try
            {
                exporter.Export(g);
                // Running the exporter within an extra thread isn't a good idea since
                // the exporter uses GDI+. NClass uses GDI+ also if it has to redraw the
                // diagram. GDI+ can't be used by two threads at the same time.
                //Thread exportThread = new Thread(exporter.Export)
                //                        {
                //                          Name = "PDFExporterThread",
                //                          IsBackground = true
                //                        };
                //exportThread.Start();
                //while (exportThread.IsAlive)
                //{
                //  Application.DoEvents();
                //  exportThread.Join(5);
                //}
                //exportThread.Join();
                PDFExportProgress.CloseAsync();
            }
            catch (Exception e)
            {
                PDFExportProgress.CloseAsync();
                DetailsErrorDialog.Show("Error", $"Export failed: {e.Message}", e.StackTrace, MessageBoxIcon.Error, true);
            }

            if (!exporter.Successful) return;

            if (new PDFExportFinished().ShowDialog(mainForm) == DialogResult.OK)
            {
                try
                {
                    if (MonoHelper.IsRunningOnMono)
                    {
                        Process.Start("xdg-open", fileName);
                    }
                    else
                    {
                        Process.Start(fileName);
                    }
                }
                catch (Exception e)
                {
                    DetailsErrorDialog.Show("PDF Export", e.Message, e.StackTrace, MessageBoxIcon.Error, true);
                }
            }
        }

        #endregion
    }
}
