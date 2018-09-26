using NClass.Core;
using NClass.CSharp;
using NReflect.NRParameters;

namespace NClass.AssemblyImport
{
	public class NRParameterDeclaration : ICSharpParameterDeclaration
	{
		readonly NRParameter parameter;

		public NRParameterDeclaration(NRParameter parameter)
		{
			this.parameter = parameter;
		}

		public string Name
		{
			get { return parameter.Name; }
		}

		public string Type
		{
			get { return parameter.Type.ToNClass(); }
		}

		public ParameterModifier Modifier
		{
			get { return parameter.ParameterModifier.ToNClass(); }
		}

		public bool HasDefaultValue
		{
			get { return !string.IsNullOrEmpty(parameter.DefaultValue); }
		}

		public string DefaultValue
		{
			get { return parameter.DefaultValue; }
		}
	}
}