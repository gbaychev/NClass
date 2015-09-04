using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using NClass.AssemblyImport.Controls;
using NClass.AssemblyImport.Lang;
using NClass.AssemblyImport.Properties;
using NReflect.Filter;

namespace NClass.AssemblyImport
{
  /// <summary>
  /// A form to set up the ImportSettings.
  /// </summary>
  public partial class ImportSettingsForm : Form
  {

    #region === Construction

    /// <summary>
    /// Initializes a new ImportSettingsForm2.
    /// </summary>
    /// <param name="settings">The <see cref="ImportSettings"/> which will be used for import. </param>
    public ImportSettingsForm(ImportSettings settings)
    {
      InitializeComponent();

      // Add the icons to the realisation check boxes.
      chkCreateRealizations.Image = DiagramEditor.Properties.Resources.Realization;
      chkCreateNesting.Image = DiagramEditor.Properties.Resources.Nesting;
      chkCreateGeneralizations.Image = DiagramEditor.Properties.Resources.Generalization;
      chkCreateAssociations.Image = DiagramEditor.Properties.Resources.Association;

      //Localization goes here...
      colFilterElement.Items.Clear();
      colFilterElement.ImageSize = new Size(16, 16);
      colFilterElement.Items.Add(new ImageComboBoxItem(Strings.Element_Elements, null, FilterElements.AllElements));
      colFilterElement.Items.Add(new ImageComboBoxItem(Strings.Element_Class, DiagramEditor.Properties.Resources.Class, FilterElements.Class));
      colFilterElement.Items.Add(new ImageComboBoxItem(Strings.Element_Struct, DiagramEditor.Properties.Resources.Structure, FilterElements.Struct));
      colFilterElement.Items.Add(new ImageComboBoxItem(Strings.Element_Interface, DiagramEditor.Properties.Resources.Interface32, FilterElements.Interface));
      colFilterElement.Items.Add(new ImageComboBoxItem(Strings.Element_Delegate, DiagramEditor.Properties.Resources.Delegate, FilterElements.Delegate));
      colFilterElement.Items.Add(new ImageComboBoxItem(Strings.Element_Enum, DiagramEditor.Properties.Resources.Enum, FilterElements.Enum));
      colFilterElement.Items.Add(new ImageComboBoxItem(Strings.Element_EnumValue, DiagramEditor.Properties.Resources.EnumItem, FilterElements.EnumValue));
      colFilterElement.Items.Add(new ImageComboBoxItem(Strings.Element_Constructor, DiagramEditor.Properties.Resources.Constructor, FilterElements.Constructor));
      colFilterElement.Items.Add(new ImageComboBoxItem(Strings.Element_Field, DiagramEditor.Properties.Resources.Field, FilterElements.Field));
      colFilterElement.Items.Add(new ImageComboBoxItem(Strings.Element_Constant, DiagramEditor.Properties.Resources.PublicConst, FilterElements.Constant));
      colFilterElement.Items.Add(new ImageComboBoxItem(Strings.Element_Property, DiagramEditor.Properties.Resources.Property, FilterElements.Property));
      colFilterElement.Items.Add(new ImageComboBoxItem(Strings.Element_Method, DiagramEditor.Properties.Resources.Method, FilterElements.Method));
      colFilterElement.Items.Add(new ImageComboBoxItem(Strings.Element_Operator, DiagramEditor.Properties.Resources.PublicOperator, FilterElements.Operator));
      colFilterElement.Items.Add(new ImageComboBoxItem(Strings.Element_Event, DiagramEditor.Properties.Resources.Event, FilterElements.Event));

      colFilterModifier.Items.Clear();
      colFilterModifier.ImageSize = new Size(16, 16);
      colFilterModifier.Items.Add(new ImageComboBoxItem(Strings.Modifier_All, null, FilterModifiers.AllModifiers));
      colFilterModifier.Items.Add(new ImageComboBoxItem(Strings.Modifier_Instance, null, FilterModifiers.Instance));
      colFilterModifier.Items.Add(new ImageComboBoxItem(Strings.Modifier_Static, DiagramEditor.Properties.Resources.Static, FilterModifiers.Static));
      colFilterModifier.Items.Add(new ImageComboBoxItem(Strings.Modifier_Default, Resources.ModifierDefault, FilterModifiers.Default));
      colFilterModifier.Items.Add(new ImageComboBoxItem(Strings.Modifier_Public, null, FilterModifiers.Public));
      colFilterModifier.Items.Add(new ImageComboBoxItem(Strings.Modifier_Private, Resources.ModifierPrivate, FilterModifiers.Private));
      colFilterModifier.Items.Add(new ImageComboBoxItem(Strings.Modifier_Internal, Resources.ModifierInternal, FilterModifiers.Internal));
      colFilterModifier.Items.Add(new ImageComboBoxItem(Strings.Modifier_Protected, Resources.ModifierProtected, FilterModifiers.Protected));
      colFilterModifier.Items.Add(new ImageComboBoxItem(Strings.Modifier_ProtectedInternal, Resources.ModifierProtectedInternal, FilterModifiers.ProtectedInternal));

      //Build reverse maps for easy access while loading a template.
      reverseElementNameMap.Clear();
      foreach (ImageComboBoxItem comboBoxItem in colFilterElement.Items)
      {
        reverseElementNameMap.Add((FilterElements)comboBoxItem.Tag, comboBoxItem);
      }
      reverseModifierNameMap.Clear();
      foreach (ImageComboBoxItem comboBoxItem in colFilterModifier.Items)
      {
        reverseModifierNameMap.Add((FilterModifiers)comboBoxItem.Tag, comboBoxItem);
      }

      importSettings = settings;

      //Templates
      cboTemplate.Items.Clear();
      if (Settings.Default.ImportSettingsTemplates == null)
      {
        Settings.Default.ImportSettingsTemplates = new TemplateList();
        ImportSettings newSettings = new ImportSettings
                                        {
                                          Name = Strings.Settings_Template_LastUsed,
                                          CreateAssociations = true,
                                          CreateGeneralizations = true,
                                          CreateNestings = true,
                                          CreateRealizations = true,
                                          CreateRelationships = true,
                                          LabelAggregations = true
                                        };
        Settings.Default.ImportSettingsTemplates.Add(newSettings);
      }
      foreach (object xTemplate in Settings.Default.ImportSettingsTemplates)
      {
        cboTemplate.Items.Add(xTemplate);
      }
      cboTemplate.SelectedItem = cboTemplate.Items[0];
      DisplaySettings((ImportSettings)cboTemplate.Items[0]);

      LocalizeComponents();
    }

