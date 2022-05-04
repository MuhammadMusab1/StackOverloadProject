using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StackOverloadProject.Models;

namespace StackOverloadProject.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Question> Question { get; set; }
        public DbSet<Answer> Answer { get; set; }
        public DbSet<Tag> Tag { get; set; }
        public DbSet<QuestionTag> QuestionTag { get; set; }
        public DbSet<AnswerComment> AnswerComment { get; set; }
        public DbSet<QuestionComment> QuestionComment { get; set; }
        public DbSet<UpvoteQuestion> UpvoteQuestion { get; set; }
        public DbSet<DownvoteQuestion> DownvoteQuestion { get; set; }
        public DbSet<UpvoteAnswer> UpvoteAnswer { get; set; }
        public DbSet<DownvoteAnswer> DownvoteAnswer { get; set; }
    }
}