using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using UnityEngine.UI;
using System.Linq;
public class Woker_Message : MonoBehaviour {


    private Vector3 startpos;
    //矿工显示组件
    [System.Serializable]
    public class Tranmofms_KD
    {
        public Text Power;
        public Text totalPower;
        public Text SuanLi;
        public Text Need_jb;
        public Text Name;
        public Text desc;
        public Text lvl;
        public Text levelUpCoin;
        public Text prevmc;
        public Text runtimemc;

        public Image PowerImage;
        public Image IamgeBody;
        public Image Color;

        public Transform StartBody;
        public Transform djgroup;
        public Transform FatherObj;
        public GameObject WkObj;

        private List<PropMessage> Group_pef_dj = new List<PropMessage>();
        public List<PropMessage> Group_dj { get { return Group_pef_dj; } }
        public void AddProp()
        {

        }

        public void UnLoadProp(string posid)
        {
            Group_pef_dj[int.Parse(posid) - 1].ReStart();
        }

        public void UpdatePropTramforms(string wid,Model_Prop GetProp)
        {
            Group_pef_dj.Clear();
            int J = 0;
            foreach (Transform child in djgroup)
            {
                PropMessage obj = child.GetComponent<PropMessage>();
                Group_pef_dj.Add(obj);
                obj.postionid = J;
                J++;
            }
          
            foreach (PropMessage child in Group_pef_dj)
            {
                child.ReStart();
            }

            foreach (KeyValuePair<string, PropData> child in GetProp.AllPropData)
            {
                if (child.Value.wid == wid)
                    Group_pef_dj[int.Parse(child.Value.postionid) - 1].SetData(child.Value);
            }
        }

        //显示当前选择矿工的属性
        public void Show(KD SelenWoker, Model_Prop GetProp)
        {

            //道具复位
            UpdatePropTramforms(SelenWoker.id, GetProp);
            //组件赋值
            this.SuanLi.text = SelenWoker.SuanLi;
            this.totalPower.text = SelenWoker.AllPower;
            this.Power.text = SelenWoker.Power;
            this.Need_jb.text = SelenWoker.Need_jb;
            this.desc.text = SelenWoker.desc.Replace("s", SelenWoker.Need_jb);
            this.Name.text = SelenWoker.name;
            this.lvl.text = SelenWoker.lvl;
            this.levelUpCoin.text = SelenWoker.levelUpCoin;
            this.prevmc.text = SelenWoker.prevmc;
            this.runtimemc.text = SelenWoker.runtimemc;
            this.Color.color = ModelManager.GetModelManager.GetColor(SelenWoker.color);
            //血量进度条显示
            this.PowerImage.fillAmount = float.Parse(SelenWoker.Power) / float.Parse(SelenWoker.AllPower);

            //星级复位
            foreach (Transform child in StartBody)
                child.gameObject.SetActive(false);
            if (int.Parse(SelenWoker.stratnub) > 0)
            {
                for (int i = 0; i < int.Parse(SelenWoker.stratnub); i++)
                {
                    StartBody.GetChild(i).gameObject.SetActive(true);
                }
            }

            //判断是否挖矿种
            if (int.Parse(SelenWoker.mine) > 0)
            {
                WkObj.SetActive(true);
            }
            else
                WkObj.SetActive(false);

            //头像赋值
            ModelManager.GetModelManager.SetIamge(IamgeBody, int.Parse(SelenWoker.id));
        }
    }


    public Tranmofms_KD ShowKD;


    //判断目标pos是否可添加道具
    public bool TryGetPos(string pos)
    {
        return !ShowKD.Group_dj[int.Parse(pos)-1].IsHave;
    }

    //开启矿工界面自动读取矿工1的属性
    public void GetFrist()
    {
        if (KD_list.Count==0)
            return;
        ModelManager.GetModelManager.kd_id = KD_list.Values.First().id;
        Show(KD_list.Values.First().id);
        ButtonEffect.GetComponent<Effect>().Hide();
    }

