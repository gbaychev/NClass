using NClass.Core;
using NClass.CSharp;
using NReflect.NRMembers;

namespace NClass.AssemblyImport
{
    public class NROperatorDeclaration : ICSharpMethodDeclaration
    {
        readonly NROperator oper;

        public NROperatorDeclaration(NROperator oper)
        {
            this.oper = oper;
        }

        public string Name
        {
            get { return oper.Name; }
        }

        public string Type
        {
            get { return oper.Type.ToNClass(); }
        }

        public AccessModifier AccessModifier
        {
            get { return oper.AccessModifier.ToNClass(); }
        }

        public IArgumentListDeclaration<IParameterDeclaration> ArgumentList
        {
            get { return new NRArgumentListDeclaration(oper.Parameters); }
        }

        public bool IsOperator
        {
            get { return true; }
        }

        public bool IsConversionOperator
        {
            get { return false; } // TODO: implement
        }

        public bool IsExplicitImplementation
        {
            get { return false; } // TODO: implement
        }

        public bool IsStatic
        {
            get { return oper.IsStatic; }
        }

        public bool IsVirtual
        {
            get { return oper.IsVirtual; }
        }

        public bool IsAbstract
        {
            get { return oper.IsAbstract; }
        }

        public bool IsOverride
        {
            get { return oper.IsOverride; }
        }

        public bool IsSealed
        {
            get { return oper.IsSealed; }
        }

        public bool IsHider
        {
            get { return false; }
        }
    }
}
