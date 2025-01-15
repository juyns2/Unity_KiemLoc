using FSPlay.GameEngine.Logic;
using FSPlay.KiemVu.Entities.Config;
using Server.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using UnityEngine;
using static FSPlay.KiemVu.Entities.Enum;

namespace FSPlay.KiemVu.Loader
{
    /// <summary>
    /// Đối tượng chứa danh sách các cấu hình trong game
    /// </summary>
    public static partial class Loader
    {
        #region Bảo thạch 
        public static GemStone gemStone = new GemStone();


        /// <summary>
        /// Tải dữ liệu thăng cấp bảo thạch
        /// </summary>
        /// <param name="xmlNode"></param>
        public static void LoadGem(XElement xmlNode)
        {
            gemStone = GemStone.Parse(xmlNode);
        }
        #endregion
    }
}
