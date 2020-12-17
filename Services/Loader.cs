using Microsoft.Extensions.Hosting;
using Mvc.Client.Data;
using Mvc.Client.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using wowCalc;

namespace info
{
    static class Loader
    {
        public static Settings DeserializeSettings()
        {
            using (FileStream fs = new FileStream("settings.xml", FileMode.Open))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(Settings));
                return (Settings)xmlSerializer.Deserialize(fs);
            }
        }
        public static Dictionary<int, List<RecipeData>> GetRecipeDataById()
        {
            const double DefaultNeedMillisecondsToCraft = 2000;
            const double BFANeedMillisecondsToCraft = 1000;
            const double DefaultNeedMillisecondsToGetProfit = DefaultNeedMillisecondsToCraft + RecipeData.NeedMillisecondsToSell;
            const double BFANeedMillisecondsToGetProfit = BFANeedMillisecondsToCraft + RecipeData.NeedMillisecondsToSell;

            List<List<RecipeData>> recipesData = new List<List<RecipeData>>();
            //recipesData.Add(Crafted_Dreadful_Gladiators_Cloak_of_Prowess());
            recipesData.Add(GoldenBerylRing_H());
            recipesData.Add(GoldenBerylRing_A());
            recipesData.Add(KubilineRing_H());
            recipesData.Add(KubilineRing_A());
            recipesData.Add(SolstoneRing_H());
            recipesData.Add(SolstoneRing_A());
            recipesData.Add(KyaniteRing_A());
            recipesData.Add(KyaniteRing_H());
            recipesData.Add(HonorableCombatantsPolearm_A());
            recipesData.Add(HonorableCombatantsPolearm_H());
            recipesData.Add(MonelHardenedPolearm_H());
            recipesData.Add(MonelHardenedPolearm_A());
            recipesData.Add(HonorableCombatantsEtchedVessel_H());
            recipesData.Add(HonorableCombatantsEtchedVessel_A());
            recipesData.Add(MagneticDiscombobulator_H());
            recipesData.Add(MagneticDiscombobulator_A());
            recipesData.Add(Warhide_Shoulderguard());
            recipesData.Add(Battlebound_Spaulders());
            recipesData.Add(Coarse_Leather_Cestus_A());
            recipesData.Add(Coarse_Leather_Cestus_H());
            recipesData.Add(Tidespray_Linen_Pants_A());
            recipesData.Add(Tidespray_Linen_Pants_H());
            recipesData.Add(Silkweave_Slippers());
            recipesData.Add(Shimmerscale_Striker_A());
            recipesData.Add(Shimmerscale_Striker_H());
            recipesData.Add(GetEnchantersUmbralWand_A());
            recipesData.Add(GetEnchantersUmbralWand_H());
            Dictionary<int, ItemData> itemsDataById = GetItemsData();
            Dictionary<int, List<RecipeData>> recipeDataById = new Dictionary<int, List<RecipeData>>();
            foreach (var recipeDataList in recipesData)
            {
                recipeDataById.Add(recipeDataList.First().ID, recipeDataList);
                foreach (var recipeData in recipeDataList)
                {
                    recipeData.SetData(itemsDataById);
                }
            }
            return recipeDataById;

            static List<RecipeData> GoldenBerylRing_A()
            {
                return new List<RecipeData>{
                    new RecipeData(
                        RecipeInfo.GoldenBerylRing_A,
                        new XmlSerializableDictionary<int, int>(){
                            {(int)ItemInfo.GoldenBeryl, 1}, {(int)ItemInfo.MoneliteOre , 10}
                        },
                        172144,
                        750,
                        0,
                        172144,
                        FactionType.ALLIANCE,
                        true)
                };
            }

            static List<RecipeData> KubilineRing_A()
            {
                return new List<RecipeData>{
                    new RecipeData(
                        RecipeInfo.KubilineRing_A,
                        new XmlSerializableDictionary<int, int>(){
                            {(int)ItemInfo.Kubiline, 1}, {(int)ItemInfo.MoneliteOre , 10}
                        },
                        171505,
                        750,
                        0,
                        171505,
                        FactionType.ALLIANCE,
                        true)
                };
            }

            static List<RecipeData> SolstoneRing_A()
            {
                return new List<RecipeData>{
                    new RecipeData(
                        RecipeInfo.SolstoneRing_A,
                        new XmlSerializableDictionary<int, int>(){
                            {(int)ItemInfo.Solstone, 1}, {(int)ItemInfo.MoneliteOre , 10}
                        },
                        172765,
                        750,
                        0,
                        172765,
                        FactionType.ALLIANCE,
                        true)
                };
            }

            static List<RecipeData> KyaniteRing_A()
            {
                return new List<RecipeData>{
                    new RecipeData(
                        RecipeInfo.KyaniteRing_A,
                        new XmlSerializableDictionary<int, int>(){
                            {(int)ItemInfo.Kyanite, 1}, {(int)ItemInfo.MoneliteOre , 10}
                        },
                        166293,
                        750,
                        0,
                        166293,
                        FactionType.ALLIANCE,
                        true)
                };
            }

            static List<RecipeData> GoldenBerylRing_H()
            {
                return new List<RecipeData>{
                    new RecipeData(
                        RecipeInfo.GoldenBerylRing_H,
                        new XmlSerializableDictionary<int, int>(){
                            {(int)ItemInfo.GoldenBeryl, 1}, {(int)ItemInfo.MoneliteOre , 10}
                        },
                        172144,
                        750,
                        0,
                        172144,
                        FactionType.HORDE,
                        true)
                };
            }

            static List<RecipeData> KubilineRing_H()
            {
                return new List<RecipeData>{
                    new RecipeData(
                        RecipeInfo.KubilineRing_H,
                        new XmlSerializableDictionary<int, int>(){
                            {(int)ItemInfo.Kubiline, 1}, {(int)ItemInfo.MoneliteOre , 10}
                        },
                        171505,
                        750,
                        0,
                        171505,
                        FactionType.HORDE,
                        true)
                };
            }

            static List<RecipeData> SolstoneRing_H()
            {
                return new List<RecipeData>{
                    new RecipeData(
                        RecipeInfo.SolstoneRing_H,
                        new XmlSerializableDictionary<int, int>(){
                            {(int)ItemInfo.Solstone, 1}, {(int)ItemInfo.MoneliteOre , 10}
                        },
                        172765,
                        750,
                        0,
                        172765,
                        FactionType.HORDE,
                        true)
                };
            }

            static List<RecipeData> KyaniteRing_H()
            {
                return new List<RecipeData>{
                    new RecipeData(
                        RecipeInfo.KyaniteRing_H,
                        new XmlSerializableDictionary<int, int>(){
                            {(int)ItemInfo.Kyanite, 1}, {(int)ItemInfo.MoneliteOre , 10}
                        },
                        166293,
                        750,
                        0,
                        166293,
                        FactionType.HORDE,
                        true)
                };
            }

            static List<RecipeData> HonorableCombatantsPolearm_A()
            {
                return new List<RecipeData>{
                    new RecipeData(
                        RecipeInfo.HonorableCombatantsPolearm_A,
                        new XmlSerializableDictionary<int, int>(){
                            {(int)ItemInfo.StormSilverOre, 56}
                        },
                        907444,
                        BFANeedMillisecondsToGetProfit,
                        30000*14,
                        907444,
                        FactionType.ALLIANCE,
                        true)
                };
            }

            static List<RecipeData> HonorableCombatantsPolearm_H()
            {
                return new List<RecipeData>{
                    new RecipeData(
                        RecipeInfo.HonorableCombatantsPolearm_H,
                        new XmlSerializableDictionary<int, int>(){
                            {(int)ItemInfo.StormSilverOre, 56}
                        },
                        913814,
                        BFANeedMillisecondsToGetProfit,
                        30000*14,
                        913814,
                        FactionType.HORDE,
                        true)
                };
            }

            static List<RecipeData> MonelHardenedPolearm_A()
            {
                return new List<RecipeData>{
                    new RecipeData(
                        RecipeInfo.MonelHardenedPolearm_A,
                        new XmlSerializableDictionary<int, int>(){
                            {(int)ItemInfo.MoneliteOre, 20}
                        },
                        513218,
                        BFANeedMillisecondsToGetProfit,
                        3000*3,
                        513218,
                        FactionType.ALLIANCE,
                        true)
                };
            }

            static List<RecipeData> MonelHardenedPolearm_H()
            {
                return new List<RecipeData>{
                    new RecipeData(
                        RecipeInfo.MonelHardenedPolearm_H,
                        new XmlSerializableDictionary<int, int>(){
                            {(int)ItemInfo.MoneliteOre, 20}
                        },
                        503313,
                        BFANeedMillisecondsToGetProfit,
                        3000*3,
                        503313,
                        FactionType.HORDE,
                        true)
                };
            }

            static List<RecipeData> HonorableCombatantsEtchedVessel_H()
            {
                return new List<RecipeData>{
                    new RecipeData(
                        RecipeInfo.HonorableCombatantsEtchedVessel_H,
                        new XmlSerializableDictionary<int, int>(){
                            {(int)ItemInfo.CrimsonInk , 30}, {(int)ItemInfo.ViridescentInk , 12}
                        },
                        465519,
                        750,
                        200,
                        465519,
                        FactionType.HORDE,
                        true),
                    new RecipeData(
                        RecipeInfo.HonorableCombatantsEtchedVessel_H,
                        new XmlSerializableDictionary<int, int>(){
                            {(int)ItemInfo.CrimsonInk , 30}, {(int)ItemInfo.ViridescentPigment , 12}
                        },
                        465519,
                        750 + 750*12,
                        200 + 20*12 + 1000*12,
                        465519,
                        FactionType.HORDE,
                        false),
                    new RecipeData(
                        RecipeInfo.HonorableCombatantsEtchedVessel_H,
                        new XmlSerializableDictionary<int, int>(){
                            {(int)ItemInfo.CrimsonInk , 30}, {(int)ItemInfo.UmbralInk , 120}
                        },
                        465519,
                        750,
                        200,
                        465519,
                        FactionType.HORDE,
                        false),
                    new RecipeData(
                        RecipeInfo.HonorableCombatantsEtchedVessel_H,
                        new XmlSerializableDictionary<int, int>(){
                            {(int)ItemInfo.CrimsonPigment , 30}, {(int)ItemInfo.UmbralInk , 120}
                        },
                        465519,
                        750 + 750*30,
                        200 + 20*30,
                        465519,
                        FactionType.HORDE,
                        false),
                    new RecipeData(
                        RecipeInfo.HonorableCombatantsEtchedVessel_H,
                        new XmlSerializableDictionary<int, int>(){
                            {(int)ItemInfo.CrimsonPigment , 30}, {(int)ItemInfo.ViridescentPigment , 12}
                        },
                        465519,
                        750 + 750*12 + 750*30,
                        200 + 20*12 + 1000*12 + 20*30,
                        465519,
                        FactionType.HORDE,
                        false),
                    new RecipeData(
                        RecipeInfo.HonorableCombatantsEtchedVessel_H,
                        new XmlSerializableDictionary<int, int>(){
                            {(int)ItemInfo.CrimsonPigment , 30}, {(int)ItemInfo.ViridescentInk , 12}
                        },
                        465519,
                        750 + 750*30,
                        200 + 20*30,
                        465519,
                        FactionType.HORDE,
                        false)
                };
            }

            static List<RecipeData> HonorableCombatantsEtchedVessel_A()
            {
                return new List<RecipeData>{
                    new RecipeData(
                        RecipeInfo.HonorableCombatantsEtchedVessel_A,
                        new XmlSerializableDictionary<int, int>(){
                            {(int)ItemInfo.CrimsonInk , 30}, {(int)ItemInfo.ViridescentInk , 12}
                        },
                        468937,
                        750,
                        200,
                        468937,
                        FactionType.ALLIANCE,
                        true),
                    new RecipeData(
                        RecipeInfo.HonorableCombatantsEtchedVessel_A,
                        new XmlSerializableDictionary<int, int>(){
                            {(int)ItemInfo.CrimsonInk , 30}, {(int)ItemInfo.ViridescentPigment , 12}
                        },
                        468937,
                        750 + 750*12,
                        200 + 20*12 + 1000*12,
                        468937,
                        FactionType.ALLIANCE,
                        false),
                    new RecipeData(
                        RecipeInfo.HonorableCombatantsEtchedVessel_A,
                        new XmlSerializableDictionary<int, int>(){
                            {(int)ItemInfo.CrimsonInk , 30}, {(int)ItemInfo.UmbralInk , 120}
                        },
                        468937,
                        750,
                        200,
                        468937,
                        FactionType.ALLIANCE,
                        false),
                    new RecipeData(
                        RecipeInfo.HonorableCombatantsEtchedVessel_A,
                        new XmlSerializableDictionary<int, int>(){
                            {(int)ItemInfo.CrimsonPigment , 30}, {(int)ItemInfo.UmbralInk , 120}
                        },
                        468937,
                        750 + 750*30,
                        200 + 20*30,
                        468937,
                        FactionType.ALLIANCE,
                        false),
                    new RecipeData(
                        RecipeInfo.HonorableCombatantsEtchedVessel_A,
                        new XmlSerializableDictionary<int, int>(){
                            {(int)ItemInfo.CrimsonPigment , 30}, {(int)ItemInfo.ViridescentPigment , 12}
                        },
                        468937,
                        750 + 750*12 + 750*30,
                        200 + 20*12 + 1000*12 + 20*30,
                        468937,
                        FactionType.ALLIANCE,
                        false),
                    new RecipeData(
                        RecipeInfo.HonorableCombatantsEtchedVessel_A,
                        new XmlSerializableDictionary<int, int>(){
                            {(int)ItemInfo.CrimsonPigment , 30}, {(int)ItemInfo.ViridescentInk , 12}
                        },
                        468937,
                        750 + 750*30,
                        200 + 20*30,
                        468937,
                        FactionType.ALLIANCE,
                        false)
                };
            }

            static List<RecipeData> MagneticDiscombobulator_A()
            {
                return new List<RecipeData>{
                    new RecipeData(
                        RecipeInfo.MagneticDiscombobulator_A,
                        new XmlSerializableDictionary<int, int>(){
                            {(int)ItemInfo.MoneliteOre , 25}, {(int)ItemInfo.StormSilverOre , 10}
                        },
                        398336,
                        BFANeedMillisecondsToGetProfit,
                        0,
                        398336,
                        FactionType.ALLIANCE,
                        true)
                };
            }

            static List<RecipeData> MagneticDiscombobulator_H()
            {
                return new List<RecipeData>{
                    new RecipeData(
                        RecipeInfo.MagneticDiscombobulator_H,
                        new XmlSerializableDictionary<int, int>(){
                            {(int)ItemInfo.MoneliteOre , 25}, {(int)ItemInfo.StormSilverOre , 10}
                        },
                        398336,
                        BFANeedMillisecondsToGetProfit,
                        0,
                        398336,
                        FactionType.HORDE,
                        true)
                };
            }

            static List<RecipeData> Crafted_Dreadful_Gladiators_Cloak_of_Prowess()
            {
                return new List<RecipeData>{
                new RecipeData(
                    RecipeInfo.Crafted_Dreadful_Gladiators_Cloak_of_Prowess,
                    new XmlSerializableDictionary<int, int>(){
                        {(int)ItemInfo.Windwool_Cloth , 20}
                    },
                    0,
                    6000,
                    0)
                };
            }

            static List<RecipeData> Warhide_Shoulderguard()
            {
                return new List<RecipeData>{
                new RecipeData(
                    RecipeInfo.Warhide_Shoulderguard,
                    new XmlSerializableDictionary<int, int>(){
                        {(int)ItemInfo.Stonehide_Leather , 12}
                    },
                    135700,
                    DefaultNeedMillisecondsToGetProfit,
                    0,
                    135700)
                };
            }

            static List<RecipeData> Battlebound_Spaulders()
            {
                return new List<RecipeData>{
                new RecipeData(
                    RecipeInfo.Battlebound_Spaulders,
                    new XmlSerializableDictionary<int, int>(){
                        {(int)ItemInfo.Stormscale , 12}
                    },
                    133656,
                    DefaultNeedMillisecondsToGetProfit,
                    0,
                    133656)
                };
            }

            static List<RecipeData> Coarse_Leather_Cestus_A()
            {
                return new List<RecipeData>{
                new RecipeData(
                    RecipeInfo.Coarse_Leather_Cestus_A,
                    new XmlSerializableDictionary<int, int>(){
                        {(int)ItemInfo.CoarseLeather , 10}, {(int)ItemInfo.BloodStainedBone , 8}
                    },
                    400336,
                    BFANeedMillisecondsToGetProfit,
                    0,
                    400336,
                    FactionType.ALLIANCE,
                    true)
                };
            }

            static List<RecipeData> Coarse_Leather_Cestus_H()
            {
                return new List<RecipeData>{
                new RecipeData(
                    RecipeInfo.Coarse_Leather_Cestus_H,
                    new XmlSerializableDictionary<int, int>(){
                        {(int)ItemInfo.CoarseLeather , 10}, {(int)ItemInfo.BloodStainedBone , 8}
                    },
                    391157,
                    BFANeedMillisecondsToGetProfit,
                    0,
                    391157,
                    FactionType.HORDE,
                    true)
                };
            }

            static List<RecipeData> Tidespray_Linen_Pants_A()
            {
                return new List<RecipeData>{
                new RecipeData(
                    RecipeInfo.Tidespray_Linen_Pants_A,
                    new XmlSerializableDictionary<int, int>(){
                        {(int)ItemInfo.Tidespray_Linen , 17}
                    },
                    271119,
                    BFANeedMillisecondsToGetProfit,
                    60000,
                    271119,
                    FactionType.ALLIANCE,
                    true)
                };
            }

            static List<RecipeData> Tidespray_Linen_Pants_H()
            {
                return new List<RecipeData>{
                new RecipeData(
                    RecipeInfo.Tidespray_Linen_Pants_H,
                    new XmlSerializableDictionary<int, int>(){
                        {(int)ItemInfo.Tidespray_Linen , 17}
                    },
                    276913,
                    BFANeedMillisecondsToGetProfit,
                    60000,
                    276913,
                    FactionType.HORDE,
                    true)
                };
            }

            static List<RecipeData> Silkweave_Slippers()
            {
                return new List<RecipeData>{
                    new RecipeData(
                        RecipeInfo.Silkweave_Slippers,
                        new XmlSerializableDictionary<int, int>(){
                            {(int)ItemInfo.Shaldorei_Silk , 12}
                        },
                        139747,
                        DefaultNeedMillisecondsToGetProfit,
                        5000,
                        139747)
                };
            }

            static List<RecipeData> Shimmerscale_Striker_A()
            {
                return new List<RecipeData>{
                    new RecipeData(
                        RecipeInfo.Shimmerscale_Striker_A,
                        new XmlSerializableDictionary<int, int>(){
                            {(int)ItemInfo.Shimmerscale , 10}, {(int)ItemInfo.BloodStainedBone , 8}
                        },
                        398924,
                        BFANeedMillisecondsToGetProfit,
                        0,
                        398924,
                        FactionType.ALLIANCE,
                        true)
                };
            }

            static List<RecipeData> Shimmerscale_Striker_H()
            {
                return new List<RecipeData>{
                    new RecipeData(
                        RecipeInfo.Shimmerscale_Striker_H,
                        new XmlSerializableDictionary<int, int>(){
                            {(int)ItemInfo.Shimmerscale , 10}, {(int)ItemInfo.BloodStainedBone , 8}
                        },
                        389706,
                        BFANeedMillisecondsToGetProfit,
                        0,
                        389706,
                        FactionType.HORDE,
                        true)
                };
            }

            static List<RecipeData> GetEnchantersUmbralWand_A()
            {
                return new List<RecipeData>{
                    new RecipeData(
                        RecipeInfo.EnchantersUmbralWand_A,
                        new XmlSerializableDictionary<int, int>(){
                            {(int)ItemInfo.GloomDust , 20}, {(int)ItemInfo.UmbraShard , 1}
                        },
                        287131,
                        BFANeedMillisecondsToGetProfit,
                        4500,
                        287131,
                        FactionType.ALLIANCE,
                        true),
                    new RecipeData(
                        RecipeInfo.EnchantersUmbralWand_A,
                        new XmlSerializableDictionary<int, int>(){
                            {(int)ItemInfo.GloomDust , 17}, {(int)ItemInfo.VeiledCrystal , 1}
                        },
                        287131,
                        BFANeedMillisecondsToCraft + 1000 + 1000,
                        4500,
                        287131,
                        FactionType.ALLIANCE,
                        false),
                    new RecipeData(
                        RecipeInfo.EnchantersUmbralWand_A,
                        new XmlSerializableDictionary<int, int>(){
                            {(int)ItemInfo.GloomDust , 20}, {(int)ItemInfo.VeiledCrystal , 1}
                        },
                        287131,
                        BFANeedMillisecondsToCraft + 1000,
                        4500,
                        287131,
                        FactionType.ALLIANCE,
                        false),
                    new RecipeData(
                        RecipeInfo.EnchantersUmbralWand_A,
                        new XmlSerializableDictionary<int, int>(){
                            {(int)ItemInfo.UmbraShard , 6}, {(int)ItemInfo.VeiledCrystal , 1}
                        },
                        287131,
                        BFANeedMillisecondsToCraft + 7*1000 + 1000,
                        4500,
                        287131,
                        FactionType.ALLIANCE,
                        false),
                    new RecipeData(
                        RecipeInfo.EnchantersUmbralWand_A,
                        new XmlSerializableDictionary<int, int>(){
                            {(int)ItemInfo.VeiledCrystal , 4}
                        },
                        287131,
                        BFANeedMillisecondsToCraft + 7*1000 + 4*1000,
                        4500,
                        287131,
                        FactionType.ALLIANCE,
                        false),
                    new RecipeData(
                        RecipeInfo.EnchantersUmbralWand_A,
                        new XmlSerializableDictionary<int, int>(){
                            {(int)ItemInfo.UmbraShard , 8}
                        },
                        287131,
                        BFANeedMillisecondsToCraft + 7*1000,
                        4500,
                        287131,
                        FactionType.ALLIANCE,
                        false)
                };
            }

            static List<RecipeData> GetEnchantersUmbralWand_H()
            {
                return new List<RecipeData>{
                    new RecipeData(
                        RecipeInfo.EnchantersUmbralWand_H,
                        new XmlSerializableDictionary<int, int>(){
                            {(int)ItemInfo.GloomDust , 20}, {(int)ItemInfo.UmbraShard , 1}
                        },
                        308607,
                        BFANeedMillisecondsToGetProfit,
                        4500,
                        308607,
                        FactionType.HORDE,
                        true),
                    new RecipeData(
                        RecipeInfo.EnchantersUmbralWand_H,
                        new XmlSerializableDictionary<int, int>(){
                            {(int)ItemInfo.GloomDust , 17}, {(int)ItemInfo.VeiledCrystal , 1}
                        },
                        308607,
                        BFANeedMillisecondsToCraft + 1000 + 1000,
                        4500,
                        308607,
                        FactionType.HORDE,
                        false),
                    new RecipeData(
                        RecipeInfo.EnchantersUmbralWand_H,
                        new XmlSerializableDictionary<int, int>(){
                            {(int)ItemInfo.GloomDust , 20}, {(int)ItemInfo.VeiledCrystal , 1}
                        },
                        308607,
                        BFANeedMillisecondsToCraft + 1000,
                        4500,
                        308607,
                        FactionType.HORDE,
                        false),
                    new RecipeData(
                        RecipeInfo.EnchantersUmbralWand_H,
                        new XmlSerializableDictionary<int, int>(){
                            {(int)ItemInfo.UmbraShard , 6}, {(int)ItemInfo.VeiledCrystal , 1}
                        },
                        308607,
                        BFANeedMillisecondsToCraft + 7*1000 + 1000,
                        4500,
                        308607,
                        FactionType.HORDE,
                        false),
                    new RecipeData(
                        RecipeInfo.EnchantersUmbralWand_H,
                        new XmlSerializableDictionary<int, int>(){
                            {(int)ItemInfo.VeiledCrystal , 4}
                        },
                        308607,
                        BFANeedMillisecondsToCraft + 7*1000 + 4*1000,
                        4500,
                        308607,
                        FactionType.HORDE,
                        false),
                    new RecipeData(
                        RecipeInfo.EnchantersUmbralWand_H,
                        new XmlSerializableDictionary<int, int>(){
                            {(int)ItemInfo.UmbraShard , 8}
                        },
                        308607,
                        BFANeedMillisecondsToCraft + 7*1000,
                        4500,
                        308607,
                        FactionType.HORDE,
                        false)
                    };
            }
        }

        public static List<RealmModel> GetServersByName(IHostEnvironment hostingEnvironment)
        {
            List<RealmModel> realms;
            //Server.RefreshTokenPrice();
            using (var db = new DatabaseContext())
            {
                if (hostingEnvironment.IsDevelopment())
                {
                    foreach (var realm in db.Realms)
                    {
                        db.Remove(realm);
                    }
                    db.SaveChanges();
                    foreach (var realmModel in Saver.GetRealmModels())
                    {
                        db.Add(realmModel);
                    }
                    db.SaveChanges();
                }
                else
                {
                    foreach (var file in new DirectoryInfo("realms").GetFiles())
                    {
                        var realmModel = JsonConvert.DeserializeObject<RealmModel>(File.ReadAllText(file.FullName));
                        var b = db.Realms.Where(x => x.Id == realmModel.Id);
                        if (b.Count() > 0)
                        {
                            var s = b.First();
                            foreach (var faction in realmModel.Fractions)
                            {
                                var sd = s.Fractions.FindAll(x => x.FactionType == faction.FactionType);
                                if (sd.Count > 0)
                                {
                                    faction.MoneyMax = sd.First().MoneyMax;
                                }
                            }
                            db.Remove(s);
                            db.SaveChanges();
                        }
                        db.Add(realmModel);
                        file.Delete();
                    }
                    db.SaveChanges();
                }
                realms = db.Realms.ToList();
            }
            return realms;
        }

        private static Dictionary<int, ItemData> GetItemsData()
        {
            List<ItemData> items = new List<ItemData>
            {
                new ItemData(ItemInfo.Shaldorei_Silk, "Shal'dorei Silk | Шал'дорайский шелк"),
                new ItemData(ItemInfo.Stonehide_Leather, "Stonehide Leather | Твердокаменная кожа"),
                new ItemData(ItemInfo.Stormscale, "Stormscale | Штормовая чешуя"),
                new ItemData(ItemInfo.Tidespray_Linen, "Tidespray Linen | Морской лен"),
                new ItemData(ItemInfo.Shimmerscale, "Shimmerscale | Поблескивающая чешуя"),
                new ItemData(ItemInfo.BloodStainedBone, "Blood-Stained Bone | Окровавленная кость"),
                new ItemData(ItemInfo.CoarseLeather, "Coarse Leather | Шершавая кожа"),
                new ItemData(ItemInfo.Windwool_Cloth, "Windwool Cloth | Ветрошерстяная ткань"),
                new ItemData(ItemInfo.StormSilverOre, "Storm Silver Ore | Руда штормового серебра"),
                new ItemData(ItemInfo.GloomDust, "Gloom Dust | Сумеречная пыль"),
                new ItemData(ItemInfo.UmbraShard, "Umbra Shard | Теневой осколок"),
                new ItemData(ItemInfo.CrimsonInk, "Crimson Ink | Алые чернила"),
                new ItemData(ItemInfo.CrimsonPigment, "Crimson Pigment | Алый краситель"),
                new ItemData(ItemInfo.ViridescentInk, "Viridescent Ink | Изумрудно-зеленые чернила"),
                new ItemData(ItemInfo.ViridescentPigment, "Viridescent Pigment | Изумрудно-зеленый краситель"),
                new ItemData(ItemInfo.UmbralInk, "Umbral Ink | Теневые чернила"),
                new ItemData(ItemInfo.Kyanite, "Kyanite | Кианит"),
                new ItemData(ItemInfo.Solstone, "Solstone | Солнцекамень"),
                new ItemData(ItemInfo.Kubiline, "Kubiline | Кубилин"),
                new ItemData(ItemInfo.GoldenBeryl, "Golden Beryl | Золотистый берилл"),
                new ItemData(ItemInfo.VeiledCrystal, "Veiled Crystal | Затуманенный кристалл"),
                new ItemData(ItemInfo.MoneliteOre, "Monelite Ore | Монелитовая руда")
            };
            Dictionary<int, ItemData> itemsDataById = new Dictionary<int, ItemData>();
            foreach (var item in items)
            {
                itemsDataById.Add(item.id, item);
            }
            return itemsDataById;
        }
    }
}
