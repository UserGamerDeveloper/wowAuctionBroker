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
                new Server(
                    RealmID.Twisting_Nether,
                    getAllRecipesHorde()),
                new Server(
                    RealmID.Blackmoore,
                    getAllRecipesHorde()),
                new Server(
                    RealmID.Antonidas,
                    getAllRecipesHorde()),
                new Server(
                    RealmID.Ravencrest,
                    getAllRecipesAlliance()),
                new Server(
                    RealmID.svezewatel,
                    getAllRecipesHorde()),
                new Server(
                    RealmID.gorduni,
                    getAllRecipesAlliance()),
                new Server(
                    RealmID.Azjol_Nerub,
                    getAllRecipesAlliance()),
                new Server(
                    RealmID.Kazzak,
                    getAllRecipesHorde()),
                new Server(
                    RealmID.Silvermoon,
                    getAllRecipesAlliance()),
                new Server(
                    RealmID.Tyrande,
                    new List<int>{
                        (int)RecipeInfo.Battlebound_Spaulders,
                        (int)RecipeInfo.Coarse_Leather_Cestus_H,
                        (int)RecipeInfo.Shimmerscale_Striker_H,
                        (int)RecipeInfo.Silkweave_Slippers,
                        (int)RecipeInfo.Tidespray_Linen_Pants_H
                    }),
                new Server(
                    RealmID.Hyjal,
                    getAllRecipesHorde()),
                new Server(
                    RealmID.Howling_Fjord,
                    getAllRecipesHorde()),
                new Server(
                    RealmID.malganis,
                    getAllRecipesAlliance())
            };
            foreach (var server in servers)
            {
                //File.WriteAllText(string.Format(@"realms\{0}.json", server.name), JsonConvert.SerializeObject(server, Formatting.Indented));
                using (FileStream fs = new FileStream(string.Format(@"realms\{0}.xml", server.name), FileMode.Create))
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
