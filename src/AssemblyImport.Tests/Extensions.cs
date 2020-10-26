using System.IO;
using System.Text;
using System.Xml;
using NClass.DiagramEditor.ClassDiagram;

namespace AssemblyImport.Tests
{
    public static class Extensions
    {
        public static string ToXml(this ClassDiagram diagram)
        {
            using (var stream = new MemoryStream())
            using (var reader = new StreamReader(stream))
            using (var writer = new XmlTextWriter(stream, Encoding.UTF8))
            {
                writer.Formatting = Formatting.Indented;

                var doc = new XmlDocument();
                var root = doc.CreateElement("ProjectItem");

                doc.AppendChild(root);
                diagram.Serialize(root);

                doc.Save(writer);
                stream.Seek(0, SeekOrigin.Begin);

                return reader.ReadToEnd();
            }
        }
    }
}
