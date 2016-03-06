// NClass - Free class diagram editor
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

using System;
using System.Xml;

namespace NClass.Core
{
    public class Actor : IEntity
    {
        public event SerializeEventHandler Serializing;
        public event SerializeEventHandler Deserializing;
        public void Serialize(XmlElement node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            XmlElement child;

            child = node.OwnerDocument.CreateElement("Name");
            child.InnerText = Name;
            node.AppendChild(child);

            OnSerializing(new SerializeEventArgs(node));
        }

        public void Deserialize(XmlElement node)
        {
            throw new NotImplementedException();
        }

        public event EventHandler Modified;
        public bool IsDirty { get; }
        public void Clean()
        {
            throw new NotImplementedException();
        }

        public string Name { get; set; }

        public EntityType EntityType
        {
            get { return EntityType.Actor;}
        }

        private void OnSerializing(SerializeEventArgs e)
        {
            if (Serializing != null)
                Serializing(this, e);
        }
    }
}