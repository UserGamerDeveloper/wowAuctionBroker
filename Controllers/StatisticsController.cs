using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mvc.Client.Models;
using wowCalc;

namespace Mvc.Client.Controllers
{
    public class StatisticsController : Controller
    {
        ParseService parseService;
        public StatisticsController(ParseService parseService)
        {
            this.parseService = parseService;
        }

        // GET: StatisticsController
        public ActionResult Index()
        {
            List<StatisticsModel> statisticsModels = new List<StatisticsModel>();
            List<Task> tasks = new List<Task>();
            foreach (var server in parseService.GetModel().Values)
            {
                tasks.Add(server.UpdateMoney());
            }
            Task.WaitAll(tasks.ToArray());
            foreach (var server in parseService.GetModel().Values.OrderByDescending(server => server.Money))
            {
                statisticsModels.Add(new StatisticsModel()
                {
                    Money = ParseService.ConvertCopperToGold(server.Money).ToString("N0"),
                    WaitMoney = ParseService.ConvertCopperToGold(server.moneyMax - server.Money).ToString("N0"),
                    Name = server.Name,
                    LastUpdate = string.Format("{0:0.} минут назад", DateTime.Now.Subtract(server.timeUpdate).TotalMinutes),
                    FarmMode = server.farmMode
                });
            }
            return View(statisticsModels);
        }

        // GET: StatisticsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: StatisticsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: StatisticsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: StatisticsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: StatisticsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: StatisticsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: StatisticsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
