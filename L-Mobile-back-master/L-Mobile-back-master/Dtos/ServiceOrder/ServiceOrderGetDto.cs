public class ServiceOrderGetDTO
{
    public int Id { get; set; }
    public int CompanyId { get; set; }
    public IEnumerable<string> UserIds { get; set; }
    public IEnumerable<int> ArticleIds { get; set; }
    public string Status { get; set; } // Status as string
    public string Progress { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}