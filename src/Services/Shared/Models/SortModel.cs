namespace Shared.Models
{
    public class SortModel
    {
        public required string ColId { get; set; }
        public required string Sort { get; set; }
        public string PairAsSqlExpression
        {
            get
            {
                return $"{ColId} {Sort}";
            }
        }
    }
}