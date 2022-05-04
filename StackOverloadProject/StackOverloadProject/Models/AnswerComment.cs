namespace StackOverloadProject.Models
{
    public class AnswerComment
    {
        public int Id { get; set; }
        public string Comment { get; set; }
        public int AnswerId { get; set; }
        public string? UserId { get; set; }
        public Answer Answer { get; set; }
        public ApplicationUser? User { get; set; }
        public DateTime DateCommented { get; set; }
        public AnswerComment(string comment, Answer answer, ApplicationUser user)
        {
            Comment = comment;
            Answer = answer;
            AnswerId = answer.Id;
            User = user;
            UserId = user.Id;
            DateCommented = DateTime.Now;
        }
        public AnswerComment()
        {

        }
    }
}
