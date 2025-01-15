using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using FSPlay.GameEngine.Logic;
using FSPlay.KiemVu.UI.MessageBox;
using FSPlay.GameEngine.Network;
using FSPlay.KiemVu.Utilities.UnityUI;

namespace FSPlay.KiemVu.UI.Main.RoleInfo
{
    /// <summary>
    /// Khung cộng điểm tiềm năng nhân vật
    /// </summary>
    public class UIRoleRemainPoint : MonoBehaviour
    {
        #region Define
        /// <summary>
        /// Text Giá trị Sức
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI UIText_Str;

        /// <summary>
        /// Text Giá trị Thân
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI UIText_Dex;

        /// <summary>
        /// Text Giá trị Ngoại
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI UIText_Sta;

        /// <summary>
        /// Text Giá trị Nội
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI UIText_Int;

        #region DEV_TAN
        /// <summary>
        /// Input nhập Giá trị Sức
        /// </summary>
        [SerializeField]
        private TMP_InputField UIInput_Str;

        /// <summary>
        /// Input nhập Giá trị Thân
        /// </summary>
        [SerializeField]
        private TMP_InputField UIInput_Dex;

        /// <summary>
        /// Input nhập Giá trị Ngoại
        /// </summary>
        [SerializeField]
        private TMP_InputField UIInput_Sta;

        /// <summary>
        /// Input nhập Giá trị Nội
        /// </summary>
        [SerializeField]
        private TMP_InputField UIInput_Int;
        #endregion DEV_TAN

        /// <summary>
        /// Text Điểm tiềm năng
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI UIText_RemainPoint;

        /// <summary>
        /// Button hủy bỏ
        /// </summary>
        [SerializeField]
        private UnityEngine.UI.Button UIButton_Cancel;

        /// <summary>
        /// Button xác nhận
        /// </summary>
        [SerializeField]
        private UnityEngine.UI.Button UIButton_Accept;

        /// <summary>
        /// Button cộng điểm Sức
        /// </summary>
        [SerializeField]
        private UnityEngine.UI.Button UIButton_AddStr;

        /// <summary>
        /// Button trừ điểm Sức
        /// </summary>
        [SerializeField]
        private UnityEngine.UI.Button UIButton_SubStr;

        /// <summary>
        /// Button cộng điểm Thân
        /// </summary>
        [SerializeField]
        private UnityEngine.UI.Button UIButton_AddDex;

        /// <summary>
        /// Button trừ điểm Thân
        /// </summary>
        [SerializeField]
        private UnityEngine.UI.Button UIButton_SubDex;

        /// <summary>
        /// Button cộng điểm Ngoại
        /// </summary>
        [SerializeField]
        private UnityEngine.UI.Button UIButton_AddSta;

        /// <summary>
        /// Button trừ điểm Ngoại
        /// </summary>
        [SerializeField]
        private UnityEngine.UI.Button UIButton_SubSta;

        /// <summary>
        /// Button cộng điểm Nội
        /// </summary>
        [SerializeField]
        private UnityEngine.UI.Button UIButton_AddInt;

        /// <summary>
        /// Button trừ điểm Nội
        /// </summary>
        [SerializeField]
        private UnityEngine.UI.Button UIButton_SubInt;

        /// <summary>
        /// Button đóng khung
        /// </summary>
        [SerializeField]
        private UnityEngine.UI.Button UIButton_Close;
        #endregion

        #region Properties
        /// <summary>
        /// Sự kiện đóng khung
        /// </summary>
        public Action Close { get; set; }

        private int _RemainPoint;
        /// <summary>
        /// Điểm tiềm năng
        /// </summary>
        public int RemainPoint
        {
            get
            {
                return this._RemainPoint;
            }
            set
            {
                this._RemainPoint = value;
                this.UIText_RemainPoint.text = value.ToString();
                this.tmpRemainPoint = value;

                if (value > 0)
                {
                    this.SetStateToAllAddButtons(true);
                }
            }
        }

        private int _Str;
        /// <summary>
        /// Sức
        /// </summary>
        public int Str
        {
            get
            {
                return this._Str;
            }
            set
            {
                this._Str = value;
                this.UIInput_Str.text = value.ToString();
            }
        }

        private int _Dex;
        /// <summary>
        /// Thân
        /// </summary>
        public int Dex
        {
            get
            {
                return this._Dex;
            }
            set
            {
                this._Dex = value;
                this.UIInput_Dex.text = value.ToString();
            }
        }

        private int _Sta;
        /// <summary>
        /// Ngoại
        /// </summary>
        public int Sta
        {
            get
            {
                return this._Sta;
            }
            set
            {
                this._Sta = value;
                this.UIInput_Sta.text = value.ToString();
            }
        }

