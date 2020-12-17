using info;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using Mvc.Client.Data;
using Mvc.Client.Models;
using NAudio.Wave;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
//using System.Windows.Forms;
using System.Xml.Serialization;
using static Mvc.Client.Models.RealmModel;

namespace wowCalc
{
    public class CharacterData
    {
        public class Faction
        {
            /// <summary>
            /// 
            /// </summary>
            public FactionType type { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string name { get; set; }
        }
        //public class Self
        //{
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public string href { get; set; }
        //}
        //public class _links
        //{
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public Self self { get; set; }
        //}
        //public class Gender
        //{
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public string type { get; set; }
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public string name { get; set; }
        //}
        //public class Key
        //{
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public string href { get; set; }
        //}
        //public class Race
        //{
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public Key key { get; set; }
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public string name { get; set; }
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public int id { get; set; }
        //}
        //public class Key
        //{
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public string href { get; set; }
        //}
        //public class Character_class
        //{
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public Key key { get; set; }
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public string name { get; set; }
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public int id { get; set; }
        //}
        //public class Key
        //{
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public string href { get; set; }
        //}
        //public class Active_spec
        //{
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public Key key { get; set; }
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public string name { get; set; }
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public int id { get; set; }
        //}
        //public class Key
        //{
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public string href { get; set; }
        //}
        //public class Realm
        //{
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public Key key { get; set; }
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public string name { get; set; }
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public int id { get; set; }
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public string slug { get; set; }
        //}
        //public class Key
        //{
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public string href { get; set; }
        //}
        //public class Key
        //{
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public string href { get; set; }
        //}
        //public class Realm
        //{
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public Key key { get; set; }
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public string name { get; set; }
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public int id { get; set; }
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public string slug { get; set; }
        //}
        //public class Faction
        //{
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public string type { get; set; }
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public string name { get; set; }
        //}
        //public class Guild
        //{
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public Key key { get; set; }
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public string name { get; set; }
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public int id { get; set; }
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public Realm realm { get; set; }
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public Faction faction { get; set; }
        //}

        //public class Achievements
        //{
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public string href { get; set; }
        //}

        //public class Titles
        //{
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public string href { get; set; }
        //}

        //public class Pvp_summary
        //{
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public string href { get; set; }
        //}

        //public class Encounters
        //{
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public string href { get; set; }
        //}

        //public class Media
        //{
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public string href { get; set; }
        //}

        //public class Specializations
        //{
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public string href { get; set; }
        //}

        //public class Statistics
        //{
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public string href { get; set; }
        //}

        //public class Mythic_keystone_profile
        //{
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public string href { get; set; }
        //}

        //public class Equipment
        //{
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public string href { get; set; }
        //}

        //public class Appearance
        //{
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public string href { get; set; }
        //}

        //public class Collections
        //{
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public string href { get; set; }
        //}

        //public class Reputations
        //{
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public string href { get; set; }
        //}

        //public class Quests
        //{
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public string href { get; set; }
        //}

        //public class Achievements_statistics
        //{
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public string href { get; set; }
        //}

