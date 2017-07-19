using System;

namespace PersonalBlog.Entities
{
    public class User
    {
        public User()
        {
        }

        public User(string name, DateTime dateOfBirth)
        {
            this.Name = name;
            this.DateOfBirth = dateOfBirth;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string UserPhoto { get; set; }
    }
}