    /// <summary>
    /// Displays the text for the current culture.
    /// </summary>
    private void LocalizeComponents()
    {
      Text = Strings.Settings_Title;
      grpTemplate.Text = Strings.Settings_Template;
      cmdLoadTemplate.Text = Strings.Settings_Template_LoadButton;
      cmdStoreTemplate.Text = Strings.Settings_Template_StoreButton;
      cmdDeleteTemplate.Text = Strings.Settings_Template_DeleteButton;

      grpFilter.Text = Strings.Settings_Filter_GroupTitle;
      rdoWhiteList.Text = Strings.Settings_Filter_WhiteList;
      rdoBlackList.Text = Strings.Settings_Filter_BlackList;
      colFilterElement.HeaderText = Strings.Settings_Filter_ElementColumnTitle;
      colFilterModifier.HeaderText = Strings.Settings_Filter_ModifierColumnTitle;

      chkCreateRelationships.Text = Strings.Settings_CreateRelationships;
      chkCreateAssociations.Text = Strings.Settings_CreateAssociations;
      chkLabelAssociations.Text = Strings.Settings_CreateLabel;
      chkRemoveFields.Text = Strings.Settings_RemoveFields;
      chkCreateGeneralizations.Text = Strings.Settings_CreateGeneralizations;
      chkCreateRealizations.Text = Strings.Settings_CreateRealizations;
      chkCreateNesting.Text = Strings.Settings_CreateNesting;

      cmdOK.Text = Strings.Settings_OKButton;
      cmdCancel.Text = Strings.Settings_CancelButton;
    }

    #endregion

    #region === Fields

    /// <summary>
    /// The settings which are used for the import
    /// </summary>
    readonly ImportSettings importSettings;
    ///// <summary>
    ///// A map from element names to the element enum.
    ///// </summary>
    //readonly Dictionary<string, Elements> elementNameMap = new Dictionary<string, Elements>();
    /// <summary>
    /// A map from element enum to the element names.
    /// </summary>
    readonly Dictionary<FilterElements, ImageComboBoxItem> reverseElementNameMap = new Dictionary<FilterElements, ImageComboBoxItem>();
    ///// <summary>
    ///// A map from the modifier names to the modifier enum.
    ///// </summary>
    //readonly Dictionary<string, Modifiers> modifierNameMap = new Dictionary<string, Modifiers>();
    /// <summary>
    /// A map from the modifier enum to the modifier names.
    /// </summary>
    readonly Dictionary<FilterModifiers, ImageComboBoxItem> reverseModifierNameMap = new Dictionary<FilterModifiers, ImageComboBoxItem>();
    
    #endregion

    #region === Event-Methods