        //public class Professions
        //{
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public string href { get; set; }
        //}
        /// <summary>
        /// 
        /// </summary>
        public Faction faction { get; set; }
        ///// <summary>
        ///// 
        ///// </summary>
        //public _links _links { get; set; }
        ///// <summary>
        ///// 
        ///// </summary>
        //public int id { get; set; }
        ///// <summary>
        ///// 
        ///// </summary>
        //public string name { get; set; }
        ///// <summary>
        ///// 
        ///// </summary>
        //public Gender gender { get; set; }
        ///// <summary>
        ///// 
        ///// </summary>
        //public Race race { get; set; }
        ///// <summary>
        ///// 
        ///// </summary>
        //public Character_class character_class { get; set; }
        ///// <summary>
        ///// 
        ///// </summary>
        //public Active_spec active_spec { get; set; }
        ///// <summary>
        ///// 
        ///// </summary>
        //public Realm realm { get; set; }
        ///// <summary>
        ///// 
        ///// </summary>
        //public Guild guild { get; set; }
        ///// <summary>
        ///// 
        ///// </summary>
        //public int level { get; set; }
        ///// <summary>
        ///// 
        ///// </summary>
        //public int experience { get; set; }
        ///// <summary>
        ///// 
        ///// </summary>
        //public int achievement_points { get; set; }
        ///// <summary>
        ///// 
        ///// </summary>
        //public Achievements achievements { get; set; }
        ///// <summary>
        ///// 
        ///// </summary>
        //public Titles titles { get; set; }
        ///// <summary>
        ///// 
        ///// </summary>
        //public Pvp_summary pvp_summary { get; set; }
        ///// <summary>
        ///// 
        ///// </summary>
        //public Encounters encounters { get; set; }
        ///// <summary>
        ///// 
        ///// </summary>
        //public Media media { get; set; }
        ///// <summary>
        ///// 
        ///// </summary>
        //public int last_login_timestamp { get; set; }
        ///// <summary>
        ///// 
        ///// </summary>
        //public int average_item_level { get; set; }
        ///// <summary>
        ///// 
        ///// </summary>
        //public int equipped_item_level { get; set; }
        ///// <summary>
        ///// 
        ///// </summary>
        //public Specializations specializations { get; set; }
        ///// <summary>
        ///// 
        ///// </summary>
        //public Statistics statistics { get; set; }
        ///// <summary>
        ///// 
        ///// </summary>
        //public Mythic_keystone_profile mythic_keystone_profile { get; set; }
        ///// <summary>
        ///// 
        ///// </summary>
        //public Equipment equipment { get; set; }
        ///// <summary>
        ///// 
        ///// </summary>
        //public Appearance appearance { get; set; }
        ///// <summary>
        ///// 
        ///// </summary>
        //public Collections collections { get; set; }
        ///// <summary>
        ///// 
        ///// </summary>
        //public Reputations reputations { get; set; }
        ///// <summary>
        ///// 
        ///// </summary>
        //public Quests quests { get; set; }
        ///// <summary>
        ///// 
        ///// </summary>
        //public Achievements_statistics achievements_statistics { get; set; }
        ///// <summary>
        ///// 
        ///// </summary>
        //public Professions professions { get; set; }
    }
    public class AuctionData
    {
        public class Auction
        {
            public class Itemm
            {
                //public class Modifier
                //{
                //    [JsonProperty("type")]
                //    public int Type { get; set; }
                //    [JsonProperty("value")]
                //    public int Value { get; set; }
                //}

                [JsonProperty("id")]
                public int Id { get; set; }
                //[JsonProperty("context")]
                //public int Context { get; set; }
                //[JsonProperty("modifiers")]
                //public List<Modifier> Modifiers { get; set; }
                //[JsonProperty("pet_breed_id")]
                //public int PetBreedId { get; set; }
                //[JsonProperty("pet_level")]
                //public int PetLevel { get; set; }
                //[JsonProperty("pet_quality_id")]
                //public int PetQualityId { get; set; }
                //[JsonProperty("pet_species_id")]
                //public int PetSpeciesId { get; set; }
            }

            [JsonProperty("item")]
            public Itemm Item { get; set; }
            [JsonProperty("unit_price")]
            public long UnitPrice { get; set; }
            [JsonProperty("quantity")]
            public int Quantity { get; set; }
            //[JsonProperty("id")]
            //public int Id { get; set; }
            //[JsonProperty("bid")]
            //public long Bid { get; set; }
            //[JsonProperty("buyout")]
            //public long Buyout { get; set; }
            //[JsonProperty("time_left")]
            //public string TimeLeft { get; set; }
        }
        //public class Links
        //{
        //    [JsonProperty("self")]
        //    public Self Self { get; set; }
        //}
        //public class Self
        //{
        //    [JsonProperty("href")]
        //    public string Href { get; set; }
        //}
        //public class ConnectedRealm
        //{
        //    [JsonProperty("href")]
        //    public string Href { get; set; }
        //}

