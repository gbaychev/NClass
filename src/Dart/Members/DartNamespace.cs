
using NClass.Core;

namespace NClass.Dart
{
    internal sealed class DartNamespace : Package
    {
        internal DartNamespace() : this("NewNamespace")
        {
        }

        /// <exception cref="BadSyntaxException">
        /// The <paramref name="name"/> does not fit to the syntax.
        /// </exception>
        internal DartNamespace(string name) : base(name)
        {
        }

        public override Language Language
        {
            get { return DartLanguage.Instance; }
        }

        public override string Stereotype
        {
            get { return "«namespace»"; }
        }

        public override string FullName
        {
            get
            {
                var subPackage = NestingParent as Package;

                if (subPackage != null)
                    return subPackage.FullName + "." + Name;
                else
                    return Name;
            }
        }

        public override Package Clone(bool cloneChildren)
        {
            DartNamespace newNamespace = new DartNamespace();
            newNamespace.CopyFrom(this, cloneChildren);
            return newNamespace;
        }
    }
}
