namespace SparkPlug.Contracts;

public class PageContext : IPageContext
{
    public PageContext(int pageNo = 1, int pageSize = 25)
    {
        PageNo = pageNo;
        PageSize = pageSize;
    }
    public int PageNo { get; set; }
    public int PageSize { get; }
    public long Total { get; set; }
    public int Skip => (PageNo > 1 ? PageNo - 1 : 0) * PageSize;
}

public static partial class Extensions
{
    public static PageContext NextPage(this PageContext pc)
    {
        pc.PageNo++;
        return pc;
    }
    public static PageContext SetTotal(this PageContext pc, long total)
    {
        pc ??= new PageContext();
        pc.Total = total;
        return pc;
    }
}