        [JsonProperty("auctions")]
        public List<Auction> Auctions { get; set; }
        //[JsonProperty("_links")]
        //public Links Links { get; set; }
        //[JsonProperty("connected_realm")]
        //public ConnectedRealm ConnectedRealm { get; set; }
    }
    public class CharacterProtectedData
    {
        public class Character
        {
            //[JsonProperty("key")]
            //public Key Key { get; set; }
            //[JsonProperty("name")]
            //public string Name { get; set; }
            //[JsonProperty("id")]
            //public int Id { get; set; }
            [JsonProperty("realm")]
            public Realm Realm { get; set; }
        }
        public class Realm
        {
            //[JsonProperty("key")]
            //public Key Key { get; set; }
            //[JsonProperty("name")]
            //public string Name { get; set; }
            //[JsonProperty("id")]
            //public int Id { get; set; }
            [JsonProperty("slug")]
            public string Slug { get; set; }
        }
        //public class Links
        //{
        //    [JsonProperty("self")]
        //    public Self Self { get; set; }
        //    [JsonProperty("user")]
        //    public User User { get; set; }
        //    [JsonProperty("profile")]
        //    public Profile Profile { get; set; }
        //}
        //public class Self
        //{
        //    [JsonProperty("href")]
        //    public string Href { get; set; }
        //}
        //public class User
        //{
        //    [JsonProperty("href")]
        //    public string Href { get; set; }
        //}
        //public class Profile
        //{
        //    [JsonProperty("href")]
        //    public string Href { get; set; }
        //}
        //public class Key
        //{
        //    [JsonProperty("href")]
        //    public string Href { get; set; }
        //}
        //public class ProtectedStats
        //{
        //    [JsonProperty("total_number_deaths")]
        //    public int TotalNumberDeaths { get; set; }
        //    [JsonProperty("total_gold_gained")]
        //    public long TotalGoldGained { get; set; }
        //    [JsonProperty("total_gold_lost")]
        //    public long TotalGoldLost { get; set; }
        //    [JsonProperty("total_item_value_gained")]
        //    public int TotalItemValueGained { get; set; }
        //    [JsonProperty("level_number_deaths")]
        //    public int LevelNumberDeaths { get; set; }
        //    [JsonProperty("level_gold_gained")]
        //    public long LevelGoldGained { get; set; }
        //    [JsonProperty("level_gold_lost")]
        //    public long LevelGoldLost { get; set; }
        //    [JsonProperty("level_item_value_gained")]
        //    public int LevelItemValueGained { get; set; }
        //}
        //public class Position
        //{
        //    [JsonProperty("zone")]
        //    public Zone Zone { get; set; }
        //    [JsonProperty("map")]
        //    public Map Map { get; set; }
        //    [JsonProperty("x")]
        //    public double X { get; set; }
        //    [JsonProperty("y")]
        //    public double Y { get; set; }
        //    [JsonProperty("z")]
        //    public double Z { get; set; }
        //    [JsonProperty("facing")]
        //    public double Facing { get; set; }
        //}
        //public class Zone
        //{
        //    [JsonProperty("name")]
        //    public string Name { get; set; }
        //    [JsonProperty("id")]
        //    public int Id { get; set; }
        //}
        //public class Map
        //{
        //    [JsonProperty("name")]
        //    public string Name { get; set; }
        //    [JsonProperty("id")]
        //    public int Id { get; set; }
        //}
        //public class BindPosition
        //{
        //    [JsonProperty("zone")]
        //    public Zone Zone { get; set; }
        //    [JsonProperty("map")]
        //    public Map Map { get; set; }
        //    [JsonProperty("x")]
        //    public double X { get; set; }
        //    [JsonProperty("y")]
        //    public double Y { get; set; }
        //    [JsonProperty("z")]
        //    public double Z { get; set; }
        //    [JsonProperty("facing")]
        //    public double Facing { get; set; }
        //}

