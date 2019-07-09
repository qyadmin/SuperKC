using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;


public class Model_Shop : MonoBehaviour
{

    [SerializeField]
    private Text ShowWorker;
    [SerializeField]
    private GameObject listworker, listdj;
    [SerializeField]
    private Transform FatherObjworker, FatherObjdj;

    Coroutine worker, prop;
    private Dictionary<string, GameObject> AllShopWorker = new Dictionary<string, GameObject>();

    [SerializeField]
    private GameObject obj;
    private GameObject GetTramform_Worker(string id)
    {
        GameObject obj = null;
        AllShopWorker.TryGetValue(id,out obj);
        return obj;
    }

    public void SetDataDJ(JsonData jd)
    {
        //if(worker!=null)
        //StopCoroutine(worker);
        // worker= StartCoroutine(SetDj(jd["prop"]));
        SetDj(jd["prop"]);
    }

    public void SetDataWorker(JsonData jd)
    {
        //if (prop!= null)
        //    StopCoroutine(prop);
        //prop = StartCoroutine(SetWorker(jd["worker"]));
        ShowDef();
        SetWorker(jd["worker"]);     
    }
		
   
    void SetWorker(JsonData GetDataWorker)
    {
        AllShopWorker.Clear();
        foreach (Transform child in FatherObjworker)
        {
           Destroy(child.gameObject);
        }

        if (GetDataWorker != null)
        {
            foreach (JsonData child in GetDataWorker)
            {
                string id = child["id"].ToString();
                GameObject NewList = GameObject.Instantiate(listworker);
                NewList.transform.SetParent(FatherObjworker);
                NewList.transform.localScale = new Vector3(1, 1, 1);
                NewList.SetActive(true);
                NewList.transform.localPosition = new Vector3(NewList.transform.localPosition.x, NewList.transform.localPosition.y, 0);
                NewList.name = listworker.name;
                Tramform_Worker TramformWorker = NewList.GetComponent<Tramform_Worker>();
                TramformWorker.SuanLi.text = child["cal"].ToString();
                TramformWorker.Nmae.text = child["name"].ToString();
                TramformWorker.Message.text = child["desc"].ToString();
                TramformWorker.Icon.text = child["buy"].ToString();
                ModelManager.GetModelManager.SetIamge(TramformWorker.HeadImage, int.Parse(id));
                TramformWorker.Buy_Button.onClick.RemoveAllListeners();
                TramformWorker.Buy_Button.onClick.AddListener(delegate ()
                {
                    ShowWorker.text = "你确定要花费" + child["buy"].ToString() + "购买矿工吗吗？";
                    BuyWorker(id);
                });
                //if (AllShopWorker.ContainsKey(id))
                // AllShopWorker.Remove(id);
                AllShopWorker.Add(id, NewList);
                // yield return 10;
            }
        }
        if (AllShopWorker.Count <= 0)
            obj.SetActive(true);
        else
            obj.SetActive(false);
        // yield return null;
    }

    [SerializeField]
    private HttpModel httpwokrer;
    [SerializeField]
    private Button SendButtonworker;
         
    private void BuyWorker(string ID )
    {
        SendButtonworker.onClick.RemoveAllListeners();
        httpwokrer.Data.AddData("id", ID);       
        SendButtonworker.onClick.AddListener(httpwokrer.Get);
    }


    //购买矿工后消除已经购买的矿工列表
    public void DisWorker(JsonData GetDataWorker)
    { 
        JsonData jd = GetDataWorker["worker"];
        if (jd == null)
            return;
        GameObject obj = GetTramform_Worker(jd["id"].ToString());
        if (obj != null)
        {
            Destroy(obj);
            AllShopWorker.Remove(jd["id"].ToString());
        }
        else
            Debug.Log("对相丢失！");
        if(AllShopWorker.Count<=0)
            obj.SetActive(true);
        else
            obj.SetActive(false);
    }


    void SetDj(JsonData GetDatadj)
    {
        foreach (Transform child in FatherObjdj)
        {
           Destroy(child.gameObject);
        }
        string maxlvl =Static.Instance.GetValue("lvl");
        if (GetDatadj != null)
        {
            foreach (JsonData child in GetDatadj)
            {
                GameObject NewList = GameObject.Instantiate(listdj);
                NewList.transform.SetParent(FatherObjdj);
                NewList.transform.localPosition -= new Vector3(0, 0, NewList.transform.localPosition.z);
                NewList.transform.localScale = new Vector3(1, 1, 1);
                NewList.SetActive(true);
                NewList.name = listdj.name;
                Tramform_Dj Tramformdj = NewList.GetComponent<Tramform_Dj>();
                Tramformdj.SuanLi.text = child["cal"].ToString();
                Tramformdj.Nmae.text = child["name"].ToString();
                Tramformdj.Message.text = child["desc"].ToString().Replace("<s>", Tramformdj.Nmae.text).Replace("s", Tramformdj.SuanLi.text); ;
                Tramformdj.Icon.text = child["buy"].ToString();
                string id = child["id"].ToString();
                string lvl = child["lvl"].ToString();
                ModelManager.GetModelManager.SetImageProp(Tramformdj.HeadImage, id, child["lvl"].ToString());
                Tramformdj.lvl = lvl;
                Tramformdj.Add();
                //Tramformdj.Mrak.SetActive(GetCL.Compare(lvl,maxlvl));
                Tramformdj.Buy_Button.onClick.RemoveAllListeners();
                Tramformdj.Buy_Button.onClick.AddListener(delegate ()
                {                  
                    ShowDJ.text = "你确定要花费" + child["buy"].ToString() + "购买道具吗？";
                    BuyProp(id, lvl);
                    Debug.Log("添加了一次购买");
                });
            
            }
        }

        GameManager.GetGameManager.SyncLvl();
    }


    [SerializeField]
    private Text ShowDJ;
    [SerializeField]
    private HttpModel httpprop;
    [SerializeField]
    private Button SendButtonprop;
    [TextArea]
    public string Message;
    private void BuyProp(string id,string lvl)
    {
        httpprop.Data.AddData("id",id);
        httpprop.Data.AddData("lvl", lvl);
        SendButtonprop.onClick.RemoveAllListeners();
        SendButtonprop.onClick.AddListener(httpprop.Get);
    }

    [SerializeField]
    private Transform ObjKD;
    [SerializeField]
    private Transform ObjdDJ;
    [SerializeField]
    private Transform ObjdYL;
    [SerializeField]
    private Effect EFTKD;
    //显示默认窗口
    private void ShowDef()
    {
        GameManager.GetGameManager.SyncEffect(EFTKD);
        GameManager.GetGameManager.GetWindown(ObjKD);
        GameManager.GetGameManager.CloseWindown(ObjdDJ);
        GameManager.GetGameManager.CloseWindown(ObjdYL);    
    }

    //[SerializeField]
    //private Button 


    [SerializeField]
    private Text money;
    void SetDrink(JsonData GetDatadj)
    {
        JsonData data = GetDatadj["drink"][0];
        if (data != null)
            money.text = data["price"].ToString();
    }
}
