// ==================================================================
// 作    者：Pablo.风暴洋-宋杨
// 説明する：在这里输入类的功能
// 作成時間：2019-06-20
// 類を作る：AndroidSdkInterfaceWeChat.cs
// 版    本：v 1.0
// 会    社：大连仟源科技
// QQと微信：731483140
// ==================================================================

using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class AndroidSdkInterfaceWeChat : MonoBehaviour
{
    private string AppId = "wxa7175ac73fa6515d";
    private string AppSecret = "3329edaae29ab723eb79d6cae2cbe04d";
    private bool session;
    JsonData json = new JsonData();
    public getQRcode getQRcode;
    public Text text;

    //这是调用在 Xcode 写的微信分享的函数
    [DllImport("__Internal")]
    private static extern void ShareByIos(int type, string url);

    private int type = 0;//0：表示分享到聊天；1：表示分享到朋友圈

    //public void Click0()
    //{
    //    StartCoroutine(SavePic());
    //    type = 0;
    //}

    //public void Click1()
    //{
    //    StartCoroutine(SavePic());
    //    type = 1;
    //}

    private void Start()
    {
        //		// example2: Stopwatch class
        //		System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
        //		sw.Start();
        //		for (int i = 0; i < 10; i++) {
        //			Texture dd = (Texture)Resources.Load("CardType " + i);
        //		}
        //		sw.Stop();
        //		TimeSpan ts2 = sw.Elapsed;
        //		NGUIDebug.Log ("tongyong_di   " + ts2.TotalMilliseconds.ToString ());

        //Debug.Log("{\"type\" = 3, \"url\" = \"www.baide.com\", \"title\" = \"标题\",\"description\" = \"百度大法好\"}");
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        jo.Call("RegisterToWeChat", AppId, AppSecret);  //填写appid
    }

    public void CallBack(string str)
    {
        if (text != null)
        {
            text.text = str;
        }
    }

    public void ShareUrl(bool session)
    {
        json.Clear();
        json["type"] = 3;
        json["url"] = "http://weibo.com/u/1235764025?refer_flag=1001030102_";
        json["title"] = "饭太黏";
        json["description"] = "饭太黏最帅";
        json["isCircleOfFriends"] = session;
        session = this.session;
        Share();
        // AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        // AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        // jo.Call("WeChat", json）//"{\"type\" = 3, \"url\" = \"www.baide.com\", \"title\" = \"标题\",\"description\" = \"百度大法好\"},\"isCircleOfFriends\" = \"true\"");
    }

    public void ShareText(bool session)
    {
        json.Clear();
        json["type"] = 4;
        json["title"] = "饭太黏";
        json["description"] = "饭太黏最帅";
        json["text"] = "饭太黏最帅text";
        json["isCircleOfFriends"] = session;
        session = this.session;
        Share();
    }

    public void ShareMusic(bool session)
    {
        json.Clear();
        json["type"] = 5;
        json["url"] = "http://music.163.com/m/song?id=110771&userid=6725175#?thirdfrom=qq";
        json["title"] = "饭太黏";
        json["description"] = "饭太黏最帅";
        json["isCircleOfFriends"] = session;
        session = this.session;
        Share();
    }

    public void ShareVideo(bool session)
    {
        json.Clear();
        json["type"] = 6;
        json["url"] = "http://www.qq.com";
        json["title"] = "饭太黏";
        json["description"] = "饭太黏最帅";
        json["isCircleOfFriends"] = session;
        session = this.session;
        Share();
    }

    public void ShareImage(bool session)
    {
#if UNITY_ANDROID
        json.Clear();
        json["type"] = 7;
        json["title"] = "饭太黏";
        json["description"] = "饭太黏最帅";
        json["isCircleOfFriends"] = session;
        json["url"] = getQRcode.qrcodeUrlFunc();
        //Debug.Log("getQRcode.qrcodeUrlFunc()" + " " + getQRcode.qrcodeUrlFunc());
        session = this.session;
        Share();
#elif UNITY_IPHONE
        StartCoroutine(SavePic());
         if (session)
            type = 1;
        else
            type = 0;
#endif

    }
    public void Selected(bool isSelect)
    {
        session = isSelect;
        Debug.Log(isSelect);
    }
    private void Share()
    {
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        jo.Call("WeChat", json.ToJson());
        jo.Call("StartAc", AppId);
    }

    /// <summary>
    /// iOS微信分享
    /// </summary>
    /// <returns></returns>
    IEnumerator SavePic()
    {
        WWW www = new WWW(getQRcode.qrcodeUrlFunc());
        yield return www;
        Debug.Log(www.text);
        Debug.Log(www.error);
        byte[] file = www.bytes;
        int length = file.Length;
        Stream sw;
        FileInfo File = new FileInfo(Application.persistentDataPath + "/share.png");
        sw = File.Create();
        sw.Write(file, 0, length);
        sw.Close();
        sw.Dispose();
        ShareByIos(type, Application.persistentDataPath + "/share.png");
    }
}
