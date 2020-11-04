using Mvc.Client.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace info
{
    public enum RealmId
    {
        Twisting_Nether = 625,
        Ravencrest = 554,
        Blackmoore = 580,
        Antonidas = 564,
        malganis = 590,
        svezewatel = 1604,
        gorduni = 1602,
        Kazzak = 1305,
        Azjol_Nerub = 503,
        Silvermoon = 549,
        Tyrande = 1384,
        Hyjal = 542,
        Howling_Fjord = 1615
    }

    static class Saver
    {
        public static List<RealmModel> GetRealmModels()
        {
            List<RealmModel> servers = new List<RealmModel>();
            servers.Add(GetDraenor());
            //servers.Add(GetTwistingNether());
            //servers.Add(GetBlackmoore());
            //servers.Add(GetAntonidas());
            //servers.Add(GetRavencrest());
            //servers.Add(GetSvezewatel());
            servers.Add(GetGorduni());
            //servers.Add(GetAzjolNerub());
            //servers.Add(GetKazzak());
            //servers.Add(GetSilvermoon());
            //servers.Add(GetTyrande());
            //servers.Add(GetHyjal());
            //servers.Add(GetHowlingFjord());
            //servers.Add(GetMalganis());
            //servers.Add(GetTarrenMill());
            //servers.Add(GetSanguino());
            //servers.Add(GetBurningLegion());
            //servers.Add(GetStormscale());
            //servers.Add(GetAmbossar());
            //servers.Add(GetBurningBlade());
            //servers.Add(GetYsondre());
            //servers.Add(GetRagnaros());
            //servers.Add(GetEredar());
            //servers.Add(GetBlackrock());
            //servers.Add(GetVashj());
            //servers.Add(GetNemesis());
            //servers.Add(GetOutland());

            //SerializeServers(servers);
            return servers;
        }

        private static void SerializeServers(List<RealmModel> servers)
        {
            foreach (var server in servers)
            {
                File.WriteAllText(string.Format(@"realms\{0}.json", server.Name), JsonConvert.SerializeObject(server, Formatting.Indented));
            }
        }
        private static RealmModel GetDraenor()
        {
            return new RealmModel
            {
                Id = 506,
                ConnectedRealmId = 1403,
                Name = "Draenor",
                TimeUpdate = DateTime.Parse("Sat, 18 Aug 2018 07:22:16 GMT"),
                ActiveRecipes = GetDefaultAllianceRecipes(),
                FarmMode = false,
                Fractions = GetAlliance(50326495),
                Characters = GetCharacters(new long[] { 159994996 })
            };
        }
        private static RealmModel GetTwistingNether()
        {                
            return new RealmModel
            {
                Id = 625,
                ConnectedRealmId = 3674,
                Name = "Twisting Nether",
                TimeUpdate = DateTime.Parse("Sat, 18 Aug 2018 07:22:16 GMT"),
                ActiveRecipes = GetAllBestRecipes(),
                FarmMode = false,
                Fractions = GetAllFactions(0, 950143768),
                Characters = GetCharacters(new long[] { 145432820, 173932966 })
            };
        }
        private static RealmModel GetBlackmoore()
        {
            return new RealmModel
            {
                Id = 580,
                ConnectedRealmId = 580,
                Name = "Blackmoore",
                TimeUpdate = DateTime.Parse("Sat, 18 Aug 2018 07:22:16 GMT"),
                ActiveRecipes = GetAllBestRecipes(),
                FarmMode = false,
                Fractions = GetAllFactions(0, 863898942),
                Characters = GetCharacters(new long[] { 135371269, 165420727 })
            };
        }
        private static RealmModel GetAntonidas()
        {
            return new RealmModel
            {
                Id = 564,
                ConnectedRealmId = 3686,
                Name = "Antonidas",
                TimeUpdate = DateTime.Parse("Sat, 18 Aug 2018 07:22:16 GMT"),
                ActiveRecipes = GetAllRecipes(),
                FarmMode = false,
                Fractions = GetAllFactions(0, 0),
                Characters = GetCharacters(new long[] { 106880759, 155553194 })
            };
        }
        private static RealmModel GetRavencrest()
        {
            return new RealmModel
            {
                Id = 554,
                ConnectedRealmId = 1329,
                Name = "Ravencrest",
                TimeUpdate = DateTime.Parse("Sat, 18 Aug 2018 07:22:16 GMT"),
                ActiveRecipes = GetAllRecipes(),
                FarmMode = false,
                Fractions = GetAllFactions(0, 0),
                Characters = GetCharacters(new long[] { 106799868, 155553178 })
            };
        }
        private static RealmModel GetSvezewatel()
        {
            return new RealmModel
            {
                Id = 1604,
                ConnectedRealmId = 1604,
                Name = "Свежеватель душ",
                TimeUpdate = DateTime.Parse("Sat, 18 Aug 2018 07:22:16 GMT"),
                ActiveRecipes = GetAllBestRecipes(),
                FarmMode = false,
                Fractions = GetAllFactions(0, 1054117211),
                Characters = GetCharacters(new long[] { 166396992, 243483699 })
            };
        }
        private static RealmModel GetGorduni()
        {
            return new RealmModel
            {
                Id = 1602,
                ConnectedRealmId = 1602,
                Name = "Гордунни",
                TimeUpdate = DateTime.Parse("Sat, 18 Aug 2018 07:22:16 GMT"),
                ActiveRecipes = GetAllRecipes(),
                FarmMode = false,
                Fractions = GetAllFactions(268692993, 0),
                Characters = GetCharacters(new long[] { 181666336, 243869269 })
            };
        }
        private static RealmModel GetAzjolNerub()
        {
            return new RealmModel
            {
                Id = 503,
                ConnectedRealmId = 1396,
                Name = "Azjol Nerub",
                TimeUpdate = DateTime.Parse("Sat, 18 Aug 2018 07:22:16 GMT"),
                ActiveRecipes = GetAllAllianceRecipes(),
                FarmMode = false,
                Fractions = GetAlliance(1673272262),
                Characters = GetCharacters(new long[] { 154866560 })
            };
        }
        private static RealmModel GetKazzak()
        {
            return new RealmModel
            {
                Id = 1305,
                ConnectedRealmId = 1305,
                Name = "Kazzak",
                TimeUpdate = DateTime.Parse("Sat, 18 Aug 2018 07:22:16 GMT"),
                ActiveRecipes = GetAllBestRecipes(),
                FarmMode = false,
                Fractions = GetAllFactions(0, 484932767),
                Characters = GetCharacters(new long[] { 186121814, 193865192 })
            };
        }
        private static RealmModel GetSilvermoon()
        {
            return new RealmModel
            {
                Id = 549,
                ConnectedRealmId = 3391,
                Name = "Silvermoon",
                TimeUpdate = DateTime.Parse("Sat, 18 Aug 2018 07:22:16 GMT"),
                ActiveRecipes = GetAllRecipes(),
                FarmMode = false,
                Fractions = GetAllFactions(1108814681, 0),
                Characters = GetCharacters(new long[] { 186033540, 194930011 })
            };
        }
        private static RealmModel GetTyrande()
        {
            return new RealmModel
            {
                Id = 1384,
                ConnectedRealmId = 1384,
                Name = "Tyrande",
                TimeUpdate = DateTime.Parse("Sat, 18 Aug 2018 07:22:16 GMT"),
                ActiveRecipes = GetAllBestRecipes(),
                FarmMode = false,
                Fractions = GetAllFactions(0, 340788698),
                Characters = GetCharacters(new long[] { 162670117, 175763203 })
            };
        }
        private static RealmModel GetHyjal()
        {
            return new RealmModel
            {
                Id = 542,
                ConnectedRealmId = 1390,
                Name = "Hyjal",
                TimeUpdate = DateTime.Parse("Sat, 18 Aug 2018 07:22:16 GMT"),
                ActiveRecipes = GetAllBestRecipes(),
                FarmMode = false,
                Fractions = GetAllFactions(0, 1107497274),
                Characters = GetCharacters(new long[] { 187157904, 194654802 })
            };
        }
        private static RealmModel GetHowlingFjord()
        {
            return new RealmModel
            {
                Id = 1615,
                ConnectedRealmId = 1615,
                Name = "Ревущий фьорд",
                TimeUpdate = DateTime.Parse("Sat, 18 Aug 2018 07:22:16 GMT"),
                ActiveRecipes = GetAllBestRecipes(),
                FarmMode = false,
                Fractions = GetAllFactions(0, 242140920),
                Characters = GetCharacters(new long[] { 165853118, 174499728 })
            };
        }
        private static RealmModel GetMalganis()
        {
            return new RealmModel
            {
                Id = 590,
                ConnectedRealmId = 3691,
                Name = "Malganis",
                TimeUpdate = DateTime.Parse("Sat, 18 Aug 2018 07:22:16 GMT"),
                ActiveRecipes = GetAllAllianceRecipes(),
                FarmMode = false,
                Fractions = GetAlliance(1058206149),
                Characters = GetCharacters(new long[] { 153387171 })
            };
        }
        private static RealmModel GetTarrenMill()
        {
            return new RealmModel
            {
                Id = 1306,
                ConnectedRealmId = 1084,
                Name = "Tarren Mill",
                TimeUpdate = DateTime.Parse("Sat, 18 Aug 2018 07:22:16 GMT"),
                ActiveRecipes = GetDefaultAllianceRecipes(),
                FarmMode = false,
                Fractions = GetAlliance(0),
                Characters = GetCharacters(new long[] { 165821812 })
            };
        }
        private static RealmModel GetSanguino()
        {
            return new RealmModel
            {
                Id = 1382,
                ConnectedRealmId = 1379,
                Name = "Sanguino",
                TimeUpdate = DateTime.Parse("Sat, 18 Aug 2018 07:22:16 GMT"),
                ActiveRecipes = GetDefaultAllianceRecipes(),
                FarmMode = false,
                Fractions = GetAlliance(0),
                Characters = GetCharacters(new long[] { 175815695 })
            };
        }
        private static RealmModel GetBurningLegion()
        {
            return new RealmModel
            {
                Id = 524,
                ConnectedRealmId = 3713,
                Name = "Burning Legion",
                TimeUpdate = DateTime.Parse("Sat, 18 Aug 2018 07:22:16 GMT"),
                ActiveRecipes = GetDefaultAllianceRecipes(),
                FarmMode = false,
                Fractions = GetAlliance(0),
                Characters = GetCharacters(new long[] { 175817505 })
            };
        }
        private static RealmModel GetStormscale()
        {
            return new RealmModel
            {
                Id = 560,
                ConnectedRealmId = 2073,
                Name = "Stormscale",
                TimeUpdate = DateTime.Parse("Sat, 18 Aug 2018 07:22:16 GMT"),
                ActiveRecipes = GetDefaultAllianceRecipes(),
                FarmMode = false,
                Fractions = GetAlliance(0),
                Characters = GetCharacters(new long[] { 155309807 })
            };
        }
        private static RealmModel GetAmbossar()
        {
            return new RealmModel
            {
                Id = 1330,
                ConnectedRealmId = 604,
                Name = "Ambossar",
                TimeUpdate = DateTime.Parse("Sat, 18 Aug 2018 07:22:16 GMT"),
                ActiveRecipes = GetDefaultAllianceRecipes(),
                FarmMode = false,
                Fractions = GetAlliance(0),
                Characters = GetCharacters(new long[] { 165954703 })
            };
        }
        private static RealmModel GetBurningBlade()
        {
            return new RealmModel
            {
                Id = 523,
                ConnectedRealmId = 1092,
                Name = "Burning Blade",
                TimeUpdate = DateTime.Parse("Sat, 18 Aug 2018 07:22:16 GMT"),
                ActiveRecipes = GetDefaultAllianceRecipes(),
                FarmMode = false,
                Fractions = GetAlliance(0),
                Characters = GetCharacters(new long[] { 165825780 })
            };
        }
        private static RealmModel GetYsondre()
        {
            return new RealmModel
            {
                Id = 1335,
                ConnectedRealmId = 1335,
                Name = "Ysondre",
                TimeUpdate = DateTime.Parse("Sat, 18 Aug 2018 07:22:16 GMT"),
                ActiveRecipes = GetDefaultAllianceRecipes(),
                FarmMode = false,
                Fractions = GetAlliance(0),
                Characters = GetCharacters(new long[] { 155347612 })
            };
        }
        private static RealmModel GetRagnaros()
        {
            return new RealmModel
            {
                Id = 626,
                ConnectedRealmId = 3682,
                Name = "Ragnaros",
                TimeUpdate = DateTime.Parse("Sat, 18 Aug 2018 07:22:16 GMT"),
                ActiveRecipes = GetDefaultAllianceRecipes(),
                FarmMode = false,
                Fractions = GetAlliance(0),
                Characters = GetCharacters(new long[] { 174625955 })
            };
        }
        private static RealmModel GetEredar()
        {
            return new RealmModel
            {
                Id = 583,
                ConnectedRealmId = 3692,
                Name = "Eredar",
                TimeUpdate = DateTime.Parse("Sat, 18 Aug 2018 07:22:16 GMT"),
                ActiveRecipes = GetDefaultAllianceRecipes(),
                FarmMode = false,
                Fractions = GetAlliance(0),
                Characters = GetCharacters(new long[] { 155347589 })
            };
        }
        private static RealmModel GetBlackrock()
        {
            return new RealmModel
            {
                Id = 581,
                ConnectedRealmId = 581,
                Name = "Blackrock",
                TimeUpdate = DateTime.Parse("Sat, 18 Aug 2018 07:22:16 GMT"),
                ActiveRecipes = GetDefaultAllianceRecipes(),
                FarmMode = false,
                Fractions = GetAlliance(0),
                Characters = GetCharacters(new long[] { 165976699 })
            };
        }
        private static RealmModel GetVashj()
        {
            return new RealmModel
            {
                Id = 629,
                ConnectedRealmId = 3656,
                Name = "Vashj",
                TimeUpdate = DateTime.Parse("Sat, 18 Aug 2018 07:22:16 GMT"),
                ActiveRecipes = GetDefaultAllianceRecipes(),
                FarmMode = false,
                Fractions = GetAlliance(0),
                Characters = GetCharacters(new long[] { 166107334 })
            };
        }
        private static RealmModel GetNemesis()
        {
            return new RealmModel
            {
                Id = 1316,
                ConnectedRealmId = 1316,
                Name = "Nemesis",
                TimeUpdate = DateTime.Parse("Sat, 18 Aug 2018 07:22:16 GMT"),
                ActiveRecipes = GetDefaultAllianceRecipes(),
                FarmMode = false,
                Fractions = GetAlliance(0),
                Characters = GetCharacters(new long[] { 195039858 })
            };
        }
        private static RealmModel GetOutland()
        {
            return new RealmModel
            {
                Id = 1301,
                ConnectedRealmId = 1301,
                Name = "Outland",
                TimeUpdate = DateTime.Parse("Sat, 18 Aug 2018 07:22:16 GMT"),
                ActiveRecipes = GetDefaultHordeRecipes(),
                FarmMode = false,
                Fractions = GetHorde(0),
                Characters = GetCharacters(new long[] { 195040393 })
            };
        }

        private static List<ActiveRecipe> GetAllRecipes()
        {
            var a = new List<ActiveRecipe>();
            a.AddRange(GetDefaultAllianceRecipes());
            a.AddRange(GetBestHordeRecipes());
            a.AddRange(GetRecipes());
            return a;
        }
        private static List<ActiveRecipe> GetAllBestRecipes()
        {
            var a = new List<ActiveRecipe>();
            a.AddRange(GetBestAllianceRecipes());
            a.AddRange(GetBestHordeRecipes());
            a.AddRange(GetRecipes());
            return a;
        }
        private static List<ActiveRecipe> GetAllAllianceRecipes()
        {
            var a = new List<ActiveRecipe>();
            a.AddRange(GetBestAllianceRecipes());
            a.AddRange(GetAllianceRecipes());
            a.AddRange(GetRecipes());
            return a;
        }
        private static List<ActiveRecipe> GetDefaultRecipes()
        {
            var a = new List<ActiveRecipe>();
            a.AddRange(GetBestAllianceRecipes());
            a.AddRange(GetBestHordeRecipes());
            return a;
        }
        private static List<ActiveRecipe> GetDefaultAllianceRecipes()
        {
            var a = new List<ActiveRecipe>();
            a.AddRange(GetBestAllianceRecipes());
            a.AddRange(GetAllianceRecipes());
            return a;
        }
        private static List<ActiveRecipe> GetDefaultHordeRecipes()
        {
            var a = new List<ActiveRecipe>();
            a.AddRange(GetBestHordeRecipes());
            a.AddRange(GetHordeRecipes());
            return a;
        }
        private static List<ActiveRecipe> GetAllHordeRecipes()
        {
            var a = new List<ActiveRecipe>();
            a.AddRange(GetBestHordeRecipes());
            a.AddRange(GetRecipes());
            return a;
        }
        private static List<ActiveRecipe> GetBestAllianceRecipes()
        {
            return new List<ActiveRecipe>{
                new ActiveRecipe
                {
                    IdRecipe = (int)RecipeInfo.Coarse_Leather_Cestus_A
                },
                new ActiveRecipe
                {
                    IdRecipe = (int)RecipeInfo.Shimmerscale_Striker_A
                }
            };
        }
        private static List<ActiveRecipe> GetAllianceRecipes()
        {
            return new List<ActiveRecipe>{
                new ActiveRecipe
                {
                    IdRecipe = (int)RecipeInfo.Tidespray_Linen_Pants_A
                }
            };
        }
        private static List<ActiveRecipe> GetRecipes()
        {
            return new List<ActiveRecipe>{
                new ActiveRecipe
                {
                    IdRecipe = (int)RecipeInfo.Battlebound_Spaulders
                },
                new ActiveRecipe
                {
                    IdRecipe = (int)RecipeInfo.Silkweave_Slippers
                },
                new ActiveRecipe
                {
                    IdRecipe = (int)RecipeInfo.Warhide_Shoulderguard
                }
            };
        }
        private static List<ActiveRecipe> GetBestHordeRecipes()
        {
            return new List<ActiveRecipe>{
                new ActiveRecipe
                {
                    IdRecipe = (int)RecipeInfo.Tidespray_Linen_Pants_H
                }
            };
        }
        private static List<ActiveRecipe> GetHordeRecipes()
        {
            return new List<ActiveRecipe>{
                new ActiveRecipe
                {
                    IdRecipe = (int)RecipeInfo.Coarse_Leather_Cestus_H
                },
                new ActiveRecipe
                {
                    IdRecipe = (int)RecipeInfo.Shimmerscale_Striker_H
                }
            };
        }
        private static List<FactionModel> GetAllFactions(long moneyMaxA, long moneyMaxH)
        {
            return new List<FactionModel>{
                new FactionModel
                {
                    FactionType = FactionType.ALLIANCE,
                    MoneyMax = moneyMaxA
                },
                new FactionModel
                {
                    FactionType = FactionType.HORDE,
                    MoneyMax = moneyMaxH
                }
            };
        }
        private static List<FactionModel> GetHorde(long moneyMax)
        {
            return new List<FactionModel>{
                new FactionModel
                {
                    FactionType = FactionType.HORDE,
                    MoneyMax = moneyMax
                }
            };
        }
        private static List<FactionModel> GetAlliance(long moneyMax)
        {
            return new List<FactionModel>{
                new FactionModel
                {
                    FactionType = FactionType.ALLIANCE,
                    MoneyMax = moneyMax
                }
            };
        }
        private static List<Character> GetCharacters(long[] charactersId)
        {
            var characters = new List<Character>();
            foreach (var characterId in charactersId)
            {
                characters.Add(new Character {
                    Id = characterId
                });
            }
            return characters;
        }
    }
}
