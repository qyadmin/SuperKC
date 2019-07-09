using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;


public class ModelManager : MonoBehaviour {


    public static ModelManager GetModelManager;
    public void Awake()
    {
        GetModelManager = this;
    }

    public string[]  Body_ConfigTag;
    public void LoadConfig()
    {
        //添加需要配置信息的消息模块
        AddConfig();
        HeadIamgeGroup = Resources.LoadAll<Sprite>("headimage");
        HeadSmallIamgeGroup= Resources.LoadAll<Sprite>("headimagesmall");
        allanimation[0].image= Resources.LoadAll<Sprite>("kg1s");
        allanimation[1].image = Resources.LoadAll<Sprite>("kg2s");
        allanimation[2].image = Resources.LoadAll<Sprite>("kg3s");
        allanimation[3].image = Resources.LoadAll<Sprite>("kg4s");
    }

    //主体消息模块
    public MessageEvent<JsonData> ServerBody = new MessageEvent<JsonData>();

    public Dictionary<string, MessageEvent<JsonData>> ConfigBody = new Dictionary<string, MessageEvent<JsonData>>();

    public MessageEvent<JsonData> GetBody(string name)
    {
        MessageEvent<JsonData> obj=null;
        ConfigBody.TryGetValue(name,out obj );
        return obj;
    }

    public void AddConfig()
    {
        foreach (string child in Body_ConfigTag)
        {
            ConfigTag config = new ConfigTag();
            config.Addself(child, ConfigBody);
        }
    }

    public void SendMessage(JsonData Data)
    {
        //主体消息分发
        ServerBody.Send(Data);
    }


    //道具颜色组件配置
    public Sprite[] ColorGroup;

    //头像配置信息
    [SerializeField]
    private Sprite[] HeadIamgeGroup;
    [SerializeField]
    private Sprite[] HeadSmallIamgeGroup;

    public void SetIamge(Image headiamge,int nub)
    {
        headiamge.sprite = HeadIamgeGroup[nub-1];
    }

    public void SetSmallIamge(Image headiamge, int nub)
    {
        headiamge.sprite = HeadSmallIamgeGroup[nub-1];
    }

    //当前加载装备的矿工id;
    public string kd_id="-1";
    //当前加载装备的位置信息;
    public string position_id = "-1";
    //记录当前是否为否买状态
    public bool IsBuy = false;
    public void CloseBuy()
    {
        IsBuy = false;
    }

    public MessageEvent<bool> SyncState = new MessageEvent<bool>();

    public void Update()
    {
        SyncState.Send(IsBuy);
    }


    //道具图标配置
    public Sprite[] PropSprite;

    [System.Serializable]
    public class ImageMessage
    {
        public Sprite[] all;
    }

    public ImageMessage[] BodyIamgeProp;

    public void SetImageProp(Image GetImage,string id,string lvl)
    {
        int nub = GetCL.GetColorNub(lvl);
        GetImage.sprite=BodyIamgeProp[nub].all[int.Parse(id)-1];
    }

    public void SetImagePropColor(Image GetImage, string id, string color)
    {
        GetImage.sprite = BodyIamgeProp[int.Parse(color)-1].all[int.Parse(id) - 1];
    }


    public Color[] configcolor;
    //颜色信息配置
    public Color GetColor(string colorvalue)
    {
      return   configcolor[int.Parse(colorvalue) - 1];
    }

    //矿工动画配置
    [System.Serializable]
    public class spriteBody
    {
        public Sprite[] image;
    }

    public spriteBody[] allanimation=new spriteBody[4];

    public void getAnimation(Mine_Pos GetMinPos)
    {
        int nub = GetCL.GetColorNub((int.Parse(GetMinPos.lvl)-1).ToString());
        GetMinPos.Worker.GetComponent<Effect>().SetSprite(allanimation[nub].image);
    }
}
