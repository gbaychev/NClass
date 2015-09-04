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
using System.Collections.Generic;

namespace NClass.DiagramEditor
{
	public class DocumentManager
	{
		List<IDocument> documents = new List<IDocument>();
		IDocument activeDocument = null;
		OrderedList<IDocument> documentHistory = new OrderedList<IDocument>();
		LinkedListNode<IDocument> switchingNode = null;

		public event DocumentEventHandler ActiveDocumentChanged;
		public event DocumentEventHandler DocumentAdded;
		public event DocumentEventHandler DocumentRemoved;
		public event DocumentMovedEventHandler DocumentMoved;

		public IEnumerable<IDocument> Documents
		{
			get { return documents; }
		}

		public IEnumerable<IDocument> DocumentHistory
		{
			get { return documentHistory; }
		}

		public int DocumentCount
		{
			get { return documents.Count; }
		}

		public bool HasDocument
		{
			get { return (documents.Count > 0); }
		}

		public IDocument ActiveDocument
		{
			get
			{
				return activeDocument;
			}
			set
			{
				if (activeDocument != value && documents.Contains(value))
				{
					IDocument oldDocument = activeDocument;
					if (!SwitchingTabs)
						documentHistory.ShiftToFirstPlace(value);
					activeDocument = value;
					OnActiveDocumentChanged(new DocumentEventArgs(oldDocument));
				}
			}
		}

		public bool SwitchingTabs
		{
			get { return (switchingNode != null); }
		}

		private void document_Closing(object sender, EventArgs e)
		{
			IDocument document = (IDocument) sender;
			Close(document);
		}

		/// <exception cref="ArgumentNullException">
		/// <paramref name="document"/> is null.
		/// </exception>
		public void AddOrActivate(IDocument document)
		{
			if (document == null)
				throw new ArgumentNullException("document");

			EndSwitching();

			IDocument oldDocument = activeDocument;
			if (documents.Contains(document))
			{
				if (activeDocument != document)
				{
					activeDocument = document;
					documentHistory.ShiftToFirstPlace(document);
					OnActiveDocumentChanged(new DocumentEventArgs(oldDocument));
				}
			}
			else
			{
				documents.Add(document);
				activeDocument = document;
				documentHistory.AddFirst(document);
				document.Closing += new EventHandler(document_Closing);
				OnDocumentAdded(new DocumentEventArgs(document));
				OnActiveDocumentChanged(new DocumentEventArgs(oldDocument));
			}
		}

		public void MoveDocument(IDocument document, int places)
		{
			int index = documents.IndexOf(document);

			if (index >= 0 && places != 0)
			{
				int position = index;
				if (places > 0)
				{
					while (position < index + places && position < DocumentCount - 1)
					{
						documents[position] = documents[position + 1];
						position++;
					}
				}
				else // places < 0
				{
					while (position > index + places && position > 0)
					{
						documents[position] = documents[position - 1];
						position--;
					}
				}

				documents[position] = document;
				OnDocumentMoved(new DocumentMovedEventArgs(document, index, position));
			}
		}

		public bool Close(IDocument document)
		{
			EndSwitching();

			if (documents.Remove(document))
			{
				documentHistory.Remove(document);
				document.Closing -= new EventHandler(document_Closing);
				OnDocumentRemoved(new DocumentEventArgs(document));
				if (activeDocument == document)
				{
					IDocument oldDocument = activeDocument;
					if (HasDocument)
						activeDocument = documentHistory.FirstValue;
					else
						activeDocument = null;
					OnActiveDocumentChanged(new DocumentEventArgs(oldDocument));
				}
				return true;
			}
			else
			{
				return false;
			}
		}

		public void CloseAll()
		{
			EndSwitching();

			if (HasDocument)
			{
				while (documents.Count > 0)
				{
					IDocument document = documents[documents.Count - 1];
					documents.RemoveAt(documents.Count - 1);
					document.Closing -= new EventHandler(document_Closing);
					OnDocumentRemoved(new DocumentEventArgs(document));
				}

				documentHistory.Clear();

				if (activeDocument != null)
				{
					IDocument oldDocument = activeDocument;
					activeDocument = null;
					OnActiveDocumentChanged(new DocumentEventArgs(oldDocument));
				}
			}
		}

		public void CloseAllOthers(IDocument exception)
		{
			EndSwitching();

			if (HasDocument && documents.Count >= 2)
			{
				while (documents.Count >= 2)
				{
					IDocument document = documents[documents.Count - 1];
					if (document != exception)
					{
						documents.RemoveAt(documents.Count - 1);
					}
					else
					{
						document = documents[documents.Count - 2];
						documents.RemoveAt(documents.Count - 2);
					}
					document.Closing -= new EventHandler(document_Closing);
					OnDocumentRemoved(new DocumentEventArgs(document));
				}

				documentHistory.Clear();
				documentHistory.Add(exception);

				if (activeDocument != exception)
				{
					IDocument oldDocument = activeDocument;
					activeDocument = exception;
					OnActiveDocumentChanged(new DocumentEventArgs(oldDocument));
				}
			}
		}

		public void SwitchDocument()
		{
			if (DocumentCount >= 2)
			{
				if (switchingNode == null)
					switchingNode = documentHistory.First;

				switchingNode = switchingNode.Next;
				if (switchingNode == null)
					switchingNode = documentHistory.First;

				ActiveDocument = switchingNode.Value;
			}
		}

		public void EndSwitching()
		{
			if (SwitchingTabs)
			{
				switchingNode = null;
				if (HasDocument)
					documentHistory.ShiftToFirstPlace(ActiveDocument);
			}
		}

		protected virtual void OnActiveDocumentChanged(DocumentEventArgs e)
		{
			if (ActiveDocumentChanged != null)
				ActiveDocumentChanged(this, e);
		}

		protected virtual void OnDocumentAdded(DocumentEventArgs e)
		{
			if (DocumentAdded != null)
				DocumentAdded(this, e);
		}

		protected virtual void OnDocumentRemoved(DocumentEventArgs e)
		{
			if (DocumentRemoved != null)
				DocumentRemoved(this, e);
		}

		protected virtual void OnDocumentMoved(DocumentMovedEventArgs e)
		{
			if (DocumentMoved != null)
				DocumentMoved(this, e);
		}
	}
}
