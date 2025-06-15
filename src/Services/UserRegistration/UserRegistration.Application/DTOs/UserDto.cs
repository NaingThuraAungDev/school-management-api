namespace UserRegistration.Application.Contracts
{
    public record UserDto(long? id, string name, string phone, string password, double balance, bool is_active, DateTime? createddate, DateTime? lastmodifieddate);
}