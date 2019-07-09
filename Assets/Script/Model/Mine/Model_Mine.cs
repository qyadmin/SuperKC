using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;

public class Mine_Pos
{
    public Button PaiQianButton;
    public GameObject Worker;
    public GameObject Effect;
    public GameObject PaiQian;
    public string mine_id;
    public string pos_id;
    public bool State = false;
    public string wid = "-1";
    public string lvl;

    public Mine_Pos()
    {

    }


    public void Getmineid(string id)
    {
        mine_id = (int.Parse(id)+1).ToString();
    }

    public void Sethave(string wid,string lvl,string power)
    {
        this.wid = wid;
        this.lvl = lvl;
        Worker.SetActive(true);
        PaiQian.SetActive(false);
        ModelManager.GetModelManager.getAnimation(this);
        State = true;
        if (power == "0")
            Worker.GetComponent<Effect>().StopLoop();
        else
            Worker.GetComponent<Effect>().StartLoop();
    }

    public void Clear()
    {
        Worker.SetActive(false);
        PaiQian.SetActive(true);
        State = false;
        wid = "-1";
}
}


//气泡组件模块
public class BoundsTranforms
{
    public Sprite BoundsIamgeJin;
    public Sprite BoundsIamgeYin;
    public Button GetButton;
    public ParticleSystem Eft;
    public ParticleSystem ReqIcon;
    public AudioSource sound;
    //显示金色或者银色
    public void SetState(string Value)
    {
        if (float.Parse(Value) < 1.0f)
            GetButton.GetComponentInChildren<Image>().sprite = BoundsIamgeYin;
        else
            GetButton.GetComponentInChildren<Image>().sprite = BoundsIamgeJin;
        GetButton.GetComponentInChildren<Text>().text = Value;
    }
}

//气泡模块
public class BoundsMessage
{
    public string bid;
    public string value;

    //绑定到网络模块并显示气泡
    public void SetState(List<BoundsTranforms> BoundsButton,HttpModel mc, BoundsTranforms ButtonTrom, string index)
    {
        ButtonTrom.SetState(value);
        ButtonTrom.GetButton.onClick.RemoveAllListeners();
        ButtonTrom.GetButton.onClick.AddListener(delegate ()
        {
            if (GameManager.GetGameManager.IsFriend)
                mc.Data.AddData("fuid", GameManager.GetGameManager.fuid);
            else
                mc.Data.RemoveData("fuid");
            mc.EventObj.Addlistener(delegate ()
            {
                BoundsButton.Remove(ButtonTrom);
                BoundsButton.Add(ButtonTrom);
                Debug.Log(BoundsButton.Count+"*****");
            });
            mc.Data.AddData("id", bid);

            GameObject EFTTIocn = GameObject.Instantiate(ButtonTrom.ReqIcon.gameObject);
            EFTTIocn.transform.SetParent(ButtonTrom.GetButton.transform.parent);
            EFTTIocn.transform.localPosition = ButtonTrom.GetButton.transform.localPosition + new Vector3(0, 0, -5000);
            EFTTIocn.transform.localScale = new Vector3(1, 1, 1);
            EFTTIocn.SetActive(true);
            EFTTIocn.name = ButtonTrom.ReqIcon.gameObject.name;
            EFTTIocn.GetComponent<ParticleSystem>().Play();
            EFTTIocn.GetComponent<AudioSource>().Play();
            //加入回收队列
            GameManager.GetGameManager.AddItem(EFTTIocn);

            mc.EventObj.Addlistener(delegate ()
            {
                //GameObject EFTT = ObjectPool.GetInstance().GetObj(ButtonTrom.Eft.gameObject);
                //EFTT.transform.SetParent(ButtonTrom.GetButton.transform.parent);
                //EFTT.transform.localPosition = ButtonTrom.GetButton.transform.localPosition + new Vector3(0, 0, -5000);
                //EFTT.transform.localScale = new Vector3(1, 1, 1);
                //EFTT.name = ButtonTrom.Eft.gameObject.name;
                //EFTT.GetComponent<ParticleSystem>().Play();
                ButtonTrom.sound.Play();
                //加入回收队列
               // GameManager.GetGameManager.AddItem(EFTT);
            });
            mc.Get();
        });
    }

