namespace Core.DTO
{
    public class ItemD
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public decimal Price { get; set; }

    }
}
