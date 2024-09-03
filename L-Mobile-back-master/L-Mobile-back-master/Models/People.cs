public class People
{
    public string Id { get; set; } // Assuming Id is an int as well
    public string Name { get; set; }
    public int CompanyId { get; set; }
    public Company Company { get; set; }
}
