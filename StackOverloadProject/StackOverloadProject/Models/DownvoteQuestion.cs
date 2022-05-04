namespace StackOverloadProject.Models
{
    public class DownvoteQuestion
    {
        public int Id { get; set; }
        public int? QuestionId { get; set; }
        public string UserId { get; set; }
        public Question? Question { get; set; }
        public ApplicationUser User { get; set; }
        public string VoterId { get; set; }
        public DownvoteQuestion(Question question, ApplicationUser user, string voterId)
        {
            Question = question;
            QuestionId = question.Id;
            User = user;
            UserId = user.Id;
            VoterId = voterId;
        }
        public DownvoteQuestion()
        {

        }
    }
}
