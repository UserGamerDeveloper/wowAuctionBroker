using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;

namespace info
{
    public class Program
    {
        public static Settings settings = Loader.DeserializeSettings();
        public static readonly object consoleLocker = new object();

        static void Main(string[] args)
        {
            try
            {
                Start();
            }
            catch (Exception e)
            {
                Util.ExceptionLogAndAlert(e);
            }
        }

        private static void Start()
        {
            Util.SetAccessToken(settings.ClientId, settings.ClientSecret);
            //Saver.SerializeServers();
            List<Server> servers = Loader.DeserializeServers();
            string serversInfo = "";
            foreach (var server in servers.OrderByDescending(server => server.Money))
            {
                serversInfo += server.GetInfo() + "\n";
            }
            Util.WriteLineAndLog(serversInfo);
            const string DELIMETR = "--------------------------------------------------------------------------------";
            Util.WriteLineAndLog(DELIMETR);

            foreach (var server in servers)
            {
                new Thread(new ThreadStart(server.Parse)).Start();
            }
        }
    }
}