    public void UpdatePropInKD()
    {
        //Show(ModelManager.GetModelManager.kd_id);
    }

    //传入矿工id
    public void Show(Text id_text)
    {
        //记录当前装备的矿工ID;
        ModelManager.GetModelManager.kd_id = id_text.text;
        ShowKD.Show(Get_kd(id_text.text), Prop);

    }

    public void Show(string id)
    {
        //记录当前装备的矿工ID;
        ModelManager.GetModelManager.kd_id = id;
        ShowKD.Show(Get_kd(id), Prop);

    }

    public void Show()
    {
        KD obj = Get_kd(ModelManager.GetModelManager.kd_id);
        if (obj != null)
            ShowKD.Show(obj, Prop);
    }


    //矿工属性列表
    public class KD
    {
        public string lvl = "-1";
        public string id = "-1";
        public string creat = "-1";
        public string mine = "-1";
        public string SuanLi = "-1";
        public string Power = "-1";
        public string Need_jb = "-1";
        public string AllPower = "-1";
        public string desc = "-1";
        public string name = "-1";
        public string stratnub = "-1";
        public string levelUpCoin = "-1";
        public string prevmc = "-1";
        public string runtimemc = "-1";
        public string pos = "-1";
        public string color = "-1";
        public GameObject Obj;
        private Tranmofms_KD KDOBJ;
        private Model_Prop DJ;
 
        public void UpdateLevel(string lvl)
        {
            Obj.transform.GetChild(1).GetComponent<Text>().text = this.lvl;
        }

        public void SetObj(GameObject GetObj, Tranmofms_KD GetDK, Model_Prop GetProp)
        {
            Obj = GetObj;
            KDOBJ = GetDK;
            DJ = GetProp;
            GetObj.GetComponent<Button>().onClick.AddListener(Send);
        }
        private void Send()
        {
            KDOBJ.Show(this, DJ);
        }

        public void SetPos(Model_Mine GetModelMine)
        {
            if (int.Parse(mine) > 0)
                GetModelMine.AddPos(id, mine, pos, lvl,Power);
        }

        public void RemovePos(Model_Mine GetModelMine)
        {
             GetModelMine.RemovwPos(id, mine);
            this.mine = "0";
        }

        public void RemovePosOnlyTd (Model_Mine GetModelMine)
        {
            GetModelMine.RemovwPos(id);
        }

        public void AddPos(Model_Mine GetModelMine,Model_RunMine GetRunMine)
        {
            this.mine = (int.Parse(GetRunMine.minepos.mine_id) - 1).ToString();
            this.pos = GetRunMine.minepos.pos_id;
            GetModelMine.AddPos(id, mine, this.pos, lvl, Power);
        }
    }

    //根据ID添加矿工到字典中
    public Dictionary<string, KD> KD_list = new Dictionary<string, KD>();

    //获取矿工对象
    public KD Get_kd(string id)
    {
        KD obj = null;
        KD_list.TryGetValue(id, out obj);
        return obj;
    }


    public string GetWorkerName(string wid)
    {
        KD obj = Get_kd(wid);
        if (obj != null)
            return obj.name;
        return string.Empty;
    }


    [SerializeField]
    private Model_Mine GetModelMine;
    [SerializeField]
    private GameObject mark;


    public void AddData(JsonData jd)
    {
        JsonData GetData = jd["worker"];
        if (GetData == null)
            return;
        AddWorker(GetData);
    }

    private Dictionary<string, GameObject> AlllistButon = new Dictionary<string, GameObject>();
    //获取矿工信息
    public void Message_KD(JsonData jd)
    {
        mark.SetActive(true);
        foreach (Transform child in FatherObj)
        {
            Destroy(child.gameObject);
        }
        KD_list.Clear();


        JsonData GetData = jd["worker"];
        if (GetData == null||GetData.Count==0)
            return;
        mark.SetActive(false);
        //清除所有矿洞信心
        GetModelMine.ClearPos();

        foreach (JsonData child in GetData)
        {
            AddWorker(child);
        }
        GetModelMine.UpdateMineState();
    }


