using System.ComponentModel.DataAnnotations.Schema;

namespace MicroserviceExample.Entities
{
    public class Review
    {
        public Guid Id { get; set; }        
        public Guid ArticleId { get; set; }
        public string Reviewer { get; set; }
        public string ReviewContent { get; set; }

        [ForeignKey("ArticleId")]
        public virtual Article Article { get; set; }
    }
}