    //储存气泡信息
    public void SetData(string bounds,string id)
    {
        this.value = (float.Parse(bounds) / 1000).ToString();
        this.bid = id;
    }
}

public class WK
{
    public GameObject LockObj;
    public Button Button_Enter;
    public string mine_id = "-1";
    public bool State = false;
    public Effect LockEffect;

    public void SetOpen()
    {
        State = true;
        LockObj.SetActive(false);
    }

    public void SetClose()
    {
        State = false;
        LockObj.SetActive(true);
    }

    public class Pos_worker
    {
        public string wid;
        public string pos;
        public string lvl;
        public string power;
    }

    private List<Pos_worker> Savepos = new List<Pos_worker>();

    public void AddPos(string Getwid,string posid,string lvl,string power)
    {
        Pos_worker wp = new Pos_worker();
        wp.wid = Getwid;
        wp.pos = posid;
        wp.lvl = lvl;
        wp.power = power;
        Pos_worker reitem = null;
        foreach (Pos_worker child in Savepos)
        {
            if (child.wid == wp.wid)
            {
                reitem = child;
            }
        }
        if (reitem != null)
        {
            Savepos.Remove(reitem);
        }
        Savepos.Add(wp);
    }

    public void RemovePos(string Getwid)
    {
        Pos_worker reitem = null;
        foreach (Pos_worker child in Savepos)
        {
            if (child.wid == Getwid)
            {
                reitem = child;
               // break;
            }
        }
        if (reitem != null)
        {
            Savepos.Remove(reitem);
        }

    }

    public void ClearMessage()
    {
        Savepos.Clear();
        
    }

    public void ResatePOS(List<Mine_Pos> getall)
    {
        foreach (Mine_Pos child in getall)
            child.Clear();
        foreach (Pos_worker child in Savepos)
        {
            getall[int.Parse(child.pos)].Sethave(child.wid,child.lvl,child.power);               
        }
    }

    //MC容器
    public List<BoundsMessage> AllBounds = new List<BoundsMessage>();

    //添加气泡
    public void AddBounds(string bounds,string bid)
    {
        BoundsMessage obj = new BoundsMessage();
            obj.SetData(bounds, bid);
        AllBounds.Add(obj);
    }

    //显示气泡
    public void ShowBounds(List<BoundsTranforms> BoundsButton,HttpModel mc)
    {
        //重置气泡组件
        foreach (BoundsTranforms child in BoundsButton)
        {
            child.GetButton.transform.localScale = Vector3.zero;
            child.GetButton.GetComponentInChildren<Text>().text = "0";
        }
        if (AllBounds.Count == 0)
            return;  
        for (int i=0; i< AllBounds.Count;i++)
        {
            //配置气泡信息属性
            AllBounds[i].SetState(BoundsButton, mc, BoundsButton[i], mine_id);
            BoundsButton[i].GetButton.transform.localScale = new Vector3(1, 1, 1);
        }
    }
}

public class Model_Mine : MonoBehaviour {
    //矿洞
    [SerializeField]
    private Transform kd_obj;
    [SerializeField]
    private List<WK> kd_body = new List<WK>();
    [SerializeField]
    private HttpModel UnLockmine;

    //从此组件下取得矿洞组件
    [SerializeField]
    private Transform minePos_body;
    //矿洞所有位置
    [SerializeField]
    private List<Mine_Pos> allPos = new List<Mine_Pos>();

