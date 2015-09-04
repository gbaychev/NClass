using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace NClass.AssemblyImport.Controls
{
  public class DataGridViewImageComboBoxColumnCell : DataGridViewComboBoxCell
  {
    // ========================================================================
    // Con- / Destruction

    #region === Con- / Destruction

    /// <summary>
    ///   Initializes a new instance of <see cref = "DataGridViewImageComboBoxColumnCell" />.
    /// </summary>
    public DataGridViewImageComboBoxColumnCell()
    {
      DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
      DisplayStyleForCurrentCellOnly = true;
    }

    #endregion

    // ========================================================================
    // Properties

    #region === Properties

    /// <summary>
    ///   Gets the type of the values of the cells. Returns the type of <see cref = "ImageComboBoxItem" />.
    /// </summary>
    public override Type ValueType
    {
      get { return typeof(ImageComboBoxItem); }
    }

    /// <summary>
    ///   Gets the type of the editing control. Returns the type of <see cref = "DataGridViewImageComboBoxEditingControl" />.
    /// </summary>
    public override Type EditType
    {
      get { return typeof(DataGridViewImageComboBoxEditingControl); }
    }

    /// <summary>
    ///   Gets a new value for the cell. Returns a newly created <see cref = "ImageComboBoxItem" />.
    /// </summary>
    public override object DefaultNewRowValue
    {
      get { return new ImageComboBoxItem(); }
    }

    #endregion

    // ========================================================================
    // Methods

    #region === Methods

    /// <summary>
    ///   Gets the formatted value of the cell's data.
    /// </summary>
    /// <returns>
    ///   The value.
    /// </returns>
    /// <param name = "value">The value to be formatted. </param>
    /// <param name = "rowIndex">The index of the cell's parent row. </param>
    /// <param name = "cellStyle">The <see cref = "T:System.Windows.Forms.DataGridViewCellStyle" /> in effect for the cell.</param>
    /// <param name = "valueTypeConverter">A <see cref = "T:System.ComponentModel.TypeConverter" /> associated with the value type that provides custom conversion to the formatted value type, or null if no such custom conversion is needed.</param>
    /// <param name = "formattedValueTypeConverter">A <see cref = "T:System.ComponentModel.TypeConverter" /> associated with the formatted value type that provides custom conversion from the value type, or null if no such custom conversion is needed.</param>
    /// <param name = "context">A bitwise combination of <see cref = "T:System.Windows.Forms.DataGridViewDataErrorContexts" /> values describing the context in which the formatted value is needed.</param>
    /// <exception cref = "T:System.Exception">Formatting failed and either there is no handler for the <see cref = "E:System.Windows.Forms.DataGridView.DataError" /> event of the <see cref = "T:System.Windows.Forms.DataGridView" /> control or the handler set the <see cref = "P:System.Windows.Forms.DataGridViewDataErrorEventArgs.ThrowException" /> property to true. The exception object can typically be cast to type <see cref = "T:System.FormatException" /> for type conversion errors or to type <see cref = "T:System.ArgumentException" /> if <paramref name = "value" /> cannot be found in the <see cref = "P:System.Windows.Forms.DataGridViewComboBoxCell.DataSource" /> or the <see cref = "P:System.Windows.Forms.DataGridViewComboBoxCell.Items" /> collection. </exception>
    protected override object GetFormattedValue(object value, int rowIndex, ref DataGridViewCellStyle cellStyle,
                                                TypeConverter valueTypeConverter,
                                                TypeConverter formattedValueTypeConverter,
                                                DataGridViewDataErrorContexts context)
    {
      return value;
    }

    /// <summary>
    ///   Gets the parsed value of the cell's formatted value.
    /// </summary>
    /// <returns>
    ///   The cell value.
    /// </returns>
    /// <param name = "formattedValue">The display value of the cell.</param>
    /// <param name = "cellStyle">The <see cref = "T:System.Windows.Forms.DataGridViewCellStyle" /> in effect for the cell.</param>
    /// <param name = "formattedValueTypeConverter">A <see cref = "T:System.ComponentModel.TypeConverter" /> for the display value type, or null to use the default converter.</param>
    /// <param name = "valueTypeConverter">A <see cref = "T:System.ComponentModel.TypeConverter" /> for the cell value type, or null to use the default converter.</param>
    public override object ParseFormattedValue(object formattedValue, DataGridViewCellStyle cellStyle,
                                               TypeConverter formattedValueTypeConverter,
                                               TypeConverter valueTypeConverter)
    {
      return formattedValue;
    }

    /// <summary>
    ///   Paints the current <see cref = "DataGridViewImageComboBoxColumnCell" />.
    /// </summary>
    /// <param name = "graphics">The <see cref = "T:System.Drawing.Graphics" /> used to paint the cell.</param>
    /// <param name = "clipBounds">A <see cref = "T:System.Drawing.Rectangle" /> that represents the area of the <see cref = "T:System.Windows.Forms.DataGridView" /> that needs to be repainted.</param>
    /// <param name = "cellBounds">A <see cref = "T:System.Drawing.Rectangle" /> that contains the bounds of the cell that is being painted.</param>
    /// <param name = "rowIndex">The row index of the cell that is being painted.</param>
    /// <param name = "elementState">A bitwise combination of <see cref = "T:System.Windows.Forms.DataGridViewElementStates" /> values that specifies the state of the cell.</param>
    /// <param name = "value">The data of the cell that is being painted.</param>
    /// <param name = "formattedValue">The formatted data of the cell that is being painted.</param>
    /// <param name = "errorText">An error message that is associated with the cell.</param>
    /// <param name = "cellStyle">A <see cref = "T:System.Windows.Forms.DataGridViewCellStyle" /> that contains formatting and style information about the cell.</param>
    /// <param name = "advancedBorderStyle">A <see cref = "T:System.Windows.Forms.DataGridViewAdvancedBorderStyle" /> that contains border styles for the cell that is being painted.</param>
    /// <param name = "paintParts">A bitwise combination of the <see cref = "T:System.Windows.Forms.DataGridViewPaintParts" /> values that specifies which parts of the cell need to be painted.</param>
    protected override void Paint(Graphics graphics,
                                  Rectangle clipBounds, Rectangle cellBounds, int rowIndex,
                                  DataGridViewElementStates elementState, object value,
                                  object formattedValue, string errorText,
                                  DataGridViewCellStyle cellStyle,
                                  DataGridViewAdvancedBorderStyle advancedBorderStyle,
                                  DataGridViewPaintParts paintParts)
    {
      //Call the base to draw the combo box itself.
      base.Paint(graphics, clipBounds, cellBounds, rowIndex, elementState, value, null,
                 errorText, cellStyle, advancedBorderStyle, paintParts);

      //Now we will draw the current content of the box.
      DataGridViewImageComboBoxColumn column = ((DataGridViewImageComboBoxColumn)OwningColumn);
      if((paintParts & DataGridViewPaintParts.Background) != 0 || (paintParts & DataGridViewPaintParts.All) != 0)
      {
        Rectangle rect = new Rectangle(cellBounds.X + 4, cellBounds.Y, cellBounds.Width - 4, cellBounds.Height - 1);
        ControlDrawHelper.DrawImageComboBoxItem(graphics, value, column.ImageList, column.ImageSize, rect, cellStyle.Font, cellStyle.ForeColor);
      }
    }

    /// <summary>
    ///   Attaches and initializes the hosted editing control.
    /// </summary>
    /// <param name = "rowIndex">The index of the cell's parent row.</param>
    /// <param name = "initialFormattedValue">The initial value to be displayed in the control.</param>
    /// <param name = "dataGridViewCellStyle">A <see cref = "T:System.Windows.Forms.DataGridViewCellStyle" /> that determines the appearance of the hosted control.</param>
    public override void InitializeEditingControl(int rowIndex, object initialFormattedValue,
                                                  DataGridViewCellStyle dataGridViewCellStyle)
    {
      base.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);

      DataGridViewImageComboBoxEditingControl control =
        DataGridView.EditingControl as DataGridViewImageComboBoxEditingControl;
      DataGridViewImageComboBoxColumn column = OwningColumn as DataGridViewImageComboBoxColumn;
      if(control != null && column != null)
      {
        control.ImageList = column.ImageList;
        control.ImageSize = column.ImageSize;
        control.ItemHeight = column.ImageSize.Height + 2;
        control.Items.Clear();
        control.Items.AddRange(column.Items.ToArray());

        control.SelectedItem = Value as ImageComboBoxItem;
      }
    }

    #endregion
  }
}