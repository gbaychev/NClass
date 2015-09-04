using System;
using System.Globalization;
using System.Windows.Forms;
using NClass.AssemblyImport.Lang;
using NClass.CSharp;
using NClass.DiagramEditor.ClassDiagram;
using NClass.GUI;

namespace NClass.AssemblyImport
{
  /// <summary>
  ///   Implements the PlugIn-Interface of NClass.
  /// </summary>
  public class NETImportPlugin : Plugin
  {
    // ========================================================================
    // Fields

    #region === Fields

    /// <summary>
    ///   The menu item used to start the export.
    /// </summary>
    private readonly ToolStripMenuItem menuItem;

    #endregion

    // ========================================================================
    // Con- / Destruction

    #region === Con- / Destruction

    /// <summary>
    ///   Set up the current culture for the strings.
    /// </summary>
    static NETImportPlugin()
    {
      try
      {
        Strings.Culture = CultureInfo.GetCultureInfo(Settings.Default.UILanguage);
      }
      catch(ArgumentException)
      {
        //Culture is not supported, maybe the setting is "default".
      }
    }

    /// <summary>
    ///   Constructs a new instance of NETImportPlugin.
    /// </summary>
    /// <param name = "environment">An instance of NClassEnvironment.</param>
    public NETImportPlugin(NClassEnvironment environment)
      : base(environment)
    {
      menuItem = new ToolStripMenuItem
                   {
                     Text = Strings.Menu_Title,
                     ToolTipText = Strings.Menu_ToolTip
                   };
      menuItem.Click += menuItem_Click;
    }

    #endregion

    // ========================================================================
    // Event handling

    #region === Event handling

    /// <summary>
    ///   Starts the export.
    /// </summary>
    /// <param name = "sender">The sender.</param>
    /// <param name = "e">Additional information.</param>
    private void menuItem_Click(object sender, EventArgs e)
    {
      Launch();
    }

    #endregion

    // ========================================================================
    // Properties

    #region === Properties

    /// <summary>
    ///   Gets a value indicating whether the plugin can be executed at the moment.
    /// </summary>
    public override bool IsAvailable
    {
      get { return Workspace.HasActiveProject; }
    }

    /// <summary>
    ///   Gets the menu item used to start the plugin.
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
    ///   Starts the functionality of the plugin.
    /// </summary>
    protected void Launch()
    {
      if(Workspace.HasActiveProject)
      {
        string fileName;
        using (OpenFileDialog dialog = new OpenFileDialog())
        {
          dialog.Filter = Strings.OpenFileDialog_Filter;
          if (dialog.ShowDialog() == DialogResult.Cancel)
            return;
          fileName = dialog.FileName;
        }

        ImportSettings settings = new ImportSettings();
        using(ImportSettingsForm settingsForm = new ImportSettingsForm(settings))
        {
          if(settingsForm.ShowDialog() == DialogResult.OK)
          {
            Diagram diagram = new Diagram(CSharpLanguage.Instance);
            NETImport importer = new NETImport(diagram, settings);

            if(importer.ImportAssembly(fileName))
            {
              Workspace.ActiveProject.Add(diagram);
            }
          }
        }
      }
    }

    #endregion
  }
}