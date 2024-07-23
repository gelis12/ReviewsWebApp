using ReviewsWebApp.Data;

namespace ReviewsWebApp.Models
{
    public class ReviewViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public int Rating { get; set; }
        public string Subject { get; set; }
        public string CreatedById { get; set; }
        public string CreatedByUser { get; set; }

        public static ReviewViewModel FromEntity(Review entity)
        {
            return new ReviewViewModel
            {
                Id = entity.Id,
                Title = entity.Title,
                Content = entity.Content,
                Created = entity.Created,
                Modified = entity.Modified,
                Rating = entity.Rating,
                Subject = entity.Subject,
                CreatedByUser = entity.CreatedBy?.UserName,
                CreatedById = entity.CreatedBy?.Id
            };
        }

        public Review ToEntity()
        {
            return new Review
            {
                Id = Id,
                Title = Title,
                Content = Content,
                Created = Created,
                Modified = Modified,
                Rating = Rating,
                Subject = Subject,
            };
        }

    }
}
