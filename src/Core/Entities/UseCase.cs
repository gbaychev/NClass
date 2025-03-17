// NClass - Free class diagram editor
// Copyright (C) 2025 Georgi Baychev
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
using System.Xml;

namespace NClass.Core
{
    public class UseCase : Element, IUseCaseEntity, INestableChild
    {
        public event SerializeEventHandler Serializing;
        public event SerializeEventHandler Deserializing;
        private string name = string.Empty;
        private readonly NestableChildHelper nestableChildHelper = null;

        public UseCase ExtendedUseCase { get; set; }
        public UseCase IncludedUseCase { get; set; }
        public IUseCaseEntity SpecializedEntity { get; set; }

        public UseCase() : this(string.Empty)
        {
        }

        private UseCase(string name)
        {
            Initializing = true;
            this.Name = name;
            nestableChildHelper = new NestableChildHelper(this);
            nestableChildHelper.NestingParentChanged += (s, a) => Changed();
            Initializing = false;
        }

        public string Name
        {
            get => name;
            set
            {
                if (value == null)
                    value = string.Empty;

                if (name != value)
                {
                    name = value;
                    Changed();
                }
            }
        }
        public EntityType EntityType => EntityType.UseCase;

        public INestableChild CloneChild()
        {
            return Clone();
        }

        public UseCase Clone()
        {
            return new UseCase(this.Name);
        }

        public void Serialize(XmlElement node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            XmlElement child = node.OwnerDocument.CreateElement("Name");
            child.InnerText = Name;
            node.AppendChild(child);

            OnSerializing(new SerializeEventArgs(node));
        }

        private void OnSerializing(SerializeEventArgs e)
        {
            Serializing?.Invoke(this, e);
        }

        public void Deserialize(XmlElement node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            XmlElement textNode = node["Name"];

            if (textNode != null)
                Name = textNode.InnerText;
            else
                Name = null;

            OnDeserializing(new SerializeEventArgs(node));
        }

        private void OnDeserializing(SerializeEventArgs e)
        {
            Deserializing?.Invoke(this, e);
        }

        public override string ToString()
        {
            return string.IsNullOrEmpty(this.name) ? "<use case>" : this.name;
        }

        public virtual INestable NestingParent
        {
            get => nestableChildHelper.NestingParent;
            set => nestableChildHelper.NestingParent = value;
        }
    }
}