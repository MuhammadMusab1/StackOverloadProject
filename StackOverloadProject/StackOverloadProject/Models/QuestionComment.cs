namespace StackOverloadProject.Models
{
    public class QuestionComment
    {
        public int Id { get; set; }
        public string Comment { get; set; }
        public int QuestionId { get; set; }
        public string? UserId { get; set; }
        public Question Question { get; set; }
        public ApplicationUser? User { get; set; }
        public DateTime DateCommented { get; set; }
        public QuestionComment(string comment, Question question, ApplicationUser user)
        {
            Comment = comment;
            Question = question;
            QuestionId = question.Id;
            User = user;
            UserId = user.Id;
            DateCommented = DateTime.Now;
        }
        public QuestionComment()
        {

        }
    }
}
