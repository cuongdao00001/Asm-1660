using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Idea1.Models;
using MailKit.Net.Smtp;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using MimeKit;

namespace Idea1.Controllers
{
    public class IdeasController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private ApplicationUserManager _userManager;


        public IdeasController()
        {
        }

        public IdeasController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().Get<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET: Ideas
        public ActionResult Index(int? topicId, int page = 1, int pageSize = 5, string SortColumn =
            "IdeaId", string IconClass = "fa-sort-asc")
        {
            //  Add TopicId and Pagination 
            if (topicId == null)
            {
                return RedirectToAction("Index", "Staff");
            }

            var topic = db.Topics.Find(topicId);
            if (topic == null)
            {
                return HttpNotFound();
            }
            var ideas = db.Ideas.Where(i => i.TopicId == topicId).ToList();

            int totalCount = ideas.Count();
            int totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
            int start = (page - 1) * pageSize;
            int end = start + pageSize;
            if (end > totalCount) end = totalCount;
            ideas = ideas.Skip(start).Take(pageSize).ToList();

            // Create to relationship Idea with TopicId
            ViewBag.TopicId = topicId;
            ViewBag.TopicTitle = topic.Title;
            ViewBag.TopicFirstDate = topic.FirstDate;
            ViewBag.TopicLastDate = topic.LastDate;

            //Adđ to Sorting for Like, Dislike, View
            ViewBag.SortColumn = SortColumn;
            ViewBag.IconClass = IconClass;
            if (SortColumn == "Views")
            {
                if (IconClass == "fa-sort-asc")
                {
                    ideas = db.Ideas.OrderByDescending(i => i.Views).ToList();
                }
                else
                {
                    ideas = db.Ideas.OrderBy(i => i.Views).ToList();
                }
            }
            else if (SortColumn == "Like")
            {
                if (IconClass == "fa-sort-asc")
                {
                    ideas = db.Ideas.OrderByDescending(i => i.Like).ToList();
                }
                else
                {
                    ideas = db.Ideas.OrderBy(i => i.Like).ToList();
                }
            }
            else if (SortColumn == "Dislike")
            {
                if (IconClass == "fa-sort-asc")
                {
                    ideas = db.Ideas.OrderByDescending(i => i.Dislike).ToList();
                }
                else
                {
                    ideas = db.Ideas.OrderBy(i => i.Dislike).ToList();
                }
            }



            // Create to Pagination
            ViewBag.Page = page;
            ViewBag.TotalPages = totalPages;

            return View(ideas);
        }


        // GET: Ideas/Details/5
        public ActionResult Details(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Idea idea = db.Ideas.Include(i => i.Comments).FirstOrDefault(i => i.IdeaId == id);
            if (idea == null)
            {
                return HttpNotFound();
            }
            //Increase to View, When we click on btn Details
            idea.Views++;
            string fileName = Path.GetFileName(idea.FileData);
            idea.FileName = fileName;
            db.SaveChanges();


            return View(idea);
        }



        [HttpPost]
        public ActionResult AddComment(int ideaId, string commentText, Idea idea)
        {
            // Get current user information
            var userId = User.Identity.GetUserName();
            var nameide = idea.Title;
            // Create a new Comment object
            var comment = new Comment
            {
                Text = commentText,
                DateTime = DateTime.Now,
                IdeaID = ideaId,
                UserId = userId,
                Title = nameide
            };

            // Add to comment for database
            db.Comments.Add(comment);
            db.SaveChanges();


            // Tạo client SMTP
            using (var client = new SmtpClient())
            {
                // Kết nối đến server SMTP
                client.Connect("smtp.gmail.com", 587, false);

                // Đăng nhập bằng tài khoản email của bạn
                client.Authenticate("qacoordinatordepartmentidea@gmail.com", "zydudyigmpqfpwmx");


                // Tạo nội dung email
                var bodyBuilder = new BodyBuilder
                {
                    HtmlBody = $"Tinme have :{comment.DateTime}" + $"<p>User: {comment.UserId}</p>" + $"<p>Comment for {comment.Title}: {comment.Text}</p>",
                    TextBody = "[comment.Datetime} \r\n {User.Identity.Name} \r\n{comment.Title} {comment.Text}"
                };

                // Tạo message
                var message = new MimeMessage
                {
                    Body = bodyBuilder.ToMessageBody()
                };
                message.From.Add(new MailboxAddress("Norely my site", "qacoordinatordepartmentidea@gmail.com"));
                message.To.Add(new MailboxAddress("User1", comment.UserId));
                message.Subject = " New Comment submitted data";
                client.Send(message);

                client.Disconnect(true);


            }



            // Lưu thông báo Status vào TempData để truyền cho View
            TempData["StatusMessage"] = $"({User.Identity.Name}) have to new comment for the idea ";

            // Redirect to idea detail page
            return RedirectToAction("Details", new { id = ideaId });
        }



