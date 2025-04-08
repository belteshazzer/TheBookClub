namespace TheBookClub.Models.Entities
{
    public class Author
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Bio { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Nationality { get; set; }

        public virtual ICollection<Book> Books { get; set; } = new List<Book>();
    }
}