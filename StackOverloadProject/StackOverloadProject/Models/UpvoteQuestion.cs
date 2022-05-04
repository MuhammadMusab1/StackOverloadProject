namespace StackOverloadProject.Models
{
    public class UpvoteQuestion
    {
        public int Id { get; set; }
        public int? QuestionId { get; set; }
        public string UserId { get; set; }
        public Question? Question { get; set; }
        public ApplicationUser User { get; set; } //Owner
        public string VoterId { get; set; }
        public UpvoteQuestion(Question question, ApplicationUser user, string voterId)
        {
            Question = question;
            QuestionId = question.Id;
            User = user;
            UserId = user.Id;
            VoterId = voterId;
        }
        public UpvoteQuestion()
        {

        }
    }
}
