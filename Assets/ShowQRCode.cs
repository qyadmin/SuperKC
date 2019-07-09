// ==================================================================
// 作    者：Pablo.风暴洋-宋杨
// 説明する：在这里输入类的功能
// 作成時間：2019-07-05
// 類を作る：ShowQRCode.cs
// 版    本：v 1.0
// 会    社：大连仟源科技
// QQと微信：731483140
// ==================================================================

using UnityEngine;
using UnityEngine.UI;

public class ShowQRCode : MonoBehaviour
{
    public HttpModel configHttp;
    public RawImage code1, code2, code3;
    public Text[] describeTxt;
    public Text[] nameTxt;
    public Text[] walletAddTxt;
    public Text[] walletName;
    private void Start()
    {
        Click();
    }

    public void Click()
    {
        configHttp.EventCallBack.Addlistener((jd) =>
        {
            LoadImage.GetLoadIamge.Load(jd["erweima"]["url1"].ToString(), new RawImage[] { code1 });
            describeTxt[0].text = jd["erweima"]["name1"].ToString() + "兑换比率为" + jd["erweima"]["bili1"].ToString();
            nameTxt[0].text = jd["erweima"]["name1"].ToString();
            walletName[0].text = jd["erweima"]["name1"].ToString() + "地址";
            walletAddTxt[0].text = jd["erweima"]["qianbao1"].ToString();

            LoadImage.GetLoadIamge.Load(jd["erweima"]["url2"].ToString(), new RawImage[] { code2 });
            describeTxt[1].text = jd["erweima"]["name2"].ToString() + "兑换比率为" + jd["erweima"]["bili2"].ToString();
            nameTxt[1].text = jd["erweima"]["name2"].ToString();
            walletName[1].text = jd["erweima"]["name2"].ToString() + "地址";
            walletAddTxt[1].text = jd["erweima"]["qianbao2"].ToString();

            LoadImage.GetLoadIamge.Load(jd["erweima"]["url3"].ToString(), new RawImage[] { code3 });
            describeTxt[2].text = jd["erweima"]["name3"].ToString() + "兑换比率为" + jd["erweima"]["bili3"].ToString();
            nameTxt[2].text = jd["erweima"]["name3"].ToString();
            walletName[2].text = jd["erweima"]["name3"].ToString() + "地址";
            walletAddTxt[2].text = jd["erweima"]["qianbao3"].ToString();
        });
    }
}
