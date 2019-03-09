using NClass.Core;
using NClass.CSharp;
using NReflect.NRMembers;

namespace NClass.AssemblyImport
{
	public class NRFieldDeclaration : ICSharpFieldDeclaration
	{
		readonly NRField field;

		public NRFieldDeclaration(NRField field)
		{
			this.field = field;
		}

		public string Name
		{
			get { return field.Name; }
		}

		public string Type
		{
			get { return field.Type.ToNClass(); }
		}

		public AccessModifier AccessModifier
		{
			get { return field.AccessModifier.ToNClass(); }
		}

		public bool HasInitialValue
		{
			get { return !string.IsNullOrEmpty(field.InitialValue); }
		}

		public string InitialValue
		{
			get { return field.InitialValue; }
		}

		public bool IsStatic
		{
			get { return field.IsStatic; }
		}

		public bool IsReadonly
		{
			get { return field.IsReadonly; }
		}

		public bool IsConstant
		{
			get { return field.IsConstant; }
		}

		public bool IsHider
		{
			get { return field.IsHider; }
		}

		public bool IsVolatile
		{
			get { return field.IsVolatile; }
		}
	}
}