using FSPlay.GameEngine.Logic;
using FSPlay.GameEngine.Network;
using FSPlay.KiemVu.Control.Component;
using FSPlay.KiemVu.Logic;
using FSPlay.KiemVu.Entities.Config;
using FSPlay.KiemVu.Loader;
using FSPlay.KiemVu.Utilities;
using HSGameEngine.GameEngine.Network.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FSPlay.KiemVu.Network
{
    /// <summary>
    /// Quản lý tương tác với Socket
    /// </summary>
    public static partial class KT_TCPHandler
    {
        #region GM-Command
        /// <summary>
        /// Lệnh GM
        /// </summary>
        /// <param name="command"></param>
        public static async void SendGMCommand(string command)
        {
            byte[] bytes = KTCrypto.Encrypt(command);
            //GameInstance.Game.GameClient.SendData(TCPOutPacket.MakeTCPOutPacket(GameInstance.Game.GameClient.OutPacketPool, bytes, 0, bytes.Length, (int) (TCPGameServerCmds.CMD_KT_GM_COMMAND)));

            if (command.Contains("reloadskilldata", StringComparison.OrdinalIgnoreCase))
            {
                UnityEngine.Debug.Log("Command contains 'reloadskilldata'. Executing reload skill data...");
                // New down Config
                //CoroutineRunner.Instance.StartStaticCoroutine(CoroutineRunner.Instance.StartDownloadFiles());
                await CoroutineRunner.Instance.RunCoroutineAsync(CoroutineRunner.Instance.StartDownloadFiles());
                await CoroutineRunner.Instance.RunCoroutineAsync(CoroutineRunner.Instance.UpdateSkillData());
                // End down
                UnityEngine.Debug.Log("<================= Do LoadSkillData affter download SUccess Config! =================>");
                //LoadConfig.LoadSkillData();
                SkillManager.Refresh();
                //SkillDataEx.Parse();
                UnityEngine.Debug.Log("Refresh Successfully!");
                //GameInstance.Game.Disconnect();
                //CoroutineRunner.Reconnect?.Invoke();
                //PlayZone.ReLoadGame(3);
            }
            else
            {
                UnityEngine.Debug.Log("Command does not contain 'reloadskilldata'.");
            }

            GameInstance.Game.GameClient.SendData(TCPOutPacket.MakeTCPOutPacket(GameInstance.Game.GameClient.OutPacketPool, bytes, 0, bytes.Length, (int)(TCPGameServerCmds.CMD_KT_GM_COMMAND)));
        }
        #endregion
    }
}
