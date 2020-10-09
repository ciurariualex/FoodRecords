namespace Core.Data.Entities.Base
{
    public interface IDeletable
    {
        bool IsDeleted { get; set; }
        void SoftDelete();
    }
}