using Idea1.Models;
using Ionic.Zip;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Idea1.Filter;

namespace Idea1.Controllers
{
    public class StaffController : Controller
    {
        // GET: Staff
        

        private ApplicationDbContext db = new ApplicationDbContext();
        [MyAuthenTilter]
        public ActionResult Index()
        {

            var topics = db.Topics.ToList();
           
            foreach (var topic in topics)
            {
                var lastDate = topic.LastDate.LocalDateTime;
                var firtsDate = topic.FirstDate.LocalDateTime;
                if (firtsDate >= DateTime.Now)
                {
                }
                else if (lastDate >= DateTime.Now)
                {
                }
                else
                {
                }
            }
            return View(topics);
        }


       [CustomAuthorize(Roles = "QA Manager")]

        public ActionResult ExportZip()
        {
            // Create a new zip file
            using (var zip = new ZipFile())
            {
                // Get all ideas from the database
                var ideas = db.Ideas.ToList();

                // Add each idea to the zip file
                foreach (var idea in ideas)
                {
                    // Create a file name for the idea based on its ID and title
                    var fileName = string.Format("{0}_{1}.txt", idea.IdeaId, idea.Title);

                    // Create a text file with the idea's information
                    var fileContent = string.Format("Title: {0}\r\nBrief: {1}\r\nContent: {2}\r\nCategory: {3}\r\nTopic: {4}\r\nLikes: {5}\r\nDislikes: {6}\r\nViews: {7}",
                        idea.Title, idea.Brief, idea.Content, idea.Category.Name, idea.Topic.Title, idea.Like, idea.Dislike, idea.Views);

                    // Add the file to the zip file
                    zip.AddEntry(fileName, fileContent);
                }

                // Set the name of the zip file
                string ZipfileName = "ideas.zip";

                // Set the content type of the response
                Response.ContentType = "application/zip";
                Response.AddHeader("Content-Disposition", "attachment; Zipfilename=" + ZipfileName);

                // Write the zip file to the response
                zip.Save(Response.OutputStream);
            }

            return new EmptyResult();
        }
        [CustomAuthorize(Roles = "QA Manager")]
        public ActionResult ExportExcel()
        {
            var ideas = db.Ideas.ToList();
            // Create a new Excel package
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage())
            {
                // Create a new worksheet
                var worksheet = package.Workbook.Worksheets.Add("ideas");


                // Add data to the worksheet
                worksheet.Cells["A1"].Value = "ID";
                worksheet.Cells["B1"].Value = "Title";
                worksheet.Cells["C1"].Value = "Brief";
                worksheet.Cells["D1"].Value = "Content";
                worksheet.Cells["E1"].Value = "Category";
                worksheet.Cells["F1"].Value = "Topic";
                worksheet.Cells["G1"].Value = "IsAnonymous";
                worksheet.Cells["H1"].Value = "Like";
                worksheet.Cells["I1"].Value = "Dislike";
                worksheet.Cells["J1"].Value = "Views";

                // Thêm dữ liệu vào bảng
                for (int i = 0; i < ideas.Count; i++)
                {
                    var idea = ideas[i];
                    worksheet.Cells[i + 2, 1].Value = idea.IdeaId;
                    worksheet.Cells[i + 2, 2].Value = idea.Title;
                    worksheet.Cells[i + 2, 3].Value = idea.Brief;
                    worksheet.Cells[i + 2, 4].Value = idea.Content;
                    worksheet.Cells[i + 2, 5].Value = idea.Category.Name;
                    worksheet.Cells[i + 2, 6].Value = idea.Topic.Title;
                    worksheet.Cells[i + 2, 7].Value = idea.IsAnonymous;
                    worksheet.Cells[i + 2, 8].Value = idea.Like;
                    worksheet.Cells[i + 2, 9].Value = idea.Dislike;
                    worksheet.Cells[i + 2, 10].Value = idea.Views;
                }

                // Set the name of the Excel file
                string fileName = "data.xlsx";

                // Set the content type of the response
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);

                // Write the Excel file to the response
                Response.BinaryWrite(package.GetAsByteArray());
            }

            return new EmptyResult();
        }
    }
}