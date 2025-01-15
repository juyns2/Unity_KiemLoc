using FSPlay.KiemVu.UI.Main.ItemBox;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using Server.Data;
using System.Collections;
using FSPlay.KiemVu.UI.Main.Bag;
using FSPlay.KiemVu.Entities.Config;
using FSPlay.GameEngine.Logic;
using Server.Tools;
using UnityEngine.UI;
using FSPlay.KiemVu.Network;
using FSPlay.GameEngine.Data;
using Newtonsoft.Json;

namespace FSPlay.KiemVu.UI.Main
{
    /// <summary>
    /// Khung đục lỗ khảm bảo thạch
    /// </summary>
    public class UIGemStone : MonoBehaviour
    {
        #region Define
        /// <summary>
        /// Button đóng khung
        /// </summary>
        [SerializeField]
        private UnityEngine.UI.Button UIButton_Close;

        /// <summary>
        /// Ô đặt trang bị
        /// </summary>
        [SerializeField]
        private UIItemBox UI_EquipBox;

        /// <summary>
        /// Ô thông tin Bảo thạch 
        /// </summary>

        [SerializeField]
        private List<UIItemBox> UI_GemBox;

        [SerializeField]
        private List<TextMeshProUGUI> UI_GemHint;


        /// <summary>
        /// Lưới ô túi đồ
        /// </summary>
        [SerializeField]
        private UIBag_Grid UIBag_Grid;
        #endregion

        #region Constants
        #endregion

        #region Private fields

        #endregion

        #region Properties
        /// <summary>
        /// Sự kiện đóng khung
        /// </summary>
        public Action Close { get; set; }

        /// <summary>
        /// Sự kiện luyện hóa trang bị
        /// </summary>
        public Action<GoodsData, List<GoodsData>, int> EquipLevelUp { get; set; }
        #endregion

        #region Core MonoBehaviour
        /// <summary>
        /// Hàm này gọi khi đối tượng được tạo ra
        /// </summary>
        private void Awake()
        {

        }

        /// <summary>
        /// Hàm này gọi ở Frame đầu tiên
        /// </summary>
        private void Start()
        {
            this.InitPrefabs();
        }
        #endregion

        #region Code UI
        /// <summary>
        /// Khởi tạo ban đầu
        /// </summary>
        private void InitPrefabs()
        {
            this.UIButton_Close.onClick.AddListener(this.ButtonClose_Clicked);
            this.UIBag_Grid.BagItemClicked = this.ButtonBagItem_Clicked;
            this.UI_EquipBox.Click = this.ButtonEquip_Clicked;

            this.UI_EquipBox.Data = null;

            for (int i = 0; i < UI_GemBox.Count; i++)
            {
                int indexGem = i + 1;
                this.UI_GemHint[i].text = "";
                this.UI_GemBox[i].IsOpened = true;
                this.UI_GemBox[i].Data = null;
                this.UI_GemBox[i].Click = () =>
                {
                    ButtonGem_Clicked(indexGem);
                };
            }
        }
        public void RefreshGemOfEquip()
        {
            if (this.UI_EquipBox.Data == null)
            {
                KTGlobal.AddNotification("Không có trang bị hiện tại!");
                return;
            }
            GoodsData itemGD = this.UI_EquipBox.Data;
            ItemGenByteData equipProp = new ItemGenByteData();
            if (itemGD.Props != null)
            {
                byte[] base64Decode = Convert.FromBase64String(itemGD.Props);
                equipProp = DataHelper.BytesToObject<ItemGenByteData>(base64Decode, 0, base64Decode.Length);
                for (int i = 0; i < UI_GemBox.Count; i++)
                {
                    CreateGemPreview(equipProp, i + 1);
                }
            }
            else
            {
                for (int i = 0; i < UI_GemBox.Count; i++)
                {
                    this.UI_GemHint[i].text = "";
                    this.UI_GemBox[i].IsOpened = true;
                    this.UI_GemBox[i].Data = null;
                }
            }
        }

        /// <summary>
        /// Sự kiện khi Button đóng khung được ấn
        /// </summary>
        private void ButtonClose_Clicked()
        {
            this.Close?.Invoke();
        }


