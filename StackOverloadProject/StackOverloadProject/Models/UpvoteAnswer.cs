﻿namespace StackOverloadProject.Models
{
    public class UpvoteAnswer
    {
        public int Id { get; set; }
        public int? AnswerId { get; set; }
        public string UserId { get; set; }
        public Answer? Answer { get; set; }
        public ApplicationUser User { get; set; }
        public string VoterId { get; set; }
        public UpvoteAnswer(Answer answer, ApplicationUser user, string voterId)
        {
            Answer = answer;
            AnswerId = answer.Id;
            User = user;
            UserId = user.Id;
            VoterId = voterId;
        }
        public UpvoteAnswer()
        {

        }
    }
}
