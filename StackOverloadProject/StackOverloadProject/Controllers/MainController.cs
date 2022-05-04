using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StackOverloadProject.Data;
using StackOverloadProject.Models;
using X.PagedList;

namespace StackOverloadProject.Controllers
{
    public class MainController : Controller
    {
        private ApplicationDbContext _db { get; set; }
        private UserManager<ApplicationUser> _userManager { get; set; }
        private RoleManager<IdentityRole> _roleManager { get; set; }

        public MainController(ApplicationDbContext Db, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = Db;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        [Authorize]
        public IActionResult Index(int? page, string? sortType)
        {
            int pageNumber = page ?? 1;
            int pageSize = 10;  //hardcode how many records will be displaying on 1 page
            X.PagedList.IPagedList<Question> sortedQuestions;
            switch (sortType)
            {
                case "MostRecent":
                    return RedirectToAction("SortQuestionsByMostRecent");
                case "MostAnswered":
                    sortedQuestions = _db.Question.AsNoTracking().Include(q => q.User).Include(q => q.Answers).OrderByDescending(q => q.Answers.Count()).ToPagedList(pageNumber, pageSize);
                    return RedirectToAction("SortQuestionsByMostAnswered");

                default:
                    sortedQuestions = _db.Question.AsNoTracking().Include(q => q.User).Include(q => q.Answers).OrderBy(q => q.User.Email).ToPagedList(pageNumber, pageSize);
                    return View(sortedQuestions);
            }
        }
        public IActionResult SortQuestionsByMostAnswered(int? page)
        {
            int pageNumber = page ?? 1;
            int pageSize = 10;  //hardcode how many records will be displaying on 1 page
            IPagedList<Question> sortByMostAnsweredQuestions = _db.Question.AsNoTracking().Include(q => q.User).Include(q => q.Answers).OrderByDescending(q => q.Answers.Count()).ToPagedList(pageNumber, pageSize);
            return View(sortByMostAnsweredQuestions);
        }
        public IActionResult SortQuestionsByMostRecent(int? page)
        {
            int pageNumber = page ?? 1;
            int pageSize = 10;  //hardcode how many records will be displaying on 1 page
            IPagedList<Question> sortByMostRecentQuestions = _db.Question.AsNoTracking().Include(q => q.User).Include(q => q.Answers).OrderByDescending(q => q.DateAsked).ToPagedList(pageNumber, pageSize);
            return View(sortByMostRecentQuestions);
        }
        public IActionResult CreateQuestion()
        {
            SelectList tagList = new SelectList(_db.Tag.ToList(), "Id", "Tagname");
            ViewBag.tagList = tagList;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateQuestion(string? title, string? description, int? tagId)
        {
            if(title != null && description != null && tagId != null)
            {
                try
                {
                    ApplicationUser user = await _userManager.FindByNameAsync(User.Identity.Name);
                    Tag tagChosen = _db.Tag.Include(t => t.QuestionTags).First(t => t.Id == tagId);
                    Question newQuestion = new Question(title, description, user);
                    QuestionTag questionTag = new QuestionTag(newQuestion, tagChosen);

                    user.Questions.Add(newQuestion);
                    newQuestion.QuestionTags.Add(questionTag);
                    tagChosen.QuestionTags.Add(questionTag);

                    await _userManager.UpdateAsync(user);
                    //this saved the new question and questionTag
                   //to the DbSet of ApplicationContext just don't save anythin in db this will save it automatically
                    return RedirectToAction("Index");
                }
                catch(Exception ex)
                {
                    return NotFound(ex.Message);
                }
            }
            else
            {
                return BadRequest();
            }
        }
        public IActionResult QuestionDetails(int? questionId)
        {
            if(questionId != null)
            {
                try
                {
                    Question questionFound = _db.Question
                        .Include(q => q.User)
                        .Include(q => q.Answers).ThenInclude(a => a.User)
                        .Include(q => q.Answers).ThenInclude(a => a.AnswerComments).ThenInclude(ac => ac.User)
                        .Include(q => q.Answers).ThenInclude(a => a.DownvoteAnswers)
                        .Include(q => q.Answers).ThenInclude(a => a.UpvoteAnswers)
                        .Include(q => q.QuestionTags).ThenInclude(qt => qt.Tag)
                        .Include(q => q.QuestionComments).ThenInclude(qc => qc.User)
                        .Include(q => q.DownvoteQuestions)
                        .Include(q => q.UpvoteQuestions)
                        .First(q => q.Id == questionId);
                    ViewBag.userChoices = new List<SelectListItem>()
                    {
                        new SelectListItem("Upvote", "Upvote"),
                        new SelectListItem("DownVote", "Downvote"),
                    };
                    return View(questionFound);
                }
                catch(Exception ex)
                {
                    return NotFound(ex.Message);
                }
            }
            return View();
        }
        public IActionResult AnswerQuestion(int? questionId)
        {
            if(questionId != null)
            {
                try
                {
                    Question questionFound = _db.Question.First(q => q.Id == questionId);
                    return View(questionFound);
                }
                catch (Exception ex)
                {
                    return NotFound(ex.Message);
                }
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpPost]
        public async Task<IActionResult> AnswerQuestion(string? answerContent, int? questionId)
        {
            if(answerContent != null && questionId != null)
            {
                try
                {
                    ApplicationUser currentUser = await _userManager.FindByEmailAsync(User.Identity.Name);
                    Question questionAnswering = _db.Question.Include(q => q.Answers).First(q => q.Id == questionId);
                    Answer answerByUser = new Answer(answerContent, questionAnswering, currentUser);

                    questionAnswering.Answers.Add(answerByUser);
                    currentUser.Answers.Add(answerByUser);

                    await _userManager.UpdateAsync(currentUser);
                    return RedirectToAction("QuestionDetails", new { questionId = questionAnswering.Id });
                }
                catch(Exception ex)
                {
                    return NotFound(ex.Message);
                }
            }
            else
            {
                return BadRequest();
            }
        }
        public IActionResult CommentOnAnswer(int? answerId)
        {
            if (answerId != null)
            {
                try
                {
                    Answer answerFound = _db.Answer.Include(a => a.User).First(a => a.Id == answerId);
                    return View(answerFound);
                }
                catch (Exception ex)
                {
                    return NotFound();
                }
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpPost]
        public async Task<IActionResult> CommentOnAnswer(int? answerId, string? userComment)
        {
            if(answerId != null && userComment != null)
            {
                try
                {
                    ApplicationUser currentUser = await _userManager.FindByEmailAsync(User.Identity.Name);
                    Answer answerCommenting = _db.Answer.Include(a => a.AnswerComments).First(a => a.Id == answerId);
                    AnswerComment commentByUser = new AnswerComment(userComment, answerCommenting, currentUser);

                    currentUser.AnswerComments.Add(commentByUser);
                    answerCommenting.AnswerComments.Add(commentByUser);
                    
                    await _userManager.UpdateAsync(currentUser);

                    return RedirectToAction("QuestionDetails", new {questionId = answerCommenting.QuestionId});
                }
                catch(Exception ex)
                {
                    return NotFound(ex.Message);
                }
            }
            else
            {
                return BadRequest();
            }
        }
        public IActionResult CommentOnQuestion(int? questionId)
        {
            if(questionId != null)
            {
                try
                {
                    Question questionFound = _db.Question.Include(q => q.User).First(q => q.Id == questionId);
                    return View(questionFound);
                }
                catch(Exception ex)
                {
                    return NotFound(ex.Message);
                }
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpPost]
        public async Task<IActionResult> CommentOnQuestion(string? userComment, int? questionId)
        {
            if(userComment != null && questionId != null)
            {
                try
                {
                    ApplicationUser currentUser = await _userManager.FindByEmailAsync(User.Identity.Name);
                    Question questionCommenting = _db.Question.Include(q => q.QuestionComments).First(q => q.Id == questionId);
                    QuestionComment commentByUser = new QuestionComment(userComment, questionCommenting, currentUser);

                    currentUser.QuestionComments.Add(commentByUser);
                    questionCommenting.QuestionComments.Add(commentByUser);

                    await _userManager.UpdateAsync(currentUser);
                    return RedirectToAction("QuestionDetails", new { questionId = questionCommenting.Id });
                }
                catch(Exception ex)
                {
                    return NotFound(ex.Message);
                }
            }
            else
            {
                return BadRequest();
            }
        }
        public IActionResult AddTagToQuestion(int? questionId)
        {
            if(questionId != null)
            {
                try
                {
                    Question question = _db.Question.Include(q => q.User).Include(q => q.QuestionTags).ThenInclude(qt => qt.Tag).First(q => q.Id == questionId);
                    List<Tag> tagsQuestionHave = _db.QuestionTag.Include(qt => qt.Tag).Where(qt => qt.QuestionId == questionId).Select(qt => qt.Tag).ToList();
                    List<Tag> tagsQuestionDontHave = _db.Tag.Where(qt => !tagsQuestionHave.Contains(qt)).ToList();

                    ViewBag.tagList = new SelectList(tagsQuestionDontHave, "Id", "Tagname");
                    return View(question);
                }
                catch (Exception ex)
                {
                    return NotFound(ex.Message);
                }
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpPost]
        public IActionResult AddTagToQuestion(int? questionId, int? tagId)
        {
            if(questionId != null && tagId != null)
            {
                try
                {
                    Question question = _db.Question.Include(q => q.QuestionTags).First(q => q.Id == questionId);
                    Tag tag = _db.Tag.Include(t => t.QuestionTags).First(t => t.Id == tagId);
                    QuestionTag tagForQuestion = new QuestionTag(question, tag);

                    question.QuestionTags.Add(tagForQuestion);
                    tag.QuestionTags.Add(tagForQuestion);
                    _db.QuestionTag.Add(tagForQuestion);
                    _db.SaveChanges();
                    return RedirectToAction("QuestionDetails", new { questionId = questionId });
                }
                catch(Exception ex)
                {
                    return NotFound(ex.Message);
                }
            }
            else
            {
                return BadRequest();
            }
        }
        public IActionResult ShowQuestionsOfTag(int? tagId, int? page)
        {
            if(tagId != null)
            {
                try
                {

                    ViewBag.tag = _db.Tag.First(t => t.Id == tagId);

                    int pageNumber = page ?? 1;
                    int pageSize = 10;  //hardcode how many records will be displaying on 1 page

                    IPagedList<Question> questionsWithThisTag = _db.QuestionTag
                        .Include(qt => qt.Question.User)
                        .Include(qt => qt.Question.Answers)
                        .Where(qt => qt.TagId == tagId).Select(qt => qt.Question).ToPagedList(pageNumber, pageSize);


                    return View(questionsWithThisTag);
                }
                catch (Exception ex)
                {
                    return NotFound(ex.Message);
                }
            }
            else
            {
                return BadRequest();
            }
        }
        public async Task<IActionResult> CreateUsersVoteForQuestion(string? userChoice, int? questionId)
        {
            if(userChoice != null && questionId != null)
            {
                try
                {
                    Question questionVoting = _db.Question
                        .Include(q => q.User).ThenInclude(u => u.DownvoteQuestions).Include(q => q.User).ThenInclude(u => u.UpvoteQuestions).Include(q => q.User).ThenInclude(u => u.UpvoteAnswers).Include(q => q.User).ThenInclude(u => u.DownvoteAnswers)
                        .Include(q => q.UpvoteQuestions)
                        .Include(q => q.DownvoteQuestions)
                        .First(q => q.Id == questionId);
                    ApplicationUser currentUser = await _userManager.FindByEmailAsync(User.Identity.Name);
                    ApplicationUser questionOwner = await _userManager.FindByIdAsync(questionVoting.UserId);
                    UpvoteQuestion ? upvoteCheckForCurrentUser = _db.UpvoteQuestion.Include(uq => uq.User).Include(uq => uq.Question).FirstOrDefault(uq => uq.QuestionId == questionId && uq.VoterId == currentUser.Id);
                    DownvoteQuestion? downvoteCheckForCurrentUser = _db.DownvoteQuestion.Include(dq => dq.User).Include(dq => dq.Question).FirstOrDefault(dq => dq.QuestionId == questionId && dq.VoterId == currentUser.Id);
                    ViewBag.questionId = questionVoting.Id;

                    switch (userChoice)
                    {
                        case "Upvote":
                            if (upvoteCheckForCurrentUser == null && downvoteCheckForCurrentUser == null)
                            {
                                UpvoteQuestion upvoteToQuestionByCurrentUser = new UpvoteQuestion(questionVoting,questionOwner, currentUser.Id);

                                questionOwner.UpvoteQuestions.Add(upvoteToQuestionByCurrentUser);
                                questionVoting.UpvoteQuestions.Add(upvoteToQuestionByCurrentUser);
                                questionOwner.CalculateReputation();

                                //_db.SaveChanges();

                                await _userManager.UpdateAsync(questionOwner);

                                ViewBag.message = "Question <b style=\"color:green\">Upvoted</b>";
                                return View("AlreadyVoted");
                            }
                            if(upvoteCheckForCurrentUser == null && downvoteCheckForCurrentUser != null) // already downvoted this question now they are upvoting it
                            {
                                questionOwner.DownvoteQuestions.Remove(downvoteCheckForCurrentUser);//remove downvote of currentUser from questionOwner
                                questionVoting.DownvoteQuestions.Remove(downvoteCheckForCurrentUser);//remove downvote of currentUser from question

                                UpvoteQuestion upvoteToQuestionByCurrentUser = new UpvoteQuestion(questionVoting, questionOwner, currentUser.Id);

                                questionOwner.UpvoteQuestions.Add(upvoteToQuestionByCurrentUser);
                                questionVoting.UpvoteQuestions.Add(upvoteToQuestionByCurrentUser);
                                questionOwner.CalculateReputation();

                                await _userManager.UpdateAsync(questionOwner);

                                ViewBag.message = "<b style=\"color:red\">Downvote</b> Removed. Question <b style=\"color:green\">Upvoted</b>";
                                return View("AlreadyVoted");
                            }
                            else
                            {
                                questionOwner.CalculateReputation();
                                ViewBag.message = "You already <b style=\"color:green\">Upvoted</b> this question";
                                return View("AlreadyVoted");
                            }
                        case "Downvote":
                            if (downvoteCheckForCurrentUser == null && upvoteCheckForCurrentUser == null)
                            {
                                DownvoteQuestion downvoteToQuestionByUser = new DownvoteQuestion(questionVoting, questionOwner, currentUser.Id);

                                questionOwner.DownvoteQuestions.Add(downvoteToQuestionByUser);
                                questionVoting.DownvoteQuestions.Add(downvoteToQuestionByUser);
                                questionOwner.CalculateReputation();

                                await _userManager.UpdateAsync(questionOwner);

                                ViewBag.message = "Question <b style=\"color:red\">Downvoted</b>";
                                return View("AlreadyVoted");
                            }
                            if (downvoteCheckForCurrentUser == null && upvoteCheckForCurrentUser != null) // already upvoted this question and now they are downvoting it
                            {
                                questionOwner.UpvoteQuestions.Remove(upvoteCheckForCurrentUser); //remove upvote of currentUser from questionOwner 
                                questionVoting.UpvoteQuestions.Remove(upvoteCheckForCurrentUser); // remove upvote of currentUser from questionVoting

                                DownvoteQuestion downvoteToQuestionByUser = new DownvoteQuestion(questionVoting, questionOwner, currentUser.Id);

                                questionOwner.DownvoteQuestions.Add(downvoteToQuestionByUser);
                                questionVoting.DownvoteQuestions.Add(downvoteToQuestionByUser);
                                questionVoting.User.CalculateReputation();

                                await _userManager.UpdateAsync(questionOwner);

                                ViewBag.message = "<b style=\"color:green\">Upvote</b> Removed. Question <b style=\"color:red\">Downvoted</b>";
                                return View("AlreadyVoted");
                            }
                            else
                            {
                                ViewBag.message = "You already <b style=\"color:red\">Downvoted</b> this question";
                                return View("AlreadyVoted");
                            }

                        default:
                            return BadRequest();
                            
                    }
                }
                catch (Exception ex)
                {
                    return NotFound();
                }
            }
            else
            {
                return BadRequest();
            }
        }
        public async Task<IActionResult> CreateUsersVoteForAnswer(string? userChoice, int? answerId)
        {
            if (userChoice != null && answerId != null)
            {
                try
                {
                    Answer answerVoting = _db.Answer
                        .Include(a => a.User).ThenInclude(u => u.UpvoteAnswers).Include(a => a.User).ThenInclude(u => u.DownvoteAnswers).Include(a => a.User).ThenInclude(u => u.UpvoteQuestions).Include(a => a.User).ThenInclude(u => u.DownvoteQuestions)
                        .Include(q => q.UpvoteAnswers)
                        .Include(q => q.DownvoteAnswers).First(q => q.Id == answerId);
                    ApplicationUser currentUser = await _userManager.FindByEmailAsync(User.Identity.Name);
                    ApplicationUser answerOwner = await _userManager.FindByIdAsync(answerVoting.UserId);
                    UpvoteAnswer? upvoteCheckOfCurrentUser = _db.UpvoteAnswer.Include(ua => ua.User).Include(ua => ua.Answer).FirstOrDefault(ua => ua.AnswerId == answerId && ua.VoterId == currentUser.Id);
                    DownvoteAnswer? downvoteCheckOfCurrentUser = _db.DownvoteAnswer.Include(da => da.User).Include(da => da.Answer).FirstOrDefault(da => da.AnswerId == answerId && da.VoterId == currentUser.Id);
                    ViewBag.questionId = answerVoting.QuestionId;

                    switch (userChoice)
                    {
                        case "Upvote":
                            if (upvoteCheckOfCurrentUser == null && downvoteCheckOfCurrentUser == null)
                            {
                                UpvoteAnswer upvoteToAnswerByCurrentUser = new UpvoteAnswer(answerVoting, answerOwner, currentUser.Id);

                                answerOwner.UpvoteAnswers.Add(upvoteToAnswerByCurrentUser);
                                answerVoting.UpvoteAnswers.Add(upvoteToAnswerByCurrentUser);
                                answerOwner.CalculateReputation();

                                await _userManager.UpdateAsync(answerOwner);

                                ViewBag.message = "Answer <b style=\"color:green\">Upvoted</b>";
                                return View("AlreadyVoted");
                            }
                            if (upvoteCheckOfCurrentUser == null && downvoteCheckOfCurrentUser != null) // already downvoted this answer now they are upvoting it
                            {
                                answerOwner.DownvoteAnswers.Remove(downvoteCheckOfCurrentUser);//remove downvote from user
                                answerVoting.DownvoteAnswers.Remove(downvoteCheckOfCurrentUser);//remove downvote from answer

                                UpvoteAnswer upvoteToAnswerByUser = new UpvoteAnswer(answerVoting, answerOwner, currentUser.Id);

                                answerOwner.UpvoteAnswers.Add(upvoteToAnswerByUser);
                                answerVoting.UpvoteAnswers.Add(upvoteToAnswerByUser);

                                answerOwner.CalculateReputation();

                                await _userManager.UpdateAsync(answerOwner); //will remove the vote from the database 

                                ViewBag.message = "<b style=\"color:red\">Downvote</b> Removed. Answer <b style=\"color:green\">Upvoted</b>";
                                return View("AlreadyVoted");
                            }
                            else
                            {
                                ViewBag.message = "You already <b style=\"color:green\">Upvoted</b> this answer";
                                return View("AlreadyVoted");
                            }
                        case "Downvote":
                            if (downvoteCheckOfCurrentUser == null && upvoteCheckOfCurrentUser == null)
                            {
                                DownvoteAnswer downvoteToAnswerByUser = new DownvoteAnswer(answerVoting, answerOwner,  currentUser.Id);

                                answerOwner.DownvoteAnswers.Add(downvoteToAnswerByUser);
                                answerVoting.DownvoteAnswers.Add(downvoteToAnswerByUser);

                                answerOwner.CalculateReputation();

                                await _userManager.UpdateAsync(answerOwner);

                                ViewBag.message = "Answer <b style=\"color:red\">Downvoted</b>";
                                return View("AlreadyVoted");
                            }
                            if (downvoteCheckOfCurrentUser == null && upvoteCheckOfCurrentUser != null) // already upvoted this question and now they are downvoting it
                            {
                                answerOwner.UpvoteAnswers.Remove(upvoteCheckOfCurrentUser); //remove upvote from user 
                                answerVoting.UpvoteAnswers.Remove(upvoteCheckOfCurrentUser); // remove upvote from answer

                                DownvoteAnswer downvoteToAnswerByUser = new DownvoteAnswer(answerVoting, answerOwner, currentUser.Id);

                                answerOwner.DownvoteAnswers.Add(downvoteToAnswerByUser);
                                answerVoting.DownvoteAnswers.Add(downvoteToAnswerByUser);

                                answerOwner.CalculateReputation();

                                await _userManager.UpdateAsync(answerOwner);//will remove the vote from the database 

                                ViewBag.message = "<b style=\"color:green\">Upvote</b> Removed. Answer <b style=\"color:red\">Downvoted</b>";
                                return View("AlreadyVoted");
                            }
                            else
                            {
                                ViewBag.message = "You already <b style=\"color:red\">Downvoted</b> this answer";
                                return View("AlreadyVoted");
                            }

                        default:
                            return BadRequest();

                    }
                }
                catch (Exception ex)
                {
                    return NotFound();
                }
            }
            else
            {
                return BadRequest();
            }
        }
        public IActionResult MakeAnswerCorrect(int? answerId, int? questionId)
        {
            if (answerId != null && questionId != null)
            {
                try
                {
                    Answer? answerThatIsCorrectInTheQuestion = _db.Answer.FirstOrDefault(a => a.QuestionId == questionId && a.IsCorrect == true);
                    Answer answerToBeCorrected = _db.Answer.First(a => a.Id == answerId);
                    ViewBag.questionId = questionId;

                    if(answerThatIsCorrectInTheQuestion == answerToBeCorrected)
                    {
                        ViewBag.message = "Answer already <b style=\"color:green\">Corrected</b>";
                        return View("AlreadyVoted");
                    }
                    if(answerThatIsCorrectInTheQuestion == null)
                    {
                        answerToBeCorrected.IsCorrect = true;
                        _db.SaveChanges();
                        return RedirectToAction("QuestionDetails", new { questionId = questionId });
                    }
                    if(answerThatIsCorrectInTheQuestion != null)
                    {
                        answerThatIsCorrectInTheQuestion.IsCorrect = false;
                        answerToBeCorrected.IsCorrect = true;
                        _db.SaveChanges();
                        return RedirectToAction("QuestionDetails", new { questionId = questionId });
                    }
                    return View();
                }
                catch (Exception ex)
                {
                    return NotFound(ex.Message);
                }
            }
            else
            {
                return BadRequest();
            }
        }
        [Authorize(Roles = "Admin")]
        public IActionResult AddRoleToUser()
        {
            ViewBag.usersList = new SelectList(_db.Users.ToList(), "Id", "UserName");
            ViewBag.rolesList = new SelectList(_db.Roles.ToList(), "Id", "Name");
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddRoleToUser(string? userId, string? roleForUser)
        {
            if (userId != null && roleForUser != null)
            {
                try
                {
                    await _roleManager.CreateAsync(new IdentityRole(roleForUser)); // create role using _roleManager and save it in the database
                    _db.SaveChanges();

                    ApplicationUser user = await _userManager.FindByIdAsync(userId); // pull the user out of the database to assign the role to it 

                    if (await _roleManager.RoleExistsAsync(roleForUser)) // check if role is in the database
                    {
                        if (!await _userManager.IsInRoleAsync(user, roleForUser)) // check if the user is already in that role
                        {
                            await _userManager.AddToRoleAsync(user, roleForUser);//if user doesn't have the role add it to the user
                        }
                    }
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    return NotFound(ex.Message);
                }
            }
            else
            {
                return BadRequest();
            }
        }
        [Authorize(Roles = "Admin")]
        public IActionResult ShowAllQuestionsToAdmin(int? page, string? sortType)
        {
            int pageNumber = page ?? 1;
            int pageSize = 10;  //hardcode how many records will be displaying on 1 page
            X.PagedList.IPagedList<Question> sortedQuestions;
            switch (sortType)
            {
                case "MostRecent":
                    sortedQuestions = _db.Question.AsNoTracking().Include(q => q.User).Include(q => q.Answers).OrderByDescending(q => q.DateAsked).ToPagedList(pageNumber, pageSize);
                    return View(sortedQuestions);
                case "MostAnswered":
                    sortedQuestions = _db.Question.AsNoTracking().Include(q => q.User).Include(q => q.Answers).OrderByDescending(q => q.Answers.Count()).ToPagedList(pageNumber, pageSize);
                    return View(sortedQuestions);

                default:
                    sortedQuestions = _db.Question.AsNoTracking().Include(q => q.User).Include(q => q.Answers).OrderBy(q => q.User.Email).ToPagedList(pageNumber, pageSize);
                    return View(sortedQuestions);
            }

        }
        public IActionResult DeleteQuestion(int? questionId, string? questionOwnerId)
        {
            if(questionId != null && questionOwnerId != null)
            {
                try
                {
                    List<ApplicationUser> alluser = _db.Users
                        .Include(u => u.UpvoteAnswers).ThenInclude(ua => ua.Answer)
                        .Include(u => u.UpvoteQuestions)
                        .Include(u => u.DownvoteQuestions)
                        .Include(u => u.DownvoteAnswers).ThenInclude(da => da.Answer)
                        .Include(u => u.AnswerComments).ThenInclude(ac => ac.Answer)
                        .Include(u => u.QuestionComments)
                        .Include(u => u.Questions)
                        .Include(u => u.Answers)
                        .ToList();
                    Question questionToBeDeleted = _db.Question.First(q => q.Id == questionId);

                    foreach (ApplicationUser user in alluser) //foreach user 
                    {
                        for (int i = 0; i < user.UpvoteAnswers.Count; i++) // remove any upvoteToAnswer given to the questionBeingDeleted
                        {
                            if (user.UpvoteAnswers.ToList()[i].Answer.QuestionId == questionId)
                            {
                                _db.UpvoteAnswer.Remove(user.UpvoteAnswers.ToList()[i]);
                                user.UpvoteAnswers.Remove(user.UpvoteAnswers.ToList()[i]);
                                user.CalculateReputation();
                            }
                        }
                        for (int i = 0; i < user.DownvoteAnswers.Count; i++) // remove any downvoteToAnswer given to the questionBeingDeleted
                        {
                            if (user.DownvoteAnswers.ToList()[i].Answer.QuestionId == questionId)
                            {
                                _db.DownvoteAnswer.Remove(user.DownvoteAnswers.ToList()[i]);
                                user.DownvoteAnswers.Remove(user.DownvoteAnswers.ToList()[i]);
                                user.CalculateReputation();
                            }
                        }
                        for (int i = 0; i < user.DownvoteQuestions.Count; i++) // remove any DownvoteQuestion given to the questionBeingDeleted
                        {
                            if (user.DownvoteQuestions.ToList()[i].QuestionId == questionId)
                            {
                                _db.DownvoteQuestion.Remove(user.DownvoteQuestions.ToList()[i]);
                                user.DownvoteQuestions.Remove(user.DownvoteQuestions.ToList()[i]);
                                user.CalculateReputation();
                            }
                        }
                        for (int i = 0; i < user.UpvoteQuestions.Count; i++) // remove any upvoteQuestion given to the questionBeingDeleted
                        {
                            if (user.UpvoteQuestions.ToList()[i].QuestionId == questionId)
                            {
                                _db.UpvoteQuestion.Remove(user.UpvoteQuestions.ToList()[i]);
                                user.UpvoteQuestions.Remove(user.UpvoteQuestions.ToList()[i]);
                                user.CalculateReputation();
                            }
                        }
                        for (int i = 0; i < user.QuestionComments.Count; i++) // remove any QuestionComments given to the questionBeingDeleted
                        {
                            if (user.QuestionComments.ToList()[i].QuestionId == questionId)
                            {
                                _db.QuestionComment.Remove(user.QuestionComments.ToList()[i]);
                                user.QuestionComments.Remove(user.QuestionComments.ToList()[i]);
                            }
                        }
                        for (int i = 0; i < user.AnswerComments.Count; i++) // remove any AnswerComments given to the questionBeingDeleted
                        {
                            if (user.AnswerComments.ToList()[i].Answer.QuestionId == questionId)
                            {
                                _db.AnswerComment.Remove(user.AnswerComments.ToList()[i]);
                                user.AnswerComments.Remove(user.AnswerComments.ToList()[i]);
                            }
                        }
                        for (int i = 0; i < user.Answers.Count; i++) // remove any Answers given to the questionBeingDeleted
                        {
                            if (user.Answers.ToList()[i].QuestionId == questionId)
                            {
                                _db.Answer.Remove(user.Answers.ToList()[i]);
                                user.Answers.Remove(user.Answers.ToList()[i]);
                            }
                        }
                        for (int i = 0; i < user.Questions.Count; i++) // remove the questionBeingDeleted from the questionOwner (which will only delete 1 question object from one user that originally asked the question)
                        {
                            if (user.Questions.ToList()[i].Id == questionId)
                            {
                                _db.Question.Remove(user.Questions.ToList()[i]);
                                user.Questions.Remove(user.Questions.ToList()[i]);
                            }
                        }
                    }
                    //remove all the things related to the question from the _db
                    List<QuestionTag> tagsToBeRemoved = _db.QuestionTag.Where(qt => qt.QuestionId == questionId).ToList(); //remove all questionTag with the matching questionId of deleted question.Id from the database
                    foreach (QuestionTag qt in tagsToBeRemoved)
                    {
                        _db.QuestionTag.Remove(qt);
                    }
                    _db.SaveChanges();

                    return RedirectToAction("ShowAllQuestionsToAdmin");
                }
                catch (Exception ex)
                {
                    return NotFound(ex.Message);
                }
            }
            else
            {
                return BadRequest();
            }
        }
    }
}

//Add a Seed Method to this application that seeds at least three Users.