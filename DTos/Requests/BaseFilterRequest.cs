namespace Hirealdoor.Dtos.Requests;

public class BaseFilterRequest
{
    public string? Search { get; set; }
    public bool Include { get; set; } = false;
    public bool? Active { get; set; }

    public int? Page { get; set; }
    public string? OrderBy { get; set; }
    public int? Pager { get; set; }
    public bool? Countable { get; set; } = true;
}