        private int _Int;
        /// <summary>
        /// Nội
        /// </summary>
        public int Int
        {
            get
            {
                return this._Int;
            }
            set
            {
                this._Int = value;
                this.UIInput_Int.text = value.ToString();
            }
        }

        #region DEV_TAN
        private int TmpStr { get; set; }
        private int TmpDex { get; set; }
        private int TmpSta { get; set; }
        private int TmpInt { get; set; }
        /// <summary>
        ///         Giá trị Str khởi tạo
        /// </summary>
        public int InitStrValue { get; set; }
        /// <summary>
        /// Giá trị Str nhỏ nhất
        /// </summary>
        public int MinStrValue { get; set; } = int.MinValue;
        /// <summary>
        /// Giá trị Str lớn nhất
        /// </summary>
        public int MaxStrValue { get; set; } = int.MaxValue;

        /// <summary>
        ///         Giá trị Dex khởi tạo
        /// </summary>
        public int InitDexValue { get; set; }
        /// <summary>
        /// Giá trị Dex nhỏ nhất
        /// </summary>
        public int MinDexValue { get; set; } = int.MinValue;
        /// <summary>
        /// Giá trị Dex lớn nhất
        /// </summary>
        public int MaxDexValue { get; set; } = int.MaxValue;

        /// <summary>
        ///         Giá trị Sta khởi tạo
        /// </summary>
        public int InitStaValue { get; set; }
        /// <summary>
        /// Giá trị Sta nhỏ nhất
        /// </summary>
        public int MinStaValue { get; set; } = int.MinValue;
        /// <summary>
        /// Giá trị Sta lớn nhất
        /// </summary>
        public int MaxStaValue { get; set; } = int.MaxValue;

        /// <summary>
        ///         Giá trị Int khởi tạo
        /// </summary>
        public int InitIntValue { get; set; }
        /// <summary>
        /// Giá trị Sta nhỏ nhất
        /// </summary>
        public int MinIntValue { get; set; } = int.MinValue;
        /// <summary>
        /// Giá trị Sta lớn nhất
        /// </summary>
        public int MaxIntValue { get; set; } = int.MaxValue;
        #endregion DEV_TAN

        /// <summary>
        /// Sự kiện khi Button chấp nhận được ấn
        /// </summary>
        public Action<int, int, int, int> Accept { get; set; }
        #endregion

        #region Private fields
        /// <summary>
        /// Điểm Sức có thay đổi
        /// </summary>
        private bool IsStrChanged
        {
            get
            {
                try
                {
                    int nStr = int.Parse(this.UIInput_Str.text);

                    return this._Str != nStr;
                }
                catch (Exception) { }
                return false;
            }
        }

        /// <summary>
        /// Điểm Thân có thay đổi
        /// </summary>
        private bool IsDexChanged
        {
            get
            {
                try
                {
                    int nDex = int.Parse(this.UIInput_Dex.text);

                    return this._Dex != nDex;
                }
                catch (Exception) { }
                return false;
            }
        }

        /// <summary>
        /// Điểm Ngoại có thay đổi
        /// </summary>
        private bool IsStaChanged
        {
            get
            {
                try
                {
                    int nSta = int.Parse(this.UIInput_Sta.text);

                    return this._Sta != nSta;
                }
                catch (Exception) { }
                return false;
            }
        }

        /// <summary>
        /// Điểm Nội có thay đổi
        /// </summary>
        private bool IsIntChanged
        {
            get
            {
                try
                {
                    int nInt = int.Parse(this.UIInput_Int.text);

                    return this._Int != nInt;
                }
                catch (Exception) { }
                return false;
            }
        }

        /// <summary>
        /// Điểm cộng có thay đổi
        /// </summary>
        private bool IsPointChanged
        {
            get
            {
                return this.IsStrChanged || this.IsDexChanged || this.IsStaChanged || this.IsIntChanged;
            }
        }

        /// <summary>
        /// Biến lưu điểm tiềm năng tạm thời
        /// </summary>
        private int tmpRemainPoint;
        #endregion

        #region Core MonoBehaviour
        /// <summary>
        /// Hàm này gọi khi đối tượng được tạo ra
        /// </summary>
        private void Awake()
        {
            this.InitPrefabs();
        }

        /// <summary>
        /// Hàm này gọi ở Frame đầu tiên
        /// </summary>
        private void Start()
        {
            this.TmpStr = this._Str;
            this.TmpDex = this._Dex;
            this.TmpSta = this._Sta;
            this.TmpInt = this._Int;
        }
        #endregion

