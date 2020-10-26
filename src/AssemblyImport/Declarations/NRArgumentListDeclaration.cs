using NClass.CSharp;
using NReflect.NRParameters;
using System.Collections;
using System.Collections.Generic;

namespace NClass.AssemblyImport
{
    public class NRArgumentListDeclaration : ICSharpArgumentListDeclaration<NRParameterDeclaration>
    {
        readonly IEnumerable<NRParameter> parameters;

        public NRArgumentListDeclaration(IEnumerable<NRParameter> parameters)
        {
            this.parameters = parameters;
        }

        public IEnumerator<NRParameterDeclaration> GetEnumerator()
        {
            foreach (var parameter in parameters)
            {
                yield return new NRParameterDeclaration(parameter);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
