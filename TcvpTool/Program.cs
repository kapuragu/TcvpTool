using System;
using System.IO;
using System.Text;
using System.Xml;

namespace TcvpTool
{
    internal static class Program
    {

        private static void Main(string[] args)
        {

            foreach (var tcvpPath in args)
            {
                if (File.Exists(tcvpPath))
                {
                    // Read input file
                    string fileExtension = Path.GetExtension(tcvpPath);
                    if (fileExtension.Equals(".xml", StringComparison.OrdinalIgnoreCase))
                    {
                        TcvpFile tcvp = ReadFromXml(tcvpPath);
                        WriteToBinary(tcvp, Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(tcvpPath)) + ".tcvp");
                    }
                    else if (fileExtension.Equals(".tcvp", StringComparison.OrdinalIgnoreCase))
                    {
                        TcvpFile tcvp = ReadFromBinary(tcvpPath);
                        WriteToXml(tcvp, Path.GetFileNameWithoutExtension(tcvpPath) + ".tcvp.xml");
                    }
                    else
                    {
                        throw new IOException("Unrecognized input type.");
                    }
                }
            }

        }

        public static void WriteToBinary(TcvpFile tcvp, string path)
        {
            using (BinaryWriter writer = new BinaryWriter(new FileStream(path, FileMode.Create)))
            {
                tcvp.Write(writer);
            }
        }

        public static TcvpFile ReadFromBinary(string path)
        {
            TcvpFile tcvp = new TcvpFile();
            using (BinaryReader reader = new BinaryReader(new FileStream(path, FileMode.Open)))
            {
                tcvp.Read(reader);
            }
            return tcvp;
        }

        public static void WriteToXml(TcvpFile tcvp, string path)
        {
            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings()
            {
                Encoding = Encoding.UTF8,
                Indent = true
            };
            using (var writer = XmlWriter.Create(path, xmlWriterSettings))
            {
                tcvp.WriteXml(writer);
            }
        }

        public static TcvpFile ReadFromXml(string path)
        {
            XmlReaderSettings xmlReaderSettings = new XmlReaderSettings
            {
                IgnoreWhitespace = true
            };

            TcvpFile tcvp = new TcvpFile();
            using (var reader = XmlReader.Create(path, xmlReaderSettings))
            {
                tcvp.ReadXml(reader);
            }
            return tcvp;
        }

    }
}