        #region Code UI
        /// <summary>
        /// Khởi tạo ban đầu
        /// </summary>
        private void InitPrefabs()
        {
            /// DEV_TAN
            this.UIInput_Str.text = this.InitStrValue.ToString();
            this.UIInput_Dex.text = this.InitDexValue.ToString();
            this.UIInput_Sta.text = this.InitStaValue.ToString();
            this.UIInput_Int.text = this.InitIntValue.ToString();
            /// END DEV_TAN
            this.UIButton_Close.onClick.AddListener(this.ButtonClose_Clicked);

            this.UIButton_Cancel.onClick.AddListener(this.ButtonCancel_Clicked);
            this.UIButton_Accept.onClick.AddListener(this.ButtonAccept_Clicked);

            #region DEVTAN
            this.UIInput_Str.onEndEdit.AddListener(delegate { CheckEndStr(); });
            this.UIInput_Dex.onEndEdit.AddListener(delegate { CheckEndDex(); });
            this.UIInput_Sta.onEndEdit.AddListener(delegate { CheckEndSta(); });
            this.UIInput_Int.onEndEdit.AddListener(delegate { CheckEndInt(); });
            #endregion DEVTAN

            this.UIButton_AddStr.onClick.AddListener(this.ButtonAddStr_Clicked);
            this.InitHoverOnButton(this.UIButton_AddStr, this.ButtonAddStr_Clicked);
            this.UIButton_SubStr.onClick.AddListener(this.ButtonSubStr_Clicked);
            this.InitHoverOnButton(this.UIButton_SubStr, this.ButtonSubStr_Clicked);

            this.UIButton_AddDex.onClick.AddListener(this.ButtonAddDex_Clicked);
            this.InitHoverOnButton(this.UIButton_AddDex, this.ButtonAddDex_Clicked);
            this.UIButton_SubDex.onClick.AddListener(this.ButtonSubDex_Clicked);
            this.InitHoverOnButton(this.UIButton_SubDex, this.ButtonSubDex_Clicked);

            this.UIButton_AddSta.onClick.AddListener(this.ButtonAddSta_Clicked);
            this.InitHoverOnButton(this.UIButton_AddSta, this.ButtonAddSta_Clicked);
            this.UIButton_SubSta.onClick.AddListener(this.ButtonSubSta_Clicked);
            this.InitHoverOnButton(this.UIButton_SubSta, this.ButtonSubSta_Clicked);

            this.UIButton_AddInt.onClick.AddListener(this.ButtonAddInt_Clicked);
            this.InitHoverOnButton(this.UIButton_AddInt, this.ButtonAddInt_Clicked);
            this.UIButton_SubInt.onClick.AddListener(this.ButtonSubInt_Clicked);
            this.InitHoverOnButton(this.UIButton_SubInt, this.ButtonSubInt_Clicked);

            this.SetStateToAllAddButtons(false);
            this.SetStateToAllSubButtons(false);

            this.UIButton_Accept.interactable = false;
            this.UIButton_Cancel.interactable = false;
        }

        #region DEVTAN check input
        private void CheckEndStr()
        {
            if (int.TryParse(this.UIInput_Str.text, out int nStr))
            {
                if (nStr < this._Str)
                {
                    this.UIInput_Str.text = this._Str.ToString();
                    this.tmpRemainPoint += this.TmpStr - this._Str;
                    this.UIText_RemainPoint.text = this.tmpRemainPoint.ToString();
                    this.TmpStr = this._Str;
                    this.UIButton_SubStr.interactable = false;
                    this.SetStateToAllAddButtons(true);
                }
                else if (this.tmpRemainPoint - (nStr - this.TmpStr) <= 0)
                {
                    int tmp = this.tmpRemainPoint - (nStr - this.TmpStr);
                    nStr += tmp;
                    this.UIInput_Str.text = nStr.ToString();
                    this.TmpStr = nStr;
                    this.tmpRemainPoint = 0;
                    this.UIText_RemainPoint.text = "0";
                    this.UIButton_SubStr.interactable = true;
                    this.SetStateToAllAddButtons(false);
                }
                else
                {
                    this.UIInput_Str.text = nStr.ToString();
                    this.tmpRemainPoint -= (nStr - this.TmpStr);
                    this.TmpStr = nStr;
                    this.UIText_RemainPoint.text = this.tmpRemainPoint.ToString();
                    this.UIButton_SubStr.interactable = true;
                }
                if (this.IsPointChanged)
                {
                    this.UIButton_Accept.interactable = true;
                    this.UIButton_Cancel.interactable = true;
                }
                else
                {
                    this.UIButton_Accept.interactable = false;
                    this.UIButton_Cancel.interactable = false;
                }
            }
        }

