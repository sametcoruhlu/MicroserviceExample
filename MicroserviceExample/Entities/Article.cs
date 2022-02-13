namespace MicroserviceExample.Entities
{
    public class Article
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string ArticleContent { get; set; }
        public DateTime PublishDate { get; set; }
        public int StarCount { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }

        public Article()
        {
            Reviews = new List<Review>();
        }
    }
}
