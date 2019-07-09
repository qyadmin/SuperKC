using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using LitJson;
using UnityEngine.Events;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class HttpModel : MonoBehaviour
{

    public TypeGo DataType;

    public NewMessageInfo Data;
    public List<GetMessageModel> MsgList = new List<GetMessageModel>();
    public UnityEvent Suc, Fal;
    //临时事件
    public MessageNone EventObj = new MessageNone();
    public MessageEvent<JsonData> EventCallBack = new MessageEvent<JsonData>();
    public bool IsLock = false;

    public UnityEvent DoAction;
    public bool NoShow = false;
    public bool HideMessage = false;
    public bool OnlyError;
    public bool IsAdd;
    [TextArea(2, 5)]
    public string ShowAddMessage;
    private string message = null;
    public float WaitTime = 0;

    public bool isLoacl;
    public string LocalHttp;
    public bool IsShell;

    public bool NoCheck;
    public bool NoldIcon;
    public bool IsWait;
    void Awake()
    {
        IsLock = Static.Instance.Lock;
        //urlC.Add("");
    }

    private bool isLocksend = false;

    List<string> urlC = new List<string>();

    public void Get()
    {
        if (isLocksend)
            return;
        StartLoad();
        Data.ErrorCode = "-1";
        Data.BackData.Clear();
        message = null;
        if (!isLoacl)
            message += "?";
        if (IsLock)
            message += EncryptDecipherTool.UserMd5();
        if (Static.Instance.IsDebug)
        {
            DebugAction(Data.DebugData);
            return;
        }

        //URL请求拼接
        string url = null;
        if (isLoacl)
            url = Static.Instance.LocalURL + LocalHttp;
        else
        {
            string uid = Static.Instance.GetValue("uid");
            string tel = Static.Instance.GetValue("phone");

            if (!string.IsNullOrEmpty(uid))
                Data.AddData("uid", uid);

            if (!string.IsNullOrEmpty(tel))
                Data.AddData("phone",tel);
            url = Static.Instance.URL + Data.URL;
        }

        //if (urlC.Contains(url))
        //{


        //}

        if (Data.SendData.Count > 0)
        {
            foreach (DataValue child in Data.SendData)
            {
                if (child.GetString() == "Error")
                {
                    EndLoad();
                    return;
                }
                if (child.GetString() == "Gone")
                    continue;
                message += "&" + child.Name + "=" + child.GetString();
            }
        }

        message = EncryptDecipherTool.GetListOld(message, IsLock);
        url = url + message;
        url = Uri.EscapeUriString(url);

        StartCoroutine(GetMessage(url));
    }
    private int nub = 0;
    IEnumerator GetMessage(string url)
    {
        //if(!NoldIcon)
        Debug.Log(url);
        WWW www = new WWW(url);
        yield return www;
        EndLoad();
        if (IsWait)
            yield return new WaitForSeconds(0.1f);
        if (www.error != null)
        {
            //if (!NoShow)
            //    MessageManager._Instantiate.Show("网络连接失败！");
            Debug.LogError(url);
            Data.ShowMessage = nub + "**" + www.error;
            nub++;
        }
        else
        {
            Data.ShowMessage = nub + "**" + www.text;
            nub++;
            //try
            //{
            string jsondata = System.Text.Encoding.UTF8.GetString(www.bytes, 0, www.bytes.Length);
            jsondata = jsondata.Remove(0, Data.CutCount);
            foreach (HttpModel child in Data.ShareModel)
            {
                child.DebugAction(jsondata);
            }
            DebugAction(jsondata);
            //}
            //    catch
            //{

            //    MessageManager._Instantiate.Show("数据解析异常！");
            //    EndLoad();
            //}
        }
    }


    public void DebugAction(string DebugData)
    {
        string jsondata = DebugData;
        int a = 0;
        Static.Instance.DeleteFile(Application.persistentDataPath, "json.txt");
        Static.Instance.CreateFile(Application.persistentDataPath, "json.txt", jsondata);
        ArrayList infoall = Static.Instance.LoadFile(Application.persistentDataPath, "json.txt");
        String sr = null;
        foreach (string str in infoall)
        {
            sr += str;
        }
        JsonData jd = JsonMapper.ToObject(sr);

        Data.ErrorMsg = jd.Keys.Contains("errmsg") ? jd["errmsg"].ToString() : "";
        Data.ErrorCode = jd.Keys.Contains("errcode") ? jd["errcode"].ToString() : "-1";
        string getcode = jd.Keys.Contains("code") ? jd["code"].ToString() : "-1";
        if (getcode == "1")
            Data.ErrorCode = "0";
        if (getcode == "0")
        {
            Data.ErrorCode = "-1";
        }

        if (IsShell)
        {
            if (Data.ErrorCode == "0")
            {
                if (jd.Keys.Contains(Data.HeaderName))
                    jd = jd[Data.HeaderName];
                else
                    Debug.Log("没有获得headName");
            }
        }

        foreach (GetMessageModel child in MsgList)
        {
            child.SetValue(jd);
        }
        if (Data.ErrorCode == "0")
        {
            List<string> Savename = new List<string>();
            Dictionary<string, string> SaveMessage = new Dictionary<string, string>();
            switch (DataType)
            {
                case TypeGo.GetTypeA:
                    break;

                case TypeGo.GetTypeB:
                    foreach (Transform child in Data.MyListMessage.FatherObj)
                    {
                        Destroy(child.gameObject);
                    }
                    Data.MyListMessage.SetVaule(jd[Data.DataName]);
                    break;

                case TypeGo.GetTypeC:
                    Data.MyListMessage.SetValueSingle(jd[Data.DataName]);
                    break;

                case TypeGo.GetTypeD:
                    Data.MyListMessage.SendData(jd);
                    break;

                case TypeGo.GetTypeE:
                    foreach (Transform child in Data.MyListMessage.FatherObj)
                    {
                        Destroy(child.gameObject);
                    }
                    Data.MyListMessage.SetValueAll(jd[Data.DataName]);
                    break;
            }

            if (Data.Action)
            {
                Data.GetData(SaveMessage);
            }
        }

        if (Data.ErrorCode == "0")
        {
            EventObj.Send();
            Suc.Invoke();
        }
        else
        {
            Fal.Invoke();
        }
       
        EventCallBack.Send(jd);
        EventObj.ClearAllEevnt();
        EventCallBack.ClearAllEevnt();
        if (!HideMessage)
        {
            ShowMessageWait();
        }

        if (MessageManager._Instantiate)
            MessageManager._Instantiate.ShowErrorCode(Data.ErrorCode);

    }


    public void ShowMessageWait()
    {
        if (Data.ErrorCode != "0")
        {
            if (Data.ErrorCode == "20001")
            {
                MessageManager._Instantiate.Show(Data.ErrorMsg + "(" + Data.ErrorCode + ")" + "2秒后自动退出游戏");
                Invoke("QuiteGo", 2);
            }
            else
            {
                MessageManager._Instantiate.Show(Data.ErrorMsg + "(" + Data.ErrorCode + ")");
            }
        }
        else
        {
            if (IsAdd)
                MessageManager._Instantiate.Show(ShowAddMessage);
        }
    }

    void QuiteGo()
    {
        SceneManager.LoadScene("mainmeun");
    }


    private void StartLoad()
    {
        if (NoldIcon)
            return;
        MessageManager._Instantiate.AddLockNub();
        isLocksend = true;
    }

    private void EndLoad()
    {
        if (NoldIcon)
            return;
        MessageManager._Instantiate.DisLockNub();
        isLocksend = false;
        //检查用户信息完成度
        if (GameManager.GetGameManager && !NoCheck)
            GameManager.GetGameManager.CheckUserInfo();
    }
}
