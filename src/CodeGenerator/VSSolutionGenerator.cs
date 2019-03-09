// NClass - Free class diagram editor
// Copyright (C) 2006-2009 Balazs Tihanyi
// Copyright (C) 2016-2018 Georgi Baychev
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
using System.Windows.Forms;
using NClass.Core;
using NClass.CSharp;
using NClass.Java;
using NClass.Core.Models;

namespace NClass.CodeGenerator
{
	internal sealed class VSSolutionGenerator : SolutionGenerator
	{
		SolutionType version = SolutionType.VisualStudio2017;

		/// <exception cref="ArgumentNullException">
		/// <paramref name="project"/> is null.
		/// </exception>
		public VSSolutionGenerator(Project project, SolutionType version) : base(project)
		{
			Version = version;
		}

		public SolutionType Version
		{
			get
			{
				return version;
			}
			set
			{
				if (value == SolutionType.VisualStudio2015 ||
					value == SolutionType.VisualStudio2017)
				{
					version = value;
				}
			}
		}

        private string VersionNumber => "12.00";

        private string VersionString
		{
			get
			{
				if (Version == SolutionType.VisualStudio2015)
					return "Visual Studio 14";
				else
					return "Visual Studio 15";
			}
		}

		/// <exception cref="ArgumentException">
		/// The <paramref name="model"/> has invalid language.
		/// </exception>
		protected override ProjectGenerator CreateProjectGenerator(ClassModel model)
		{
			Language language = model.Language;

			if (language == CSharpLanguage.Instance)
				return new CSharpProjectGenerator(model, Version);
			if (language == JavaLanguage.Instance)
				return new JavaProjectGenerator(model);

			throw new ArgumentException("The model has an unknown language.");
		}

		protected override bool GenerateSolutionFile(string location)
		{
			try
			{
				string templateDir = Path.Combine(Application.StartupPath, "Templates");
				string templatePath = Path.Combine(templateDir, "sln.template");
				string solutionDir = Path.Combine(location, SolutionName);
				string solutionPath = Path.Combine(solutionDir, SolutionName + ".sln");

				using (StreamReader reader = new StreamReader(templatePath))
				using (StreamWriter writer = new StreamWriter(
					solutionPath, false, reader.CurrentEncoding))
				{
					while (!reader.EndOfStream)
					{
						CopyLine(reader, writer);
					}
				}
				return true;
			}
			catch
			{
				return false;
			}
		}

		private void CopyLine(StreamReader reader, StreamWriter writer)
		{
			string line = reader.ReadLine();

			line = line.Replace("${VersionNumber}", VersionNumber);
			line = line.Replace("${VersionString}", VersionString);

			if (line.Contains("${ProjectFile}"))
			{
				string nextLine = reader.ReadLine();
				foreach (ProjectGenerator generator in ProjectGenerators)
				{
					string newLine = line.Replace("${ProjectFile}",
						generator.RelativeProjectFileName);
					newLine = newLine.Replace("${ProjectName}", generator.ProjectName);
					
					writer.WriteLine(newLine);
					writer.WriteLine(nextLine);
				}
			}
			else
			{
				writer.WriteLine(line);
			}
		}
	}
}
