namespace Core.Data.Entities.Base
{
    public interface IEntity<T>
    {
        T Id { get; set; }
    }
}