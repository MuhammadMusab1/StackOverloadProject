namespace StackOverloadProject.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public string Tagname { get; set; }
        public ICollection<QuestionTag> QuestionTags { get; set; }
        public Tag(string tagName)
        {
            Tagname = tagName;
            QuestionTags = new HashSet<QuestionTag>();
        }
        public Tag()
        {

        }
    }
}
