namespace UserRegistration.Application.Contracts
{
    public record FilterDto(int skip, int take, List<Sort> sort, Filter filter);
    public record Sort(string dir, string field);
    public record Filter(List<Filters> filters);
    public record Filters(string field, string value);
}