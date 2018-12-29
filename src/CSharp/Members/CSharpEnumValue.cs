// NClass - Free class diagram editor
// Copyright (C) 2006-2009 Balazs Tihanyi
// 
// This program is free software; you can redistribute it and/or modify it under 
// the terms of the GNU General Public License as published by the Free Software 
// Foundation; either version 3 of the License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful, but WITHOUT 
// ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS 
// FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License along with 
// this program; if not, write to the Free Software Foundation, Inc., 
// 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA

using System.Text.RegularExpressions;
using NClass.Core;
using NClass.Translations;

namespace NClass.CSharp
{
	internal sealed class CSharpEnumValue : EnumValue
	{
		// <name> [= value]
		const string EnumNamePattern = "(?<name>" + CSharpLanguage.NamePattern + ")";
		const string EnumItemPattern = @"^\s*" + EnumNamePattern +
			@"(\s*=\s*(?<value>\d+))?\s*$";

		static Regex enumItemRegex = new Regex(EnumItemPattern, RegexOptions.ExplicitCapture);
				
		int? initValue;

		/// <exception cref="BadSyntaxException">
		/// The <paramref name="declaration"/> does not fit to the syntax.
		/// </exception>
		internal CSharpEnumValue(string declaration) : base(declaration)
		{
		}

		public int? InitValue
		{
			get { return initValue; }
		}

		/// <exception cref="BadSyntaxException">
		/// The <paramref name="declaration"/> does not fit to the syntax.
		/// </exception>
		public override void InitFromString(string declaration)
		{
			Match match = enumItemRegex.Match(declaration);

			try {
				RaiseChangedEvent = false;

				if (match.Success) {
					Group nameGroup = match.Groups["name"];
					Group valueGroup = match.Groups["value"];

					Name = nameGroup.Value;
				  if (valueGroup.Success)
          {
            int intValue;
            if(int.TryParse(valueGroup.Value, out intValue))
              initValue = intValue;
            else
              initValue = null;
          }
          else
          {
            initValue = null;
          }
				}
				else {
					throw new BadSyntaxException(Strings.ErrorInvalidDeclaration);
				}
			}
			finally {
				RaiseChangedEvent = true;
			}
		}

		public override string GetDeclaration()
		{
		  if (InitValue == null)
				return Name;
		  return Name + " = " + InitValue;
		}

	  protected override EnumValue Clone()
		{
			return new CSharpEnumValue(GetDeclaration());
		}
	}
}
