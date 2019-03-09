using NClass.Core;

namespace NClass.Java
{
	public interface IJavaConstructorDeclaration : IMethodDeclaration
	{
		bool IsStatic { get; }
	}
}