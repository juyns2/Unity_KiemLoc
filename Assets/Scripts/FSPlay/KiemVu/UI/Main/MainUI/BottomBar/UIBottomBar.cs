using FSPlay.GameEngine.Logic;
using FSPlay.GameEngine.Network;
using FSPlay.KiemVu.UI.Main.MainUI;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using FSPlay.GameFramework.Logic;

namespace FSPlay.KiemVu.UI.Main
{
    /// <summary>
    /// Khung chức năng phía dưới gồm SkillBar và ControlButtons
    /// </summary>
    public class UIBottomBar : MonoBehaviour
    {
        #region Define
        [SerializeField]
        private UnityEngine.UI.Button UIButton_Zoom;
        [SerializeField]
        private UnityEngine.UI.Button UIButton_Logout;
        [SerializeField]
        private TextMeshProUGUI UIText_ZoomNum;

        /// <summary>
        /// Toggle mở khung menu
        /// </summary>
        [SerializeField]
        private UnityEngine.UI.Toggle UIToggle_MenuPart;
		
        /// <summary>
        /// Khung Button chức năng
        /// </summary>
        [SerializeField]
        private UIControlButtons _UIControlButtons;

        /// <summary>
        /// Khung kỹ năng
        /// </summary>
        [SerializeField]
        private UISkillBar _UISkillBar;

        /// <summary>
        /// Tọa độ vị trí bên ngoài biên của ControlButtons
        /// </summary>
        [SerializeField]
        private Vector2 UIControlButton_InvisibleOffset;

        /// <summary>
        /// Tọa độ vị trí bên ngoài biên của SkillBar
        /// </summary>
        [SerializeField]
        private Vector2 UISkillBar_InvisibleOffset;

        /// <summary>
        /// Button hiển thị danh sách người chơi xung quanh
        /// </summary>
        [SerializeField]
        private UnityEngine.UI.Button UIButton_ShowNearbyPlayer;

        /// <summary>
        /// Thời gian hiệu ứng
        /// </summary>
        [SerializeField]
        private float AnimationTime = 2f;
        #endregion

        /// <summary>
        /// Tọa độ vị trí xuất hiện của ControlButtons
        /// </summary>
        private Vector2 UIControlButton_InitOffset;

        /// <summary>
        /// Tọa độ vị trí xuất hiện của SkillBar
        /// </summary>
        private Vector2 UISkillBar_InitOffset;

        /// <summary>
        /// RectTransform của ControlButtons
        /// </summary>
        private RectTransform UIControlButtons_RectTransform;

        /// <summary>
        /// RectTransform của SkillBar
        /// </summary>
        private RectTransform UISkillBar_RectTransform;

        #region Properties
        /// <summary>
        /// Khung kỹ năng
        /// </summary>
        /// <returns></returns>
        public UISkillBar UISkillBar
        {
            get
            {
                return this._UISkillBar;
            }
        }

        /// <summary>
        /// Khung Button chức năng
        /// </summary>
        /// <returns></returns>
        public UIControlButtons UIControlButtons
        {
            get
            {
                return this._UIControlButtons;
            }
        }
		
		/// <summary>
        /// Sự kiện nút menu được ấn
        /// </summary>
        public Action<bool> MenuFaceSelected { get; set; }

        /// <summary>
        /// Sự kiện khi khung SkillBar hiện ra
        /// </summary>
        public Action UISkillBarVisible { get; set; }

        /// <summary>
        /// Sự kiện khi khung ControlButtons hiện ra
        /// </summary>
        public Action UIControlButtonVisible { get; set; }

        /// <summary>
        /// Sự kiện hiện danh sách người chơi xung quanh
        /// </summary>
        public Action ShowNearbyPlayer { get; set; }
        #endregion

        #region Core MonoBehaviour
        /// <summary>
        /// Hàm này gọi khi đối tượng được tạo ra
        /// </summary>
        private void Awake()
        {
            this.UIControlButtons_RectTransform = this._UIControlButtons.GetComponent<RectTransform>();
            this.UISkillBar_RectTransform = this._UISkillBar.GetComponent<RectTransform>();

            this.UIControlButton_InitOffset = this.UIControlButtons_RectTransform.anchoredPosition;
            this.UISkillBar_InitOffset = this.UISkillBar_RectTransform.anchoredPosition;
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
            this.UIButton_ShowNearbyPlayer.onClick.AddListener(this.ButtonShowNearbyPlayer_Clicked);
			this.UIToggle_MenuPart.onValueChanged.AddListener(this.ToggleMenuFace_Selected);
            this.UIText_ZoomNum.text = Global.MainCamera.orthographicSize == 350 ? "1" : (Global.MainCamera.orthographicSize == 450 ? "2" : (Global.MainCamera.orthographicSize == 600 ? "3" : "4"));
            this.UIButton_Zoom.onClick.AddListener(this.ButtonZoom_Clicked);
            this.UIButton_Logout.onClick.AddListener(this.ButtonLogout_Clicked);
        }

