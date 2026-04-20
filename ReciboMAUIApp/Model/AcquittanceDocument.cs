public class AcquittanceDocument
{
    public AcquittanceDocument(int id, double? price, string customerName, string description)
    {
        Id = id;
        Price = price.GetValueOrDefault();
        CustomerName = customerName;
        Description = description;
    }

    public int Id { get; set; }
    public double Price { get; set; }
    public string? CustomerName { get; set; }
    public string? Description { get; set; }
    public string? City { get; set; }

    public override string ToString()
    {
        return $"{Id}_{DateTime.Now.Day}_{DateTime.Now.Month}_{DateTime.Now.Year}";
    }
}