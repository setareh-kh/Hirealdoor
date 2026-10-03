namespace Hirealdoor.DTos.Response;

public class PaginateResponseDto<TEntity>
{
    public int? Page { get; set; } = 1;
    public int? Pager { get; set; } = 10;
    public int Pages { get; set; } = 0;
    public int Total { get; set; } = 0;
    public List<TEntity>? Items { get; set; }
}