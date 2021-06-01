using System.IO;
using System.Xml.Serialization;

namespace TcvpTool
{
    public interface ILocatorTcvp : IXmlSerializable
    {
        Vector3 Translation { get; set; }

        void Read(BinaryReader reader);
        void Write(BinaryWriter writer);
    }
}