        [JsonProperty("money")]
        public long Money { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("character")]
        public Character Characterr { get; set; }
        //[JsonProperty("_links")]
        //public Links Links { get; set; }
        //[JsonProperty("id")]
        //public int Id { get; set; }
        //[JsonProperty("protected_stats")]
        //public ProtectedStats ProtectedStats { get; set; }
        //[JsonProperty("position")]
        //public Position Position { get; set; }
        //[JsonProperty("bind_position")]
        //public BindPosition BindPosition { get; set; }
        //[JsonProperty("wow_account")]
        //public int WowAccount { get; set; }
    }
    public class TokenPriceData
    {
        //public class Links
        //{
        //    [JsonProperty("self")]
        //    public Self Self { get; set; }
        //}
        //public class Self
        //{
        //    [JsonProperty("href")]
        //    public string Href { get; set; }
        //}

        //[JsonProperty("_links")]
        //public Links Links { get; set; }
        //[JsonProperty("last_updated_timestamp")]
        //public long LastUpdatedTimestamp { get; set; }
        [JsonProperty("price")]
        public long Price { get; set; }
    }
    public class ReputationsData
    {
        public class ReputationData
        {
            public class Faction
            {
                //public class Key
                //{
                //    [JsonProperty("href")]
                //    public string Href { get; set; }
                //}

                //[JsonProperty("key")]
                //public Key Key { get; set; }
                //[JsonProperty("name")]
                //public string Name { get; set; }
                [JsonProperty("id")]
                public int Id { get; set; }
            }
            public class Standing
            {
                //[JsonProperty("raw")]
                //public int Raw { get; set; }
                //[JsonProperty("value")]
                //public int Value { get; set; }
                //[JsonProperty("max")]
                //public int Max { get; set; }
                [JsonProperty("tier")]
                public ReputationTier Tier { get; set; }
                //[JsonProperty("name")]
                //public string Name { get; set; }
            }

            [JsonProperty("faction")]
            public Faction Factionn { get; set; }
            [JsonProperty("standing")]
            public Standing Standingg { get; set; }
        }
        //public class Links
        //{
        //    [JsonProperty("self")]
        //    public Self Self { get; set; }
        //}
        //public class Self
        //{
        //    [JsonProperty("href")]
        //    public string Href { get; set; }
        //}
        //public class Character
        //{
        //    [JsonProperty("key")]
        //    public Key Key { get; set; }
        //    [JsonProperty("name")]
        //    public string Name { get; set; }
        //    [JsonProperty("id")]
        //    public int Id { get; set; }
        //    [JsonProperty("realm")]
        //    public Realm Realm { get; set; }
        //}
        //public class Realm
        //{
        //    [JsonProperty("key")]
        //    public Key Key { get; set; }
        //    [JsonProperty("name")]
        //    public string Name { get; set; }
        //    [JsonProperty("id")]
        //    public int Id { get; set; }
        //    [JsonProperty("slug")]
        //    public string Slug { get; set; }
        //}

        [JsonProperty("reputations")]
        public List<ReputationData> Reputations { get; set; }
        //[JsonProperty("_links")]
        //public Links Links { get; set; }
        //[JsonProperty("character")]
        //public Character Character { get; set; }
    }
    public class LogHub : Hub
    {

    }
    public class ConnectedRealmData
    {
        public class ConnectedRealm
        {
            public class Data
            {
                //public class Key
                //{
                //    public string href { get; set; }
                //}

                //public class Name
                //{
                //    public string it_IT { get; set; }
                //    public string ru_RU { get; set; }
                //    public string en_GB { get; set; }
                //    public string zh_TW { get; set; }
                //    public string ko_KR { get; set; }
                //    public string en_US { get; set; }
                //    public string es_MX { get; set; }
                //    public string pt_BR { get; set; }
                //    public string es_ES { get; set; }
                //    public string zh_CN { get; set; }
                //    public string fr_FR { get; set; }
                //    public string de_DE { get; set; }
                //}