        /// <summary>
        /// Sự kiện khi Button trong lưới vật phẩm túi đồ được ấn
        /// </summary>
        /// <param name="itemGD"></param>
        private void ButtonBagItem_Clicked(GoodsData itemGD)
        {
            if (itemGD == null)
            {
                return;
            }

            List<KeyValuePair<string, Action>> buttons = new List<KeyValuePair<string, Action>>();
            ItemData _ItemData = null;
            if (!Loader.Loader.Items.TryGetValue(itemGD.GoodsID, out _ItemData))
            {
                return;
            }
            /// Nếu là trang bị
            if (_ItemData.DetailType <= 11 && this.UI_EquipBox.Data == null && _ItemData.Genre != 24)
            {

                buttons.Add(new KeyValuePair<string, Action>("Đặt lên", () =>
                {
                    /// Nếu đã có trang bị
                    if (this.UI_EquipBox.Data != null)
                    {
                        KTGlobal.AddNotification("Hãy gỡ trang bị hiện tại xuống trước!");
                        return;
                    }

                    /// Xóa khỏi túi đồ
                    this.UIBag_Grid.RemoveItem(itemGD);
                    /// Thêm vào ô trang bị
                    this.UI_EquipBox.Data = itemGD;
                    ItemGenByteData equipProp = new ItemGenByteData();
                    if (itemGD.Props != null)
                    {
                        byte[] base64Decode = Convert.FromBase64String(itemGD.Props);
                        equipProp = DataHelper.BytesToObject<ItemGenByteData>(base64Decode, 0, base64Decode.Length);
                        for (int i = 0; i < UI_GemBox.Count; i++)
                        {
                            CreateGemPreview(equipProp, i + 1);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < UI_GemBox.Count; i++)
                        {
                            this.UI_GemHint[i].text = "";
                            this.UI_GemBox[i].IsOpened = true;
                            this.UI_GemBox[i].Data = null;
                        }
                    }

                    /// Đóng Tooltip
                    KTGlobal.CloseItemInfo();
                }));
            }
            /// Nếu không phải trang bị
            else
            {
                /// Nếu có trang bị đặt vào
                if (this.UI_EquipBox.Data != null)
                {
                    if (_ItemData.Genre == 24)
                    {
                        buttons.Add(new KeyValuePair<string, Action>("Khảm lỗ 1", () =>
                        {
                            KT_TCPHandler.SendAttachGem(this.UI_EquipBox.Data.Id, _ItemData.ItemID, 1);
                            KTGlobal.CloseItemInfo();
                        }));
                        buttons.Add(new KeyValuePair<string, Action>("Khảm lỗ 2", () =>
                        {
                            KT_TCPHandler.SendAttachGem(this.UI_EquipBox.Data.Id, _ItemData.ItemID, 2);
                            KTGlobal.CloseItemInfo();
                        }));
                        buttons.Add(new KeyValuePair<string, Action>("Khảm lỗ 3", () =>
                        {
                            KT_TCPHandler.SendAttachGem(this.UI_EquipBox.Data.Id, _ItemData.ItemID, 3);
                            KTGlobal.CloseItemInfo();
                        }));
                        buttons.Add(new KeyValuePair<string, Action>("Khảm lỗ 4", () =>
                        {
                            KT_TCPHandler.SendAttachGem(this.UI_EquipBox.Data.Id, _ItemData.ItemID, 4);
                            KTGlobal.CloseItemInfo();
                        }));
                    }
                }
            }
            KTGlobal.ShowItemInfo(itemGD, buttons);
        }

        /// <summary>
        /// Sự kiện khi Button trang bị đang đặt được ấn
        /// </summary>
        private void ButtonEquip_Clicked()
        {
            if (this.UI_EquipBox.Data == null)
            {
                return;
            }

            List<KeyValuePair<string, Action>> buttons = new List<KeyValuePair<string, Action>>();
            buttons.Add(new KeyValuePair<string, Action>("Tháo xuống", () =>
            {
                /// Xóa khỏi túi đồ
                this.UIBag_Grid.AddItem(this.UI_EquipBox.Data);
                this.UI_EquipBox.Data = null;
                for (int i = 0; i < UI_GemBox.Count; i++)
                {
                    this.UI_GemHint[i].text = "";
                    this.UI_GemBox[i].IsOpened = true;
                    this.UI_GemBox[i].Data = null;
                }
                /// Đóng Tooltip
                KTGlobal.CloseItemInfo();
            }));
            KTGlobal.ShowItemInfo(this.UI_EquipBox.Data, buttons);
        }

