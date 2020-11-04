using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using info;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Mvc.Client.Data;
using Mvc.Client.Models;
using Newtonsoft.Json;
using wowCalc;

namespace Mvc.Client.Controllers
{
    public class CalculatorController : Controller
    {
        ParseService parseService;
        public CalculatorController(ParseService parseService)
        {
            this.parseService = parseService;
        }

        // GET: CalculatorController
        public ActionResult Index()
        {
            //CalculatorModel calculatorModel = new CalculatorModel
            //{
            //    ReamlsNameSelectList = new SelectList(parseService.GetModel().Values, "Name", "Name")
            //};
            CalculatorModel calculatorModel = new CalculatorModel();
            foreach (var realm in ParseService.GetRealms())
            {
                calculatorModel.ReamlsNameSelectList.Add(new SelectListItem { Text = realm.Name, Value = realm.Id.ToString() });
            }
            calculatorModel.ReamlsNameSelectList.Add(new SelectListItem { Text = "Выберите реалм", Value = "", Selected = true, Disabled = true });
            calculatorModel.RequiredIncomeGold = ParseService.settings.TARGET_INCOME_IN_HOUR;
            //TempData["model"] = JsonConvert.SerializeObject(calculatorModel);
            TempData["LastSelectedRealmId"] = -1;
            TempData["LastSelectedFaction"] = FactionType.NONE;
            return View(calculatorModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(CalculatorModel calculatorModel)
        {
            //CalculatorModel calculatorModel = new CalculatorModel();
            //calculatorModel.SelectedRealmName = selectedRealmName;
            //calculatorModel.SelectedRecipeTreeName = selectedRecipeTreeName;
            //CalculatorModel calculatorModela = JsonConvert.DeserializeObject<CalculatorModel>(TempData["model"].ToString());
            //calculatorModel.ReamlsNameSelectList =
            //    new SelectList(parseService.GetModel().Values, "Name", "Name", parseService.GetModel()[calculatorModel.SelectedRealmName]);
            if (calculatorModel.SelectedRealmId != (int)TempData["LastSelectedRealmId"])
            {
                calculatorModel.SelectedFaction = FactionType.NONE;
            }
            if (calculatorModel.SelectedFaction != (FactionType)TempData["LastSelectedFaction"])
            {
                calculatorModel.SelectedItemID = int.MinValue;
            }
            Server selectedRealm = null;
            using (var db = new DatabaseContext())
            {
                foreach (var realm in db.Realms)
                {
                    if (realm.Id == calculatorModel.SelectedRealmId)
                    {
                        selectedRealm = new Server(realm, Loader.GetRecipeDataById());
                        calculatorModel.ReamlsNameSelectList.Add(new SelectListItem { Text = realm.Name, Value = realm.Id.ToString(), Selected = true });
                    }
                    else
                    {
                        calculatorModel.ReamlsNameSelectList.Add(new SelectListItem { Text = realm.Name, Value = realm.Id.ToString() });
                    }
                }
            }
            Faction selectedFaction = null;
            if (selectedRealm.factions.Values.Count == 1)
            {
                selectedFaction = selectedRealm.factions.Values.First();
                calculatorModel.SelectedFaction = selectedFaction.factionType;
                calculatorModel.FactionSelectList.Add(new SelectListItem
                {
                    Text = selectedFaction.factionType.ToString(),
                    Value = ((int)selectedFaction.factionType).ToString(),
                    Selected = true
                });
            }
            else
            {
                foreach (var faction in selectedRealm.factions.Values)
                {
                    if (faction.factionType == calculatorModel.SelectedFaction)
                    {
                        selectedFaction = selectedRealm.factions[calculatorModel.SelectedFaction];
                        calculatorModel.FactionSelectList.Add(new SelectListItem
                        {
                            Text = faction.factionType.ToString(),
                            Value = ((int)faction.factionType).ToString(),
                            Selected = true
                        });
                    }
                    else
                    {
                        calculatorModel.FactionSelectList.Add(new SelectListItem
                        {
                            Text = faction.factionType.ToString(),
                            Value = ((int)faction.factionType).ToString(),
                        });
                    }
                }
            }
            if (calculatorModel.SelectedFaction != FactionType.NONE)
            {
                HashSet<ItemData> itemsData = new HashSet<ItemData>();
                foreach (var recipeTree in selectedFaction.RecipeDataTrees.Values)
                {
                    foreach (var recipe in recipeTree)
                    {
                        foreach (var id in recipe.ItemsData)
                        {
                            itemsData.Add(id);
                        }
                    }
                }
                foreach (var itemData in itemsData)
                {
                    if (itemData.id == calculatorModel.SelectedItemID)
                    {
                        calculatorModel.RecipeTreeNameSelectList.Add(new SelectListItem { Text = itemData.itemName, Value = itemData.id.ToString(), Selected = true });
                    }
                    else
                    {
                        calculatorModel.RecipeTreeNameSelectList.Add(new SelectListItem { Text = itemData.itemName, Value = itemData.id.ToString() });
                    }
                }
                if (calculatorModel.SelectedItemID != int.MinValue)
                {
                    HashSet<RecipeData> recipesData = new HashSet<RecipeData>();
                    foreach (var recipesDataTree in selectedFaction.RecipeDataTrees.Values)
                    {
                        foreach (var recipeData in recipesDataTree)
                        {
                            if (recipeData.ID_ITEM_AND_NEED_AMOUNT.Keys.Contains(calculatorModel.SelectedItemID))
                            {
                                recipesData.Add(recipeData);
                            }
                        }
                    }
                    if (recipesData.Count == 1 && recipesData.First().ItemsData.Count == 1)
                    {
                        RecipeData recipeData = recipesData.First();
                        long spending = Convert.ToInt64(recipeData.SPENDING * selectedFaction.GetSpendingRate());
                        double value = (recipeData.SellNormalPrice - spending -
                            calculatorModel.GetTargetIncomeCopperInMillisecond() * recipeData.GetNeedMillisecondsToGetProfit()) /
                            recipeData.ID_ITEM_AND_NEED_AMOUNT.Values.First();
                        calculatorModel.Result.Add(
                            recipeData.Name,
                            string.Format("{0:# ##}", Math.Floor(value / 100)));
                    }
                    else
                    {
                        foreach (var recipeData in recipesData)
                        {
                            foreach (var itemData in recipeData.ItemsData)
                            {
                                if (itemData.id != calculatorModel.SelectedItemID)
                                {
                                    calculatorModel.ItemList.Add(itemData);
                                }
                            }
                        }
                        if (calculatorModel.Items.Count > 0 &&
                            calculatorModel.Items.Keys.SelectMany(
                                id => calculatorModel.ItemList.Where(itemData => itemData.id == id)).Count() == calculatorModel.Items.Count)
                        {
                            double value = double.MinValue;
                            RecipeData recipe = null;
                            foreach (var recipeData in recipesData)
                            {
                                long spending = 0;
                                var idItems = recipeData.ID_ITEM_AND_NEED_AMOUNT.Keys.ToList();
                                idItems.Remove(calculatorModel.SelectedItemID);
                                foreach (var idItem in idItems)
                                {
                                    spending += calculatorModel.Items[idItem] * 100 * recipeData.ID_ITEM_AND_NEED_AMOUNT[idItem];
                                }
                                spending += Convert.ToInt64(recipeData.SPENDING * selectedFaction.GetSpendingRate());
                                double tempValue = (recipeData.SellNormalPrice - spending -
                                    calculatorModel.GetTargetIncomeCopperInMillisecond() * recipeData.GetNeedMillisecondsToGetProfit()) /
                                    recipeData.ID_ITEM_AND_NEED_AMOUNT[calculatorModel.SelectedItemID];
                                if (value < tempValue)
                                {
                                    value = tempValue;
                                    recipe = recipeData;
                                }
                            }
                            calculatorModel.Result.Add(recipe.Name, string.Format("  {0:# ##}", Math.Floor(value / 100)));
                        }
                    }
                }
            }
            //calculatorModel.LastSelectedRealmName = calculatorModel.SelectedRealmName;
            TempData["LastSelectedRealmId"] = calculatorModel.SelectedRealmId;
            TempData["LastSelectedFaction"] = calculatorModel.SelectedFaction;
            return View(calculatorModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SelectRealm(string realmName)
        {
            //List<SelectListItem> recipeTreeNameSelectListItem = new List<SelectListItem>();
            //foreach (var key in parseService.GetModel()[realmName].RecipeDataTrees.Keys)
            //{
            //    recipeTreeNameSelectListItem.Add(
            //        new SelectListItem { Text = key, Value = key });
            //}
            //ViewBag.RecipeTreeNameSelectList = recipeTreeNameSelectListItem;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SelectRecipeTree(string recipeTreeName)
        {
            return View();
        }

        // GET: CalculatorController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CalculatorController/Delete/5
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
