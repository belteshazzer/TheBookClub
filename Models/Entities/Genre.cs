namespace TheBookClub.Models.Entities
{
    public class Genre
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsDeleted { get; set; } = false;

        public virtual ICollection<Book> Books { get; set; } = new List<Book>();
    }
}