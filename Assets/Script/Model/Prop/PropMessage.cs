using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PropMessage : MonoBehaviour {

    [SerializeField]
    private Button UnLaod;
    [SerializeField]
    private HttpModel http_unload;

    [SerializeField]
    private Button Laod;

    [SerializeField]
    private Button ChongDian;

    [SerializeField]
    private HttpModel http_chongdian;

    [SerializeField]
    private Text dl;

    [SerializeField]
    private GameObject obj;

    [SerializeField]
    private Transform Pos;

    [SerializeField]
    private Transform ActionObj;

    [SerializeField]
    private Image Icon;


    public string pid;
    public string wid;
    public string pos;

    public bool IsHave { get { return obj.activeSelf; } }

    //记录当前自己的位置信息
    public int postionid;
    private Model_Prop Prop;
    private void Start()
    {
        Laod.onClick.AddListener(LoadAction);      
        ChongDian.onClick.AddListener(ChongDianAction);
        obj.GetComponent<Button>().onClick.AddListener(ShowObj);
        Prop = GameManager.GetGameManager.GetModel_Prop;
    }

    private void ShowObj()
    {
        UnLaod.onClick.RemoveAllListeners();
        if (ActionObj.localScale == Vector3.zero)
        {
            UnLaod.onClick.AddListener(UnLoadAction);
            ActionObj.position = Pos.position;
            ActionObj.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            UnLaod.onClick.RemoveListener(UnLoadAction);
            ActionObj.localScale = Vector3.zero;
        }
        
        
    }

    //传送当前道具购买状态和道具位置信息
    private void LoadAction()
    {
        ModelManager.GetModelManager.IsBuy = true;
        ModelManager.GetModelManager.position_id = (postionid+1).ToString();
        ActionObj.localScale = Vector3.zero;
    }

    //向网络模块中添加参数
    private void UnLoadAction()
    {
        http_unload.Data.AddData("pid", pid);
        http_unload.Data.AddData("wid", wid);
        http_unload.Data.AddData("pos", pos);
        http_unload.EventObj.Addlistener(delegate()
        {
            //卸载成功刷新状态
            Prop.UpdateUnLoadProp(this);

            GameManager.GetGameManager.GetModeMine.UpdateMineMessage();
        });
        http_unload.Get();
        ActionObj.localScale = Vector3.zero;
    }

    //向网路模块添加参数
    private void ChongDianAction()
    {
        http_unload.Data.AddData("pid", pid);
        http_chongdian.Get();
    }

    //获取当前道具框属性信息
    public void SetData(PropData GetProp)
    {
        obj.SetActive(true);
        this.pid = GetProp.pid;
        this.wid = GetProp.wid;
        this.pos = GetProp.postionid;
        ModelManager.GetModelManager.SetImagePropColor(Icon, GetProp.id, GetProp.color);
    }

    //刷新当前属性道具框状态
    public void ReStart()
    {
        obj.SetActive(false);
    }
}


