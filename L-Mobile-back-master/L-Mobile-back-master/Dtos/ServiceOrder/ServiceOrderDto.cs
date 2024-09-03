public class ServiceOrderDTO
{
    public int Id { get; set; }
    public int CompanyId { get; set; }
    public string UserId { get; set; } // Change from UserIds to UserId
    public List<int> ArticleIds { get; set; }
    public string Status { get; set; } // Status is now a string
    public string Progress { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public List<DispatcherDTO> Dispatchers { get; set; } // Collection of Dispatchers
}