                //public class Name2
                //{
                //    public string it_IT { get; set; }
                //    public string ru_RU { get; set; }
                //    public string en_GB { get; set; }
                //    public string zh_TW { get; set; }
                //    public string ko_KR { get; set; }
                //    public string en_US { get; set; }
                //    public string es_MX { get; set; }
                //    public string pt_BR { get; set; }
                //    public string es_ES { get; set; }
                //    public string zh_CN { get; set; }
                //    public string fr_FR { get; set; }
                //    public string de_DE { get; set; }
                //}

                //public class Region
                //{
                //    public Name2 name { get; set; }
                //    public int id { get; set; }
                //}

                //public class Category
                //{
                //    public string it_IT { get; set; }
                //    public string ru_RU { get; set; }
                //    public string en_GB { get; set; }
                //    public string zh_TW { get; set; }
                //    public string ko_KR { get; set; }
                //    public string en_US { get; set; }
                //    public string es_MX { get; set; }
                //    public string pt_BR { get; set; }
                //    public string es_ES { get; set; }
                //    public string zh_CN { get; set; }
                //    public string fr_FR { get; set; }
                //    public string de_DE { get; set; }
                //}

                //public class Name3
                //{
                //    public string it_IT { get; set; }
                //    public string ru_RU { get; set; }
                //    public string en_GB { get; set; }
                //    public string zh_TW { get; set; }
                //    public string ko_KR { get; set; }
                //    public string en_US { get; set; }
                //    public string es_MX { get; set; }
                //    public string pt_BR { get; set; }
                //    public string es_ES { get; set; }
                //    public string zh_CN { get; set; }
                //    public string fr_FR { get; set; }
                //    public string de_DE { get; set; }
                //}

                //public class Type
                //{
                //    public Name3 name { get; set; }
                //    public string type { get; set; }
                //}

                //public class Realm
                //{
                //    public bool is_tournament { get; set; }
                //    public string timezone { get; set; }
                //    public Name name { get; set; }
                //    public int id { get; set; }
                //    public Region region { get; set; }
                //    public Category category { get; set; }
                //    public string locale { get; set; }
                //    public Type type { get; set; }
                //    public string slug { get; set; }
                //}

                //public class Name4
                //{
                //    public string it_IT { get; set; }
                //    public string ru_RU { get; set; }
                //    public string en_GB { get; set; }
                //    public string zh_TW { get; set; }
                //    public string ko_KR { get; set; }
                //    public string en_US { get; set; }
                //    public string es_MX { get; set; }
                //    public string pt_BR { get; set; }
                //    public string es_ES { get; set; }
                //    public string zh_CN { get; set; }
                //    public string fr_FR { get; set; }
                //    public string de_DE { get; set; }
                //}

                //public class Status
                //{
                //    public Name4 name { get; set; }
                //    public string type { get; set; }
                //}

                //public class Name5
                //{
                //    public string it_IT { get; set; }
                //    public string ru_RU { get; set; }
                //    public string en_GB { get; set; }
                //    public string zh_TW { get; set; }
                //    public string ko_KR { get; set; }
                //    public string en_US { get; set; }
                //    public string es_MX { get; set; }
                //    public string pt_BR { get; set; }
                //    public string es_ES { get; set; }
                //    public string zh_CN { get; set; }
                //    public string fr_FR { get; set; }
                //    public string de_DE { get; set; }
                //}

                //public class Data
                //{
                //    public List<Realm> realms { get; set; }
                //    public int id { get; set; }
                //    public bool has_queue { get; set; }
                //    public Status status { get; set; }
                //    public Population population { get; set; }
                //}

                //public class Result
                //{
                //    public Key key { get; set; }
                //    public Data data { get; set; }
                //}

