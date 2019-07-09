using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using LitJson;
public class GameManager : MonoBehaviour {

    public static GameManager GetGameManager;

    public void Awake()
    {
        GetGameManager = this;
    }

    [SerializeField]
    private Transform WindownBody;
    [SerializeField]
    private Transform MineBody;
    [SerializeField]
    private Model_Prop StartModeProp;
    public Model_Prop GetModel_Prop { get { return StartModeProp; } }
    [SerializeField]
    private Model_RenWu StartModeRenWu;
    [SerializeField]
    private Model_Mine StartModelMine;
    public Model_Mine GetModeMine { get { return StartModelMine; } }

    public UnityEvent ActionEvent;


    [SerializeField]
    private Transform[] AllWindowBody;
    [SerializeField]
    private Transform[] NoEffectWindowBody;

    private List<Transform> NoEffectObj = new List<Transform>();

    private void Start()
    {
        //配置游戏信息
        ModelManager.GetModelManager.LoadConfig();

        foreach (Transform child in WindownBody)
        {
            child.localScale = new Vector3(1, 1, 1);
            child.gameObject.SetActive(false);
            //AllWindown.Add(child,child.localPosition);
            // child.transform.localPosition = new Vector3(0, 100000, 0);
        }
        foreach (Transform child in AllWindowBody)
        {
            child.localScale = new Vector3(1, 1, 1);
            child.gameObject.SetActive(false);
            // AllWindown.Add(child, child.localPosition);
            // child.transform.localPosition = new Vector3(0, 100000, 0);
        }
        foreach (Transform child in NoEffectWindowBody)
        {
            child.localScale = new Vector3(1, 1, 1);
            child.gameObject.SetActive(false);
            NoEffectObj.Add(child);
        }

        StartModeProp.Add();
        StartModeRenWu.Add();
        StartModelMine.Add();

        ActionEvent.Invoke();

        Invoke("callback", 5f);

        //初始化刷新数据
		LoadData();

    }

	public bool Islabby, IsShop, Isreqaddpower;

	public void LoadData()
	{
		Labbly.EventObj.Addlistener(delegate ()
			{
				Islabby=true;
				Shop.EventObj.Addlistener(delegate ()
					{
						IsShop=true;
						reqaddpower.EventObj.Addlistener(delegate ()
							{
								Isreqaddpower=true;
							});			
						reqaddpower.Get();
					});				
				Shop.Get();
			});
		Labbly.Get();
	}

    public List<Transform> ClearEffect = new List<Transform>();

    //储存窗口坐标信息
    Dictionary<Transform, Vector3> AllWindown = new Dictionary<Transform, Vector3>();
    //获取当前显示的窗口坐标信息
    public void GetWindown(Transform key)
    {
        AddRffectNub(key);
            key.gameObject.SetActive(true);
        //return;
        //}
        //IsMove = true;
        //key.GetComponent<Image>().color = new Color(0, 0, 0, 0);
        //key.gameObject.SetActive(true);
        //StartCoroutine(Up(key));
    }

    public void CloseWindown(Transform key)
    {
        //if (NoEffectObj.Contains(key))
        //{    
        RemoveEffectNub(key);
        key.gameObject.SetActive(false);
        // return;
        //}
        //key.GetComponent<Image>().color = new Color(0, 0, 0, 0);
        //StartCoroutine(Dwon(key));
    }

    public void AddRffectNub(Transform key)
    {       
        if (!NoEffectObj.Contains(key))
        {
            ClearEffect.Add(key);
        }
        if (ClearEffect.Count > 0)
            StartModelMine.UpdateEffect(false);
    }

    public void RemoveEffectNub(Transform key)
    {
        if(ClearEffect.Contains(key))
           ClearEffect.Remove(key);
        if (ClearEffect.Count == 0)
            StartModelMine.UpdateEffect(true);
    }

    IEnumerator Up(Transform Obj)
    {
        //IsMove = true;
        float addnub = 0.5f;
        while (addnub < 1.0f)
        {
            Obj.localScale = new Vector3(addnub, addnub, 1);
            yield return new WaitForSeconds(0.03f);
            addnub += 0.1f / (1 - addnub);
        }
        Obj.localScale = new Vector3(1, 1, 1);
        Obj.GetComponent<Image>().color = new Color(0, 0, 0, 0.6f);
        //IsMove = false;
    }

    IEnumerator Dwon(Transform Obj)
    {
        float addnub = 1.0f;
        while (addnub > 0.5f)
        {
            Obj.localScale = new Vector3(addnub, addnub, 1);
            yield return new WaitForSeconds(0.03f);
            addnub -= 0.1f / (addnub);
        }
        Obj.localScale = new Vector3(0.5f, 0.5f, 1);
        Obj.gameObject.SetActive(false);
        //IsMove = false;
    }