    /// <summary>
    /// Gets called when the OK-button is clicked. Closes the dialog.
    /// </summary>
    /// <param name="sender">The sender of the event.</param>
    /// <param name="e">Information about the event.</param>
    private void cmdOK_Click(object sender, EventArgs e)
    {
      Close();
    }

    /// <summary>
    /// Gets called when the dialog is closed. Stores all settings.
    /// </summary>
    /// <param name="sender">The sender of the event.</param>
    /// <param name="e">Information about the event.</param>
    private void ImportSettingsForm_FormClosed(object sender, FormClosedEventArgs e)
    {
      StoreSettings(importSettings);
      StoreSettings((ImportSettings)Settings.Default.ImportSettingsTemplates[0]); //<last used>
      Settings.Default.Save();
    }

    /// <summary>
    /// Gets called when a key is pressed while the dialog is opened.
    /// Closes the dialog if the key is escape.
    /// </summary>
    /// <param name="sender">The sender of the event.</param>
    /// <param name="e">Information about the event.</param>
    private void ImportSettingsForm_KeyDown(object sender, KeyEventArgs e)
    {
      if(e.KeyCode == Keys.Escape)
      {
        DialogResult = DialogResult.Cancel;
        Close();
        e.Handled = true;
      }
    }

    /// <summary>
    /// Gets called when the LoadTemplate-button is clicked. Displays the
    /// settings belonging to the actual template.
    /// </summary>
    /// <param name="sender">The sender of the event.</param>
    /// <param name="e">Information about the event.</param>
    private void cmdLoadTemplate_Click(object sender, EventArgs e)
    {
      if(cboTemplate.SelectedItem == null)
      {
        MessageBox.Show(Strings.Settings_Error_NoTemplateSelected, Strings.Error_MessageBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
        return;
      }
      DisplaySettings((ImportSettings)cboTemplate.SelectedItem);
    }

    /// <summary>
    /// Gets called when the StoreTemplate-button is clicked. Stores the settings
    /// to a template.
    /// </summary>
    /// <param name="sender">The sender of the event.</param>
    /// <param name="e">Information about the event.</param>
    private void cmdStoreTemplate_Click(object sender, EventArgs e)
    {
      if(string.IsNullOrEmpty(cboTemplate.Text))
      {
        MessageBox.Show(Strings.Settings_Error_NoTemplateName, Strings.Error_MessageBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
        return;
      }
      if(cboTemplate.Text.Contains("<"))
      {
        MessageBox.Show(Strings.Settings_Error_AngleBracketNotAllowed, Strings.Error_MessageBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
        return;
      }
      ImportSettings settings = (ImportSettings)cboTemplate.SelectedItem ?? new ImportSettings();
      StoreSettings(settings);
      settings.Name = cboTemplate.Text;
      if(cboTemplate.SelectedItem == null)
      {
        //New entry
        cboTemplate.Items.Add(settings);
        cboTemplate.SelectedItem = settings;
        Settings.Default.ImportSettingsTemplates.Add(settings);
      }
    }

    /// <summary>
    /// Gets called when the DeleteTemplate-button is clicked. Deletes the
    /// actual template.
    /// </summary>
    /// <param name="sender">The sender of the event.</param>
    /// <param name="e">Information about the event.</param>
    private void cmdDeleteTemplate_Click(object sender, EventArgs e)
    {
      if(cboTemplate.SelectedItem == null)
      {
        MessageBox.Show(Strings.Settings_Error_NoTemplateSelected, Strings.Error_MessageBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
        return;
      }
      if(cboTemplate.Text.Contains("<"))
      {
        MessageBox.Show(Strings.Settings_Error_TemplateCantBeDeleted, Strings.Error_MessageBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
        return;
      }
      //Settings.Default.ImportSettingsTemplates.Remove(cboTemplate.SelectedItem);
      cboTemplate.Items.Remove(cboTemplate.SelectedItem);
    }

    /// <summary>
    /// Gets called when the CreateRelationships-checkbox is (un)checked. 
    /// Updates the user interface.
    /// </summary>
    /// <param name="sender">The sender of the event.</param>
    /// <param name="e">Information about the event.</param>
    private void chkCreateRelationships_CheckedChanged(object sender, EventArgs e)
    {
      chkCreateAssociations.Enabled = chkCreateRelationships.Checked;
      chkLabelAssociations.Enabled = chkCreateRelationships.Checked && chkCreateAssociations.Checked;
      chkRemoveFields.Enabled = chkCreateRelationships.Checked && chkCreateAssociations.Checked;
      chkCreateGeneralizations.Enabled = chkCreateRelationships.Checked;
      chkCreateRealizations.Enabled = chkCreateRelationships.Checked;
      chkCreateNesting.Enabled = chkCreateRelationships.Checked;
    }

    /// <summary>
    /// Gets called when the CreateAggregations-checkbox is (un)checked. 
    /// Updates the user interface.
    /// </summary>
    /// <param name="sender">The sender of the event.</param>
    /// <param name="e">Information about the event.</param>
    private void chkCreateAssociations_CheckedChanged(object sender, EventArgs e)
    {
      chkLabelAssociations.Enabled = chkCreateRelationships.Checked && chkCreateAssociations.Checked;
      chkRemoveFields.Enabled = chkCreateRelationships.Checked && chkCreateAssociations.Checked;
    }

    /// <summary>
    /// Gets called when the data displayed at the filter list changed.
    /// </summary>
    /// <param name="sender">The sender of the event.</param>
    /// <param name="e">Information about the event.</param>
    private void dgvFilter_CellValueChanged(object sender, DataGridViewCellEventArgs e)
    {
      if (e.RowIndex < 0 || e.ColumnIndex < 0)
      {
        return;
      }
      DataGridViewRow row = dgvFilter.Rows[e.RowIndex];
      ImageComboBoxItem modifier = row.Cells[0].Value as ImageComboBoxItem;
      ImageComboBoxItem element = row.Cells[1].Value as ImageComboBoxItem;
      if (modifier != null && element != null)
      {
        if (element.Tag == null)
        {
          // A modifier was just selected...
          row.Cells[1].Value = colFilterElement.Items[0];
        }
        if (modifier.Tag == null)
        {
          // An element was just selected...
          row.Cells[0].Value = colFilterModifier.Items[0];
        }
        
      }
    }

    /// <summary>
    /// Gets called when the dirty state of a cell changed.
    /// </summary>
    /// <param name="sender">The sender of the event.</param>
    /// <param name="e">Information about the event.</param>
    private void dgvFilter_CurrentCellDirtyStateChanged(object sender, EventArgs e)
    {
      dgvFilter.CommitEdit(DataGridViewDataErrorContexts.Commit);
    }

    #endregion

    #region === Methods

    /// <summary>
    /// Displays the given Settings.
    /// </summary>
    /// <param name="settings">The Settings which shall be displayed.</param>
    private void DisplaySettings(ImportSettings settings)
    {
      dgvFilter.Rows.Clear();

      rdoWhiteList.Checked = settings.UseAsWhiteList;
      rdoBlackList.Checked = !settings.UseAsWhiteList;
      if(settings.FilterRules != null)
      {
        foreach(FilterRule filterRule in settings.FilterRules)
        {
          dgvFilter.Rows.Add(reverseModifierNameMap[filterRule.Modifier], reverseElementNameMap[filterRule.Element]);
        }
      }

      chkCreateRelationships.Checked = settings.CreateRelationships;
      chkCreateAssociations.Checked = settings.CreateAssociations;
      chkLabelAssociations.Checked = settings.LabelAggregations;
      chkRemoveFields.Checked = settings.RemoveFields;
      chkCreateGeneralizations.Checked = settings.CreateGeneralizations;
      chkCreateRealizations.Checked = settings.CreateRealizations;
      chkCreateNesting.Checked = settings.CreateNestings;
    }

    /// <summary>
    /// Stores the displayed settings to <paramref name="settings"/>
    /// </summary>
    /// <param name="settings">The destination of the store operation.</param>
    private void StoreSettings(ImportSettings settings)
    {
      settings.UseAsWhiteList = rdoWhiteList.Checked;
      List<FilterRule> filterRules = new List<FilterRule>(dgvFilter.Rows.Count);
      foreach (DataGridViewRow row in dgvFilter.Rows)
      {
        ImageComboBoxItem modifier = row.Cells[0].Value as ImageComboBoxItem;
        ImageComboBoxItem element = row.Cells[1].Value as ImageComboBoxItem;
        if (modifier != null && element != null)
        {
          if (modifier.Tag is FilterModifiers && element.Tag is FilterElements)
          {
            filterRules.Add(new FilterRule((FilterModifiers)modifier.Tag, (FilterElements)element.Tag));
          }
        }
      }
      settings.FilterRules = filterRules;

      settings.CreateRelationships = chkCreateRelationships.Checked;
      settings.CreateAssociations = chkCreateAssociations.Checked;
      settings.LabelAggregations = chkLabelAssociations.Checked;
      settings.RemoveFields = chkRemoveFields.Checked;
      settings.CreateGeneralizations = chkCreateGeneralizations.Checked;
      settings.CreateRealizations = chkCreateRealizations.Checked;
      settings.CreateNestings = chkCreateNesting.Checked;
    }

    #endregion
  }
}