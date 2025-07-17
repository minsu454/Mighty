using System;
using System.Net;
using System.Threading;
using Server.Data;
using Server.Game.Job;
using Server.Game.Room;
using ServerCore;

namespace Server
{
    class Program
	{
		static Listener _listener = new Listener();
		
        static void Main(string[] args)
		{
			ConfigManager.LoadConfig();

            R_Lobby room = RoomManagers.R_Lobby.Add();
			TimeManager.AddTickRoom(room, 50);

			// DNS (Domain Name System)
			string host = Dns.GetHostName();
			IPHostEntry ipHost = Dns.GetHostEntry(host);
			IPAddress ipAddr = ipHost.AddressList[0];
			IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);

			_listener.Init(endPoint, () => { return SessionManager.Instance.Generate(); });
			Console.WriteLine("Listening...");

			while (true)
			{
				Thread.Sleep(100);
			}
		}
	}
}
