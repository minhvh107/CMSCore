using CMSCore.Data.Enums;

namespace CMSCore.Data.Interfaces
{
    public interface ISwitchable
    {
        Status Status { set; get; }
    }
}