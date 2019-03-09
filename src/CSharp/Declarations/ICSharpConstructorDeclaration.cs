using NClass.Core;

namespace NClass.CSharp
{
	public interface ICSharpConstructorDeclaration : IMethodDeclaration
	{
		bool IsStatic { get; }
	}
}