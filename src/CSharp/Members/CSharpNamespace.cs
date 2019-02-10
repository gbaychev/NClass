using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NClass.Core;
using NClass.Translations;

namespace NClass.CSharp
{
    internal sealed class CSharpNamespace : Package
    {
        internal CSharpNamespace() : this("NewNamespace")
		{
        }

        /// <exception cref="BadSyntaxException">
        /// The <paramref name="name"/> does not fit to the syntax.
        /// </exception>
        internal CSharpNamespace(string name) : base(name)
        {
        }

        public override Language Language
        {
            get { return CSharpLanguage.Instance; }
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

        protected override void CopyFrom(Package type)
        {
            base.CopyFrom(type);

            //TODO
        }

        public override Package Clone()
        {
            CSharpNamespace newNamespace = new CSharpNamespace();
            newNamespace.CopyFrom(this);
            return newNamespace;
        }
    }
}
