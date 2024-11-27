namespace LibraryManager.Models
{
    public partial class Chapters
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int ChapterNumber { get; set; }
        public string Title { get; set; }
        public string FilePath { get; set; }

        public virtual Books Book { get; set; }
    }
}
