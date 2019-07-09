using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;

//用于储存任务消息的容器
[System.Serializable]
public class PropData
{
    //public string uid;
    public string dl;
    public string wid;
    public string pid;
    public string postionid;
    public string id;
    public string suanli;
    public string name;
    public string color;
    public string desc;
    public Sprite HeadIamge;
    public string kd_id="-1";
    public ActionEvent_Prop ActonObj;

    //读取配置文件
    public EventConfig MessageBody;
    public void  AddPropMesage()
    {
        MessageBody = new EventConfig("desc", "prop_desc", this.id);
    }
}

public class Model_Prop : MonoBehaviour
{
    public Dictionary<string, PropData> AllPropData = new Dictionary<string, PropData>();

    //装在道具后更新状态
    public void UpdateProp(ActionEvent_Prop Propitem, string pid,string wid,string posid)
    {
        PropData obj = GetPropObj(pid);
        obj.wid = wid;
        obj.postionid = posid;
        WorkerBody.ShowKD.UpdatePropTramforms(wid,this);

        Propitem.wid.text = WorkerBody.GetWorkerName(obj.wid);
        if (obj.wid != "0")
        {
            Propitem.Mark.SetActive(true);
        }
        else
        {
            Propitem.Mark.SetActive(false);
        }
    }

    //卸载道具后更新状态
    public void UpdateUnLoadProp(PropMessage PropPos)
    {
        WorkerBody.ShowKD.UnLoadProp(PropPos.pos);
        PropData Prop = GetPropObj(PropPos.pid);

        Prop.wid = "0";
        Prop.postionid = "0";
        Prop.ActonObj.wid.text = "";
        PropPos.wid="0";
        PropPos.pos = "0";
        Prop.ActonObj.Mark.SetActive(false);

    }



    //道具对象装载
    public void Add()
    {
        //AllBody.Clear();
        //foreach (PropMesage child in Alldata)
        //{
        //    AllBody.Add(child.id, child);
        //    //初始化事件绑定
        //    child.AddPropMesage();
        //}
    }


    //从容器中获取道具对象
    public PropData GetPropObj(string wid)
    {
        PropData obj = null;
        AllPropData.TryGetValue(wid, out obj);
        return obj;
    }


    [SerializeField]
    private Transform FatherObj;
    [SerializeField]
    private GameObject ListObj;
    [SerializeField]
    private HttpModel Http_LoadProp;
    [SerializeField]
    private HttpModel Http_CheckProp;
    //矿工容器
    [SerializeField]
    private Woker_Message WorkerBody;

    //给道具属性赋值
    List<GameObject> AllObj = new List<GameObject>();

    public void AddData(JsonData GetData)
    {
        if (GetData == null)
            return;
        JsonData data = GetData["prop"];
        AddProp(data,true);
    }

    public void SetData(JsonData GetData)
    {
        if (GetData == null)
            return ;

        foreach (Transform child in FatherObj)
        {
          Destroy(child.gameObject);
        }  
        JsonData data = GetData["prop"];
        foreach (JsonData jd in data)
        {
            AddProp(jd,false);
        }

    }

    private void AddProp(JsonData jd,bool up)
    {
        PropData obj = new PropData();
        //给道具容器赋值
        obj.id = jd["id"].ToString();
        obj.pid = jd["uuid"].ToString();
        obj.wid = jd["wid"].ToString();
        obj.postionid = jd["pos"].ToString();
        obj.id = jd["id"].ToString();
        obj.color = jd["color"].ToString();
        obj.desc = jd["desc"].ToString();
        obj.suanli = jd["cal"].ToString();
        obj.name = jd["name"].ToString();


        GameObject NewList = GameObject.Instantiate(ListObj);
        NewList.transform.SetParent(FatherObj);
        NewList.transform.localScale = new Vector3(1, 1, 1);
        NewList.SetActive(true);
        NewList.name = ListObj.name;
        if(up)
            NewList.transform.SetSiblingIndex(0);
        ActionEvent_Prop GetProp = NewList.GetComponent<ActionEvent_Prop>();
        obj.ActonObj = GetProp;
        if (AllPropData.ContainsKey(obj.pid))
            AllPropData.Remove(obj.pid);
        AllPropData.Add(obj.pid, obj);
        if (obj != null)
        {
            GetProp.suanli.text = obj.suanli;
            GetProp.Message.text = obj.desc.Replace("<s>", obj.name).Replace("s", obj.suanli);
            GetProp.Name.text = obj.name;
            GetProp.wid.text = WorkerBody.GetWorkerName(obj.wid);
            ModelManager.GetModelManager.SetImagePropColor(GetProp.HeadIamge, obj.id, obj.color);
            if (obj.wid != "0")
            {
                GetProp.Mark.SetActive(true);
            }
            else
            {
                GetProp.Mark.SetActive(false);
            }
        }
        GetProp.LoadButton.onClick.RemoveAllListeners();
        GetProp.LoadButton.onClick.AddListener(delegate ()
        {
            if (!WorkerBody.TryGetPos(ModelManager.GetModelManager.position_id))
            {
                MessageManager._Instantiate.Show("道具栏已经添加了道具，不能再次添加");
                return;
            }

            Http_CheckProp.Data.AddData("pid", obj.pid);
            Http_CheckProp.Data.AddData("wid", ModelManager.GetModelManager.kd_id);

            Http_CheckProp.EventObj.Addlistener(delegate ()
            {
                Http_LoadProp.Data.AddData("pid", obj.pid);
                Http_LoadProp.Data.AddData("pos", ModelManager.GetModelManager.position_id);
                Http_LoadProp.Data.AddData("wid", ModelManager.GetModelManager.kd_id);
                Http_LoadProp.EventObj.Addlistener(delegate ()
                {
                    //加载成功更新矿工，道具信息
                    UpdateProp(GetProp, obj.pid, ModelManager.GetModelManager.kd_id, ModelManager.GetModelManager.position_id);
                    //更新矿井算力
                    GameManager.GetGameManager.GetModeMine.UpdateMineMessage();
                });
                Http_LoadProp.Get();
            });
            Http_CheckProp.Get();

        }
        );
    }
}
