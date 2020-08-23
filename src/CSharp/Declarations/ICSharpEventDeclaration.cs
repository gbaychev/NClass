using NClass.Core;

namespace NClass.CSharp
{
    public interface ICSharpEventDeclaration : IEventDeclaration
    {
        bool IsExplicitImplementation { get; }
    }
}
