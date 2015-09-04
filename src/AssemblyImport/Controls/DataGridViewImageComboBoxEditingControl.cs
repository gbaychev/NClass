using System;
using System.Drawing;
using System.Windows.Forms;

namespace NClass.AssemblyImport.Controls
{
  public class DataGridViewImageComboBoxEditingControl : ImageComboBox, IDataGridViewEditingControl
  {
    // ========================================================================
    // Properties

    #region === Properties

    /// <summary>
    /// Gets or sets the <see cref="T:System.Windows.Forms.DataGridView"/> that contains the cell.
    /// </summary>
    /// <returns>
    /// The <see cref="T:System.Windows.Forms.DataGridView"/> that contains the <see cref="T:System.Windows.Forms.DataGridViewCell"/> that is being edited; null if there is no associated <see cref="T:System.Windows.Forms.DataGridView"/>.
    /// </returns>
    public DataGridView EditingControlDataGridView { get; set; }

    /// <summary>
    /// Gets or sets the formatted value of the cell being modified by the editor.
    /// </summary>
    /// <returns>
    /// An <see cref="T:System.Object"/> that represents the formatted value of the cell.
    /// </returns>
    public object EditingControlFormattedValue
    {
      get { return GetEditingControlFormattedValue(DataGridViewDataErrorContexts.Formatting); }
      set
      {
        ImageComboBoxItem item = value as ImageComboBoxItem;
        if(item != null)
        {
          SelectedItem = item;
        }
      }
    }

    /// <summary>
    /// Gets or sets the index of the hosting cell's parent row.
    /// </summary>
    /// <returns>
    /// The index of the row that contains the cell, or –1 if there is no parent row.
    /// </returns>
    public int EditingControlRowIndex { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the value of the editing control differs from the value of the hosting cell.
    /// </summary>
    /// <returns>
    /// <c>true</c> if the value of the control differs from the cell value; otherwise, <c>false</c>.
    /// </returns>
    public bool EditingControlValueChanged { get; set; }

    /// <summary>
    /// Gets the cursor used when the mouse pointer is over the <see cref="P:System.Windows.Forms.DataGridView.EditingPanel"/> but not over the editing control.
    /// </summary>
    /// <returns>
    /// Returns the default cursor. 
    /// </returns>
    public Cursor EditingPanelCursor
    {
      get { return Cursors.Default; }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the cell contents need to be repositioned whenever the value changes.
    /// </summary>
    /// <returns>
    /// <c>false</c>.
    /// </returns>
    public bool RepositionEditingControlOnValueChange
    {
      get { return false; }
    }

    #endregion

    // ========================================================================
    // Methods

    #region === Methods

    /// <summary>
    /// Changes the control's user interface (UI) to be consistent with the specified cell style.
    /// </summary>
    /// <param name="dataGridViewCellStyle">The <see cref="T:System.Windows.Forms.DataGridViewCellStyle"/> to use as the model for the UI.</param>
    public void ApplyCellStyleToEditingControl(DataGridViewCellStyle dataGridViewCellStyle)
    {
      Font = dataGridViewCellStyle.Font;
      // Don't use colors which are opaque.
      BackColor = Color.FromArgb(255, dataGridViewCellStyle.BackColor);
      ForeColor = dataGridViewCellStyle.ForeColor;
    }

    /// <summary>
    /// Determines whether the specified key is a regular input key that the editing control should process or a special key that the <see cref="T:System.Windows.Forms.DataGridView"/> should process.
    /// </summary>
    /// <returns>
    /// <c>true</c> if the specified key is a regular input key that should be handled by the editing control; otherwise, <c>false</c>.
    /// </returns>
    /// <param name="keyData">A <see cref="T:System.Windows.Forms.Keys"/> that represents the key that was pressed.</param>
    /// <param name="dataGridViewWantsInputKey"><c>true</c> when the <see cref="T:System.Windows.Forms.DataGridView"/> wants to process the <see cref="T:System.Windows.Forms.Keys"/> in <paramref name="keyData"/>; otherwise, <c>false</c>.</param>
    public bool EditingControlWantsInputKey(Keys keyData, bool dataGridViewWantsInputKey)
    {
      if((keyData & Keys.KeyCode) == Keys.Down ||
         (keyData & Keys.KeyCode) == Keys.Up ||
         (DroppedDown && ((keyData & Keys.KeyCode) == Keys.Escape) || (keyData & Keys.KeyCode) == Keys.Enter))
      {
        return true;
      }
      return !dataGridViewWantsInputKey;
    }

    /// <summary>
    /// Retrieves the formatted value of the cell.
    /// </summary>
    /// <returns>
    /// An <see cref="ImageComboBoxItem"/> that represents the new value.
    /// </returns>
    /// <param name="context">A bitwise combination of <see cref="T:System.Windows.Forms.DataGridViewDataErrorContexts"/> values that specifies the context in which the data is needed.</param>
    public object GetEditingControlFormattedValue(DataGridViewDataErrorContexts context)
    {
      return SelectedItem;
    }

    /// <summary>
    /// Prepares the currently selected cell for editing.
    /// </summary>
    /// <param name="selectAll"><c>true</c> to select all of the cell's content; otherwise, false.</param>
    public void PrepareEditingControlForEdit(bool selectAll)
    {
      DroppedDown = true;
    }

    /// <summary>
    /// Raises the <see cref="E:System.Windows.Forms.ComboBox.SelectedIndexChanged"/> event.
    /// </summary>
    /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data. </param>
    protected override void OnSelectedIndexChanged(EventArgs e)
    {
      base.OnSelectedIndexChanged(e);
      EditingControlValueChanged = true;
      EditingControlDataGridView.NotifyCurrentCellDirty(true);
    }

    #endregion
  }
}