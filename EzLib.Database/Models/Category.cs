namespace EzLib.Models
{
    public class Category
    {
        public Category()
        {
            LibraryItems = new HashSet<LibraryItem>();
        }

        public int Id { get; set; }

        public string CategoryName { get; set; }

        public virtual ICollection<LibraryItem> LibraryItems { get; set; }
    }
}
