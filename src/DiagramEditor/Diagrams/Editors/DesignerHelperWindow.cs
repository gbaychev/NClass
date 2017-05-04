// NClass - Free class diagram editor
// Copyright (C) 2006-2009 Balazs Tihanyi
// Copyright (C) 2016 Georgi Baychev
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

#if DEBUG
namespace NClass.DiagramEditor.Diagrams.Editors
{
    /// <summary>
    /// The only purpose of this class is to have a non-abstract
    /// class, from which the other editor windows can derive from,
    /// so that the editor window can be shown in the Visual Studio 
    /// designer. For more information check this link out:
    /// http://stackoverflow.com/questions/481305/the-designer-must-create-an-instance-of-cannot-because-the-type-is-declared-ab
    /// </summary>
    public partial class DesignerHelperWindow : EditorWindow
    {
        public DesignerHelperWindow()
        {
            InitializeComponent();
        }

        internal override void Init(DiagramElement element)
        {
            throw new System.NotImplementedException();
        }

        internal override void Relocate(DiagramElement element)
        {
            throw new System.NotImplementedException();
        }

        public override void ValidateData()
        {
            throw new System.NotImplementedException();
        }
    }
}
#endif