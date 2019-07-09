// ==================================================================
// 作    者：Pablo.风暴洋-宋杨
// 説明する：在这里输入类的功能
// 作成時間：#CreateTime
// 類を作る：getQRcode.cs
// 版    本：v 1.0
// 会    社：大连仟源科技
// QQと微信：731483140
// ==================================================================

using UnityEngine;
using UnityEngine.UI;
using System;

public class getQRcode : MonoBehaviour {
    public HttpModel tuiguangQRcode;
    public RawImage qrcode;
    public Func<string> qrcodeUrlFunc;
    private string qrcodeUrlstr;


    public void getCode()
    {
        tuiguangQRcode.Data.AddData("uid", Static.Instance.GetValue("uid"));
        tuiguangQRcode.EventCallBack.Addlistener((jd) =>
        {
            if (jd["code"].ToString() == "1")
            {
                LoadImage.GetLoadIamge.Load(jd["data"].ToString(), new RawImage[] { qrcode });
                qrcodeUrlstr = jd["data"].ToString();
                qrcodeUrlFunc = QrcodeUrlstr;
            }            
        });
        tuiguangQRcode.Get();
    }

    private string QrcodeUrlstr()
    {
        return qrcodeUrlstr;
    }
}
