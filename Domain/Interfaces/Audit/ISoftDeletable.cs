namespace ULMS.Domain.Interfaces.Audit
{
    public interface ISoftDeletable
    {
        public bool IsDeleted { get; set; }
    }
}
