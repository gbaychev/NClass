using NClass.Core;
using NClass.CSharp;
using NReflect.NRMembers;

namespace NClass.AssemblyImport
{
    public class NRPropertyDeclaration : ICSharpPropertyDeclaration
    {
        readonly NRProperty property;

        public NRPropertyDeclaration(NRProperty property)
        {
            this.property = property;
        }

        public string Name
        {
            get { return property.Name; }
        }

        public string Type
        {
            get { return property.Type.ToNClass(); }
        }

        public AccessModifier AccessModifier
        {
            get { return property.AccessModifier.ToNClass(); }
        }

        public IArgumentListDeclaration<IParameterDeclaration> ArgumentList
        {
            get { return new NRArgumentListDeclaration(property.Parameters); }
        }

        public bool IsExplicitImplementation
        {
            get { return false; }
        }

        public bool IsStatic
        {
            get { return property.IsStatic; }
        }

        public bool IsVirtual
        {
            get { return property.IsVirtual; }
        }

        public bool IsAbstract
        {
            get { return property.IsAbstract; }
        }

        public bool IsOverride
        {
            get { return property.IsOverride; }
        }

        public bool IsSealed
        {
            get { return property.IsSealed; }
        }

        public bool IsHider
        {
            get { return property.IsHider; }
        }

        public bool IsReadonly
        {
            get { return property.HasGetter && !property.HasSetter; }
        }

        public bool IsWriteonly
        {
            get { return !property.HasGetter && property.HasSetter; }
        }

        public AccessModifier ReadAccess
        {
            get { return NormalizeAccessModifier(property.GetterModifier.ToNClass()); }
        }

        public AccessModifier WriteAccess
        {
            get { return NormalizeAccessModifier(property.SetterModifier.ToNClass()); }
        }

        private AccessModifier NormalizeAccessModifier(AccessModifier modifier)
        {
            switch (modifier)
            {
                case AccessModifier.Public:
                    return AccessModifier.Default;

                default:
                    return modifier;
            }
        }
    }
}
