using NClass.Core;
using NClass.Core.Entities;
using NReflect;
using NReflect.NRCode;

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

    /// <summary>
    /// Converts the parameter modifier of the NReflect library into the corresponding
    /// NClass parameter modifier.
    /// </summary>
    /// <param name="parameterModifier">The parameter modifier of the NReflect library to convert.</param>
    /// <returns>The converted NClass parameter modifier.</returns>
    public static ParameterModifier ToNClass(this NReflect.Modifier.ParameterModifier parameterModifier)
    {
      switch (parameterModifier)
      {
        case NReflect.Modifier.ParameterModifier.In:
          return ParameterModifier.In;
        case NReflect.Modifier.ParameterModifier.InOut:
          return ParameterModifier.Inout;
        case NReflect.Modifier.ParameterModifier.Out:
          return ParameterModifier.Out;
        case NReflect.Modifier.ParameterModifier.Params:
          return ParameterModifier.Params;
        default:
          return ParameterModifier.In;
      }
    }

    public static string ToNClass(this NRTypeUsage type)
    {
      return type.Declaration().TrimEnd('&');
    }

    #endregion
  }
}
