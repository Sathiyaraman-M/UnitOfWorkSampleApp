using UnitOfWorkSampleApp.Common;

namespace UnitOfWorkSampleApp;

public class BookHeader : AuditableEntity<int>
{
    public string Barcode { get; set; }
    public bool IsAvailable { get; set; }
    public string Condition { get; set; }
    public Book Book { get; set; }
    public int BookId { get; set; }
}

public class Book : AuditableEntity<int>
{
    public string Name { get; set; }
    public string ISBN { get; set; }
    public string Author { get; set; }
    public string DeweyIndex { get; set; }
    public string Publisher { get; set; }
    public string Edition { get; set; }
    public int PublicationYear { get; set; }
    public string Description { get; set; }
    public decimal Cost { get; set; }
    public string ImageUrl { get; set; }
    public int Copies { get; set; }
    public int AvailableCount { get; set; }
    public int DamagedCount { get; set; }
    public int MissingCount { get; set; }
}