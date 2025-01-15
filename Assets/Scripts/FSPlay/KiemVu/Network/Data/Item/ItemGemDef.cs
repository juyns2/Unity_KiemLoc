using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;
using GameServer.KiemVu.Utilities;
using ProtoBuf;

namespace Server.Data
{
    //Bảo thạch 
    [XmlRoot(ElementName = "GemStone")]
    public class GemStone
    {

        public int MaxGemPerEquip { get; set; }
        public List<Require> DrillingRequire { get; set; }
        public List<Require> TakeOutRequire { get; set; }
        public List<Require> MergeGem { get; set; }
        public List<GemData> gemData { get; set; }


        public static GemStone Parse(XElement xmlNode)
        {
            GemStone gemStone = new GemStone()
            {
                DrillingRequire = new List<Require>(),
                TakeOutRequire = new List<Require>(),
                MergeGem = new List<Require>(),
                gemData = new List<GemData>(),
            };
            gemStone.MaxGemPerEquip = int.Parse(xmlNode.Elements("Config").First().Attribute("MaxGemPerEquip").Value);

            foreach (XElement node in xmlNode.Elements("DrillingRequire").First().Elements("Level"))
            {
                gemStone.DrillingRequire.Add(Require.Parse(node));
            }
            foreach (XElement node in xmlNode.Elements("TakeOutRequire").First().Elements("Level"))
            {
                gemStone.TakeOutRequire.Add(Require.Parse(node));
            }
            foreach (XElement node in xmlNode.Elements("MergeGem").First().Elements("Level"))
            {
                gemStone.MergeGem.Add(Require.Parse(node));
            }
            foreach (XElement node in xmlNode.Elements("GemData").First().Elements("Gem"))
            {
                gemStone.gemData.Add(GemData.Parse(node));
            }
            return gemStone;
        }

    }
    public class ItemRequire
    {
        public int ID { get; set; }
        public int Count { get; set; }
        public static ItemRequire Parse(XElement xmlNode)
        {
            ItemRequire ItemRequire = new ItemRequire()
            {
                ID = int.Parse(xmlNode.Attribute("ID").Value),
                Count = int.Parse(xmlNode.Attribute("Count").Value)
            };

            /// Trả về kết quả
            return ItemRequire;
        }

    }
    public class GemAttribute
    {
        public int ID { get; set; }
        public double AttrValue { get; set; }
        public string AttrName { get; set; }
        public static GemAttribute Parse(XElement xmlNode)
        {
            GemAttribute GemAttribute = new GemAttribute()
            {
                ID = int.Parse(xmlNode.Attribute("ID").Value),
                AttrValue = double.Parse(xmlNode.Attribute("AttrValue").Value),
                AttrName = "",
            };
            GemAttribute.AttrName = "";
            if (PropertyDefine.PropertiesByID.TryGetValue(GemAttribute.ID, out PropertyDefine.Property property))
            {
                GemAttribute.AttrName = property.SymbolName;
            }
            /// Trả về kết quả
            return GemAttribute;
        }
    }
    public class GemData
    {
        public int SourceID { get; set; }
        public int NextID { get; set; }
        public int Level { get; set; }
        public int Type { get; set; }
        public string Equip { get; set; }
        public List<GemAttribute> gemAttribute { get; set; }
        public static GemData Parse(XElement xmlNode)
        {
            GemData GemData = new GemData()
            {
                SourceID = int.Parse(xmlNode.Attribute("SourceID").Value),
                NextID = int.Parse(xmlNode.Attribute("NextID").Value),
                Level = int.Parse(xmlNode.Attribute("Level").Value),
                Type = 0,
                Equip = xmlNode.Attribute("Equip").Value,
                gemAttribute = new List<GemAttribute>()
            };
            foreach (XElement node in xmlNode.Elements("Attribute"))
            {
                GemData.gemAttribute.Add(GemAttribute.Parse(node));
            }
            if (xmlNode.Attribute("Type") != null)
            {
                GemData.Type = int.Parse(xmlNode.Attribute("Type").Value);
            }
            /// Trả về kết quả
            return GemData;
        }
    }
    public class Require
    {
        public int ID { get; set; }
        public int Money { get; set; }
        public int BoundMoney { get; set; }
        public int Gold { get; set; }
        public int BoundGold { get; set; }
        public int THL { get; set; }
        public int EXP { get; set; }
        public List<ItemRequire> itemRequire { get; set; }
        public static Require Parse(XElement xmlNode)
        {
            Require require = new Require()
            {
                ID = int.Parse(xmlNode.Attribute("ID").Value),
                Money = int.Parse(xmlNode.Attribute("Money").Value),
                BoundMoney = int.Parse(xmlNode.Attribute("BoundMoney").Value),
                Gold = int.Parse(xmlNode.Attribute("Gold").Value),
                BoundGold = int.Parse(xmlNode.Attribute("BoundGold").Value),
                THL = int.Parse(xmlNode.Attribute("THL").Value),
                EXP = int.Parse(xmlNode.Attribute("EXP").Value),
                itemRequire = new List<ItemRequire>()
            };
            foreach (XElement node in xmlNode.Elements("ItemRequire"))
            {
                require.itemRequire.Add(ItemRequire.Parse(node));
            }
            /// Trả về kết quả
            return require;
        }
    }

}
