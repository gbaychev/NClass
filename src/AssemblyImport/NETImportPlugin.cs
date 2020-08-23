// NClass - Free class diagram editor
// Copyright (C) 2006-2009 Balazs Tihanyi
// Copyright (C) 2020 Georgi Baychev
// 
// This program is free software; you can redistribute it and/or modify it under 
// the terms of the GNU General Public License as published by the Free Software 
// Foundation; either version 3 of the License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful, but WITHOUT 
// ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS 
// FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License along with 
// this program; if not, write to the Free Software Foundation, Inc., 
// 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA

using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using NClass.AssemblyImport.Lang;
using NClass.CSharp;
using NClass.DiagramEditor.ClassDiagram;
using NClass.DiagramEditor.Diagrams;
using NClass.GUI;
using NClass.GUI.Dialogs;
using NReflect;
using NReflect.Filter;

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
        get
        {
            var diagram = DocumentManager.ActiveDocument as ClassDiagram;
            return Workspace.HasActiveProject && diagram?.Language == CSharpLanguage.Instance;
        }
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
            ClassDiagram diagram = new ClassDiagram(CSharpLanguage.Instance);
            if(ImportAssembly(fileName, diagram, settings))
            {
              Workspace.ActiveProject.Add(diagram);
            }
          }
        }
      }
    }

    private bool ImportAssembly(string fileName, ClassDiagram diagram, ImportSettings settings)
    {
      if (string.IsNullOrEmpty(fileName))
      {
        MessageBox.Show(Strings.Error_NoAssembly, Strings.Error_MessageBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
        return false;
      }

      try
      {
        NETImport importer = new NETImport(diagram, settings);
        return importer.ImportAssembly(fileName);
      }
      catch (UnsafeTypesPresentException)
      {
        MessageBox.Show(null, Strings.UnsafeTypesPresent, Strings.WarningTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return true;
      }
      catch (ReflectionTypeLoadException)
      {
        MessageBox.Show(Strings.Error_MissingReferencedAssemblies, Strings.Error_MessageBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
        return false;
      }
      catch (FileLoadException)
      {
        MessageBox.Show(Strings.Error_MissingReferencedAssemblies, Strings.Error_MessageBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
        return false;
      }
      catch (BadImageFormatException)
      {
        MessageBox.Show(Strings.Error_BadImageFormat, Strings.Error_MessageBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
        return false;
      }
      catch (Exception ex)
      {
          DetailsErrorDialog.Show(Strings.Error_MessageBoxTitle, Strings.Error_GeneralException, ex.ToString(), MessageBoxIcon.Error);
          return false;
      }
    }

    #endregion
  }
}