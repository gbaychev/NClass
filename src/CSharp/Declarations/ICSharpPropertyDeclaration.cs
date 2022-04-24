using NClass.Core;

namespace NClass.CSharp
{
    public interface ICSharpPropertyDeclaration : IPropertyDeclaration
    {
        bool IsExplicitImplementation { get; }
    }
}
