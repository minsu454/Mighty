using Server.Game.Room;
using System;
using System.Collections.Generic;
using System.Timers;

namespace Server.Game.Job
{
    public static class TimeManager
    {
        private static Dictionary<int, Timer> _timerList = new Dictionary<int, Timer>();

        public static void AddTickRoom(IRoom room, int tick = 100)
        {
            var timer = new Timer();
            timer.Interval = tick;
            timer.Elapsed += ((s, e) => { room.Update(); });
            timer.AutoReset = true;
            timer.Enabled = true;

            _timerList.Add(room.RoomId, timer);
        }

        public static void RemoveTickRoom(IRoom room)
        {
            if (_timerList.Remove(room.RoomId, out Timer timer))
            {
                Console.WriteLine("is not Room");
                return;
            }

            timer.Stop();
            timer.Close();
        }
    }
}
