using NClass.Core;
using NClass.CSharp;
using NReflect.NRCode;
using NReflect.NRMembers;

namespace NClass.AssemblyImport
{
    public class NRConstructorDeclaration : ICSharpConstructorDeclaration
    {
        readonly NRConstructor constructor;

        public NRConstructorDeclaration(NRConstructor constructor)
        {
            this.constructor = constructor;
        }

        public string Name
        {
            get { return constructor.Name; }
        }

        public string Type
        {
            get { return constructor.Type.Declaration(); }
        }

        public AccessModifier AccessModifier
        {
            get { return constructor.AccessModifier.ToNClass(); }
        }

        public IArgumentListDeclaration<IParameterDeclaration> ArgumentList
        {
            get { return new NRArgumentListDeclaration(constructor.Parameters); }
        }

        public bool IsStatic
        {
            get { return constructor.IsStatic; }
        }
    }
}
