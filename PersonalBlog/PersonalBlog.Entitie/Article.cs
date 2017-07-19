using System;
using System.Collections.Generic;

namespace PersonalBlog.Entities
{
    public class Article
    {
        public Article()
        {
        }

        public Article(string title, DateTime createDate, string mainImage, string text)
        {
            this.Title = title;
            this.CreateDate = createDate;
            this.MainImage = mainImage;
            this.Text = text;
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public DateTime CreateDate { get; set; }

        public string MainImage { get; set; }

        public string Text { get; set; }

        public string Author { get; set; }

        public IEnumerable<Tag> Tags { get; set; }

        public IEnumerable<Comment> Comments { get; set; }
    }
}