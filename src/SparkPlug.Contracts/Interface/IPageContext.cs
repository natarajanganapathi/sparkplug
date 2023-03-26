namespace SparkPlug.Contracts;

public interface IPageContext
{
    int PageNo { get; set; }
    int PageSize { get; }
    long Total { get; set; }
    int Skip { get; }
}