        public ActionResult Like(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Idea idea = db.Ideas.Find(id);
            if (idea == null)
            {
                return HttpNotFound();
            }
            if (Session["LikedIdeas"] != null && ((List<int>)Session["LikedIdeas"]).Contains(id.Value))
            {
                // The user liked the previous post, so we will cancel the like by subtracting 1 like
                idea.Like--;
                ((List<int>)Session["LikedIdeas"]).Remove(id.Value);

                if (Session["DislikedIdeas"] != null && ((List<int>)Session["DislikedIdeas"]).Contains(id.Value))
                {
                    // The user disliked the previous post, so we will cancel the dislike by subtracting 1 dislike
                    idea.Dislike--;
                    ((List<int>)Session["DislikedIdeas"]).Remove(id.Value);
                }
            }
            else
            {
                // The user has not liked the previous post, so we will increase the like by adding 1 like
                idea.Like++;
                if (Session["LikedIdeas"] == null)
                {
                    Session["LikedIdeas"] = new List<int>();
                }
                ((List<int>)Session["LikedIdeas"]).Add(id.Value);
            }
            db.SaveChanges();

            return RedirectToAction("Index", new { topicId = idea.TopicId });
        }

        public ActionResult Dislike(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Idea idea = db.Ideas.Find(id);
            if (idea == null)
            {
                return HttpNotFound();
            }
            if (Session["DislikedIdeas"] != null && ((List<int>)Session["DislikedIdeas"]).Contains(id.Value))
            {
                // User disliked the previous post, so we will cancel the dislike by subtracting 1 dislike
                idea.Dislike--;
                ((List<int>)Session["DislikedIdeas"]).Remove(id.Value);
            }
            else
            {
                // User has not disliked the previous post, so we will increase the dislike by adding 1 dislike
                idea.Dislike++;
                if (Session["DislikedIdeas"] == null)
                {
                    Session["DislikedIdeas"] = new List<int>();
                }
                ((List<int>)Session["DislikedIdeas"]).Add(id.Value);
            }

            // Check if the user liked the previous post
            if (Session["LikedIdeas"] != null && ((List<int>)Session["LikedIdeas"]).Contains(id.Value))
            {
                // The user liked the previous post, so we will cancel the like by subtracting 1 like
                idea.Like--;
                ((List<int>)Session["LikedIdeas"]).Remove(id.Value);
            }

            db.SaveChanges();
            return RedirectToAction("Index", new { topicId = idea.TopicId });
        }


        // GET: Ideas/Create
        public ActionResult Create(string id)
        {
            var categories = db.Categories.ToList();

            ViewBag.CategoryId = new SelectList(categories, "CategoryId", "Name");
            ViewBag.TopicId = id;
            var topicId = Convert.ToInt32(id);
            var topics = db.Topics.Where(t => t.TopicId == topicId).ToList();
            var selectList = new SelectList(topics, "TopicId", "Title");
            ViewBag.TopicId = selectList;
            return View();
        }

