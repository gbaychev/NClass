using NClass.Core;
using NClass.Translations;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace NClass.CSharp
{
    public class CSharpMethodDeclaration : ICSharpMethodDeclaration
    {
        const string OverloadableOperators =
            @"(\+|-|\*|/|%|&|\||\^|!|~|\+\+|--|<<|>>|==|!=|>|<|>=|<=| true| false)";

        // operator <operator>
        const string OperatorPattern = @"operator\s*(?<operator>" + OverloadableOperators + ")";

        // {implicit | explicit} operator <operator>
        const string ConvOperatorPattern = @"(?<convop>implicit|explicit)" +
                                                                             @"\s+operator\s+(?<operator>" + CSharpLanguage.GenericTypePattern2 + ")";

        // [<access>] [<modifier>]
        // { {explicit | implicit | <type>} operator <operator>(<args>) | <type> <name>(<args>) }
        const string MethodPattern =
            @"^\s*" + CSharpLanguage.AccessPattern + CSharpLanguage.OperationModifiersPattern +
            @"((?<name>" + ConvOperatorPattern + ")|" +
            @"(?<type>" + CSharpLanguage.GenericTypePattern2 + @")\s+" +
            @"(?<name>" + OperatorPattern + "|" + CSharpLanguage.GenericOperationNamePattern + "))" +
            @"\s*\((?<args>.*)\)" + CSharpLanguage.DeclarationEnding;
        
        static readonly Regex methodRegex = new Regex(MethodPattern, RegexOptions.ExplicitCapture);

        readonly Match match;
        readonly HashSet<string> modifiers;

        public CSharpMethodDeclaration(Match match)
        {
            if (!match.Success)
            {
                throw new BadSyntaxException(Strings.ErrorInvalidDeclaration);
            }

            this.match = match;
            this.modifiers = GetModifiers(match);
        }

        public static CSharpMethodDeclaration Create(string declaration)
        {
            var match = methodRegex.Match(declaration);
            return new CSharpMethodDeclaration(match);
        }

        public string Name
        {
            get { return match.Groups["name"].Value; }
        }

        public string Type
        {
            get { return match.Groups["type"].Value; }
        }

        public AccessModifier AccessModifier
        {
            get { return ParseAccessModifier(match.Groups["access"].Value); }
        }

        public IArgumentListDeclaration<IParameterDeclaration> ArgumentList
        {
            get { return CSharpArgumentListDeclaration.Create(match.Groups["args"].Value); }
        }

        public bool IsOperator
        {
            get { return match.Groups["operator"].Success; }
        }

        public bool IsConversionOperator
        {
            get { return match.Groups["convop"].Success; }
        }

        public bool IsExplicitImplementation
        {
            get { return match.Groups["namedot"].Success; }
        }

        public bool IsVirtual
        {
            get { return modifiers.Contains("virtual"); }
        }

        public bool IsAbstract
        {
            get { return modifiers.Contains("abstract"); }
        }

        public bool IsOverride
        {
            get { return modifiers.Contains("override"); }
        }

        public bool IsSealed
        {
            get { return modifiers.Contains("sealed"); }
        }

        public bool IsStatic
        {
            get { return modifiers.Contains("static"); }
        }

        public bool IsHider
        {
            get { return modifiers.Contains("new"); }
        }

        private static AccessModifier ParseAccessModifier(string value)
        {
            return CSharpLanguage.Instance.TryParseAccessModifier(value);
        }

        private static HashSet<string> GetModifiers(Match match)
        {
            var captures = match.Groups["modifier"].Captures;
            var modifiers = new HashSet<string>();

            foreach (Capture capture in captures)
            {
                modifiers.Add(capture.Value);
            }

            return modifiers;
        }
    }
}
