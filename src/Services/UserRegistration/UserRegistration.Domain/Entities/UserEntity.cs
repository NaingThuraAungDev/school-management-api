namespace UserRegistration.Domain.Entities
{
    public class UserEntity
    {
        public long id { get; set; }
        public required string Name { get; set; }
        public string? phone { get; set; }
        public string? password { get; set; }
        public string? salt { get; set; }
        public double balance { get; set; }
        public string? device_token { get; set; }
        public int device_platform { get; set; }
        public bool is_active { get; set; }
        public DateTime createddate { get; set; }
        public DateTime lastmodifieddate { get; set; }
    }
}