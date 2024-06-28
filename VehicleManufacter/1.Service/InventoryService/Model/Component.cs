namespace InventoryService.Model
{
    public class Component
    {
        public Guid Id { get; set; }
        public string ComponentType { get; set; }
        public int Quantity { get; set; }
    }
}