        /// <summary>
        /// Sự kiện khi Button hiện danh sách người chơi xung quanh được ấn
        /// </summary>
        private void ButtonShowNearbyPlayer_Clicked()
        {
            this.ShowNearbyPlayer?.Invoke();
        }

        /// <summary>
        /// Sự kiện khi Button Zoom được ấn
        /// </summary>
        private void ButtonZoom_Clicked()
        {
            Global.MainCamera.orthographicSize = Global.MainCamera.orthographicSize == 350 ? 450 : (Global.MainCamera.orthographicSize == 450 ? 600 : (Global.MainCamera.orthographicSize == 600 ? 900 : 350));
            this.UIText_ZoomNum.text = Global.MainCamera.orthographicSize == 350 ? "1" : (Global.MainCamera.orthographicSize == 450 ? "2" : (Global.MainCamera.orthographicSize == 600 ? "3" : "4"));
        }
        private void ButtonLogout_Clicked()
        {
            //Super.DestroyLoginScene();
            Super.ShowGameLogin();
        }

        /// <summary>
        /// Sự kiện khi menu được ấn
        /// </summary>
        /// <param name="isSelected"></param>
        private void ToggleMenuFace_Selected(bool isSelected)
        {
            this.MenuFaceSelected?.Invoke(isSelected);
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Thực hiện hiệu ứng bay
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="fromPos"></param>
        /// <param name="toPos"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        private IEnumerator DoFly(RectTransform obj, Vector2 fromPos, Vector2 toPos, Action callback = null)
        {
            obj.anchoredPosition = fromPos;

            Vector2 diffVector = toPos - fromPos;
            float time = 0;
            while (true)
            {
                yield return null;
                time += Time.deltaTime;
                if (time >= this.AnimationTime)
                {
                    break;
                }

                float percent = time / this.AnimationTime;
                Vector2 newPos = fromPos + diffVector * percent;
                obj.anchoredPosition = newPos;
            }
            obj.anchoredPosition = toPos;
            callback?.Invoke();
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Hiện khung Button chức năng
        /// </summary>
        public void ShowUIControlButtons(bool skipAnimation = false)
        {
            this.StopAllCoroutines();
            if (skipAnimation)
            {
                this.UIControlButtons_RectTransform.anchoredPosition = this.UIControlButton_InitOffset;
                this.UISkillBar_RectTransform.anchoredPosition = this.UISkillBar_InvisibleOffset;
                this.UIControlButtonVisible?.Invoke();
            }
            else
            {
                this.StartCoroutine(this.DoFly(this.UIControlButtons_RectTransform, this.UIControlButton_InvisibleOffset, this.UIControlButton_InitOffset, this.UIControlButtonVisible));
                this.StartCoroutine(this.DoFly(this.UISkillBar_RectTransform, this.UISkillBar_InitOffset, this.UISkillBar_InvisibleOffset));
            }
        }

        /// <summary>
        /// Hiện khung kỹ năng
        /// </summary>
        public void ShowUISkillBar(bool skipAnimation = false)
        {
            this.StopAllCoroutines();
            if (skipAnimation)
            {
                this.UIControlButtons_RectTransform.anchoredPosition = this.UIControlButton_InvisibleOffset;
                this.UISkillBar_RectTransform.anchoredPosition = this.UISkillBar_InitOffset;
                this.UISkillBarVisible?.Invoke();
            }
            else
            {
                this.StartCoroutine(this.DoFly(this.UIControlButtons_RectTransform, this.UIControlButton_InitOffset, this.UIControlButton_InvisibleOffset));
                this.StartCoroutine(this.DoFly(this.UISkillBar_RectTransform, this.UISkillBar_InvisibleOffset, this.UISkillBar_InitOffset, this.UISkillBarVisible));
            }
        }
        #endregion
    }
}