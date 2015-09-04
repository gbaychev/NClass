using System;
using System.Drawing;

namespace NClass.AssemblyImport.Controls
{
  /// <summary>
  ///   Represents an item of a <see cref = "ImageComboBox" />.
  /// </summary>
  [Serializable]
  public class ImageComboBoxItem
  {
    // ========================================================================
    // Con- / Destruction

    #region === Con- / Destruction

    /// <summary>
    ///   Initializes a new instance of <see cref = "ImageComboBoxItem" />.
    /// </summary>
    public ImageComboBoxItem()
      : this("", -1, null)
    {
    }

    /// <summary>
    ///   Initializes a new instance of <see cref = "ImageComboBoxItem" />.
    /// </summary>
    /// <param name = "text">The text ot the item.</param>
    public ImageComboBoxItem(string text)
      : this(text, -1, null)
    {
    }

    /// <summary>
    ///   Initializes a new instance of <see cref = "ImageComboBoxItem" />.
    /// </summary>
    /// <param name = "text">The text ot the item.</param>
    /// <param name = "imageIndex">The image index of the item.</param>
    public ImageComboBoxItem(string text, int imageIndex)
      : this(text, imageIndex, null)
    {
    }

    /// <summary>
    ///   Initializes a new instance of <see cref = "ImageComboBoxItem" />.
    /// </summary>
    /// <param name = "text">The text ot the item.</param>
    /// <param name = "imageIndex">The image index of the item.</param>
    /// <param name = "tag">A user defined value of the item.</param>
    public ImageComboBoxItem(string text, int imageIndex, Object tag)
    {
      Text = text;
      ImageIndex = imageIndex;
      Tag = tag;
    }

    /// <summary>
    ///   Initializes a new instance of <see cref = "ImageComboBoxItem" />.
    /// </summary>
    /// <param name = "text">The text ot the item.</param>
    /// <param name = "image">The image of the item.</param>
    public ImageComboBoxItem(string text, Image image)
      : this(text, image, null)
    {
    }

    /// <summary>
    ///   Initializes a new instance of <see cref = "ImageComboBoxItem" />.
    /// </summary>
    /// <param name = "text">The text ot the item.</param>
    /// <param name = "image">The image of the item.</param>
    /// <param name = "tag">A user defined value of the item.</param>
    public ImageComboBoxItem(string text, Image image, Object tag)
    {
      Text = text;
      Image = image;
      Tag = tag;
      ImageIndex = -1;
    }

    #endregion

    // ========================================================================
    // Properties

    #region === Properties

    /// <summary>
    ///   Gets or sets the text of the item.
    /// </summary>
    public string Text { get; set; }

    /// <summary>
    ///   Gets or sets the image index of the item.
    /// </summary>
    public int ImageIndex { get; set; }

    /// <summary>
    /// Gets or sets the image of the item.
    /// </summary>
    public Image Image { get; set; }

    /// <summary>
    /// Gets or sets a user defined value of the item.
    /// </summary>
    public Object Tag { get; set; }

    #endregion

    // ========================================================================
    // Methods

    #region === Methods

    /// <summary>
    ///   Returns a <see cref = "string" /> that represents the current <see cref = "ImageComboBoxItem" />.
    /// </summary>
    /// <returns>
    ///   A <see cref = "string" /> that represents the current <see cref = "ImageComboBoxItem" />.
    /// </returns>
    public override string ToString()
    {
      return Text;
    }

    #endregion
  }
}