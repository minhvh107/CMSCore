namespace CMSCore.Data.Interfaces
{
    public interface IHasSoftDelete
    {
        bool IsDelete { set; get; }
    }
}