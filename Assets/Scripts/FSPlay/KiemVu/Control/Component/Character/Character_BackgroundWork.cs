using FSPlay.GameEngine.Logic;
using FSPlay.KiemVu.Entities.Config;
using FSPlay.KiemVu.Logic.Settings;
using FSPlay.KiemVu.UI.Main.RoleInfo;
using Server.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FSPlay.KiemVu.Control.Component
{
    /// <summary>
    /// Quản lý luồng thực thi ngầm
    /// </summary>
    public partial class Character
    {
        /// <summary>
        /// Âm thanh lần trước
        /// </summary>
        private int lastVolume = -1;

        /// <summary>
        /// Đánh dấu hiển thị nhân vật lần trước
        /// </summary>
        private bool lastHideRole = false;

        /// <summary>
        /// Kiểm tra vị trí và thực hiện đổi màu nếu cần
        /// </summary>
        private void SynsLocation()
        {
            /// Nếu toác
            if (Global.CurrentMapData == null)
            {
                return;
            }

            /// Tọa độ lưới
            Vector2 gridPos = KTGlobal.WorldPositionToGridPosition(this.transform.localPosition);
            /// Nếu nằm trong khu vực mờ
            if (Global.CurrentMapData.BlurPositions[(int)gridPos.x, (int)gridPos.y] % 2 == 1)
            {
                this.groupColor.Alpha = this.MaxAlpha / 2;
            }
            /// Nếu không nằm trong khu vực mờ
            else
            {
                this.groupColor.Alpha = this.MaxAlpha;
            }
        }

        /// <summary>
        /// Cập nhật UI
        /// </summary>
        private void UpdateUI()
        {
            if (this.UIHeader != null)
            {
                /// Nếu là nhân vật
                if (Global.Data.RoleData.RoleID == this.RefObject.RoleID)
                {
                    if (Global.Data.RoleData.TeamID == -1)
                    {
                        this.UIHeader.TeamType = 0;
                    }
                    else
                    {
                        if (Global.Data.RoleData.RoleID == Global.Data.RoleData.TeamLeaderRoleID)
                        {
                            this.UIHeader.TeamType = 1;
                        }
                        else
                        {
                            this.UIHeader.TeamType = 2;
                        }
                    }

                    /// Thiết lập sát khí
                    this.UIHeader.PKValue = Global.Data.RoleData.PKValue;
                }
                /// Nếu là người chơi khác
                else if (Global.Data.OtherRoles.TryGetValue(this.RefObject.RoleID, out RoleData _roleData))
                {
                    if (_roleData.TeamID == -1)
                    {
                        this.UIHeader.TeamType = 0;
                    }
                    else
                    {
                        if (_roleData.RoleID == _roleData.TeamLeaderRoleID)
                        {
                            this.UIHeader.TeamType = 1;
                        }
                        else
                        {
                            this.UIHeader.TeamType = 2;
                        }
                    }

                    /// Thiết lập sát khí
                    this.UIHeader.PKValue = _roleData.PKValue;
                }
                else
                {
                    this.UIHeader.TeamType = 0;

                    /// Thiết lập sát khí
                    this.UIHeader.PKValue = 0;
                }
            }
        }

        /// <summary>
        /// Thực thi sự kiện ngầm
        /// </summary>
        /// <returns></returns>
        private IEnumerator ExecuteBackgroundWork()
        {
            while (true)
            {
                yield return new WaitForSeconds(0.2f);
                this.SynsLocation();
                this.UpdateUIHeaderColor();
                this.SortingOrderHandler();
                this.UpdateUI();


                if (this.RefObject == Global.Data.Leader)
                {
                    GoodsData mask = null;
                    Dictionary<KiemVu.Entities.Enum.KE_EQUIP_POSITION, GoodsData> equips = KTGlobal.GetEquips(Global.Data.RoleData);
                    equips.TryGetValue(KiemVu.Entities.Enum.KE_EQUIP_POSITION.emEQUIPPOS_MASK, out mask);
                    if (mask != null)
                    {
                        if (Loader.Loader.Items.TryGetValue(mask.GoodsID, out ItemData itemData))
                        {
                            this.isActiveMask = !UIRoleInfo.hideMask;
                            this.SetModelVisible(true);
                        }
                    }
                    else
                    {
                        this.isActiveMask = false;
                        this.SetModelVisible(true);
                    }
                    //Nếu có FixLag
                    if (Global.Data.FixLag)
                    {
                        if (mask != null)
                        {
                            if (Loader.Loader.Items.TryGetValue(mask.GoodsID, out ItemData itemData))
                            {
                                this.isActiveMask = false;
                                this.SetModelVisible(true);
                            }
                        }
                        else
                        {
                            this.isActiveMask = false;
                            this.SetModelVisible(true);
                        }
                    }
                    else // Khi không FixLag
                    {
                        if (mask != null)
                        {
                            if (Loader.Loader.Items.TryGetValue(mask.GoodsID, out ItemData itemData))
                            {
                                if (UIRoleInfo.hideMask == true)
                                {
                                    this.isActiveMask = false;
                                }
                                else
                                {
                                    this.isActiveMask = true;
                                }
                                this.SetModelVisible(true);
                            }
                        }
                        else
                        {
                            this.isActiveMask = false;
                            this.SetModelVisible(true);
                        }
                    }
                }
                else if (this.RefObject != Global.Data.Leader)
                {
                    RoleData roleData = null;
                    if (Global.Data.OtherRoles.ContainsKey(this.RefObject.RoleID))
                    {
                        roleData = Global.Data.OtherRoles[this.RefObject.RoleID];
                    }
                    if (roleData == null)
                    {
                        yield return null;
                    }
                    GoodsData mask = null;
                    Dictionary<KiemVu.Entities.Enum.KE_EQUIP_POSITION, GoodsData> equips = KTGlobal.GetEquips(roleData);
                    equips.TryGetValue(KiemVu.Entities.Enum.KE_EQUIP_POSITION.emEQUIPPOS_MASK, out mask);
                    if (mask != null)
                    {
                        if (Loader.Loader.Items.TryGetValue(mask.GoodsID, out ItemData itemData))
                        {
                            this.isActiveMask = !Global.Data.FixLag;
                            this.SetModelVisible(true);
                        }
                    }
                    else
                    {
                        this.isActiveMask = false;
                        this.SetModelVisible(true);
                    }
                }
            }
        }

        /// <summary>
        /// Thực thi sự kiện bỏ qua một số Frame
        /// </summary>
        /// <param name="skip"></param>
        /// <param name="work"></param>
        /// <returns></returns>
        private IEnumerator ExecuteSkipFrames(int skip, Action work)
        {
            for (int i = 1; i <= skip; i++)
            {
                yield return null;
            }
            work?.Invoke();
        }

        /// <summary>
        /// Thực thi sự kiện sau khoảng thời gian tương ứng
        /// </summary>
        /// <param name="sec"></param>
        /// <param name="work"></param>
        /// <returns></returns>
        private IEnumerator DelayTask(float sec, Action work)
        {
            yield return new WaitForSeconds(sec);
            work?.Invoke();
        }
    }
}
