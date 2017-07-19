using System;

namespace PersonalBlog.Entities
{
    public class Comment
    {
        public Comment()
        {
        }

        public Comment(string userId, string articleId, string title, string text, DateTime createDate)
        {
            this.Title = title;
            this.Text = text;
            this.CreateDate = createDate;
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public DateTime CreateDate { get; set; }

        public string Text { get; set; }

        public User Author { get; set; }
    }
}