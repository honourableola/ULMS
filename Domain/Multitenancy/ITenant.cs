namespace Domain.Multitenancy
{
    public interface ITenant
    {
        public string TenantId { get; set; }
    }
}