    //当前偷去的好友ID;
    public string fuid;
    public bool IsFriend = false;

    //好友模式需要隐藏的按钮
    public Transform[] Hide;
    //好友模式需要显示按钮
    public Transform[] ShowInFriend;
    //好友模式需要隐藏的窗口
    public Transform[] Hidewindow;
    //获取矿场位置信息隐藏
    [SerializeField]
    private Transform kdbody;
    [SerializeField]
    private Transform BackMySelf;


    public void SetState(bool isget)
    {
        IsFriend = isget;
        if (isget)
        {
            BackMySelf.gameObject.SetActive(true);
            foreach (Transform child in Hide)
                child.localScale = new Vector3(0, 0, 0);
            foreach (Transform child in kdbody)
                child.localScale = new Vector3(0, 0, 0);
            foreach (Transform child in Hidewindow)
                CloseWindown(child);
            foreach (Transform child in ShowInFriend)
                child.localScale = new Vector3(1, 1, 1);
            CloseWindown(MineBody);
        }
        else
        {
            BackMySelf.gameObject.SetActive(false);
            foreach (Transform child in Hide)
                child.localScale = new Vector3(1, 1, 1);
            foreach (Transform child in kdbody)
                child.localScale = new Vector3(1, 1, 1);
            foreach (Transform child in ShowInFriend)
                child.localScale = new Vector3(0, 0, 0);
        }
    }

    [System.Serializable]
    public class CatchDataMessage
    {
        public string name;
        public HttpModel datatmodel;
    }

    //public List<CatchDataMessage> alldatat = new List<CatchDataMessage>();

    //public bool IsMove = false;

    //public void CatchData(string DebugData)
    //{
    //    if (alldatat.Count <= 0)
    //        return;
    //    Debug.Log(DebugData + "GAMMANEG");
    //    //try
    //    //{
    //    //JsonData jdd = JsonMapper.ToObject(DebugData);
    //    //if (jdd.Keys.Contains("data"))
    //    //{
    //    //    JsonData jd = jdd["data"];
    //    //    foreach (CatchDataMessage child in alldatat)
    //    //    {
    //    //        if (jd.Keys.Contains(child.name))
    //    //        {
    //    //            child.datatmodel.DebugAction(DebugData);
    //    //        }
    //    //    }
    //    //}
    //    //}
    //    //catch
    //    //{
    //    //    Debug.Log("捕获错误");
    //    //}

    //}

    public HttpModel User;
    public HttpModel Labbly;
    public HttpModel Shop;
    public HttpModel reqaddpower;
    //升级解锁
    public void Levle_UnLock(int nub)
    {
       
        if (nub % 5 != 1 || nub == 1)
        {
            MessageManager._Instantiate.Show("升级成功！");
            return;
        }
        string getvalue = ((nub - 1) / 5).ToString();
        switch (getvalue)
        {
            case "1":
                MessageManager._Instantiate.Show("升级成功！恭喜你，你已解锁黄色道具", 1);
                break;
            case "2":
                MessageManager._Instantiate.Show("升级成功！恭喜你，你已解锁绿色道具", 1);
                break;
            case "3":
                MessageManager._Instantiate.Show("升级成功！恭喜你，你已解锁红色道具", 1);
                break;
        }
        //User.EventObj.Addlistener(delegate ()
        //{
        //Shop.Get();
        //int maxlvl = int.Parse(Static.Instance.GetValue("lvl"));
        //if (nub>maxlvl)
        //UpdateMaxLvl(nub.ToString());
        //});
        //User.Get();
    }


    //特效切换指令
    public MessageEvent<Effect> SyncEffectStatus = new MessageEvent<Effect>();

    public void SyncEffect(Effect tag)
    {
        SyncEffectStatus.Send(tag);
    }

    public List<GameObject> RecallBody = new List<GameObject>();
    public void AddItem(GameObject item)
    {
        RecallBody.Add(item);
    }
    void callback()
    {
        if (RecallBody.Count > 20)
        {
            GameObject.Instantiate(RecallBody[0]);
            RecallBody.RemoveAt(0);
        }
    }

    public MessageEvent<string> LVL_Event =new MessageEvent<string>();
    public void UpdateMaxLvl(string data)
    {
        Static.Instance.AddValue("lvl",data);
        LVL_Event.Send(data);
    }

    public void SyncLvl()
    {
        LVL_Event.Send(Static.Instance.GetValue("lvl"));
    }


	public void CheckUserInfo()
	{
		if (!Islabby||!IsShop||!Isreqaddpower) 
		{
			MessageManager._Instantiate.WindowShoMessage ("用户数据加载失败，请重尝试", LoadData, false);
		}
	}
}