    //初始化矿工数据，并且绑定
    public void Add()
    {
        int i = 0;
        foreach (Transform child in kd_obj)
        {
            WK mine = new WK();
            mine.LockObj = child.GetChild(0).gameObject;
            mine.Button_Enter = child.GetComponent<Button>();
            mine.mine_id = (i + 1).ToString();
            mine.LockEffect = child.GetComponentInChildren<Effect>();
            mine.Button_Enter.onClick.AddListener(delegate ()
            {
                UnLock(mine);

            });
            kd_body.Add(mine);
            i++;
        }

        int j = 0;
        foreach (Transform child in minePos_body)
        {
            Mine_Pos minepos = new Mine_Pos();
            minepos.Effect = child.GetChild(0).gameObject;
            minepos.PaiQian = child.GetChild(1).gameObject;
            minepos.Worker = child.GetChild(2).gameObject;
            minepos.Worker.SetActive(false);
            minepos.PaiQianButton = child.GetComponent<Button>();
            minepos.pos_id = j.ToString();
            minepos.PaiQianButton.onClick.AddListener(delegate ()
            {
                ModelManager.GetModelManager.kd_id = minepos.wid;
                Send(minepos);
                WorkerBody.ShowPaiQian(ModelManager.GetModelManager.kd_id);

            });
            SyncID.Addlistener(minepos.Getmineid);
            allPos.Add(minepos);
            j++;
        }

        int k = 0;
        foreach (Transform child in BodyBoumds)
        {
            BoundsTranforms obj = new BoundsTranforms();
            obj.BoundsIamgeJin = Jin;
            obj.BoundsIamgeYin = Yin;
            obj.GetButton = child.GetComponent<Button>();
            obj.Eft = BoundsEft;
            obj.sound = BoundsSound;
            obj.ReqIcon = IconEft;
            child.localScale = new Vector3(0, 0, 0);
            AllBoundsBody.Add(obj);
            k++;
        }
    }


    //解锁矿场状态
    public void SetData(JsonData jd)
    {
        Debug.Log(JsonMapper.ToJson(jd));
        JsonData GetData = jd["mine"];
        if (GetData == null)
            return;
        foreach (WK child in kd_body)
            child.SetClose();

        foreach (JsonData child in GetData)
        {
            //解锁矿洞
            kd_body[int.Parse(child["id"].ToString()) - 1].SetOpen();
        }
    }


    [SerializeField]
    private Transform MineObj;

    //进入矿场
    public void Enter(WK item)
    {
        GameManager.GetGameManager.GetWindown(MineObj);

        //初始化矿洞位置
        nowmine = item;
        UpdateMineState();
    }

    //记录当前矿洞信息，撤销成功后调用刷新
    private WK nowmine;
    public void UpdateMineState()
    {
        if (nowmine != null)
            nowmine.ResatePOS(allPos);
    }

    public void playUnLockEffect()
    {
        nowmine.LockEffect.Play();
    }


    private MessageEvent<string> SyncID = new MessageEvent<string>();

    //刷新算力
    public void UpdateMineMessage()
    {
        if (nowmine == null)
            return;
        Http_UpdateMine.Data.AddData("mine", nowmine.mine_id);
        Http_UpdateMine.Get();
    }

    [SerializeField]
    private HttpModel HttpMinePiece;

    //进入矿场
    public void UnLock(WK item)
    {
        if (item.State)
        {
            //同步当前空洞位置的归属
            if (SyncID != null)
                SyncID.Send(item.mine_id);
            Enter(item);
            if (GameManager.GetGameManager.IsFriend)
                Http_UpdateMine.Data.AddData("fuid", GameManager.GetGameManager.fuid);
            else
                Http_UpdateMine.Data.RemoveData("fuid");

            Http_UpdateMine.Data.AddData("mine", item.mine_id);
            Http_UpdateMine.Get();
            //刷新MC
            SendBound();

        }
        else
        {
            if (!GameManager.GetGameManager.IsFriend)
            {
                HttpMinePiece.Data.AddData("index", item.mine_id);
                HttpMinePiece.Get();
                BuMineButton.onClick.RemoveAllListeners();
                BuMineButton.onClick.AddListener(delegate ()
                {
                    UnLockmine.Data.AddData("index", item.mine_id);
                    UnLockmine.Get();
                });

            }
        }
    }

    [SerializeField]
    private Transform SureBuyMine;
    [SerializeField]
    private Button BuMineButton;

    //矿工信息传入接口
    [SerializeField]
    private Woker_Message WorkerBody;

