using System.Runtime.Serialization;

namespace Audacia.Azure.ReturnOptions.ImageOption
{
    public enum ImageType
    {
        [EnumMember(Value = "png")]
        Png,
        [EnumMember(Value = "jpg")]
        Jpg
    }
}