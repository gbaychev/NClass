using System.Drawing;
using System.Windows.Forms;

namespace NClass.AssemblyImport.Controls
{
  public class ControlDrawHelper
  {
    // ========================================================================
    // Methods

    #region === Methods

    /// <summary>
    /// Draws a single combobox item.
    /// </summary>
    /// <param name="graphics">An instance of <see cref="Graphics"/> used to draw the item.</param>
    /// <param name="value">The item to draw. If null, nothing is drawn.</param>
    /// <param name="imageList">An <see cref="ImageList"/> to take the images from (if no image is given via the item).</param>
    /// <param name="imageSize">The size of the image area. If no image is present, this place won't be drawn.</param>
    /// <param name="bounds">A <see cref="Rectangle"/> representing the area of the item.</param>
    /// <param name="font">The font of the text.</param>
    /// <param name="foreColor">The color of the text.</param>
    public static void DrawImageComboBoxItem(Graphics graphics, object value, ImageList imageList, Size imageSize,
                                             Rectangle bounds, Font font, Color foreColor)
    {
      int left = bounds.Left;
      int top;
      string text = "";
      Image image = null;

      if(value != null)
      {
        ImageComboBoxItem item = value as ImageComboBoxItem;
        if(item != null)
        {
          text = item.Text;

          if(item.Image != null)
          {
            image = item.Image;
          }
          else if(imageList != null && item.ImageIndex >= 0)
          {
            image = imageList.Images[item.ImageIndex];
          }
        }
        else
        {
          //Not an ImageComboBoxItem
          text = value.ToString();
        }
      }
      if(image != null)
      {
        top = (bounds.Height - image.Height)/2 + bounds.Top;
        graphics.DrawImage(image, left, top, imageSize.Width, imageSize.Height);
      }
      left += imageSize.Width;

      top = (int)((bounds.Height - graphics.MeasureString(text, font).Height)/2 + bounds.Top);
      graphics.DrawString(text, font, new SolidBrush(foreColor), left, top);
    }

    #endregion
  }
}