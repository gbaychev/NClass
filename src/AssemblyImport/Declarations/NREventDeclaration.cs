using NClass.Core;
using NClass.CSharp;
using NReflect.NRMembers;

namespace NClass.AssemblyImport
{
	public class NREventDeclaration : ICSharpEventDeclaration
	{
		readonly NREvent nrEvent;

		public NREventDeclaration(NREvent nrEvent)
		{
			this.nrEvent = nrEvent;
		}

		public string Name
		{
			get { return nrEvent.Name; }
		}

		public string Type
		{
			get { return nrEvent.Type.ToNClass(); }
		}

		public AccessModifier AccessModifier
		{
			get { return nrEvent.AccessModifier.ToNClass(); }
		}
	
		public bool IsExplicitImplementation
		{
			get { return false; }
		}

		public bool IsStatic
		{
			get { return nrEvent.IsStatic; }
		}

		public bool IsVirtual
		{
			get { return nrEvent.IsVirtual; }
		}

		public bool IsAbstract
		{
			get { return nrEvent.IsAbstract; }
		}

		public bool IsOverride
		{
			get { return nrEvent.IsOverride; }
		}

		public bool IsSealed
		{
			get { return nrEvent.IsSealed; }
		}

		public bool IsHider
		{
			get { return nrEvent.IsHider; }
		}
	}
}