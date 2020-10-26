using NClass.Core;

namespace NClass.CSharp
{
    public interface ICSharpFieldDeclaration : IFieldDeclaration
    {
        bool IsHider { get; }
    }
}