        private void CheckEndDex()
        {
            if (int.TryParse(this.UIInput_Dex.text, out int nDex))
            {
                if (nDex < this._Dex)
                {
                    this.UIInput_Dex.text = this._Dex.ToString();
                    this.tmpRemainPoint += this.TmpDex - this._Dex;
                    this.UIText_RemainPoint.text = this.tmpRemainPoint.ToString();
                    this.TmpDex = this._Dex;
                    this.UIButton_SubDex.interactable = false;
                    this.SetStateToAllAddButtons(true);
                }
                else if (this.tmpRemainPoint - (nDex - this.TmpDex) <= 0)
                {
                    int tmp = this.tmpRemainPoint - (nDex - this.TmpDex);
                    nDex += tmp;
                    this.UIInput_Dex.text = nDex.ToString();
                    this.TmpDex = nDex;
                    this.tmpRemainPoint = 0;
                    this.UIText_RemainPoint.text = "0";
                    this.UIButton_SubDex.interactable = true;
                    this.SetStateToAllAddButtons(false);
                }
                else
                {
                    this.UIInput_Dex.text = nDex.ToString();
                    this.tmpRemainPoint -= (nDex - this.TmpDex);
                    this.TmpDex = nDex;
                    this.UIText_RemainPoint.text = this.tmpRemainPoint.ToString();
                    this.UIButton_SubDex.interactable = true;
                }
                if (this.IsPointChanged)
                {
                    this.UIButton_Accept.interactable = true;
                    this.UIButton_Cancel.interactable = true;
                }
                else
                {
                    this.UIButton_Accept.interactable = false;
                    this.UIButton_Cancel.interactable = false;
                }
            }
        }

        private void CheckEndSta()
        {
            if (int.TryParse(this.UIInput_Sta.text, out int nSta))
            {
                if (nSta < this._Sta)
                {
                    this.UIInput_Sta.text = this._Sta.ToString();
                    this.tmpRemainPoint += this.TmpSta - this._Sta;
                    this.UIText_RemainPoint.text = this.tmpRemainPoint.ToString();
                    this.TmpSta = this._Sta;
                    this.UIButton_SubSta.interactable = false;
                    this.SetStateToAllAddButtons(true);
                }
                else if (this.tmpRemainPoint - (nSta - this.TmpSta) <= 0)
                {
                    int tmp = this.tmpRemainPoint - (nSta - this.TmpSta);
                    nSta += tmp;
                    this.UIInput_Sta.text = nSta.ToString();
                    this.TmpSta = nSta;
                    this.tmpRemainPoint = 0;
                    this.UIText_RemainPoint.text = "0";
                    this.UIButton_SubSta.interactable = true;
                    this.SetStateToAllAddButtons(false);
                }
                else
                {
                    this.UIInput_Sta.text = nSta.ToString();
                    this.tmpRemainPoint -= (nSta - this.TmpSta);
                    this.TmpSta = nSta;
                    this.UIText_RemainPoint.text = this.tmpRemainPoint.ToString();
                    this.UIButton_SubSta.interactable = true;
                }
                if (this.IsPointChanged)
                {
                    this.UIButton_Accept.interactable = true;
                    this.UIButton_Cancel.interactable = true;
                }
                else
                {
                    this.UIButton_Accept.interactable = false;
                    this.UIButton_Cancel.interactable = false;
                }
            }
        }
        
        private void CheckEndInt()
        {
            if (int.TryParse(this.UIInput_Int.text, out int nInt))
            {
                if (nInt < this._Int)
                {
                    this.UIInput_Int.text = this._Int.ToString();
                    this.tmpRemainPoint += this.TmpInt - this._Int;
                    this.UIText_RemainPoint.text = this.tmpRemainPoint.ToString();
                    this.TmpInt = this._Int;
                    this.UIButton_SubInt.interactable = false;
                    this.SetStateToAllAddButtons(true);
                }
                else if (this.tmpRemainPoint - (nInt - this.TmpInt) <= 0)
                {
                    int tmp = this.tmpRemainPoint - (nInt - this.TmpInt);
                    nInt += tmp;
                    this.UIInput_Int.text = nInt.ToString();
                    this.TmpInt = nInt;
                    this.tmpRemainPoint = 0;
                    this.UIText_RemainPoint.text = "0";
                    this.UIButton_SubInt.interactable = true;
                    this.SetStateToAllAddButtons(false);
                }
                else
                {
                    this.UIInput_Int.text = nInt.ToString();
                    this.tmpRemainPoint -= (nInt - this.TmpInt);
                    this.TmpInt = nInt;
                    this.UIText_RemainPoint.text = this.tmpRemainPoint.ToString();
                    this.UIButton_SubInt.interactable = true;
                }
                if (this.IsPointChanged)
                {
                    this.UIButton_Accept.interactable = true;
                    this.UIButton_Cancel.interactable = true;
                }
                else
                {
                    this.UIButton_Accept.interactable = false;
                    this.UIButton_Cancel.interactable = false;
                }
            }
        }
        #endregion DEVTAN check input

