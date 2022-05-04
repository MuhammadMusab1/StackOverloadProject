namespace StackOverloadProject.Models
{
    public class Answer
    {
        public int Id { get; set; }
        public string AnswerContent { get; set; }
        public bool IsCorrect { get; set; }
        public int QuestionId { get; set; }
        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }
        public Question Question { get; set; }
        public DateTime DateAnswered { get; set; }
        public ICollection<AnswerComment> AnswerComments { get; set;}
        public ICollection<UpvoteAnswer> UpvoteAnswers { get; set; }
        public ICollection<DownvoteAnswer> DownvoteAnswers { get; set; }
        public Answer(string answerContent, Question question, ApplicationUser user)
        {
            AnswerContent = answerContent;
            QuestionId = question.Id;
            Question = question;
            User = user;
            UserId = user.Id;
            AnswerComments = new HashSet<AnswerComment>();
            UpvoteAnswers = new HashSet<UpvoteAnswer>();
            DownvoteAnswers = new HashSet<DownvoteAnswer>();
            DateAnswered = DateTime.Now;
            IsCorrect = false;
        }
        public Answer()
        {

        }
    }
}
