using NClass.Core;

namespace NClass.CSharp
{
    public interface ICSharpArgumentListDeclaration<out TParameter> : IArgumentListDeclaration<TParameter> 
        where TParameter : ICSharpParameterDeclaration
    { }
}
