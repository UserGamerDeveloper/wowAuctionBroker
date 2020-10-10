using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace info
{
    static class Saver
    {
        public static void SerializeServers()
        {
            List<Server> servers = new List<Server> {
                //new Server(
                //    ConnectedRealmId.Twisting_Nether,
                //    getAllRecipesHorde(),
                //    145432820,
                //    RealmId.Twisting_Nether),
                //new Server(
                //    ConnectedRealmId.Blackmoore,
                //    getAllRecipesHorde(),
                //    135371269,
                //    RealmId.Blackmoore),
                //new Server(
                //    ConnectedRealmId.Antonidas,
                //    getAllRecipesAlliance(),
                //    106880759,
                //    RealmId.Antonidas),
                //new Server(
                //    ConnectedRealmId.Ravencrest,
                //    getAllRecipesAlliance(),
                //    106799868,
                //    RealmId.Ravencrest),
                //new Server(
                //    ConnectedRealmId.svezewatel,
                //    getAllRecipesHorde(),
                //    166396992,
                //    RealmId.svezewatel),
                //new Server(
                //    ConnectedRealmId.gorduni,
                //    getAllRecipesAlliance(),
                //    181666336,
                //    RealmId.gorduni),
                //new Server(
                //    ConnectedRealmId.Azjol_Nerub,
                //    getAllRecipesAlliance(),
                //    154866560,
                //    RealmId.Azjol_Nerub),
                //new Server(
                //    ConnectedRealmId.Kazzak,
                //    getAllRecipesHorde(),
                //    186121814,
                //    RealmId.Kazzak),
                new Server(
                    ConnectedRealmId.Silvermoon,
                    getAllRecipesAlliance(),
                    186033540,
                    RealmId.Silvermoon),
                //new Server(
                //    ConnectedRealmId.Tyrande,
                //    new List<int>{
                //        (int)RecipeInfo.Battlebound_Spaulders,
                //        (int)RecipeInfo.Coarse_Leather_Cestus_H,
                //        (int)RecipeInfo.Shimmerscale_Striker_H,
                //        (int)RecipeInfo.Silkweave_Slippers,
                //        (int)RecipeInfo.Tidespray_Linen_Pants_H
                //    },
                //    162670117,
                //    RealmId.Tyrande),
                //new Server(
                //    ConnectedRealmId.Hyjal,
                //    getAllRecipesHorde(),
                //    187157904,
                //    RealmId.Hyjal),
                //new Server(
                //    ConnectedRealmId.Howling_Fjord,
                //    getAllRecipesHorde(),
                //    165853118,
                //    RealmId.Howling_Fjord),
                //new Server(
                //    ConnectedRealmId.malganis,
                //    getAllRecipesAlliance(),
                //    153387171,
                //    RealmId.malganis)
            };
            foreach (var server in servers)
            {
                //File.WriteAllText(string.Format(@"realms\{0}.json", server.name), JsonConvert.SerializeObject(server, Formatting.Indented));
                using (FileStream fs = new FileStream(string.Format(@"realms\{0}.xml", server.Name), FileMode.Create))
                {
                    XmlSerializer serverXmlSerializer = new XmlSerializer(typeof(Server));
                    serverXmlSerializer.Serialize(fs, server);
                }
            }
            //using (FileStream fs = new FileStream("servers.xml", FileMode.Create))
            //{
            //    XmlSerializer serverXmlSerializer = new XmlSerializer(typeof(Server[]));
            //    serverXmlSerializer.Serialize(fs, servers);
            //}
        }

        private static List<int> getAllRecipesHorde()
        {
            return new List<int>{
                        (int)RecipeInfo.Battlebound_Spaulders,
                        (int)RecipeInfo.Coarse_Leather_Cestus_H,
                        (int)RecipeInfo.Shimmerscale_Striker_H,
                        (int)RecipeInfo.Silkweave_Slippers,
                        (int)RecipeInfo.Tidespray_Linen_Pants_H,
                        (int)RecipeInfo.Warhide_Shoulderguard
            };
        }

        private static List<int> getAllRecipesAlliance()
        {
            return new List<int>{
                        (int)RecipeInfo.Battlebound_Spaulders,
                        (int)RecipeInfo.Coarse_Leather_Cestus_A,
                        (int)RecipeInfo.Shimmerscale_Striker_A,
                        (int)RecipeInfo.Silkweave_Slippers,
                        (int)RecipeInfo.Tidespray_Linen_Pants_A,
                        (int)RecipeInfo.Warhide_Shoulderguard
            };
        }
    }
}
