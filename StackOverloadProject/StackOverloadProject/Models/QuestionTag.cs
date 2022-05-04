namespace StackOverloadProject.Models
{
    public class QuestionTag
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public int TagId { get; set; }
        public Question Question { get; set; }
        public Tag Tag { get; set; }
        public QuestionTag(Question question, Tag tag)
        {
            Question = question;
            QuestionId = question.Id;
            Tag = tag;
            TagId = tag.Id;
        }
        public QuestionTag()
        {

        }
    }
}