                //public class Root
                //{
                //    public int page { get; set; }
                //    public int pageSize { get; set; }
                //    public int maxPageSize { get; set; }
                //    public int pageCount { get; set; }
                //    public List<Result> results { get; set; }
                //}

                public class Realm
                {
                    public class Name
                    {
                        public string it_IT { get; set; }
                        public string ru_RU { get; set; }
                        public string en_GB { get; set; }
                        public string zh_TW { get; set; }
                        public string ko_KR { get; set; }
                        public string en_US { get; set; }
                        public string es_MX { get; set; }
                        public string pt_BR { get; set; }
                        public string es_ES { get; set; }
                        public string zh_CN { get; set; }
                        public string fr_FR { get; set; }
                        public string de_DE { get; set; }
                    }

                    //public bool is_tournament { get; set; }
                    //public string timezone { get; set; }
                    public Name name { get; set; }
                    //public int id { get; set; }
                    //public Region region { get; set; }
                    //public Category category { get; set; }
                    //public string locale { get; set; }
                    //public Type type { get; set; }
                    //public string slug { get; set; }
                }
                public class Population
                {
                    //public Name5 name { get; set; }
                    public string type { get; set; }
                }

                public List<Realm> realms { get; set; }
                public int id { get; set; }
                public Population population { get; set; }
                //public bool has_queue { get; set; }
                //public Status status { get; set; }
            }

            //public Key key { get; set; }
            public Data data { get; set; }
        }

        //public int page { get; set; }
        //public int pageSize { get; set; }
        //public int maxPageSize { get; set; }
        //public int pageCount { get; set; }
        public List<ConnectedRealm> results { get; set; }
    }
    public class ParseService
    {
        static IHubContext<LogHub> hubContext;
        private static IHostEnvironment HostingEnvironment;
        public static readonly object logLocker = new object();
        //public static readonly object getAuctionDataLocker = new object();
        public static Settings settings = new Settings();
        public const int AmountCopperInGold = 10000;
        private const int AmountCopperInSilver = 100;
        private const string URL_ITEM_PAGE_FORMAT = "https://eu.api.blizzard.com/data/wow/connected-realm/{0}/auctions?namespace=dynamic-eu&locale=en_US";
        const string TokenPriceURL = "https://eu.api.blizzard.com/data/wow/token/index?namespace=dynamic-eu&locale=en_US";
        const string CharacterProtectedDataURLFormat = "https://eu.api.blizzard.com/profile/user/wow/protected-character/{0}-{1}?namespace=profile-eu&locale=en_US";
        const string ReputationsDataURLFormat = "https://eu.api.blizzard.com/profile/wow/character/{0}/{1}/reputations?namespace=profile-eu&locale=en_US";
        const string CharacterDataURLFormat = "https://eu.api.blizzard.com/profile/wow/character/{0}/{1}?namespace=profile-eu&locale=en_US";
        const string ConnectedRealmListURL = "https://eu.api.blizzard.com/data/wow/search/connected-realm?namespace=dynamic-eu&locale=en_US&status.type=UP&orderby=population.type&_page=1";
        public static readonly object exeptionLocker = new object();
        public static string accessToken;

        public ParseService(IHubContext<LogHub> hubContext, IHostEnvironment hostingEnvironment)
        {
            ParseService.hubContext = hubContext;
            HostingEnvironment = hostingEnvironment;
        }

        public void Start(string accessToken)
        {
            ParseService.accessToken = accessToken;
            //var s = GetConnectedRealmData();
            //string str = "";
            //foreach (var connectedRealm in s.results)
            //{
            //    str += connectedRealm.data.id + ";";
            //    foreach (var realm in connectedRealm.data.realms)
            //    {
            //        str += realm.name.ru_RU + " / ";
            //    }
            //    str += ";";
            //    str += connectedRealm.data.population.type + "\n";
            //}
            //File.WriteAllText("realmsco.txt", str);
            foreach (var server in Loader.GetServersByName(HostingEnvironment))
            {
                new Thread(new ParameterizedThreadStart(Parse))
                //{
                //    IsBackground = true
                //}
                .Start(server.Id);
            }
        }

