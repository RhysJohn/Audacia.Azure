using System.Runtime.Serialization;

namespace Audacia.Azure.BlobStorage.Models
{
    /// <summary>
    /// This is all the types of data which a blob can have while using this library.
    /// </summary>
    public enum BlobDataType
    {
        [EnumMember(Value = "Base 64")]
        Base64,
        Stream,
        
        [EnumMember(Value = "Byte Array")]
        ByteArray,
        
        [EnumMember(Value = "File Location")]
        FileLocation
    }
}