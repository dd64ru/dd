namespace PersonalBlog.Entities
{
    public class Tag
    {
        public Tag()
        {
        }

        public Tag(string title)
        {
            this.Title = title;
        }

        public int Id { get; set; }

        public string Title { get; set; }
    }
}