        /// <summary>
        /// Sự kiện khi nút đóng khung được ấn
        /// </summary>
        private void ButtonClose_Clicked()
        {
            if (IsPointChanged)
            {
                KTGlobal.ShowMessageBox("Điểm tiềm năng cộng vào vẫn chưa được lưu lại, nếu đóng khung lúc này, các giá trị đã được phân phối sẽ mất, và bạn phải bắt đầu lại từ lần kế tiếp mở khung. Xác nhận đóng khung?", () => {
                    GameObject.Destroy(this.gameObject);
                    this.Close?.Invoke();
                }, true);
            }
            else
            {
                GameObject.Destroy(this.gameObject);
                this.Close?.Invoke();
            }
        }

        /// <summary>
        /// Sự kiện khi Button hủy bỏ được ấn
        /// </summary>
        private void ButtonCancel_Clicked()
        {
            KTGlobal.ShowMessageBox("Sau khi làm mới dữ liệu, các giá trị đã được phân phối sẽ mất, bạn phải tiến hành nhập lại. Xác nhận làm mới dữ liệu lại từ đầu?", () => {
                /* DEV_TAN
                this.UIText_Str.text = this._Str.ToString();
                this.UIText_Dex.text = this._Dex.ToString();
                this.UIText_Sta.text = this._Sta.ToString();
                this.UIText_Int.text = this._Int.ToString(); */
                this.UIText_RemainPoint.text = this.tmpRemainPoint.ToString();

                if (this.tmpRemainPoint > 0)
                {
                    this.SetStateToAllAddButtons(true);
                }
                else
                {
                    this.SetStateToAllAddButtons(false);
                }
                this.SetStateToAllSubButtons(false);

                if (this.IsPointChanged)
                {
                    this.UIButton_Accept.interactable = true;
                    this.UIButton_Cancel.interactable = true;
                }
                else
                {
                    this.UIButton_Accept.interactable = false;
                    this.UIButton_Cancel.interactable = false;
                }
            }, true);
        }

        /// <summary>
        /// Sự kiện khi Button đồng ý được ấn
        /// </summary>
        private void ButtonAccept_Clicked()
        {
            KTGlobal.ShowMessageBox("Xác nhận cộng điểm vào các thuộc tính này?", () => {
                try
                {
                    /* DEV_TAN
                    int nStr = int.Parse(this.UIText_Str.text) - this._Str;
                    int nDex = int.Parse(this.UIText_Dex.text) - this._Dex;
                    int nSta = int.Parse(this.UIText_Sta.text) - this._Sta;
                    int nInt = int.Parse(this.UIText_Int.text) - this._Int;

                    if (nStr < 0 || nDex < 0 || nSta < 0 || nInt < 0)
                    {
                        throw new Exception();
                    }   */

                    if (int.TryParse(this.UIInput_Str.text, out int nStr) && int.TryParse(this.UIInput_Dex.text, out int nDex) && int.TryParse(this.UIInput_Sta.text, out int nSta) && int.TryParse(this.UIInput_Int.text, out int nInt))
                    {
                        nStr -= this._Str;
                        nDex -= this._Dex;
                        nSta -= this._Sta;
                        nInt -= this._Int;
                        if (nStr < 0 || nDex < 0 || nSta < 0 || nInt < 0)
                        {
                            throw new Exception();
                        }

                        this.Accept?.Invoke(nStr, nDex, nSta, nInt);
                        GameInstance.Game.SpriteRecommendPoint(nStr, nInt, nDex, nSta);
                    }
                    /*
                    this.Accept?.Invoke(nStr, nDex, nSta, nInt);
                    GameInstance.Game.SpriteRecommendPoint(nStr, nInt, nDex, nSta);     */
                    /// END DEV_TAN
                }
                catch (Exception)
                {
                    KTGlobal.AddNotification("Có sai sót trong thao tác dữ liệu, hãy thử mở lại khung!");
                }
            }, true);
        }