        public static List<RealmModel> GetRealms()
        {
            using (var db = new DatabaseContext())
            {
                return db.Realms.ToList();
            }
        }

        private static void Parse(object realmId)
        {
            Serilog.Log.ForContext("realmId", realmId);
            while (true)
            {
                try
                {
                    Thread.Sleep(ParseAndGetTimeToNextParse(realmId));
                }
                catch (Exception e)
                {
                    ParseService.ExceptionLogAndAlert(e);
                }
            }
            static TimeSpan ParseAndGetTimeToNextParse(object realmId)
            {
                Server server;
                using (var db = new DatabaseContext())
                {
                    var realm = db.Realms.Where(x => x.Id == (int)realmId).First();
                    server = new Server(realm, Loader.GetRecipeDataById());
                }
                return server.Parse();
            }
        }
        public static double GetIncomeGoldInHour(double profit, double milliseconds)
        {
            TimeSpan timeSpan = TimeSpan.FromMilliseconds(milliseconds);
            if (timeSpan.Ticks != 0)
            {
                return ConvertCopperToGold(profit / timeSpan.Ticks * TimeSpan.TicksPerHour);
            }
            else
            {
                return double.NaN;
            }
        }

        public static double GetIncomeRUBInHour(double profit, double milliseconds)
        {
            TimeSpan timeSpan = TimeSpan.FromMilliseconds(milliseconds);
            if (timeSpan.Ticks != 0)
            {
                return ConvertCopperToRUB(profit / timeSpan.Ticks * TimeSpan.TicksPerHour);
            }
            else
            {
                return double.NaN;
            }
        }

        public static double ConvertCopperToGold(double copper)
        {
            return copper / AmountCopperInGold;
        }
        public static double ConvertCopperToRUB(double copper)
        {
            const int costTokenInRUB = 825;
            return costTokenInRUB * copper / Server.TokenPrice;
        }

        public static double ConvertCopperToSilver(double copper)
        {
            return copper / AmountCopperInSilver;
        }

        public async static void SendAndLog(Response message)
        {
            await hubContext.Clients.All.SendAsync("Notify", message);

            Log(message);
        }

        public static void Log(Response message)
        {
            lock (logLocker)
            {
                File.AppendAllText("log.log", message.ToString().Replace("<br>", "\n").Replace("&#9;", "\t"));
            }
        }

        public static void WriteLineAndLogWhithTime(string str)
        {
            Console.WriteLine(DateTime.Now + "\n" + str);
            File.AppendAllText("log.txt", DateTime.Now + "\n" + str + "\n");
        }

        public static void ExceptionLogAndAlert(Exception e)
        {
            //throw e;
            Serilog.Log.Logger.Error(e, "Exception");
            lock (exeptionLocker)
            {
                using (var audioFile = new AudioFileReader("wwwroot/media/music.aac"))
                using (var outputDevice = new WaveOutEvent())
                {
                    outputDevice.Init(audioFile);
                    outputDevice.Play();
                    Console.Read();
                }
            }
        }

        static public AuctionData GetAuctionData(int idRealm)
        {
            string auctionDataStr = GetResponseString(string.Format(URL_ITEM_PAGE_FORMAT, idRealm), Timeout.InfiniteTimeSpan);
            AuctionData auctionData = JsonConvert.DeserializeObject<AuctionData>(auctionDataStr);
            return auctionData;
        }
        static public ConnectedRealmData GetConnectedRealmData()
        {
            string auctionDataStr = GetResponseString(ConnectedRealmListURL, TimeSpan.FromSeconds(4));
            ConnectedRealmData auctionData = JsonConvert.DeserializeObject<ConnectedRealmData>(auctionDataStr);
            return auctionData;
        }

