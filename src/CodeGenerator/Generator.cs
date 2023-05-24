﻿// NClass - Free class diagram editor
// Copyright (C) 2006-2009 Balazs Tihanyi
// Copyright (C) 2020 Georgi Baychev

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

namespace NClass.CodeGenerator
{
    public sealed class Generator
    {
        readonly SolutionGenerator solutionGenerator;

        /// <exception cref="ArgumentNullException">
        /// <paramref name="project"/> is null.
        /// </exception>
        public Generator(Project project, SolutionType type)
        {
            if (project == null)
                throw new ArgumentNullException("project");

            solutionGenerator = CreateSolutionGenerator(project, type);
        }

        private SolutionGenerator CreateSolutionGenerator(Project project, SolutionType type)
        {
            if (type == SolutionType.Dart)
            {
                return new DartSolutionGenerator(project, type);
            }

            return new VSSolutionGenerator(project, type);

        }

        /// <exception cref="ArgumentException">
        /// <paramref name="location"/> contains invalid path characters.
        /// </exception>
        public GenerationResult Generate(string location)
        {
            GenerationResult result = solutionGenerator.Generate(location);
            SourceFileGenerator.FinishWork();

            return result;
        }
    }
}
