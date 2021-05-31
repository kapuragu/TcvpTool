using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace TcvpTool
{
    public enum Version
    {
        GZ = 0,
        TPP = 1
    }

    public class TcvpFile : IXmlSerializable
    {
        public Version version;
        public List<ILocatorTcvpGZ> Locators = new List<ILocatorTcvpGZ>();

        /// <summary>
        /// Reads and populates data from a binary lba file.
        /// </summary>
        public void Read(BinaryReader reader)
        {
            // Read header
            char[] signature = reader.ReadChars(4); //TCVP string
            if (new string(signature) != "TCVP")
            {
                throw new ArgumentOutOfRangeException();
            }
            version = (Version) reader.ReadUInt16();
            switch (version)
            {
                case Version.GZ:
                    break;
                case Version.TPP:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            ushort locatorCount = reader.ReadUInt16();
            reader.ReadUInt32(); // something/12

            // Read locators
            for (int i = 0; i < locatorCount; i++)
            {
                ILocatorTcvpGZ locator = new TcvpLocatorGZ();
                locator.Read(reader);
                Locators.Add(locator);
            }

        }

        /// <summary>
        /// Writes data to a binary lba file.
        /// </summary>
        public void Write(BinaryWriter writer)
        {
            // Write header
            writer.Write('T'); writer.Write('C'); writer.Write('V'); writer.Write('P');
            writer.Write((short)version);
            writer.Write((ushort)Locators.Count);
            writer.Write(12);

            // Write locators
            foreach (var locator in Locators)
            {
                locator.Write(writer);
            }
        }

        /// <summary>
        /// Reads and populates data from an xml file.
        /// </summary>
        public void ReadXml(XmlReader reader)
        {
            reader.Read();
            reader.Read();

            version = (Version)short.Parse(reader["version"]);

            reader.ReadStartElement("tcvp");
            while (2 > 1)
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        ILocatorTcvpGZ newLocator = CreateLocator();
                        newLocator.ReadXml(reader);
                        Locators.Add(newLocator);
                        reader.ReadEndElement();
                        continue;
                    case XmlNodeType.EndElement:
                        return;
                }
            }
        }

        /// <summary>
        /// Writes data to an xml file.
        /// </summary>
        /// <param name="writer"></param>
        public void WriteXml(XmlWriter writer)
        {
            writer.WriteStartDocument();
            writer.WriteStartElement("tcvp");
            writer.WriteAttributeString("version", ((short)version).ToString());

            foreach (ILocatorTcvpGZ locator in Locators)
            {
                writer.WriteStartElement("locator");
                locator.WriteXml(writer);
                writer.WriteEndElement();
            }
            writer.WriteEndDocument();
        }
        
        ILocatorTcvpGZ CreateLocator()
        {
            return new TcvpLocatorGZ();
        }

        public XmlSchema GetSchema()
        {
            return null;
        }
    }
}
