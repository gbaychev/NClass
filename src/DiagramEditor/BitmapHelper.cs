// NClass - Free class diagram editor
// Copyright (C) 2019 Georgi Baychev
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

using System.Drawing;
using NClass.Core;
using NClass.DiagramEditor.Properties;

namespace NClass.DiagramEditor
{
    public static class BitmapHelper
    {
        public static Bitmap GetBitmapForDocument(IDocument document)
        {
            if (document is ClassDiagram.ClassDiagram classDiagram)
            {
                if (classDiagram.Language == Language.GetLanguage("csharp"))
                    return Resources.CSharp; 
                if (classDiagram.Language == Language.GetLanguage("java"))
                    return Resources.Java;
            }

            if (document is UseCaseDiagram.UseCaseDiagram)
            {
                return Resources.UseCase;
            }

            return null;
        }
    }
}