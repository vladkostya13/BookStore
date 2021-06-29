using System;

namespace BookStore.Domain.Models
{
    public class Book : Entity
    {
        public string Name { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public DateTime PublishDate { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}