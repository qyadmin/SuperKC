using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;

//用于储存任务消息的容器
[System.Serializable]
public class RenWuMesage
{
    public string id;
    public string Title;
    public string Message;
    public string ButtonText;
    public string req;//是否领取
    public string complete;//是否完成
    public string current;
    public string count;
    public string coin;

    public Sprite HeadIamge;
    private string configTag = "renwu";

    public bool IsFalse;
    public Transform ShowRenWu;
}


public class Model_RenWu : MonoBehaviour {

    private Dictionary<string, RenWuMesage> AllBody = new Dictionary<string, RenWuMesage>();

    public RenWuMesage[] Alldata;

    //任务对象装载
    public void Add()
    {
        AllBody.Clear();
        foreach (RenWuMesage child in Alldata)
        {
            AllBody.Add(child.id, child);
        }     
    }

    //获取任务对象
    public RenWuMesage GetBody(string GetName)
    { 
        RenWuMesage obj = null;
        AllBody.TryGetValue(GetName, out obj);
        return obj;
    }

    public RenWuMesage SetData(string GetData)
    {
        if (GetData == null)
            return null;
        JsonData jd = JsonMapper.ToObject(GetData);
        RenWuMesage obj = GetBody(jd["id"].ToString());
        //给任务容器赋值
        obj.id = jd["id"].ToString();
        obj.req = jd["req"].ToString();
        obj.complete = jd["complete"].ToString();
        obj.current = jd["current"].ToString();
        obj.count = jd["count"].ToString();
        obj.coin = jd["coin"].ToString();

        return obj;
    }

}
