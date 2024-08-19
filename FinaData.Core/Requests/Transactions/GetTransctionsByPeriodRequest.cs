namespace FinaData.Core.Requests.Transactions;

public class GetTransctionsByPeriodRequest : PagedRequest
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}
