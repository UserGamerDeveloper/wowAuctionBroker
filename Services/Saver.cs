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
            //servers.Add(GetDraenor());
            //servers.Add(GetTwistingNether());
            //servers.Add(GetBlackmoore());
            ////servers.Add(GetAntonidas());
            ////servers.Add(GetRavencrest());
            ////servers.Add(GetSvezewatel());
            ////servers.Add(GetGorduni());
            ////servers.Add(GetAzjolNerub());
            ////servers.Add(GetKazzak());
            ////servers.Add(GetSilvermoon());
            //servers.Add(GetTyrande());
            //servers.Add(GetHyjal());
            ////servers.Add(GetHowlingFjord());
            ////servers.Add(GetMalganis());
            //servers.Add(GetTarrenMill());
            ////servers.Add(GetSanguino());
            ////servers.Add(GetBurningLegion());
            ////servers.Add(GetStormscale());
            ////servers.Add(GetAmbossar());
            ////servers.Add(GetBurningBlade());
            ////servers.Add(GetYsondre());
            ////servers.Add(GetRagnaros());
            ////servers.Add(GetEredar());
            ////servers.Add(GetBlackrock());
            ////servers.Add(GetVashj());
            //servers.Add(GetNemesis());
            ////servers.Add(GetOutland());
            ////servers.Add(GetArgentDawn());
            ////servers.Add(GetDefiasBrotherhood());
            ////servers.Add(GetThermaplugg());
            ////servers.Add(GetArchimonde());
            ////servers.Add(GetArathor());
            ////servers.Add(GetDeepholm());
            ////servers.Add(GetAshenvale());
            ////servers.Add(GetAzuregos());
            ////servers.Add(GetGreymane());
            ////servers.Add(GetDeathguard());
            //servers.Add(GetFordragon());
            //servers.Add(GetEversong());
            //servers.Add(GetTuralyon());

            SerializeServers(servers);
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
                ActiveRecipes = GetAllAllianceRecipes(),
                
                Fractions = GetAlliance(50326495),
                Characters = GetCharacters(new long[] { 163098237 })
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
                ActiveRecipes = GetAllHordeRecipes(),
                
                Fractions = GetHorde(0),
                Characters = GetCharacters(new long[] { 145432820 })
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
                ActiveRecipes = GetAllAllianceRecipes(),
                
                Fractions = GetAlliance(0),
                Characters = GetCharacters(new long[] { 165420727 })
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
                ActiveRecipes = GetAllAllianceRecipes(),
                Fractions = GetAlliance(72899),
                Characters = GetCharacters(new long[] { 106880759 })
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
                ActiveRecipes = GetAllRecipes(),
                
                Fractions = GetAllFactions(0, 110333),
                Characters = GetCharacters(new long[] { 166396992, 245608575 })
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
                ActiveRecipes = GetAllAllianceRecipes(),
                
                Fractions = GetAlliance(268692993),
                Characters = GetCharacters(new long[] { 181666336 })
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
                ActiveRecipes = GetAllAllianceRecipes(),
                
                Fractions = GetAlliance(0),
                Characters = GetCharacters(new long[] { 197222240 })
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
                ActiveRecipes = GetAllHordeRecipes(),
                
                Fractions = GetHorde(160000000),
                Characters = GetCharacters(new long[] { 194930011 })
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
                ActiveRecipes = GetAllHordeRecipes(),
                Fractions = GetHorde(340788698),
                Characters = GetCharacters(new long[] { 162670117 })
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
                ActiveRecipes = GetAllAllianceRecipes(),
                Fractions = GetAlliance(0),
                Characters = GetCharacters(new long[] { 197231158 })
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
                ActiveRecipes = GetAllRecipes(),
                
                Fractions = GetAllFactions(0, 242140920),
                Characters = GetCharacters(new long[] { 165853118, 176033760 })
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
                ActiveRecipes = GetAllAllianceRecipes(),
                Fractions = GetAlliance(0),
                Characters = GetCharacters(new long[] { 169765279 })
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
                ActiveRecipes = GetAllAllianceRecipes(),
                
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
                ActiveRecipes = GetAllAllianceRecipes(),
                
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
                ActiveRecipes = GetAllAllianceRecipes(),
                
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
                ActiveRecipes = GetAllAllianceRecipes(),
                
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
                ActiveRecipes = GetAllAllianceRecipes(),
                
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
                ActiveRecipes = GetAllAllianceRecipes(),
                
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
                ActiveRecipes = GetAllAllianceRecipes(),
                
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
                ActiveRecipes = GetAllAllianceRecipes(),
                
                Fractions = GetAlliance(0),
                Characters = GetCharacters(new long[] { 155665676 })
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
                ActiveRecipes = GetAllAllianceRecipes(),
                
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
                ActiveRecipes = GetAllAllianceRecipes(),
                
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
                ActiveRecipes = GetAllHordeRecipes(),
                
                Fractions = GetHorde(0),
                Characters = GetCharacters(new long[] { 197245888 })
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
                ActiveRecipes = GetAllHordeRecipes(),
                
                Fractions = GetHorde(0),
                Characters = GetCharacters(new long[] { 195040393 })
            };
        }
        private static RealmModel GetArgentDawn()
        {
            return new RealmModel
            {
                Id = 536,
                ConnectedRealmId = 3702,
                Name = "Argent Dawn",
                TimeUpdate = DateTime.Parse("Sat, 18 Aug 2018 07:22:16 GMT"),
                ActiveRecipes = GetAllAllianceRecipes(),
                Fractions = GetAlliance(0),
                Characters = GetCharacters(new long[] { 168371145 })
            };
        }
        private static RealmModel GetDefiasBrotherhood()
        {
            return new RealmModel
            {
                Id = 635,
                ConnectedRealmId = 1096,
                Name = "Defias Brotherhood",
                TimeUpdate = DateTime.Parse("Sat, 18 Aug 2018 07:22:16 GMT"),
                ActiveRecipes = GetAllHordeRecipes(),
                Fractions = GetHorde(0),
                Characters = GetCharacters(new long[] { 167458503 })
            };
        }
        private static RealmModel GetThermaplugg()
        {
            return new RealmModel
            {
                Id = 1926,
                ConnectedRealmId = 1929,
                Name = "Термоштепсель",
                TimeUpdate = DateTime.Parse("Sat, 18 Aug 2018 07:22:16 GMT"),
                ActiveRecipes = GetAllHordeRecipes(),
                Fractions = GetHorde(0),
                Characters = GetCharacters(new long[] { 246027016 })
            };
        }
        private static RealmModel GetArchimonde()
        {
            return new RealmModel
            {
                Id = 539,
                ConnectedRealmId = 1302,
                Name = "Archimonde",
                TimeUpdate = DateTime.Parse("Sat, 18 Aug 2018 07:22:16 GMT"),
                ActiveRecipes = GetAllHordeRecipes(),
                Fractions = GetHorde(0),
                Characters = GetCharacters(new long[] { 196924079 })
            };
        }
        private static RealmModel GetArathor()
        {
            return new RealmModel
            {
                Id = 501,
                ConnectedRealmId = 1587,
                Name = "Arathor",
                TimeUpdate = DateTime.Parse("Sat, 18 Aug 2018 07:22:16 GMT"),
                ActiveRecipes = GetAllAllianceRecipes(),
                Fractions = GetAlliance(0),
                Characters = GetCharacters(new long[] { 245763295 })
            };
        }
        private static RealmModel GetDeepholm()
        {
            return new RealmModel
            {
                Id = 1609,
                ConnectedRealmId = 1614,
                Name = "Подземье",
                TimeUpdate = DateTime.Parse("Sat, 18 Aug 2018 07:22:16 GMT"),
                ActiveRecipes = GetAllAllianceRecipes(),
                Fractions = GetAlliance(0),
                Characters = GetCharacters(new long[] { 176239290 })
            };
        }
        private static RealmModel GetAshenvale()
        {
            return new RealmModel
            {
                Id = 1923,
                ConnectedRealmId = 1923,
                Name = "Ясеневый лес",
                TimeUpdate = DateTime.Parse("Sat, 18 Aug 2018 07:22:16 GMT"),
                ActiveRecipes = GetAllAllianceRecipes(),
                Fractions = GetAlliance(0),
                Characters = GetCharacters(new long[] { 245763357 })
            };
        }
        private static RealmModel GetAzuregos()
        {
            return new RealmModel
            {
                Id = 1922,
                ConnectedRealmId = 1922,
                Name = "Азурегос",
                TimeUpdate = DateTime.Parse("Sat, 18 Aug 2018 07:22:16 GMT"),
                ActiveRecipes = GetAllAllianceRecipes(),
                Fractions = GetAlliance(0),
                Characters = GetCharacters(new long[] { 245764170 })
            };
        }
        private static RealmModel GetGreymane()
        {
            return new RealmModel
            {
                Id = 1610,
                ConnectedRealmId = 1928,
                Name = "Седогрив",
                TimeUpdate = DateTime.Parse("Sat, 18 Aug 2018 07:22:16 GMT"),
                ActiveRecipes = GetAllAllianceRecipes(),
                Fractions = GetAlliance(0),
                Characters = GetCharacters(new long[] { 245764119 })
            };
        }
        private static RealmModel GetDeathguard()
        {
            return new RealmModel
            {
                Id = 1605,
                ConnectedRealmId = 1605,
                Name = "Страж Смерти",
                TimeUpdate = DateTime.Parse("Sat, 18 Aug 2018 07:22:16 GMT"),
                ActiveRecipes = GetAllHordeRecipes(),
                Fractions = GetHorde(0),
                Characters = GetCharacters(new long[] { 246240897 })
            };
        }
        private static RealmModel GetFordragon()
        {
            return new RealmModel
            {
                Id = 1623,
                ConnectedRealmId = 1623,
                Name = "Дракономор",
                TimeUpdate = DateTime.Parse("Sat, 18 Aug 2018 07:22:16 GMT"),
                ActiveRecipes = GetAllHordeRecipes(),
                Fractions = GetHorde(0),
                Characters = GetCharacters(new long[] { 176821096 })
            };
        }
        private static RealmModel GetEversong()
        {
            return new RealmModel
            {
                Id = 1925,
                ConnectedRealmId = 1925,
                Name = "Вечная Песня",
                TimeUpdate = DateTime.Parse("Sat, 18 Aug 2018 07:22:16 GMT"),
                ActiveRecipes = GetAllAllianceRecipes(),
                Fractions = GetAlliance(0),
                Characters = GetCharacters(new long[] { 246573063 })
            };
        }
        private static RealmModel GetTuralyon()
        {
            return new RealmModel
            {
                Id = 553,
                ConnectedRealmId = 1402,
                Name = "Turalyon",
                TimeUpdate = DateTime.Parse("Sat, 18 Aug 2018 07:22:16 GMT"),
                ActiveRecipes = GetAllAllianceRecipes(),
                Fractions = GetAlliance(0),
                Characters = GetCharacters(new long[] { 162609411 })
            };
        }

        private static List<ActiveRecipe> GetAllRecipes()
        {
            var a = new List<ActiveRecipe>();
            a.AddRange(GetDefaultRecipes());
            a.AddRange(GetRecipes());
            return a;

        }
        static List<ActiveRecipe> GetDefaultRecipes()
        {
            var a = new List<ActiveRecipe>();
            a.AddRange(GetDefaultAllianceRecipes());
            a.AddRange(GetDefaultHordeRecipes());
            return a;
        }

        //private static List<ActiveRecipe> GetAllBestRecipes()
        //{
        //    var a = new List<ActiveRecipe>();
        //    a.AddRange(GetBestAllianceRecipes());
        //    a.AddRange(GetBestHordeRecipes());
        //    a.AddRange(GetRecipes());
        //    return a;
        //}
        private static List<ActiveRecipe> GetAllAllianceRecipes()
        {
            var a = new List<ActiveRecipe>();
            a.AddRange(GetDefaultAllianceRecipes());
            a.AddRange(GetRecipes());
            return a;

        }
        static List<ActiveRecipe> GetDefaultAllianceRecipes()
        {
            var a = new List<ActiveRecipe>();
            a.AddRange(GetBestAllianceRecipes());
            a.AddRange(GetNotBestAllianceRecipes());
            return a;
        }

        private static List<ActiveRecipe> GetAllHordeRecipes()
        {
            var a = new List<ActiveRecipe>();
            a.AddRange(GetDefaultHordeRecipes());
            a.AddRange(GetRecipes());
            return a;

        }
        static List<ActiveRecipe> GetDefaultHordeRecipes()
        {
            var a = new List<ActiveRecipe>();
            a.AddRange(GetBestHordeRecipes());
            a.AddRange(GetNotBestHordeRecipes());
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
                },
                new ActiveRecipe
                {
                    IdRecipe = (int)RecipeInfo.MagneticDiscombobulator_A
                },
                new ActiveRecipe
                {
                    IdRecipe = (int)RecipeInfo.HonorableCombatantsEtchedVessel_A
                },
                new ActiveRecipe
                {
                    IdRecipe = (int)RecipeInfo.MonelHardenedPolearm_A
                },
                new ActiveRecipe
                {
                    IdRecipe = (int)RecipeInfo.KyaniteRing_A
                },
                new ActiveRecipe
                {
                    IdRecipe = (int)RecipeInfo.SolstoneRing_A
                },
                new ActiveRecipe
                {
                    IdRecipe = (int)RecipeInfo.KubilineRing_A
                },
                new ActiveRecipe
                {
                    IdRecipe = (int)RecipeInfo.GoldenBerylRing_A
                }
            };
        }
        private static List<ActiveRecipe> GetNotBestAllianceRecipes()
        {
            return new List<ActiveRecipe>{
                new ActiveRecipe
                {
                    IdRecipe = (int)RecipeInfo.Tidespray_Linen_Pants_A
                },
                new ActiveRecipe
                {
                    IdRecipe = (int)RecipeInfo.EnchantersUmbralWand_A
                },
                new ActiveRecipe
                {
                    IdRecipe = (int)RecipeInfo.HonorableCombatantsPolearm_A
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
                },
                new ActiveRecipe
                {
                    IdRecipe = (int)RecipeInfo.EnchantersUmbralWand_H
                },
                new ActiveRecipe
                {
                    IdRecipe = (int)RecipeInfo.MagneticDiscombobulator_H
                },
                new ActiveRecipe
                {
                    IdRecipe = (int)RecipeInfo.HonorableCombatantsPolearm_H
                },
                new ActiveRecipe
                {
                    IdRecipe = (int)RecipeInfo.KyaniteRing_H
                },
                new ActiveRecipe
                {
                    IdRecipe = (int)RecipeInfo.SolstoneRing_H
                },
                new ActiveRecipe
                {
                    IdRecipe = (int)RecipeInfo.KubilineRing_H
                },
                new ActiveRecipe
                {
                    IdRecipe = (int)RecipeInfo.GoldenBerylRing_H
                }
            };
        }
        private static List<ActiveRecipe> GetNotBestHordeRecipes()
        {
            return new List<ActiveRecipe>{
                new ActiveRecipe
                {
                    IdRecipe = (int)RecipeInfo.Coarse_Leather_Cestus_H
                },
                new ActiveRecipe
                {
                    IdRecipe = (int)RecipeInfo.Shimmerscale_Striker_H
                },
                new ActiveRecipe
                {
                    IdRecipe = (int)RecipeInfo.HonorableCombatantsEtchedVessel_H
                },
                new ActiveRecipe
                {
                    IdRecipe = (int)RecipeInfo.MonelHardenedPolearm_H
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
