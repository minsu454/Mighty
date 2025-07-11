using Google.Protobuf.UnityProtocol;
using Server.Game.Object;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Server.Data
{
    public static class DataManager
    {
        public static List<Wall> WallList { get; private set; } = new List<Wall>();

        public static void LoadData()
        {
            string path = "../../../../../Common/MapData.xml";

            WallList = LoadXMLMap(path);
        }

        static List<Wall> LoadXMLMap(string path)
        {
            List<Wall> list = new List<Wall>();

            XDocument xDocument;
            xDocument = XDocument.Load(path);

            foreach (XElement wall in xDocument.Root.Elements("Wall"))
            {
                P_Vector3 pos = new P_Vector3(float.Parse(wall.Attribute("PosX").Value), float.Parse(wall.Attribute("PosY").Value), float.Parse(wall.Attribute("PosZ").Value));
                P_Quaternion rot = new P_Quaternion(float.Parse(wall.Attribute("RotX").Value), float.Parse(wall.Attribute("RotY").Value), float.Parse(wall.Attribute("RotZ").Value), float.Parse(wall.Attribute("RotW").Value));
                P_Vector3 scale = new P_Vector3(float.Parse(wall.Attribute("ScaleX").Value), float.Parse(wall.Attribute("ScaleY").Value), float.Parse(wall.Attribute("ScaleZ").Value));

                Wall wallObj = new Wall(pos, rot, scale);

                list.Add(wallObj);
            }

            return list;
        }
    }
}