    public void AddWorker(JsonData child)
    {
        //读取矿工信息
        KD woker = new KD();
        woker.lvl = child["lvl"].ToString();
        woker.id = child["id"].ToString();
        woker.creat = child["create"].ToString();
        woker.mine = child["mine"].ToString();
        woker.Power = child["power"].ToString();
        woker.AllPower = child["totalpower"].ToString();
        woker.desc = child["desc"].ToString();
        woker.name = child["name"].ToString();
        woker.stratnub = child["star"].ToString();
        woker.SuanLi = child["cal"].ToString();
        woker.levelUpCoin = child["levelupcoin"].ToString();
        woker.prevmc = (float.Parse(child["prevmc"].ToString()) / 1000).ToString();
        woker.runtimemc = (float.Parse(child["runtimemc"].ToString()) / 1000).ToString();
        woker.pos = child["pos"].ToString();
        woker.color = child["color"].ToString();
        woker.Need_jb = child["levelupcoin"].ToString();
        //生成列表
        GameObject NewList = GameObject.Instantiate(ListObj);
        NewList.transform.SetParent(FatherObj);
        NewList.transform.localPosition -= new Vector3(0, 0, NewList.transform.localPosition.z);
        NewList.transform.localScale = new Vector3(1, 1, 1);
        NewList.SetActive(true);
        NewList.name = ListObj.name;
        NewList.transform.GetChild(0).GetComponent<Text>().text = woker.name;
        NewList.transform.GetChild(1).GetComponent<Text>().text = woker.lvl;
        NewList.GetComponent<Button>().onClick.AddListener(delegate ()
        {
            ModelManager.GetModelManager.kd_id = woker.id;
            ButtonEffect.position = NewList.transform.position;
            ButtonEffect.localScale = new Vector3(1, 1, 1);
            AtionObj.localScale = Vector3.zero;
        });
        ModelManager.GetModelManager.SetSmallIamge(NewList.transform.GetChild(2).GetComponent<Image>(), int.Parse(woker.id));
        woker.SetObj(NewList, ShowKD, Prop);
        if (KD_list.ContainsKey(woker.id))
            KD_list.Remove(woker.id);
        KD_list.Add(woker.id, woker);
        woker.SetPos(GetModelMine);
    }



    //更新矿工信息
    public void UpdateWorker(JsonData jd)
    {
        JsonData child = jd["worker"];
        if (child == null)
            return;
        mark.SetActive(false);
        //读取矿工信息
        KD woker = new KD();
        woker.lvl = child["lvl"].ToString();
        woker.id = child["id"].ToString();
        woker.creat = child["create"].ToString();
        woker.mine = child["mine"].ToString();
        woker.Power = child["power"].ToString();
        woker.AllPower = child["totalpower"].ToString();
        woker.desc = child["desc"].ToString();
        woker.name = child["name"].ToString();
        woker.stratnub = child["star"].ToString();
        woker.SuanLi = child["cal"].ToString();
        woker.levelUpCoin = child["levelupcoin"].ToString();
        woker.prevmc = (float.Parse(child["prevmc"].ToString()) / 1000).ToString();
        woker.runtimemc = (float.Parse(child["runtimemc"].ToString()) / 1000).ToString();
        woker.pos = child["pos"].ToString();
        woker.color = child["color"].ToString();
        woker.Need_jb = child["levelupcoin"].ToString();
        if (KD_list.ContainsKey(woker.id))
        {
            woker.SetObj(Get_kd(woker.id).Obj, ShowKD, Prop);
            KD_list.Remove(woker.id);
        }
        KD_list.Add(woker.id, woker);

        if (woker.mine == "0")
            woker.RemovePosOnlyTd(GetModelMine);
        woker.SetPos(GetModelMine);
        GetModelMine.UpdateMineState();
        ShowPaiQian(woker.id);
        ShowKD.Show(woker,Prop);
    }


