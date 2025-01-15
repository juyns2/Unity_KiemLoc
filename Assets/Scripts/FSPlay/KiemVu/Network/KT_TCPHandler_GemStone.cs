using FSPlay.GameEngine.Network;
using HSGameEngine.GameEngine.Network.Protocol;
using Server.Data;
using Server.Tools;
using System.Collections.Generic;
using System.Text;

namespace FSPlay.KiemVu.Network
{
    /// <summary>
    /// Quản lý tương tác với Socket
    /// </summary>
    public static partial class KT_TCPHandler
    {
        public static void ReceiveDrillGem(string[] fields)
        {
            int ret = int.Parse(fields[0]);
            /// Nếu thao tác thành công
            if (ret == 1)
            {
                if (PlayZone.GlobalPlayZone.UIGemStone != null)
                {
                    PlayZone.GlobalPlayZone.UIGemStone.RefreshGemOfEquip();
                }
            }
        }
        public static void SendDrillGem(int GoodId, int indexGem)
        {
            string strCmd = string.Format("{0}:{1}", GoodId, indexGem);
            byte[] bytes = new ASCIIEncoding().GetBytes(strCmd);
            GameInstance.Game.GameClient.SendData(TCPOutPacket.MakeTCPOutPacket(GameInstance.Game.GameClient.OutPacketPool, bytes, 0, bytes.Length, (int)(TCPGameServerCmds.CMD_KT_CLIENT_DRILL_HOLE_ITEM)));
        }

        public static void ReceiveAttachGem(string[] fields)
        {
            int ret = int.Parse(fields[0]);
            /// Nếu thao tác thành công
            if (ret == 1)
            {
                if (PlayZone.GlobalPlayZone.UIGemStone != null)
                {
                    PlayZone.GlobalPlayZone.UIGemStone.RefreshGemOfEquip();
                }
            }
        }
        public static void SendAttachGem(int goodId, int gemId, int indexGem)
        {
            string strCmd = string.Format("{0}:{1}:{2}", goodId, gemId, indexGem);
            byte[] bytes = new ASCIIEncoding().GetBytes(strCmd);
            GameInstance.Game.GameClient.SendData(TCPOutPacket.MakeTCPOutPacket(GameInstance.Game.GameClient.OutPacketPool, bytes, 0, bytes.Length, (int)(TCPGameServerCmds.CMD_KT_CLIENT_ATTACH_GEM)));
        }

        public static void ReceiveRemoveGem(string[] fields)
        {
            int ret = int.Parse(fields[0]);
            /// Nếu thao tác thành công
            if (ret == 1)
            {
                if (PlayZone.GlobalPlayZone.UIGemStone != null)
                {
                    PlayZone.GlobalPlayZone.UIGemStone.RefreshGemOfEquip();
                }
            }
        }
        public static void SendRemoveGem(int GoodId, int indexGem)
        {
            string strCmd = string.Format("{0}:{1}", GoodId, indexGem);
            byte[] bytes = new ASCIIEncoding().GetBytes(strCmd);
            GameInstance.Game.GameClient.SendData(TCPOutPacket.MakeTCPOutPacket(GameInstance.Game.GameClient.OutPacketPool, bytes, 0, bytes.Length, (int)(TCPGameServerCmds.CMD_KT_CLIENT_REMOVE_GEM)));
        }


    }
}