    //派遣界面
    [SerializeField]
    private Transform paiqian;
    //派遣并且传入当前pos_id
    public void Send(Mine_Pos minepos)
    {
        GameManager.GetGameManager.GetWindown(paiqian);
        //显示派遣界面
        paiqian.GetComponent<Model_RunMine>().SetState(minepos);
    }


    //清除所有位置信息***矿工生成是调用
    public void ClearPos()
    {
        //清除矿洞储存的位置信息
        foreach (WK child in kd_body)
            child.ClearMessage();
        ClearTranmson();
    }

    [SerializeField]
    private HttpModel Labbly;
    public void ClearTranmson()
    {
        foreach (Mine_Pos child in allPos)
        {
            child.Clear();
        }
    }

    [SerializeField]
    private ParticleSystem[] alleft;
    //切换特效
    public void UpdateEffect(bool status)
    {
        foreach (Mine_Pos child in allPos)
        {
            child.Effect.SetActive(status);
        }
        foreach (ParticleSystem child in alleft)
        {
            child.gameObject.SetActive(status);
        }
    }


    //向矿洞中添加矿工信息
    public void AddPos(string wid, string getmineid, string getposid, string getlvl,string power)
    {
        kd_body[int.Parse(getmineid) - 1].AddPos(wid, getposid, getlvl, power);
    }
    //向矿洞移除矿工信息
    public void RemovwPos(string wid, string getmineid)
    {
        if (kd_body.Count == 0||getmineid=="0")
            return;
        kd_body[int.Parse(getmineid) - 1].RemovePos(wid);
    }

    //向矿洞移除矿工信息
    public void RemovwPos(string wid)
    {
        if (kd_body.Count == 0)
            return;
        if(nowmine!=null)
       nowmine.RemovePos(wid);
    }

    //mc气泡组件
    [SerializeField]
    private Transform BodyBoumds;
    private List<BoundsTranforms> AllBoundsBody = new List<BoundsTranforms>();
    [SerializeField]
    private ParticleSystem BoundsEft;
    [SerializeField]
    private ParticleSystem IconEft;
    [SerializeField]
    private AudioSource BoundsSound;
    public Sprite Jin, Yin;
    public HttpModel Http_mc;
    public HttpModel Http_mc_Friend;


    [SerializeField]
    private Text power,cal;
    //获取矿场数据成功
    public void SetDataUpdate(JsonData jd)
    {
        JsonData GetData = jd["mine"];
        if (GetData == null)
            return;
        power.text = GetData["totalcal"].ToString();
        cal.text = GetData["alltotalcal"].ToString();

    }

    //更新矿洞信息
    [SerializeField]
    private HttpModel Http_UpdateMine;

    [SerializeField]
    private HttpModel Http_UpdateBounds;


    //获取MC
    public void SendBound()
    {
        if (GameManager.GetGameManager.IsFriend)
            Http_UpdateBounds.Data.AddData("fuid", GameManager.GetGameManager.fuid);
        else
            Http_UpdateBounds.Data.RemoveData("fuid");
        Http_UpdateBounds.Data.AddData("mine",nowmine.mine_id);
        Http_UpdateBounds.Get();
    }


    //刷新MC
    public void SetBoundsUpdate(JsonData jd)
    {
        JsonData GetData = jd["minebonus"];
        if (GetData == null)
            return;
        nowmine.AllBounds.Clear();
        foreach (JsonData child in GetData)
        {
            nowmine.AddBounds(child["mc"].ToString(), child["key"].ToString());
        }
        //显示气泡
        if (GameManager.GetGameManager.IsFriend)
            nowmine.ShowBounds(AllBoundsBody, Http_mc_Friend);
        else
            nowmine.ShowBounds(AllBoundsBody, Http_mc);
    }


    //刷新派遣界面
    [SerializeField]
    private GameObject worker, noworker;
    public void updateRecall()
    {
       noworker.SetActive(true);
        worker.SetActive(false);
    }


    public void ClearBonusTranforms()
    {
        //重置气泡组件
        foreach (BoundsTranforms I in AllBoundsBody)
        {
            I.GetButton.transform.localScale = Vector3.zero;
        }
    }
}
