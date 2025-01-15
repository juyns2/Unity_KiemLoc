using UnityEngine;
using System.Collections;
using FSPlay.KiemVu.Loader;
using FSPlay.GameFramework.Logic;
using FSPlay.KiemVu.UI.LoadingResources;
using System;
using System.Threading.Tasks;
using FSPlay.GameEngine.Logic;

public class CoroutineRunner : MonoBehaviour
{
    // Singleton instance
    private static CoroutineRunner instance;
    public Action Faild { get; set; }
    private AssetBundle GameConfigAssetBundle = null;
    public Action OnLoadFinish { get; set; }
    public static Action Reconnect { get; set; }

    public static CoroutineRunner Instance
    {
        get
        {
            if (instance == null)
            {
                // Tạo một GameObject để chứa CoroutineRunner
                GameObject runner = new GameObject("CoroutineRunner");
                instance = runner.AddComponent<CoroutineRunner>();
                DontDestroyOnLoad(runner); // Đảm bảo không bị phá hủy khi chuyển cảnh
            }
            return instance;
        }
    }

    // Hàm để bắt đầu một Coroutine từ bên ngoài
    //public void StartStaticCoroutine(IEnumerator coroutine)
    //{
    //    StartCoroutine(coroutine);
    //}


    // Hàm chuyển Coroutine thành Task
    public Task RunCoroutineAsync(IEnumerator coroutine)
    {
        var tcs = new TaskCompletionSource<bool>();

        StartCoroutine(RunAndComplete(coroutine, tcs));

        return tcs.Task;
    }
    private IEnumerator RunAndComplete(IEnumerator coroutine, TaskCompletionSource<bool> tcs)
    {
        yield return coroutine; // Chờ Coroutine hoàn thành
        tcs.SetResult(true);    // Đánh dấu Task là hoàn thành
    }

    public IEnumerator StartDownloadFiles()
    {
        KTDownloadManager.Downloader downloader = KTDownloadManager.Create();
        downloader.URL = string.Format("https://cdn.kiemloc.com/android/Zip/Data/Config.unity3d");

        downloader.OutputFileDir = string.Format("{0}", "Data/Config.unity3d");
        downloader.IsBackground = true;
        KTDownloadManager.Instance.Run(downloader);
        UnityEngine.Debug.Log("Is Background: " + downloader.IsBackground.ToString());
        short num = 1;
        while (!downloader.Completed)
        {
            //UnityEngine.Debug.Log(downloader.Completed + ": " + num++.ToString());
            yield return null;
        }
        /// Nếu là lỗi 404
        if (downloader.ResponseCode == 404)
        {
            UnityEngine.Debug.Log("==============================================>  Error404!");
            Super.ShowMessageBox("Lỗi tải tài nguyên", string.Format("Có lỗi khi tải xuống File {0}, hãy liên hệ với Admin hoặc hỗ trợ để được xử lý"), () =>
            {
                /// Thực thi sự kiện khi quá trình hỏng
                this.Faild?.Invoke();
                /// Hủy đối tượng
                this.Destroy();
            });
            /// Không làm gì cả
            while (true)
            {
                yield return null;
            }
        }

        /// Nếu có lỗi
        if (downloader.HasError)
        {
            UnityEngine.Debug.Log(" ==============================================> HasError!");
        }
        UnityEngine.Debug.Log(downloader.Completed + ": ==============================================> " + num++.ToString());
    }

    public IEnumerator UpdateSkillData()
    {
        yield return null;
        int totalDoneLoadingAssetBundles = 0;
        #region Tải AssetBundles
        string configPath = Global.WebPath(string.Format("Data/{0}", Consts.GAME_CONFIG_FILE));
        //UnityEngine.Debug.Log(configPath);
        ResourceLoader.LoadAssetBundleAsync(configPath, true, (assetBundle) => {
            this.GameConfigAssetBundle = assetBundle;
            /// Tăng tổng số AssetBundle đã tải thành công
            totalDoneLoadingAssetBundles++;
            UnityEngine.Debug.Log("Before LoadSkillData!");
        }, (errorMessage) => {
            Super.ShowMessageBox("Lỗi tải dữ liệu", "Không thể tải dữ liệu từ file Config.unity3d. Hãy kiểm tra!");
        });
        #endregion
        /// Chừng nào chưa tải xong
        while (totalDoneLoadingAssetBundles < 1)
        {
            /// Đợi
            //UnityEngine.Debug.Log("0 Before LoadSkillData!");
            yield return null;
            //UnityEngine.Debug.Log("1 Before LoadSkillData!");
        }
        //WaitForSeconds wait = new WaitForSeconds(10f);
        WaitForSeconds wait = null;
        yield return wait;
        UnityEngine.Debug.Log("After LoadSkillData!");
        Loader.LoadSkillData(ResourceLoader.LoadXMLFromBundle(this.GameConfigAssetBundle, Consts.XML_SKILLDATA_FILE));
        this.OnLoadFinish?.Invoke();
        /// Xóa đối tượng
        GameObject.Destroy(this.gameObject);
        /// Xóa AssetBundle
        this.GameConfigAssetBundle.Unload(true);
        GameObject.Destroy(this.GameConfigAssetBundle);
        /// Gọi GC dọn rác
        GC.Collect(); 
    }

    private void Destroy()
    {
        GameObject.Destroy(this.gameObject);
    }

}