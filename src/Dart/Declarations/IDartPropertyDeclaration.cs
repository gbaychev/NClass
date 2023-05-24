using NClass.Core;

namespace NClass.Dart
{
    public interface IDartPropertyDeclaration : IPropertyDeclaration
    {
        bool IsExplicitImplementation { get; }
    }
}
