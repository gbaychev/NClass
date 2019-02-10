using NClass.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        public override Language Language
        {
            get { return JavaLanguage.Instance; }
        }

        public override string Stereotype
        {
            get { return "«package»"; }
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

        protected override void CopyFrom(Package type)
        {
            base.CopyFrom(type);

            //TODO
        }

        public override Package Clone()
        {
            JavaPackage newPackage = new JavaPackage();
            newPackage.CopyFrom(this);
            return newPackage;
        }
    }
}
