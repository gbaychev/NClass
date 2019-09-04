using NClass.Core;

namespace NClass.Java
{
    internal sealed class JavaPackage : Package
    {
        internal JavaPackage() : this("NewPackage")
        {
        }

        /// <exception cref="BadSyntaxException">
        /// The <paramref name="name"/> does not fit to the syntax.
        /// </exception>
        internal JavaPackage(string name) : base(name)
        {
        }

        public override Language Language => JavaLanguage.Instance;

        public override string Stereotype => "«package»";

        public override string FullName
        {
            get
            {
                if (NestingParent is Package subPackage)
                    return subPackage.FullName + "." + Name;
                else
                    return Name;
            }
        }

        public override Package Clone(bool cloneChildren)
        {
            JavaPackage newPackage = new JavaPackage();
            newPackage.CopyFrom(this, cloneChildren);
            return newPackage;
        }
    }
}
