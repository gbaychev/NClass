using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace NClass.AssemblyImport.Controls
{
  /// <summary>
  ///   A column for a <see cref = "DataGridView" /> which contains <see cref = "ImageComboBox" />es.
  /// </summary>
  public class DataGridViewImageComboBoxColumn : DataGridViewColumn
  {
    // ========================================================================
    // Con- / Destruction

    #region === Con- / Destruction

    /// <summary>
    ///   Initializes a new instance of <see cref = "DataGridViewImageComboBoxColumn" />.
    /// </summary>
    public DataGridViewImageComboBoxColumn()
      : base(new DataGridViewImageComboBoxColumnCell())
    {
      ImageList = new ImageList();
      Items = new List<ImageComboBoxItem>();
    }

    #endregion

    // ========================================================================
    // Properties

    #region === Properties

    /// <summary>
    ///   Gets or sets an <see cref = "ImageList" /> containing the images to display.
    /// </summary>
    [Category("Data")]
    [Description("The image list containing the images to display.")]
    public ImageList ImageList { get; set; }

    /// <summary>
    ///   Gets or sets the list of <see cref = "ImageComboBoxItem" />s contained within the
    ///   combo box.
    /// </summary>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    [Category("Data")]
    [Description("The elements available at the combobox.")]
    public List<ImageComboBoxItem> Items { get; set; }

    /// <summary>
    ///   Gets or sets the size of the images.
    /// </summary>
    public Size ImageSize { get; set; }

    /// <summary>
    ///   Gets or sets the template used to create new cells.
    /// </summary>
    /// <returns>
    ///   A <see cref = "DataGridViewCell" /> that all other cells in the column are modeled
    ///   after. The default is null.
    /// </returns>
    /// <exception cref = "ArgumentException">
    ///   This exception is thrown if you try to assign a <see cref = "DataGridViewCell" />
    ///   which is not a <see cref = "DataGridViewImageComboBoxColumnCell" />.
    /// </exception>
    public override DataGridViewCell CellTemplate
    {
      get { return base.CellTemplate; }
      set
      {
        // Ensure that the cell used for the template is a DataGridViewImageComboBoxColumnCell.
        if(value != null &&
           !value.GetType().IsAssignableFrom(typeof(DataGridViewImageComboBoxColumnCell)))
        {
          throw new ArgumentException(
            "CellTemplate of DataGridViewImageComboBoxColumn has to be a DataGridViewImageComboBoxColumnCell");
        }
        base.CellTemplate = value;
      }
    }

    #endregion

    // ========================================================================
    // Methods

    #region === Methods

    /// <returns>
    /// An <see cref="T:System.Object"/> that represents the cloned <see cref="T:System.Windows.Forms.DataGridViewBand"/>.
    /// </returns>
    public override object Clone()
    {
      DataGridViewImageComboBoxColumn clone = base.Clone() as DataGridViewImageComboBoxColumn;
      if(clone != null)
      {
        clone.ImageList = ImageList;
        clone.ImageSize = ImageSize;
        clone.Items = new List<ImageComboBoxItem>(Items);
      }
      return clone;
    }

    #endregion
  }
}