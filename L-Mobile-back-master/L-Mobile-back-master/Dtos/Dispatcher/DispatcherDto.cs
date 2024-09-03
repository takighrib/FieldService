public class DispatcherDTO
{
    public int Id { get; set; }
    public List<string> TechniciansIds { get; set; } // Collection of Technician IDs
    public DateTime DispatchDate { get; set; }
    public string Message { get; set; }
}