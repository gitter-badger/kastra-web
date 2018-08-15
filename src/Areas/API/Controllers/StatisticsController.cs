using System;
using System.Collections.Generic;
using System.Linq;
using Kastra.Core.Business;
using Kastra.Core.Dto;
using Kastra.Core.DTO;
using Kastra.Web.API.Models.Statistics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Kastra.Web.API.Controllers
{
    [Area("Api")]
    [Authorize("Administration")]
    public class StatisticsController : Controller
    {
        private readonly IStatisticsManager _statisticsManager = null;
        private readonly UserManager<ApplicationUser> _userManager = null;

        public StatisticsController(IStatisticsManager statisticsManager, UserManager<ApplicationUser> userManager)
        {
            _statisticsManager = statisticsManager;
            _userManager = userManager;
        }

        public IActionResult GetGlobalStats()
        {
            GlobalStatsModel model = new GlobalStatsModel();

            // Visits per day
            DateTime dateNow = DateTime.Now;
            DateTime dateDay = new DateTime(dateNow.Year, dateNow.Month, dateNow.Day);

            model.VisitsPerDay = _statisticsManager.CountVisitsFromTo(dateDay, dateNow);

            // Total visits
            model.TotalVisits = _statisticsManager.CountVisitsFromTo(DateTime.MinValue, dateNow);

            // Number of users
            model.NumberOfUsers = _userManager.Users.Count();
            
            return Json(model);
        }

        public IActionResult GetVisitorsByDay()
        {
            int nbVisits = 0;
            string label = null;
            DateTime date;
            DateTime dateNow = DateTime.Now;
            DateTime dateFrom = dateNow.AddDays(-14);
            DataSet dataSet = new DataSet();
            List<string> labels = new List<string>(14);

            IList<VisitorInfo> visitsPerDay = _statisticsManager.GetVisitsFromDate(dateFrom, dateNow);

            ChartModel model = new ChartModel();
            model.Datasets = new List<DataSet>(1);

            for (int i = 1; i < 15; i++)
            {
                date = dateFrom.AddDays(i);
                    
                // Label
                label = date.ToString("dd/MM");
                labels.Add(label);

                // Value
                nbVisits = visitsPerDay
                    .Count(v => v.LastVisitAt.Day == date.Day && v.LastVisitAt.Month == date.Month && v.LastVisitAt.Year == date.Year);

                dataSet.Data.Add(nbVisits);
            }

            model.Datasets.Add(dataSet);
            model.Labels = labels;

            return Json(model);
        }

        public IActionResult GetVisits(int page = 0)
        {
            int pageSize = 5;
            VisitModel visitModel = null;
            List<VisitModel> model = new List<VisitModel>(pageSize);
            IList<VisitorInfo> visits = _statisticsManager.GetVisitsFromDate(DateTime.MinValue, DateTime.Now)
                                                          .OrderByDescending(v => v.LastVisitAt).Skip(page * pageSize)
                                                          .Take(pageSize).ToList();

            foreach(VisitorInfo visitor in visits)
            {
                visitModel = new VisitModel();
                visitModel.Date = visitor.LastVisitAt.ToString("dd/MM HH:mm:ss");
                visitModel.VisitId = visitor.Id.ToString();
                visitModel.UserAgent = visitor.UserAgent;
                visitModel.IpAddress = visitor.IpAddress;
                visitModel.Username =  _userManager.Users.SingleOrDefault(v => !String.IsNullOrEmpty(visitor.UserId) 
                                            && v.Id == visitor.UserId)?.UserName ?? "Visitor";

                model.Add(visitModel);
            }

            return Json(model);
        }

        public IActionResult GetRecentUsers()
        {
            UserModel userModel = null;
            List<UserModel> model = new List<UserModel>(5);
            IEnumerable<ApplicationUser> users = _userManager.Users
                                                      .ToList()
                                                      .TakeLast(5);

            foreach(ApplicationUser user in users)
            {
                userModel = new UserModel();
                userModel.Id = user.Id;
                userModel.Username = user.UserName;
                userModel.Email = user.Email;

                model.Add(userModel);
            }

            return Json(model);
        }
    }
}
