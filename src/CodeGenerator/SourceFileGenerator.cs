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

using System;
using System.IO;
using System.Text;
using NClass.Core;
using System.Text.RegularExpressions;

namespace NClass.CodeGenerator
{
	public abstract class SourceFileGenerator
	{
		// This builder object is static to increase performance
		static StringBuilder codeBuilder;
		const int DefaultBuilderCapacity = 10240; // 10 KB

		TypeBase type;
		string rootNamespace;
		int indentLevel = 0;

		/// <exception cref="ArgumentNullException">
		/// <paramref name="type"/> is null.
		/// </exception>
		protected SourceFileGenerator(TypeBase type, string rootNamespace)
		{
			if (type == null)
				throw new ArgumentNullException("type");

			this.type = type;
			this.rootNamespace = rootNamespace;
		}

		protected TypeBase Type
		{
			get { return type; }
		}

		protected string RootNamespace
		{
			get { return rootNamespace; }
		}

		protected int IndentLevel
		{
			get
			{
				return indentLevel;
			}
			set
			{
				if (value >= 0)
					indentLevel = value;
			}
		}

		protected abstract string Extension
		{
			get;
		}

		/// <exception cref="FileGenerationException">
		/// An error has occured while generating the source file.
		/// </exception>
		public string Generate(string directory)
		{
			try
			{
				if (!Directory.Exists(directory))
					Directory.CreateDirectory(directory);

				string fileName = Type.Name + Extension;
				fileName = Regex.Replace(fileName, @"\<(?<type>.+)\>", @"[${type}]");
				string path = Path.Combine(directory, fileName);

				using (StreamWriter writer = new StreamWriter(path, false))
				{
					WriteFileContent(writer);
				}
				return fileName;
			}
			catch (Exception ex)
			{
				throw new FileGenerationException(directory, ex);
			}
		}

		/// <exception cref="IOException">
		/// An I/O error occurs.
		/// </exception>
		/// <exception cref="ObjectDisposedException">
		/// The <see cref="TextWriter"/> is closed.
		/// </exception>
		private void WriteFileContent(TextWriter writer)
		{
			if (codeBuilder == null)
				codeBuilder = new StringBuilder(DefaultBuilderCapacity);
			else
				codeBuilder.Length = 0;

			WriteFileContent();
			writer.Write(codeBuilder.ToString());
		}

		protected abstract void WriteFileContent();

		internal static void FinishWork()
		{
			codeBuilder = null;
		}

		protected void AddBlankLine()
		{
			AddBlankLine(false);
		}

		protected void AddBlankLine(bool indentation)
		{
			if (indentation)
				AddIndent();
			codeBuilder.AppendLine();
		}

		protected void WriteLine(string text)
		{
			WriteLine(text, true);
		}

		protected void WriteLine(string text, bool indentation)
		{
			if (indentation)
				AddIndent();
			codeBuilder.AppendLine(text);
		}

		private void AddIndent()
		{
			string indentString;
			if (Settings.Default.UseTabsForIndents)
				indentString = new string('\t', IndentLevel);
			else
				indentString = new string(' ', IndentLevel * Settings.Default.IndentSize);

			codeBuilder.Append(indentString);
		}
	}
}