        /// <summary>
        /// Sự kiện khi Button cộng điểm Sức được ấn
        /// </summary>
        private void ButtonAddStr_Clicked()
        {
            if (!this.UIButton_AddStr.interactable)
            {
                return;
            }

            if (int.TryParse(this.UIInput_Str.text, out int nStr))
            {
                nStr = Math.Min(nStr, this.MaxStrValue);
                nStr = Math.Max(nStr, this.MinStrValue);

                if (nStr < this.MaxStrValue)
                {
                    nStr++;
                    this.TmpStr = nStr;
                    this.UIInput_Str.text = nStr.ToString();

                    this.tmpRemainPoint--;
                    this.UIText_RemainPoint.text = this.tmpRemainPoint.ToString();
                    if (this.tmpRemainPoint <= 0)
                    {
                        this.SetStateToAllAddButtons(false);
                    }

                    if (this.IsStrChanged)
                    {
                        this.UIButton_SubStr.interactable = true;
                    }
                    else
                    {
                        this.UIButton_SubStr.interactable = false;
                    }

                    if (this.IsPointChanged)
                    {
                        this.UIButton_Accept.interactable = true;
                        this.UIButton_Cancel.interactable = true;
                    }
                    else
                    {
                        this.UIButton_Accept.interactable = false;
                        this.UIButton_Cancel.interactable = false;
                    }
                }
                else
                {
                    this.UIButton_AddStr.interactable = false;
                }
            }
            else
            {
                this.UIButton_AddStr.interactable = false;
            }
        }

        /// <summary>
        /// Sự kiện khi Button trừ điểm Sức
        /// </summary>
        private void ButtonSubStr_Clicked()
        {
            if (!this.UIButton_SubStr.interactable)
            {
                return;
            }
            if (int.TryParse(this.UIInput_Str.text, out int nStr))
            {
                
                nStr = Math.Min(nStr, this.MaxStrValue);
                nStr = Math.Max(nStr, this.MinStrValue);

                if (nStr < this.MaxStrValue)
                {
                    nStr--;
                    this.TmpStr = nStr;
                    this.UIInput_Str.text = nStr.ToString();

                    this.tmpRemainPoint++;
                    this.UIText_RemainPoint.text = this.tmpRemainPoint.ToString();
                    if (this.tmpRemainPoint > 0)
                    {
                        this.SetStateToAllAddButtons(true);
                    }

                    if (this.IsStrChanged)
                    {
                        this.UIButton_SubStr.interactable = true;
                    }
                    else
                    {
                        this.UIButton_SubStr.interactable = false;
                    }

                    if (this.IsPointChanged)
                    {
                        this.UIButton_Accept.interactable = true;
                        this.UIButton_Cancel.interactable = true;
                    }
                    else
                    {
                        this.UIButton_Accept.interactable = false;
                        this.UIButton_Cancel.interactable = false;
                    }
                }
                else
                {
                    this.UIButton_AddStr.interactable = false;
                }
            }
            else
            {
                this.UIButton_AddStr.interactable = false;
            }
        }

        /// <summary>
        /// Sự kiện khi Button cộng điểm Thân được ấn
        /// </summary>
        private void ButtonAddDex_Clicked()
        {
            if (!this.UIButton_AddDex.interactable)
            {
                return;
            }

            if (int.TryParse(this.UIInput_Dex.text, out int nDex))
            {
                nDex = Math.Min(nDex, this.MaxDexValue);
                nDex = Math.Max(nDex, this.MinDexValue);

                if (nDex < this.MaxDexValue)
                {
                    nDex++;
                    this.TmpDex = nDex;
                    this.UIInput_Dex.text = nDex.ToString();

                    this.tmpRemainPoint--;
                    this.UIText_RemainPoint.text = this.tmpRemainPoint.ToString();
                    if (this.tmpRemainPoint <= 0)
                    {
                        this.SetStateToAllAddButtons(false);
                    }

                    if (this.IsDexChanged)
                    {
                        this.UIButton_SubDex.interactable = true;
                    }
                    else
                    {
                        this.UIButton_SubDex.interactable = false;
                    }

                    if (this.IsPointChanged)
                    {
                        this.UIButton_Accept.interactable = true;
                        this.UIButton_Cancel.interactable = true;
                    }
                    else
                    {
                        this.UIButton_Accept.interactable = false;
                        this.UIButton_Cancel.interactable = false;
                    }
                }
                else
                {
                    this.UIButton_AddDex.interactable = false;
                }
            }
            else
            {
                this.UIButton_AddDex.interactable = false;
            }
        }

