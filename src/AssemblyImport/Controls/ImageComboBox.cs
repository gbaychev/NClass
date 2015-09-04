using System.Drawing;
using System.Windows.Forms;

namespace NClass.AssemblyImport.Controls
{
  /// <summary>
  ///   An improved <see cref = "ComboBox" /> which is able to display images next to
  ///   text of each item.
  /// </summary>
  public class ImageComboBox : ComboBox
  {
    // ========================================================================
    // Con- / Destruction

    #region === Con- / Destruction

    /// <summary>
    ///   Initializes a new instance of <see cref = "ImageComboBox" />.
    /// </summary>
    public ImageComboBox()
    {
      DropDownStyle = ComboBoxStyle.DropDownList;
      DrawMode = DrawMode.OwnerDrawFixed;
    }

    #endregion

    // ========================================================================
    // Properties

    #region === Properties

    /// <summary>
    ///   Gets or sets an <see cref = "ImageList" /> to take the images to display from.
    /// </summary>
    public ImageList ImageList { get; set; }

    /// <summary>
    ///   Gets or sets the selected item of the combobox.
    /// </summary>
    public new ImageComboBoxItem SelectedItem
    {
      get { return base.SelectedItem as ImageComboBoxItem; }
      set { base.SelectedItem = value; }
    }

    /// <summary>
    ///   Gets or sets the size of the images.
    /// </summary>
    public Size ImageSize { get; set; }

    #endregion

    // ========================================================================
    // Methods

    #region === Methods

    /// <summary>
    ///   Raises the <see cref = "ComboBox.DrawItem" /> event.
    /// </summary>
    /// <param name = "e">A <see cref = "DrawItemEventArgs" /> that contains the event data. </param>
    protected override void OnDrawItem(DrawItemEventArgs e)
    {
      e.DrawBackground();
      e.DrawFocusRectangle();

      object item = e.Index >= 0 ? Items[e.Index] : null;

      ControlDrawHelper.DrawImageComboBoxItem(e.Graphics, item, ImageList, ImageSize, e.Bounds, Font, ForeColor);

      base.OnDrawItem(e);
    }

    #endregion
  }
}