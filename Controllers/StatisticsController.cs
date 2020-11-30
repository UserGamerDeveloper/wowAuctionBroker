using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using info;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mvc.Client.Data;
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
            StatisticsModel statisticsModel = new StatisticsModel();
            List<Task<List<FactionStatisticsModel>>> tasks = new List<Task<List<FactionStatisticsModel>>>();
            using (var db = new DatabaseContext())
            {
                foreach (var realmModel in db.Realms)
                {
                    var task = new Task<List<FactionStatisticsModel>>(() => { return GetReamlStats(realmModel.Id); });
                    task.Start();
                    tasks.Add(task);
                }
            }
            Task.WaitAll();
            foreach (var task in tasks)
            {
                foreach (var item in task.Result)
                {
                    statisticsModel.Factions.Add(item);
                    statisticsModel.AllMoney += item.Money + item.WaitMoney;
                }
            }
            return View(statisticsModel);
        }

        private static List<FactionStatisticsModel> GetReamlStats(int realmId)
        {
            List<FactionStatisticsModel> statisticsModels = new List<FactionStatisticsModel>();
            Server realm = null;
            using (var db = new DatabaseContext())
            {
                realm = new Server(db.Realms.Where(x => x.Id == realmId).First(), Loader.GetRecipeDataById());
            }
            foreach (var faction in realm.factions.Values)
            {
                statisticsModels.Add(new FactionStatisticsModel
                {
                    RealmName = realm.Name,
                    LastUpdate = string.Format("{0:0.} минут назад", DateTime.Now.Subtract(realm.timeUpdate).TotalMinutes),
                    FarmMode = faction.farmMode,
                    FractionName = faction.factionType.ToString(),
                    Money = ParseService.ConvertCopperToGold(faction.Money),
                    WaitMoney =  ParseService.ConvertCopperToGold(faction.moneyMax - faction.Money)
                });
            }
            return statisticsModels;
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
