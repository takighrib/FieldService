public class ServiceOrder
{
    public int Id { get; set; }
    public int CompanyId { get; set; }
    public List<int> ArticleIds { get; set; }
    public ServiceOrderStatus Status { get; set; }
    public string Progress { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Relations
    public virtual Company Company { get; set; }

    // Remplacer la collection d'utilisateurs par une référence unique à un utilisateur
    public string UserId { get; set; }
    public virtual User User { get; set; } // Nouvelle relation: un seul utilisateur

    public virtual ICollection<Article> Articles { get; set; }

    public virtual ICollection<Dispatcher> Dispatchers { get; set; }

    // Méthode pour obtenir le nom du statut
    public string GetServiceOrderStatusName()
    {
        return Status switch
        {
            ServiceOrderStatus.New => "New",
            ServiceOrderStatus.ReadyForScheduling => "Ready For Scheduling",
            ServiceOrderStatus.Scheduled => "Scheduled",
            ServiceOrderStatus.InProgress => "In Progress",
            ServiceOrderStatus.TechnicallyCompleted => "Technically Completed",
            ServiceOrderStatus.ReadyForInvoice => "Ready For Invoice",
            ServiceOrderStatus.Invoiced => "Invoiced",
            ServiceOrderStatus.Completed => "Completed",
            _ => throw new ArgumentOutOfRangeException(nameof(Status), "Invalid status index.")
        };
    }
}
public enum ServiceOrderStatus
{
    New,
    ReadyForScheduling,
    Scheduled,
    InProgress,
    TechnicallyCompleted,
    ReadyForInvoice,
    Invoiced,
    Completed
}

