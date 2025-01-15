using FSPlay.GameEngine.Logic;
using FSPlay.GameEngine.Network;
using FSPlay.KiemVu;
using FSPlay.KiemVu.UI;
using FSPlay.KiemVu.UI.Main;
using UnityEngine;

/// <summary>
/// Quản lý các khung giao diện trong màn chơi
/// </summary>
public partial class PlayZone
{
    #region Giao dịch
    /// <summary>
    /// Khung giao dịch
    /// </summary>
    public UIExchange UIExchange { get; protected set; } = null;

    /// <summary>
    /// Mở khung giao dịch
    /// </summary>
    /// <param name="partnerID"></param>
    public void ShowUIExchange(int partnerID)
    {
        if (this.UIExchange != null)
        {
            return;
        }

        CanvasManager canvas = Global.MainCanvas.gameObject.GetComponent<CanvasManager>();
        this.UIExchange = canvas.LoadUIPrefab<UIExchange>("MainGame/UIExchange");
        canvas.AddUI(this.UIExchange);

        this.UIExchange.PartnerID = partnerID;
        this.UIExchange.Close = () =>
        {
            /// Gửi gói tin thông báo người chơi hủy giao dịch
            GameInstance.Game.SpriteGoodsExchange(partnerID, (int)GoodsExchangeCmds.Cancel, Global.Data.ExchangeID);

            /// Đóng khung
            this.CloseUIExchange();
        };
        this.UIExchange.AddItemToExchangeField = (itemGD) =>
        {
            /// Nếu vật phẩm không tồn tại
            if (itemGD == null || itemGD.GCount <= 0)
            {
                KTGlobal.AddNotification("Vật phẩm không tồn tại!");
                return;
            }
            /// Nếu vật phẩm đã bị khóa
            else if (itemGD.Binding == 1)
            {
                KTGlobal.AddNotification("Vật phẩm đã khóa, không thể đặt lên!");
                return;
            }

            /// Gửi gói tin thông báo thêm vật phẩm vào sàn giao dịch
            GameInstance.Game.SpriteGoodsExchange(partnerID, (int)GoodsExchangeCmds.AddGoods, itemGD.Id);

        };
        this.UIExchange.RemoveItemFromExchangeField = (itemGD) =>
        {
            /// Nếu vật phẩm không tồn tại
            if (itemGD == null || itemGD.GCount <= 0)
            {
                KTGlobal.AddNotification("Vật phẩm không tồn tại!");
                return;
            }

            /// Gửi gói tin thông báo xóa vật phẩm vào sàn giao dịch
            GameInstance.Game.SpriteGoodsExchange(partnerID, (int)GoodsExchangeCmds.RemoveGoods, itemGD.Id);
        };
        this.UIExchange.AddMoneyToExchangeField = (money) =>
        {
            /// Nếu số bạc mang theo không đủ
            if (Global.Data.RoleData.Money < money)
            {
                KTGlobal.AddNotification("Số bạc mang theo không đủ!");
                return;
            }

            /// Gửi gói tin thông báo đặt tiền vào sàn giao dịch
            GameInstance.Game.SpriteGoodsExchange(partnerID, (int)GoodsExchangeCmds.UpdateMoney, money);
        };
        this.UIExchange.Confirm = () =>
        {
            /// Gửi gói tin thông báo xác nhận
            GameInstance.Game.SpriteGoodsExchange(partnerID, (int)GoodsExchangeCmds.Lock, Global.Data.ExchangeID);
        };
        this.UIExchange.Exchange = () =>
        {
            /// Gửi gói tin giao dịch
            GameInstance.Game.SpriteGoodsExchange(partnerID, (int)GoodsExchangeCmds.Done, Global.Data.ExchangeID);
        };
        this.UIExchange.Split = (itemGD, number) =>
        {
            GameInstance.Game.SpriteModGoods((int)ModGoodsTypes.SplitItem, itemGD.Id, itemGD.GoodsID, -1, itemGD.Site, itemGD.GCount - number, itemGD.BagIndex, number.ToString());
            KTGlobal.CloseItemInfo();
        };
    }

