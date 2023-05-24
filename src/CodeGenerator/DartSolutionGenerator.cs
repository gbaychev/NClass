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
using NClass.Core;
using NClass.Dart;
using NClass.Core.Models;

namespace NClass.CodeGenerator
{
    internal sealed class DartSolutionGenerator : SolutionGenerator
    {
        SolutionType version = SolutionType.Dart;

        /// <exception cref="ArgumentNullException">
        /// <paramref name="project"/> is null.
        /// </exception>
        public DartSolutionGenerator(Project project, SolutionType version) : base(project)
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
                if (value == SolutionType.Dart)
                {
                    version = value;
                }
            }
        }

        /// <exception cref="ArgumentException">
        /// The <paramref name="model"/> has invalid language.
        /// </exception>
        protected override ProjectGenerator CreateProjectGenerator(ClassModel model)
        {
            Language language = model.Language;

            if (language == DartLanguage.Instance)
                return new DartProjectGenerator(model, Version);

            throw new ArgumentException("The model is not a Dart language model.");
        }

        // No solution file for Dart
        protected override bool GenerateSolutionFile(string location)
        {
            return true;
        }

    }
}
