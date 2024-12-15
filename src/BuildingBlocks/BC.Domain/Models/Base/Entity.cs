namespace BC.Domain.Models.Base
{
    public abstract class Entity<T> : IEntity<T>
    {
        public T Id { get; set; }

        public string Company {  get; set; }    

        public string CreatedBy { get; set; } = default!;

        public DateTime CreatedOn { get; set; } = default!;

        public string? LastModifiedBy { get; set; } = default!;

        public DateTime? LastModifiedOn { get; set; } = default!;
        
    }
}
