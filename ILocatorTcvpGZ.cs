using System.IO;
using System.Xml.Serialization;

namespace TcvpTool
{
    public interface ILocatorTcvpGZ : IXmlSerializable
    {
        Vector3 Translation { get; set; }
        TcvpParamsGZ Params { get; set; }

        void Read(BinaryReader reader);
        void Write(BinaryWriter writer);
    }
}
