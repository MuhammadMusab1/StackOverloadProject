namespace StackOverloadProject.Models
{
    public class Question
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DateAsked { get; set; }
        public string UserId { get; set; }
        public ICollection<Answer> Answers { get; set; }
        public ApplicationUser User { get; set; }
        public ICollection<QuestionTag> QuestionTags { get; set; }
        public ICollection<QuestionComment> QuestionComments { get; set; }
        public ICollection<UpvoteQuestion> UpvoteQuestions { get; set; }
        public ICollection<DownvoteQuestion> DownvoteQuestions { get; set; }
        public Question(string title, string description, ApplicationUser user)
        {
            Title = title;
            Description = description;
            User = user;
            UserId = user.Id;
            DateAsked = DateTime.Now;
            Answers = new HashSet<Answer>();
            QuestionTags = new HashSet<QuestionTag>();
            QuestionComments = new HashSet<QuestionComment>();
            UpvoteQuestions = new HashSet<UpvoteQuestion>();
            DownvoteQuestions = new HashSet<DownvoteQuestion>();
        }
        public Question()
        {

        }
    }
}