    //显示买饮料
    public void ShowDrink()
    {
         GameManager.GetGameManager.GetWindown(YinLiaoWindowBuy);
    }
    //显示购买饮料界面
    public void ShowDrinkAndBuy()
    {
        if (Static.Instance.GetValue("drink") == "0")
            GameManager.GetGameManager.GetWindown(YinLiaoWindowBuy);
        else
            GameManager.GetGameManager.GetWindown(YinLiaoWindowUse);
    }

    private void Start()
    {
        LevelButton.onClick.AddListener(LevelUp);
        AddDrinkButtonPQ.onClick.AddListener(delegate ()
        {
            ShowDrinkAndBuy();
        });
        AddDrinkButton.onClick.AddListener(delegate ()
        {
            ShowDrinkAndBuy();
        });
        startpos=ButtonEffect.position;
    }

    [SerializeField]
    private Transform AtionObj;
    //道具信息传入接口
    public Model_Prop Prop;
    [SerializeField]
    private HttpModel WorkerLevelUp;
    [SerializeField]
    private Button LevelButton;
    //升级矿工
    private void LevelUp()
    {
        WorkerLevelUp.Data.AddData("id", ModelManager.GetModelManager.kd_id);
        WorkerLevelUp.EventObj.Addlistener(delegate() 
        {
            //GameManager.GetGameManager.Labbly.EventObj.Addlistener(delegate()
            //{           

            //});
            //GameManager.GetGameManager.Labbly.Get();
            KD Worker = Get_kd(ModelManager.GetModelManager.kd_id);
            //升级传送给升级模块处理,提示玩家解锁道具||升级成功
            GameManager.GetGameManager.Levle_UnLock(int.Parse(Worker.lvl));
            //升级之后刷新当前矿洞算力
            GetModelMine.UpdateMineMessage();
            Worker.UpdateLevel(Worker.lvl);
        });
        WorkerLevelUp.Get();
    }

    //实例化列表Obj
    public GameObject ListObj;
    public Transform FatherObj;

    //矿工派遣接口
    [SerializeField]
    private Model_RunMine GetMineRun;
    [SerializeField]
    private Transform MineListFather;
    [SerializeField]
    private GameObject ListObjMine;

    private Mine_Pos NowMinePos;
    public void ShowPaiQianList(Mine_Pos GetPosmessage)
    {
        NowMinePos = GetPosmessage;
        ShowReal();
    }

    public void ShowReal()
    {
        if (NowMinePos == null)
            return;     
        nowppaiwoker_id = NowMinePos.wid;
        foreach (Transform child in MineListFather)
           Destroy(child.gameObject);
        foreach (KeyValuePair<string, KD> child in KD_list)
        {
            KD woker = child.Value;
            if (woker.mine != "0")
                continue;
            //生成矿中矿工列表wwq
            GameObject NewList = GameObject.Instantiate(ListObjMine);
            NewList.transform.SetParent(MineListFather);
            NewList.transform.localPosition -= new Vector3(0, 0, NewList.transform.localPosition.z);
            NewList.transform.localScale = new Vector3(1, 1, 1);
            NewList.SetActive(true);
            NewList.transform.GetChild(2).GetComponent<Text>().text = woker.name + "LVL." + woker.lvl;
            ModelManager.GetModelManager.SetIamge(NewList.transform.GetChild(0).GetComponent<Image>(), int.Parse(woker.id));
            NewList.GetComponent<Button>().onClick.AddListener(delegate ()
            {
                SendPai(woker.id);
            });
        }

        GetMineRun.ReCallButton.onClick.RemoveAllListeners();
        GetMineRun.ReCallButton.onClick.AddListener(delegate ()
        {
            ReCall(NowMinePos.wid);
        });
        //隐藏特效
        GetModelMine.UpdateEffect(false);
    }

    private string nowppaiwoker_id;