        /// <summary>
        /// Sự kiện khi Button trừ điểm Thân
        /// </summary>
        private void ButtonSubDex_Clicked()
        {
            if (!this.UIButton_SubDex.interactable)
            {
                return;
            }

            if (int.TryParse(this.UIInput_Dex.text, out int nDex))
            {

                nDex = Math.Min(nDex, this.MaxDexValue);
                nDex = Math.Max(nDex, this.MinDexValue);

                if (nDex < this.MaxDexValue)
                {
                    nDex--;
                    this.TmpDex = nDex;
                    this.UIInput_Dex.text = nDex.ToString();

                    this.tmpRemainPoint++;
                    this.UIText_RemainPoint.text = this.tmpRemainPoint.ToString();
                    if (this.tmpRemainPoint > 0)
                    {
                        this.SetStateToAllAddButtons(true);
                    }

                    if (this.IsDexChanged)
                    {
                        this.UIButton_SubDex.interactable = true;
                    }
                    else
                    {
                        this.UIButton_SubDex.interactable = false;
                    }

                    if (this.IsPointChanged)
                    {
                        this.UIButton_Accept.interactable = true;
                        this.UIButton_Cancel.interactable = true;
                    }
                    else
                    {
                        this.UIButton_Accept.interactable = false;
                        this.UIButton_Cancel.interactable = false;
                    }
                }
                else
                {
                    this.UIButton_AddDex.interactable = false;
                }
            }
            else
            {
                this.UIButton_AddDex.interactable = false;
            }
        }

        /// <summary>
        /// Sự kiện khi Button cộng điểm Ngoại được ấn
        /// </summary>
        private void ButtonAddSta_Clicked()
        {
            if (!this.UIButton_AddSta.interactable)
            {
                return;
            }

            if (int.TryParse(this.UIInput_Sta.text, out int nSta))
            {
                nSta = Math.Min(nSta, this.MaxStaValue);
                nSta = Math.Max(nSta, this.MinStaValue);

                if (nSta < this.MaxStaValue)
                {
                    nSta++;
                    this.TmpSta = nSta;
                    this.UIInput_Sta.text = nSta.ToString();

                    this.tmpRemainPoint--;
                    this.UIText_RemainPoint.text = this.tmpRemainPoint.ToString();
                    if (this.tmpRemainPoint <= 0)
                    {
                        this.SetStateToAllAddButtons(false);
                    }

                    if (this.IsStaChanged)
                    {
                        this.UIButton_SubSta.interactable = true;
                    }
                    else
                    {
                        this.UIButton_SubSta.interactable = false;
                    }

                    if (this.IsPointChanged)
                    {
                        this.UIButton_Accept.interactable = true;
                        this.UIButton_Cancel.interactable = true;
                    }
                    else
                    {
                        this.UIButton_Accept.interactable = false;
                        this.UIButton_Cancel.interactable = false;
                    }
                }
                else
                {
                    this.UIButton_AddSta.interactable = false;
                }
            }
            else
            {
                this.UIButton_AddSta.interactable = false;
            }
        }

        /// <summary>
        /// Sự kiện khi Button trừ điểm Ngoại
        /// </summary>
        private void ButtonSubSta_Clicked()
        {
            if (!this.UIButton_SubSta.interactable)
            {
                return;
            }

            if (int.TryParse(this.UIInput_Sta.text, out int nSta))
            {

                nSta = Math.Min(nSta, this.MaxStaValue);
                nSta = Math.Max(nSta, this.MinStaValue);

                if (nSta < this.MaxStaValue)
                {
                    nSta--;
                    this.TmpSta = nSta;
                    this.UIInput_Sta.text = nSta.ToString();

                    this.tmpRemainPoint++;
                    this.UIText_RemainPoint.text = this.tmpRemainPoint.ToString();
                    if (this.tmpRemainPoint > 0)
                    {
                        this.SetStateToAllAddButtons(true);
                    }

                    if (this.IsStaChanged)
                    {
                        this.UIButton_SubSta.interactable = true;
                    }
                    else
                    {
                        this.UIButton_SubSta.interactable = false;
                    }

                    if (this.IsPointChanged)
                    {
                        this.UIButton_Accept.interactable = true;
                        this.UIButton_Cancel.interactable = true;
                    }
                    else
                    {
                        this.UIButton_Accept.interactable = false;
                        this.UIButton_Cancel.interactable = false;
                    }
                }
                else
                {
                    this.UIButton_AddSta.interactable = false;
                }
            }
            else
            {
                this.UIButton_AddSta.interactable = false;
            }
        }