    /// <summary>
    /// Đóng khung giao dịch
    /// </summary>
    public void CloseUIExchange()
    {
        if (this.UIExchange != null)
        {
            GameObject.Destroy(this.UIExchange.gameObject);
            this.UIExchange = null;
        }
    }
    #endregion

    #region Oẳn Tù Tì
    public UIOTT UIOTT { get; protected set; } = null;

    public void ShowUIOTT(int partnerID)
    {
        if (this.UIOTT != null)
        {
            return;
        }

        CanvasManager canvas = Global.MainCanvas.gameObject.GetComponent<CanvasManager>();
        this.UIOTT = canvas.LoadUIPrefab<UIOTT>("MainGame/UIOTT");
        canvas.AddUI(this.UIOTT);

        this.UIOTT.PartnerID = partnerID;
        this.UIOTT.Close = () =>
        {
            /// Gửi gói tin thông báo người chơi hủy giao dịch
            GameInstance.Game.SpriteGoodsOTT(partnerID, (int)GoodsExchangeCmds.Cancel, Global.Data.ExchangeID);

            /// Đóng khung
            this.CloseUIOTT();
        };
        this.UIOTT.AddItemToExchangeField = (itemGD) =>
        {
            /// Nếu vật phẩm không tồn tại
            if (itemGD == null || itemGD.GCount <= 0)
            {
                KTGlobal.AddNotification("Vật phẩm không tồn tại!");
                return;
            }
            /// Nếu vật phẩm đã bị khóa
            else if (itemGD.Binding == 1)
            {
                KTGlobal.AddNotification("Vật phẩm đã khóa, không thể đặt lên!");
                return;
            }

            /// Gửi gói tin thông báo thêm vật phẩm vào sàn giao dịch
            GameInstance.Game.SpriteGoodsOTT(partnerID, (int)GoodsExchangeCmds.AddGoods, itemGD.Id);

        };
        this.UIOTT.RemoveItemFromExchangeField = (itemGD) =>
        {
            /// Nếu vật phẩm không tồn tại
            if (itemGD == null || itemGD.GCount <= 0)
            {
                KTGlobal.AddNotification("Vật phẩm không tồn tại!");
                return;
            }

            /// Gửi gói tin thông báo xóa vật phẩm vào sàn giao dịch
            GameInstance.Game.SpriteGoodsOTT(partnerID, (int)GoodsExchangeCmds.RemoveGoods, itemGD.Id);
        };
        this.UIOTT.AddMoneyToExchangeField = (money) =>
        {
            /// Nếu số bạc mang theo không đủ
            if (Global.Data.RoleData.Money < money)
            {
                KTGlobal.AddNotification("Số bạc mang theo không đủ!");
                return;
            }

            /// Gửi gói tin thông báo đặt tiền vào sàn giao dịch
            GameInstance.Game.SpriteGoodsOTT(partnerID, (int)GoodsExchangeCmds.UpdateMoney, money);
        };
        this.UIOTT.Confirm = () =>
        {
            /// Gửi gói tin thông báo xác nhận
            GameInstance.Game.SpriteGoodsOTT(partnerID, (int)GoodsExchangeCmds.Lock, Global.Data.ExchangeID);
        };
        this.UIOTT.Exchange = (int ChoiceType) =>
        {
            /// Gửi gói tin giao dịch
            GameInstance.Game.SpriteGoodsOTT(partnerID, (int)GoodsExchangeCmds.Done, ChoiceType);
        };
        this.UIOTT.Split = (itemGD, number) =>
        {
            GameInstance.Game.SpriteModGoods((int)ModGoodsTypes.SplitItem, itemGD.Id, itemGD.GoodsID, -1, itemGD.Site, itemGD.GCount - number, itemGD.BagIndex, number.ToString());
            KTGlobal.CloseItemInfo();
        };
    }

    /// <summary>
    /// Đóng khung giao dịch
    /// </summary>
    public void CloseUIOTT()
    {
        if (this.UIOTT != null)
        {
            GameObject.Destroy(this.UIOTT.gameObject);
            this.UIOTT = null;
        }
    }
    #endregion
}
