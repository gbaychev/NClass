using NClass.Core;

namespace NClass.Java
{
	public interface IJavaArgumentListDeclaration<out TParameter> : IArgumentListDeclaration<TParameter> 
		where TParameter : IJavaParameterDeclaration
	{ }
}