        /// <summary>
        /// Sự kiện khi Button đục lỗ được ấn
        /// </summary>
        private void ButtonGem_Clicked(int indexGem)
        {
            if (this.UI_EquipBox.Data == null)
            {
                return;
            }
            GoodsData itemGD = this.UI_EquipBox.Data;
            if (itemGD.Props != null)
            {
                byte[] base64Decode = Convert.FromBase64String(itemGD.Props);
                ItemGenByteData equipProp = DataHelper.BytesToObject<ItemGenByteData>(base64Decode, 0, base64Decode.Length);
                if (equipProp.NumHole < indexGem)
                {
                    Require drillingRequire = Loader.Loader.gemStone.DrillingRequire[indexGem - 1];
                    if (drillingRequire != null)
                    {
                        string notifyString = "Đục lỗ " + indexGem + " trang bị này cần:\n";
                        if (drillingRequire.Money > 0)
                        {
                            notifyString += KTGlobal.GetDisplayMoney(drillingRequire.Money) + " Bạc\n";
                        }
                        if (drillingRequire.BoundMoney > 0)
                        {
                            notifyString += KTGlobal.GetDisplayMoney(drillingRequire.BoundMoney) + " Bạc Khóa\n";
                        }
                        if (drillingRequire.Gold > 0)
                        {
                            notifyString += KTGlobal.GetDisplayMoney(drillingRequire.Gold) + " Vàng\n";
                        }
                        if (drillingRequire.BoundGold > 0)
                        {
                            notifyString += KTGlobal.GetDisplayMoney(drillingRequire.BoundGold) + " Vàng Khóa\n";
                        }
                        if (drillingRequire.THL > 0)
                        {
                            notifyString += drillingRequire.THL + " Tinh Hoạt Lực\n";
                        }
                        foreach (ItemRequire item in drillingRequire.itemRequire)
                        {
                            if (Loader.Loader.Items.TryGetValue(item.ID, out ItemData _Item))
                            {
                                notifyString += _Item.Name + " +" + item.Count + "\n";
                            }
                        }
                        notifyString += "Bạn có xác nhận muốn đục lỗ không?";
                        KTGlobal.ShowMessageBox("Đục Lỗ", notifyString, () =>
                        {
                            KT_TCPHandler.SendDrillGem(itemGD.Id, indexGem);
                        }, true);
                    }


                }
                else if (equipProp.ListGem != null && equipProp.ListGem[indexGem - 1] > 0)
                {
                    if (this.UI_GemBox[indexGem - 1].Data != null)
                    {
                        //Show nút tháo
                        List<KeyValuePair<string, Action>> buttons = new List<KeyValuePair<string, Action>>();
                        buttons.Add(new KeyValuePair<string, Action>("Tháo Bảo Thạch", () =>
                        {
                            GemData sourceGemData = Loader.Loader.gemStone.gemData.Find(x => x.SourceID == this.UI_GemBox[indexGem - 1].Data.GoodsID);
                            Require drillingRequire = Loader.Loader.gemStone.TakeOutRequire.Find(x => x.ID == sourceGemData.Level);
                            if (drillingRequire != null)
                            {
                                string notifyString = "Tháo bảo thạch khỏi lỗ " + indexGem + " này cần:\n";
                                if (drillingRequire.Money > 0)
                                {
                                    notifyString += KTGlobal.GetDisplayMoney(drillingRequire.Money) + " Bạc\n";
                                }
                                if (drillingRequire.BoundMoney > 0)
                                {
                                    notifyString += KTGlobal.GetDisplayMoney(drillingRequire.BoundMoney) + " Bạc khóa\n";
                                }
                                if (drillingRequire.Gold > 0)
                                {
                                    notifyString += KTGlobal.GetDisplayMoney(drillingRequire.Gold) + " Vàng\n";
                                }
                                if (drillingRequire.BoundGold > 0)
                                {
                                    notifyString += KTGlobal.GetDisplayMoney(drillingRequire.BoundGold) + " Vàng khóa\n";
                                }
                                if (drillingRequire.THL > 0)
                                {
                                    notifyString += drillingRequire.THL + " Tinh Hoạt Lực\n";
                                }
                                foreach (ItemRequire item in drillingRequire.itemRequire)
                                {
                                    if (Loader.Loader.Items.TryGetValue(item.ID, out ItemData _Item))
                                    {
                                        notifyString += _Item.Name + " +" + item.Count + "\n";
                                    }
                                }
                                notifyString += "Bạn có xác nhận muốn tháo không?";
                                KTGlobal.ShowMessageBox("Tháo bảo thạch", notifyString, () =>
                                {
                                    KT_TCPHandler.SendRemoveGem(itemGD.Id, indexGem);
                                    KTGlobal.CloseItemInfo();
                                }, true);
                            }
                            else
                            {
                                KTGlobal.CloseItemInfo();
                            }
                        }));
                        KTGlobal.ShowItemInfo(this.UI_GemBox[indexGem - 1].Data, buttons);
                    }
                }
            }

        }

        #endregion

        #region Private methods
        private void CreateGemPreview(ItemGenByteData equipProp, int indexGem)
        {
            GoodsData productGD = null;
            if (equipProp.NumHole < indexGem)
            {
                UI_GemHint[indexGem - 1].text = "Chưa đục lỗ";
                UI_GemBox[indexGem - 1].IsOpened = true;
                UI_GemBox[indexGem - 1].Data = null;

            }
            else if (equipProp.ListGem != null && equipProp.ListGem[indexGem - 1] == 0)
            {
                UI_GemHint[indexGem - 1].text = "Chưa khảm bảo thạch";
                UI_GemBox[indexGem - 1].IsOpened = false;
                UI_GemBox[indexGem - 1].Data = null;
            }
            else if (equipProp.ListGem != null && equipProp.ListGem[indexGem - 1] > 0)
            {
                if (Loader.Loader.Items.TryGetValue(equipProp.ListGem[indexGem - 1], out ItemData itemData))
                {
                    productGD = KTGlobal.CreateItemPreview(itemData);
                    productGD.Binding = 1;
                    productGD.GCount = 0;
                    UI_GemHint[indexGem - 1].text = itemData.Name;
                    UI_GemBox[indexGem - 1].IsOpened = false;
                    UI_GemBox[indexGem - 1].Data = productGD;
                }
            }
            else
            {
                UI_GemHint[indexGem - 1].text = "";
                UI_GemBox[indexGem - 1].IsOpened = true;
                UI_GemBox[indexGem - 1].Data = null;
            }
        }
        #endregion
    }
}