        static public string GetResponseString(string uri)
        {
            return GetResponseString(uri, TimeSpan.FromSeconds(4));
        }
        static private string GetResponseString(string uri, TimeSpan timeout)
        {
            while (true)
            {
                try
                {
                    using (var httpClient = new HttpClient())
                    {
                        httpClient.Timeout = timeout;
                        using (var request = new HttpRequestMessage(new HttpMethod("GET"), uri))
                        {
                            request.Headers.Authorization = AuthenticationHeaderValue.Parse($"Bearer {accessToken}");
                            var task = httpClient.SendAsync(request);
                            task.Wait();
                            var taskl = task.Result.Content.ReadAsStringAsync();
                            taskl.Wait();
                            return taskl.Result;
                        }
                    }
                }
                catch (AggregateException ae)
                {
                    foreach (var e in ae.InnerExceptions)
                    {
                        if (!(e is TaskCanceledException) &&
                            !(e is HttpRequestException) &&
                            !(e is IOException) &&
                            !(e is System.Net.Sockets.SocketException))
                        {
                            throw;
                        }
                        else
                        {
                            Serilog.Log.Logger.Debug(e, "Exception");
                        }
                    }
                }
            }
        }
        static public string GetTimeUpdateStr(int idRealm)
        {
            while (true)
            {
                try
                {
                    HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(string.Format(URL_ITEM_PAGE_FORMAT, idRealm));
                    httpWebRequest.AllowAutoRedirect = false;
                    httpWebRequest.Method = "GET";
                    httpWebRequest.Timeout = 2000;
                    httpWebRequest.Headers.Add(HttpRequestHeader.Authorization, $"Bearer {accessToken}");
                    using (var webResponse = httpWebRequest.GetResponse())
                    {
                        return webResponse.Headers["Last-Modified"];
                    }
                }
                catch (WebException e)
                {
                    Serilog.Log.Logger.Debug(e, "Exception");
                    if (e.Response != null)
                    {
                        ExceptionLogAndAlert(e);
                        //HttpWebResponse httpWebResponse = e.Response as HttpWebResponse;
                        //if (httpWebResponse.StatusCode == HttpStatusCode.Unauthorized)
                        //{
                        //    throw e;
                        //}
                    }
                }
                catch (OperationCanceledException e)
                {
                    Serilog.Log.Logger.Debug(e, "Exception");
                }
            }
        }

        public static CharacterProtectedData GetCharacterProtectedData(int id, long characterId)
        {
            string CharacterDataStr = GetResponseString(string.Format(CharacterProtectedDataURLFormat, id, characterId));
            CharacterProtectedData characterData = JsonConvert.DeserializeObject<CharacterProtectedData>(CharacterDataStr);
            return characterData;
        }
        public static CharacterData GetCharacterData(string realmSlug, string characterName)
        {
            string CharacterDataStr = GetResponseString(string.Format(CharacterDataURLFormat, realmSlug, characterName.ToLower()));
            CharacterData characterData = JsonConvert.DeserializeObject<CharacterData>(CharacterDataStr);
            return characterData;
        }
        public static ReputationsData GetReputationsData(string realmSlug, string characterName)
        {
            string ReputationsDataStr = GetResponseString(
                string.Format(ReputationsDataURLFormat, realmSlug, characterName.ToLower()));
            ReputationsData reputationsData = JsonConvert.DeserializeObject<ReputationsData>(ReputationsDataStr);
            return reputationsData;
        }
        internal static long GetTokenPrice()
        {
            string TokenPriceDataStr = GetResponseString(TokenPriceURL);
            return JsonConvert.DeserializeObject<TokenPriceData>(TokenPriceDataStr).Price;
        }
        public static T Clone<T>(object parsersByIdItem)
        {
            string s = JsonConvert.SerializeObject(parsersByIdItem);
            return JsonConvert.DeserializeObject<T>(s);
        }
    }
}