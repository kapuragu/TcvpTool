﻿using System;
using System.Globalization;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace TcvpTool
{
    public class Vector3 : IXmlSerializable
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        
        public virtual void Read(BinaryReader reader)
        {
            X = reader.ReadSingle();
            Y = reader.ReadSingle();
            Z = reader.ReadSingle();
            Console.WriteLine($"Read Vector3 X: {X}, Y: {Y}, Z: {Z}");
        }

        public virtual void Write(BinaryWriter writer)
        {
            writer.Write(X);
            writer.Write(Y);
            writer.Write(Z);
            Console.WriteLine($"Write Vector3 X: {X}, Y: {Y}, Z: {Z}");
        }

        public virtual void ReadXml(XmlReader reader)
        {
            X = Extensions.ParseFloatRoundtrip(reader["x"]);
            Y = Extensions.ParseFloatRoundtrip(reader["y"]);
            Z = Extensions.ParseFloatRoundtrip(reader["z"]);
            Console.WriteLine($"Read XML Vector3 X: {X}, Y: {Y}, Z: {Z}");
        }

        public virtual void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("x", X.ToString(CultureInfo.InvariantCulture));
            writer.WriteAttributeString("y", Y.ToString(CultureInfo.InvariantCulture));
            writer.WriteAttributeString("z", Z.ToString(CultureInfo.InvariantCulture));
            Console.WriteLine($"Write XML Vector3 X: {X}, Y: {Y}, Z: {Z}");
        }

        public XmlSchema GetSchema()
        {
            return null;
        }
    }
}