    //派遣成功回调显示
    public void ShowPaiQian(string wid)
    {
        KD SelenWoker= Get_kd(wid);
        if (GetMineRun == null || SelenWoker==null)
            return;
        //组件赋值
        GetMineRun.SuanLi.text = SelenWoker.SuanLi;
        GetMineRun.Name.text = SelenWoker.name;
        GetMineRun.lvl.text = SelenWoker.lvl;
        GetMineRun.prevmc.text = SelenWoker.prevmc;
        GetMineRun.runtimemc.text = SelenWoker.runtimemc;


        //血量进度条显示
        GetMineRun.power.text = SelenWoker.Power;
        GetMineRun.totoppower.text = SelenWoker.AllPower;
        GetMineRun.PowerImage.fillAmount = float.Parse(SelenWoker.Power) / float.Parse(SelenWoker.AllPower);

        //星级复位
        foreach (Transform child in GetMineRun.StartBody)
            child.gameObject.SetActive(false);
        if (int.Parse(SelenWoker.stratnub) > 0)
        {
            for (int i = 0; i < int.Parse(SelenWoker.stratnub); i++)
            {
                GetMineRun.StartBody.GetChild(i).gameObject.SetActive(true);
            }
        }

        ModelManager.GetModelManager.SetIamge(GetMineRun.IamgeBody, int.Parse(SelenWoker.id));

        GetMineRun.ReCallButton.onClick.RemoveAllListeners();
        GetMineRun.ReCallButton.onClick.AddListener(delegate ()
        {
            ReCall(SelenWoker.id);
        });
        GetMineRun.Have.SetActive(true);
        GetMineRun.NOHave.SetActive(false);
    }


    //想网路模块添加解锁矿场消息
    [SerializeField]
    private HttpModel http_send;
    [SerializeField]
    private HttpModel WokeModel;
    private void SendPai(string wid)
    {
        if (GetMineRun.minepos.State)
        {
            MessageManager._Instantiate.Show("这个位置你已经派遣了一名矿工，不能重复派遣!");
            return;
        }
        http_send.Data.AddData("pos",GetMineRun.minepos.pos_id);
        http_send.Data.AddData("index",(int.Parse(GetMineRun.minepos.mine_id)-1).ToString());
        http_send.Data.AddData("id", wid);

        //派遣成功回调更新
        http_send.EventObj.Addlistener(delegate()
        {
         
            ShowPaiQian(wid);
            KD Sendworker = Get_kd(wid);
            Sendworker.AddPos(GetModelMine,GetMineRun);
            GetModelMine.UpdateMineMessage();
            GetModelMine.UpdateMineState();
            ShowReal();
            ModelManager.GetModelManager.kd_id = wid;
            //GameManager.GetGameManager.Labbly.Get();
            //WokeModel.EventObj.Addlistener(delegate()
            //{
            //    ShowReal();
            //});
        });

        http_send.Get();
        nowppaiwoker_id = wid;
    }

    [SerializeField]
    private HttpModel http_ReCall;
    private void ReCall(string wid)
    {
        http_ReCall.Data.AddData("index",(int.Parse(GetMineRun.minepos.mine_id) - 1).ToString());
        http_ReCall.Data.AddData("id", wid);
        //收回成功更新
        http_ReCall.EventObj.Addlistener(delegate() 
        {
            //GameManager.GetGameManager.Labbly.Get();
            KD Recallworker = Get_kd(wid);      
            Recallworker.RemovePos(GetModelMine);
            GetModelMine.UpdateMineMessage();
            GetModelMine.UpdateMineState();

            ////撤回成功后刷新矿洞信息
            //WokeModel.EventObj.Addlistener(delegate ()
            //{

            //   // ShowPaiQian(wid);
            //    //ShowReal();
            //    //GetMineRun.Close();

            //});
        });



        http_ReCall.Get();
    }

    [SerializeField]
    private Transform YinLiaoWindowBuy, YinLiaoWindowUse;
    [SerializeField]
    private Button AddDrinkButton;
    [SerializeField]
    private Button AddDrinkButtonPQ;
    [SerializeField]
    private Text YinLiao;


    //按钮特效
    [SerializeField]
    private Transform ButtonEffect;

}
