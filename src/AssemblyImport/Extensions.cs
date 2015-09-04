using NClass.Core;

namespace NClass.AssemblyImport
{
  public static class Extensions
  {
    // ========================================================================
    // Methods

    #region === Methods

    /// <summary>
    /// Converts the access modifier of the NReflect library into the corresponding
    /// NClass access modifier.
    /// </summary>
    /// <param name="accessModifier">The access modifier of the NReflect library to convert.</param>
    /// <returns>The converted NClass access modifier.</returns>
    public static AccessModifier ToNClass(this NReflect.Modifier.AccessModifier accessModifier)
    {
      switch (accessModifier)
      {
        case NReflect.Modifier.AccessModifier.Default:
          return AccessModifier.Default;
        case NReflect.Modifier.AccessModifier.Public:
          return AccessModifier.Public;
        case NReflect.Modifier.AccessModifier.ProtectedInternal:
          return AccessModifier.ProtectedInternal;
        case NReflect.Modifier.AccessModifier.Internal:
          return AccessModifier.Internal;
        case NReflect.Modifier.AccessModifier.Protected:
          return AccessModifier.Protected;
        case NReflect.Modifier.AccessModifier.Private:
          return AccessModifier.Private;
        default:
          return AccessModifier.Default;
      }
    }

    /// <summary>
    /// Converts the class modifier of the NReflect library into the corresponding
    /// NClass class modifier.
    /// </summary>
    /// <param name="classModifier">The class modifier of the NReflect library to convert.</param>
    /// <returns>The converted NClass class modifier.</returns>
    public static ClassModifier ToNClass(this NReflect.Modifier.ClassModifier classModifier)
    {
      switch (classModifier)
      {
        case NReflect.Modifier.ClassModifier.None:
          return ClassModifier.None;
        case NReflect.Modifier.ClassModifier.Abstract:
          return ClassModifier.Abstract;
        case NReflect.Modifier.ClassModifier.Sealed:
          return ClassModifier.Sealed;
        case NReflect.Modifier.ClassModifier.Static:
          return ClassModifier.Static;
        default:
          return ClassModifier.None;
      }
    }

    #endregion
  }
}
