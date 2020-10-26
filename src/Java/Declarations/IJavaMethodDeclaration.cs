using NClass.Core;

namespace NClass.Java
{
    public interface IJavaMethodDeclaration : IMethodDeclaration
    {
        bool IsStatic { get; }
        bool IsAbstract { get; }
        bool IsSealed { get; }
    }
}
