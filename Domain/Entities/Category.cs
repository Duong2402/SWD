namespace Domain.Entities
{
    public  class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateCreate { get; set; } = DateTime.Now;
        public DateTime? DateUpdate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
