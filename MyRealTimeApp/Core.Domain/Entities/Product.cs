namespace MyRealTimeApp.Core.Domain.Entities
{
    //pure data model
    //no dependency on database, API or external libraries
    public class Product
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public decimal Price { get; set; } 
    }
}