        /// <summary>
        /// Sự kiện khi Button cộng điểm Nội được ấn
        /// </summary>
        private void ButtonAddInt_Clicked()
        {
            if (!this.UIButton_AddInt.interactable)
            {
                return;
            }

            if (int.TryParse(this.UIInput_Int.text, out int nInt))
            {
                nInt = Math.Min(nInt, this.MaxIntValue);
                nInt = Math.Max(nInt, this.MinIntValue);

                if (nInt < this.MaxIntValue)
                {
                    nInt++;
                    this.TmpInt = nInt;
                    this.UIInput_Int.text = nInt.ToString();

                    this.tmpRemainPoint--;
                    this.UIText_RemainPoint.text = this.tmpRemainPoint.ToString();
                    if (this.tmpRemainPoint <= 0)
                    {
                        this.SetStateToAllAddButtons(false);
                    }

                    if (this.IsIntChanged)
                    {
                        this.UIButton_SubInt.interactable = true;
                    }
                    else
                    {
                        this.UIButton_SubInt.interactable = false;
                    }

                    if (this.IsPointChanged)
                    {
                        this.UIButton_Accept.interactable = true;
                        this.UIButton_Cancel.interactable = true;
                    }
                    else
                    {
                        this.UIButton_Accept.interactable = false;
                        this.UIButton_Cancel.interactable = false;
                    }
                }
                else
                {
                    this.UIButton_AddInt.interactable = false;
                }
            }
            else
            {
                this.UIButton_AddInt.interactable = false;
            }
        }

        /// <summary>
        /// Sự kiện khi Button trừ điểm Nội
        /// </summary>
        private void ButtonSubInt_Clicked()
        {
            if (!this.UIButton_SubInt.interactable)
            {
                return;
            }

            if (int.TryParse(this.UIInput_Int.text, out int nInt))
            {

                nInt = Math.Min(nInt, this.MaxIntValue);
                nInt = Math.Max(nInt, this.MinIntValue);

                if (nInt < this.MaxIntValue)
                {
                    nInt--;
                    this.TmpInt = nInt;
                    this.UIInput_Int.text = nInt.ToString();

                    this.tmpRemainPoint++;
                    this.UIText_RemainPoint.text = this.tmpRemainPoint.ToString();
                    if (this.tmpRemainPoint > 0)
                    {
                        this.SetStateToAllAddButtons(true);
                    }

                    if (this.IsIntChanged)
                    {
                        this.UIButton_SubInt.interactable = true;
                    }
                    else
                    {
                        this.UIButton_SubInt.interactable = false;
                    }

                    if (this.IsPointChanged)
                    {
                        this.UIButton_Accept.interactable = true;
                        this.UIButton_Cancel.interactable = true;
                    }
                    else
                    {
                        this.UIButton_Accept.interactable = false;
                        this.UIButton_Cancel.interactable = false;
                    }
                }
                else
                {
                    this.UIButton_AddInt.interactable = false;
                }
            }
            else
            {
                this.UIButton_AddInt.interactable = false;
            }
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Khởi tạo sự kiện Hover cho các Button cộng điểm
        /// </summary>
        /// <param name="button"></param>
        /// <param name="tickFunc"></param>
        private void InitHoverOnButton(UnityEngine.UI.Button button, Action tickFunc)
        {
            UIHoverableObject hoverable = button.gameObject.GetComponent<UIHoverableObject>();
            hoverable.Continuously = true;
            hoverable.HoverDuration = 0.2f;
            hoverable.HoverTick = 0.05f;
            float nSkip = 1;
            float nMultiply = 1.2f;
            const float MaxSkip = 512;
            hoverable.Tick = () => {
                if (nSkip < MaxSkip)
                {
                    nSkip *= nMultiply;
                }
                for (int i = 1; i <= nSkip; i++)
                {
                    tickFunc?.Invoke();
                }
            };
            hoverable.HoverEnd = () => {
                nSkip = 1;
            };
        }

        /// <summary>
        /// Cập nhật trạng thái cho tất cả các Button cộng trừ điểm
        /// </summary>
        /// <param name="isEnable"></param>
        private void SetStateToAllAddButtons(bool isEnable)
        {
            this.UIButton_AddStr.interactable = isEnable;
            this.UIButton_AddDex.interactable = isEnable;
            this.UIButton_AddSta.interactable = isEnable;
            this.UIButton_AddInt.interactable = isEnable;
        }

        /// <summary>
        /// Cập nhật trạng thái cho tất cả các Button cộng trừ điểm
        /// </summary>
        /// <param name="isEnable"></param>
        private void SetStateToAllSubButtons(bool isEnable)
        {
            this.UIButton_SubStr.interactable = isEnable;
            this.UIButton_SubDex.interactable = isEnable;
            this.UIButton_SubSta.interactable = isEnable;
            this.UIButton_SubInt.interactable = isEnable;
        }
        #endregion
    }
}