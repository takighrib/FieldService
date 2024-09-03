public class Dispatcher
{
    public int Id { get; set; }
    public List<string> TechniciansIds { get; set; }
    public DateTime DispatchDate { get; set; }
    public string Message { get; set; }

    // Relation avec ServiceOrder
    public int ServiceOrderId { get; set; }
    public virtual ServiceOrder ServiceOrder { get; set; }


}