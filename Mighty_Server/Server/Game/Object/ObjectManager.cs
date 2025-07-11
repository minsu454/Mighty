using Google.Protobuf.UnityProtocol;
using System;
using System.Collections.Generic;

namespace Server.Game.Object
{
    public class ObjectManager
    {
        public static ObjectManager Instance { get; } = new ObjectManager();

        object _lock = new object();

        // [UNUSED(1)][TYPE(7)][ID(24)]
        int _counter = 0;

        public T Add<T>() where T : GameObject, new()
        {
            T go = new T();

            lock (_lock)
            {
                go.Id = GenerateId(go.Tag);
            }

            return go;
        }

        private int GenerateId(ObjectTag type)
        {
            lock (_lock)
            {
                return ((int)type << 24) | (_counter++);
            }
        }

        public static ObjectTag GetObjectTagById(int id)
        {
            int type = (id >> 24) & 0x7F;
            return (ObjectTag)type;
        }

        //public bool ReMove(int id)
        //{
        //    ObjectTag objectType = GetObjectTypeById(id);

        //    lock (_lock)
        //    {
        //        if(objectType == ObjectTag.TagPlayer)
        //            return _playerDic.Remove(id);
        //    }

        //    return false;
        //}

        //public Player Find(int id)
        //{
        //    ObjectTag objectType = GetObjectTypeById(id);

        //    lock (_lock)
        //    {
        //        if (objectType == ObjectTag.TagPlayer)
        //        {
        //            Player player = null;

        //            _playerDic.TryGetValue(id, out player);
        //            return player;
        //        }
        //    }

        //    return null;
        //}
    }
}