        // POST: Ideas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdeaId,Title,Brief,FileName,FileData,Content,CategoryId,TopicId,IsAnonymous")] Idea idea, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                // Gán thông tin ApplicationUser vào Idea
                idea.UserId = User.Identity.GetUserId();

                if (db.Ideas.Any(t => t.Title == idea.Title))
                {
                    ModelState.AddModelError("Title", "Title already exists");
                    return View(idea);
                }

                // Kiểm tra xem có đang ở chế độ ẩn danh hay không
                if (idea.IsAnonymous)
                {
                    idea.UserId = null;
                }

                // Lấy topic từ database theo TopicId được chọn
                var topic = db.Topics.Find(idea.TopicId);
                if (topic == null)
                {
                    return HttpNotFound();
                }

                if (file != null && file.ContentLength > 0)
                {
                    var newFileName = Guid.NewGuid();
                    var _extension = Path.GetExtension(file.FileName);
                    string NewName = newFileName + _extension;
                    string fileName = Path.GetFileName(NewName);
                    string filePath = Path.Combine(Server.MapPath("~/Uploads"), fileName);
                    file.SaveAs(filePath);
                    idea.FileData = filePath;
                    idea.FileName = fileName;
                }


                // Lưu idea vào database
                db.Ideas.Add(idea);
                db.SaveChanges();


                // Tạo đối tượng Status mới và lưu thông tin của nó vào cơ sở dữ liệu
                var status = new Stastus
                {
                    Title = "Idea Created",
                    Description = "A new idea has been created.",
                    DateCreated = DateTime.Now,
                    UserId = User.Identity.GetUserName()
                };
                db.Stastus.Add(status);
                db.SaveChanges();
                using (var client = new SmtpClient())
                {
                    // Kết nối đến server SMTP
                    client.Connect("smtp.gmail.com", 587, false);

                    // Đăng nhập bằng tài khoản email của bạn
                    client.Authenticate("qacoordinatordepartmentidea@gmail.com", "zydudyigmpqfpwmx");


                    // Tạo nội dung email
                    var bodyBuilder = new BodyBuilder
                    {
                        HtmlBody = $"<p>Time: {status.DateCreated}</p>" + $"<p>Topic: {idea.TopicId}</p>" + $"<p>Title: {idea.Title}</p>" + $"<p>Brief: {idea.Brief}</p>" + $"<p>User: {status.UserId}</p>",
                        TextBody = "{status.DateCreated} \r\n{idea.Topic} \r\n {idea.Title} \r\n {idea.Brief} \r\n{status.UserId}"
                    };

                    // Tạo message
                    var message = new MimeMessage
                    {
                        Body = bodyBuilder.ToMessageBody()
                    };
                    message.From.Add(new MailboxAddress("Norely my site", "qacoordinatordepartmentidea@gmail.com"));
                    message.To.Add(new MailboxAddress("User1", status.UserId));
                    message.Subject = "A new idea has been created";
                    client.Send(message);

                    client.Disconnect(true);


                }

                // Lưu thông báo Status vào TempData để truyền cho View
                TempData["StatusMessage"] = $"Create a new Idea successfully by ({User.Identity.Name}) sent to Department's QA Coordinator";


                return RedirectToAction("Index", new { topicId = idea.TopicId });

            }

            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name", idea.CategoryId);
            ViewBag.TopicId = new SelectList(db.Topics, "TopicId", "Title", idea.TopicId);


            return View(idea);
        }


        // POST: Ideas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdeaId,Title,Brief,FileName,FileData,Content")] Idea idea)
        {
            if (ModelState.IsValid)
            {
                db.Entry(idea).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(idea);
        }

        // GET: Ideas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Idea idea = db.Ideas.Find(id);
            if (idea == null)
            {
                return HttpNotFound();
            }
            return View(idea);
        }

        // POST: Ideas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Idea idea = db.Ideas.Find(id);
            db.Ideas.Remove(idea);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public FileResult Download(string fileName)
        {
            var fileVirtualPath = "~/Uploads/" + fileName;
            return File(fileVirtualPath, "application/force-download", fileName);
        }
    }
}
