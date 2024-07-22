using Microsoft.AspNetCore.Identity;

namespace ReviewsWebApp.Data
{
    public class Review
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public int Rating { get; set; }
        public string Subject { get; set; }
        public IdentityUser CreatedBy { get; set; }
    }
}
