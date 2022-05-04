using Microsoft.AspNetCore.Identity;

namespace StackOverloadProject.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<Question> Questions { get; set; }
        public ICollection<Answer> Answers { get; set; }
        public ICollection<QuestionComment> QuestionComments { get; set; }
        public ICollection<AnswerComment> AnswerComments { get; set;}
        public ICollection<UpvoteQuestion> UpvoteQuestions { get; set; }
        public ICollection<UpvoteAnswer> UpvoteAnswers { get; set; }
        public ICollection<DownvoteQuestion> DownvoteQuestions { get; set; }
        public ICollection<DownvoteAnswer> DownvoteAnswers { get; set; }
        public int Reputation { get; set; }
        public ApplicationUser()
        {
            Questions = new HashSet<Question>();
            Answers = new HashSet<Answer>();
            QuestionComments = new HashSet<QuestionComment>();
            AnswerComments = new HashSet<AnswerComment>();
            UpvoteQuestions = new HashSet<UpvoteQuestion>();
            DownvoteQuestions = new HashSet<DownvoteQuestion>();
            UpvoteAnswers = new HashSet<UpvoteAnswer>();
            DownvoteAnswers = new HashSet<DownvoteAnswer>();
        }
        public void CalculateReputation()
        {
            int totalUpvotes = UpvoteAnswers.Count() + UpvoteQuestions.Count();
            int totalDownvotes = DownvoteAnswers.Count() + DownvoteQuestions.Count();

            Reputation = (totalUpvotes * 5);
            Reputation -= (totalDownvotes * 5);
        }
    }
}
