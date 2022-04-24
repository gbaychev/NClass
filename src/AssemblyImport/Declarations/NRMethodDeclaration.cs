using NClass.Core;
using NClass.CSharp;
using NReflect.NRMembers;

namespace NClass.AssemblyImport
{
    public class NRMethodDeclaration : ICSharpMethodDeclaration
    {
        readonly NRMethod method;

        public NRMethodDeclaration(NRMethod method)
        {
            this.method = method;
        }

        public string Name
        {
            get { return method.Name; }
        }

        public string Type
        {
            get { return method.Type.ToNClass(); }
        }

        public AccessModifier AccessModifier
        {
            get { return method.AccessModifier.ToNClass(); }
        }

        public IArgumentListDeclaration<IParameterDeclaration> ArgumentList
        {
            get { return new NRArgumentListDeclaration(method.Parameters); }
        }

        public bool IsOperator
        {
            get { return false; }
        }

        public bool IsConversionOperator
        {
            get { return false; }
        }

        public bool IsExplicitImplementation
        {
            get { return false; }
        }

        public bool IsStatic
        {
            get { return method.IsStatic; }
        }

        public bool IsVirtual
        {
            get { return method.IsVirtual; }
        }

        public bool IsAbstract
        {
            get { return method.IsAbstract; }
        }

        public bool IsOverride
        {
            get { return method.IsOverride; }
        }

        public bool IsSealed
        {
            get { return method.IsSealed; }
        }

        public bool IsHider
        {
            get { return method.IsHider; }
        }
    }
}
