using System.Drawing;
using System.Drawing.Drawing2D;

namespace PDFExport
{
  /// <summary>
  /// Extension-Methods for the Region-Class. Not used yet.
  /// </summary>
  public static class RegionExtend
  {
    // ========================================================================
    // Attributes

    #region === Attributes

    #endregion

    // ========================================================================
    // Con- / Destruktion

    #region === Con- / Destruktion

    #endregion

    // ========================================================================
    // Properties

    #region === Properties

    #endregion

    // ========================================================================
    // Methods

    #region === Methods

    public static GraphicsPath GetPath(this Region region)
    {
      GraphicsPath path = new GraphicsPath();

      path.AddRectangle(new Rectangle(0,0,100,100));

      return path;
    }

    /// <summary>
    /// Enumerates all elements of the region. Calls the matching method for each
    /// element on the given IRegionElementConsumer object.
    /// </summary>
    /// <param name="region">The region to enumerate.</param>
    public static void EnumerateRegion(this Region region)
    {
      
    }

    #endregion

    // ========================================================================
    // Events

    #region === Events


    #endregion

    // ========================================================================
    // Event - Behandlung

    #region === Events - Behandlung


    #endregion

  }
}