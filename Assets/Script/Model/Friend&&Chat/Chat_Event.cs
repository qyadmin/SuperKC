using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using LitJson;

public class Chat_Event : MonoBehaviour
{
    public Dictionary<string, List<ChatInfo>> privateChat = new Dictionary<string, List<ChatInfo>>();
    public Dictionary<string, string> privateChatCid = new Dictionary<string, string>();

    List<ChatInfo> allchatList = new List<ChatInfo>();

    List<ChatInfo> worldChatList = new List<ChatInfo>();

    string allchatCid = null;

    string worldchatCid = null;

    [SerializeField]
    HttpModel AllChatList;


    public AddFriendinChat addFriend;

    public class ChatInfo
    {
        public string Name;
        public string Count;
        public string id;
        public string type;
        public string image;
        public string mc;
        public string coin;
    }
    public static Chat_Event Instance;

    [SerializeField]
    public Sprite disable, pressed;

    [System.Serializable]
    public class classification
    {
        public UnityEvent Event;
        public UnityEvent HttpEvent;
        public Button ClickButton;
        public HttpModel ChatHttpEvent;
        public bool isworld = false;

        public void Start()
        {
            ClickButton.onClick.AddListener(delegate ()
            {
                AllEvent();
            }
            );
        }


        public void HttpButton()
        {
            Chat_Event.Instance.SendMessageButton.onClick.RemoveAllListeners();
            Chat_Event.Instance.SendMessageButton.onClick.AddListener(delegate () 
            {
				
                Chat_Event.Instance.SendMessageButton.interactable = false;
					Chat_Event.Instance.OpenButton();
                Chat_Event.Instance.refresh_chat_stop();
                HttpEvent.Invoke();
                Chat_Event.Instance.refresh_chat_start(5);
            });
        }

        
        public void Reset()
        {
            ClickButton.GetComponent<Image>().sprite = Chat_Event.Instance.disable;
        }
        public void Click()
        {
            ClickButton.GetComponent<Image>().sprite = Chat_Event.Instance.pressed;
        }
        public void AllEvent()
        {
            Chat_Event.Instance.refresh_chat_stop();
            Chat_Event.Instance.ClickAnimationReset();
            HttpButton();
            Event.Invoke();
            if (!isworld)
            {
                if (ChatHttpEvent && Chat_Event.Instance.father.childCount != 0)
                    ChatHttpEvent.Get();
            }
            else
                if(ChatHttpEvent)
                ChatHttpEvent.Get();
            Chat_Event.Instance.current_chat = ChatHttpEvent;
            Chat_Event.Instance.refresh_chat_start(5);
            Click();

            if(Chat_Event.Instance.father.childCount == 0 && !isworld)
                Chat_Event.Instance.refresh_chat_stop();
        }

    }
    [SerializeField]
    public Transform father;
    [SerializeField]
    classification[] ClickFuntion;

    [SerializeField]
    Button SendMessageButton;
    [SerializeField]
    InputField MessageCount;
    public void setMessagecountnull()
    {
        MessageCount.text = null;
    }
    // Use this for initialization
    private void Awake()
    {
        Instance = this;
        
    }

	public void Open()
	{
		Chat_Event.Instance.SendMessageButton.interactable = true;
	}

	public void OpenButton()
	{
		Invoke ("Open",0.8f);
	
	}
    private void Start()
    {
		
        foreach (classification i in ClickFuntion)
        {
            i.Start();
            i.Reset();
        }
        ClickFuntion[0].AllEvent();
        closeprivatechat.onClick.AddListener(delegate () { Friend_Event.Instance.RemovePrivateChat(Friend_Event.Instance.clickObj); });
        refresh_chat_stop();
        ALL_chatListClick();
        AllofChatList();
    }
    // Update is called once per frame
    void Update()
    {
     
    }


    public void AllofChatList()
    {
        refresh_chat_stop();
        current_chat = AllChatList;
        current_chat.Get();
        refresh_chat_start(5);
    }

    public void AddAll_ChatDic(JsonData obj)
    {
        if (obj == null)
            return;

        JsonData dataobj = obj["data"];
        JsonData objcid = obj["cid"];

        allchatCid = objcid.ToString();
        Static.Instance.AddValue("allcid", objcid.ToString());
        foreach (JsonData i in dataobj)
        {
            ChatInfo newinfo = new ChatInfo();

            
            newinfo.Name = i["name"].ToString();
            newinfo.Count = i["content"].ToString();
            newinfo.type = i["type"].ToString();
            allchatList.Add(newinfo);
            InstantiateAllChat(newinfo, all_chatlist_father);
        }
        all_chatlist_father.gameObject.SetActive(false);
        all_chatlist_father.gameObject.SetActive(true);
    }


    public void ClickAnimationReset()
    {
        foreach (classification i in ClickFuntion)
        {
            i.Reset();
        }
    }


    public void private_Chat()
    {
        ClickFuntion[1].AllEvent();
    }


    public void Show(GameObject obj)
    {
        obj.SetActive(true);
    }
    public void Hide(GameObject obj)
    {
        obj.SetActive(false);
    }


    [SerializeField]
    chatList chatOther, chatMine;
    [SerializeField]
    Text Allchatlist;
    [SerializeField]
    Transform chatlist_father,allchatlist_father,all_chatlist_father;
    public void AddChatDic(JsonData obj)
    {
        if (obj == null)
            return;

        JsonData dataobj = obj["data"];
        JsonData objcid = obj["cid"];
        
        Debug.Log(JsonMapper.ToJson(dataobj));

        if (privateChatCid == null)
            privateChatCid.Add(Friend_Event.Instance.clickObj.Self.f_id, objcid.ToString());
        else
        {
            string a = null;
            privateChatCid.TryGetValue(Friend_Event.Instance.clickObj.Self.f_id, out a);
            if (a == null)
            {
                privateChatCid.Add(Friend_Event.Instance.clickObj.Self.f_id, objcid.ToString());
            }
            else
            {
                privateChatCid.Remove(Friend_Event.Instance.clickObj.Self.f_id);
                privateChatCid.Add(Friend_Event.Instance.clickObj.Self.f_id, objcid.ToString());
            }
                
        }
        Static.Instance.AddValue("cid", objcid.ToString());

        string abc = null;
        privateChatCid.TryGetValue(Friend_Event.Instance.clickObj.Self.f_id,out abc);

        foreach (JsonData i in dataobj)
        {
            ChatInfo newinfo = new ChatInfo();
            
            newinfo.Name = i["receive_name"].ToString();
            newinfo.Count =i["content"].ToString();
            newinfo.id = i["receive_id"].ToString();
            newinfo.image = i["img"].ToString();

            if (privateChat == null)
            {
                List<ChatInfo> info = new List<ChatInfo>();
                info.Add(newinfo);
                privateChat.Add(Friend_Event.Instance.clickObj.Self.f_id,info);


                InstantiateChat(newinfo.id, Static.Instance.GetValue("uid"),newinfo,chatlist_father);


            }
            else
            {
                List<ChatInfo> a = null;
                privateChat.TryGetValue(Friend_Event.Instance.clickObj.Self.f_id, out a);
                if (a == null)
                {
                    List<ChatInfo> info = new List<ChatInfo>();
                    info.Add(newinfo);
                    privateChat.Add(Friend_Event.Instance.clickObj.Self.f_id, info);

                    InstantiateChat(newinfo.id, Static.Instance.GetValue("uid"), newinfo,chatlist_father);
                }
                else
                {
                    a.Add(newinfo);
                    privateChat.Remove(Friend_Event.Instance.clickObj.Self.f_id);
                    privateChat.Add(Friend_Event.Instance.clickObj.Self.f_id,a);
                    InstantiateChat(newinfo.id, Static.Instance.GetValue("uid"), newinfo,chatlist_father);
                }
            }

            
            
        }

    }
    public void resetPrivateChat(string key)
    {
        removechatlist();
        List<ChatInfo> All = new List<ChatInfo>();
        privateChat.TryGetValue(key,out All);
        if (All == null)
        return;
        for (int i = 0;i< All.Count;i++)
        {
            if (i>(All.Count - 35))
            InstantiateChat(All[i].id,Static.Instance.GetValue("uid"),All[i],chatlist_father);
        }
    }

    public void removechatlist()
    {
        for (int i = 0; i < chatlist_father.transform.childCount; i++)
        {
            Destroy(chatlist_father.GetChild(i).gameObject);
        }
    }

    public void removeall_chatlist()
    {
        for (int i = 0; i < all_chatlist_father.transform.childCount; i++)
        {
            Destroy(all_chatlist_father.GetChild(i).gameObject);
        }
    }

    public string getCid(string key)
    {
        string cid = null;
        privateChatCid.TryGetValue(key, out cid);
        if (cid == null)
            return "0";
        else
            return cid;
    }

    public string getallchatCid()
    {
        if (worldchatCid == null)
            return "0";
        else
            return worldchatCid;
    }

    public string getall_chatCid()
    {
        if (allchatCid == null)
            return "0";
        else
            return allchatCid;
    }

    public void AddAllChatDic(JsonData obj)
    {
        if (obj == null)
            return;

        JsonData dataobj = obj["data"];
        JsonData objcid = obj["cid"];


        worldchatCid = objcid.ToString();
        Static.Instance.AddValue("worldcid", objcid.ToString());
        foreach (JsonData i in dataobj)
        {
            ChatInfo newinfo = new ChatInfo();

            newinfo.Name = i["name"].ToString();
            newinfo.Count = i["content"].ToString();
			newinfo.id =i.Keys.Contains("huiyuan_id")?i["huiyuan_id"].ToString():"";
            newinfo.image = i["img"].ToString();
            newinfo.mc = i["cal"].ToString();
            newinfo.coin = i["coin"].ToString();
            worldChatList.Add(newinfo);
            InstantiateChat(newinfo.id, Static.Instance.GetValue("uid"), newinfo,allchatlist_father);
        }

    }

    public void AllChatListClick()
    {
        Static.Instance.AddValue("worldcid",getallchatCid());
        resetAllChat();
    }

    public void ALL_chatListClick()
    {
        Static.Instance.AddValue("allcid",getall_chatCid());
        resetAll_Chat();
    }

    public void resetAll_Chat()
    {
        removeall_chatlist();

        if (allchatList == null)
            return;
        for (int i = 0;i< allchatList.Count;i++)
        {
            if (i > (allchatList.Count- 35))
            InstantiateAllChat(allchatList[i],all_chatlist_father);
        }
    }

    public void resetAllChat()
    {
        removeallchatlist();
        
        if (worldChatList == null)
            return;
        for(int i = 0;i<worldChatList.Count;i++)
        {
            if (i> (worldChatList.Count-35))
            InstantiateChat(worldChatList[i].id, Static.Instance.GetValue("uid"), worldChatList[i],allchatlist_father);
        }
    }

    public void removeallchatlist()
    {
        for (int i = 0; i < allchatlist_father.transform.childCount; i++)
        {
            Destroy(allchatlist_father.GetChild(i).gameObject);
        }
    }

    void InstantiateChat(string key,string key2, ChatInfo info,Transform father)
    {
        if (key == key2)
        {
            chatList self = Instantiate(chatMine);
            self.Name = info.Name;
            self.Contant = info.Count;
            self.head = info.image;
            self.transform.parent = father;
            self.transform.localScale = new Vector3(1, 1, 1);
            self.trySetSize();
        }
        else
        {
            chatList other = Instantiate(chatOther);
            other.Name = info.Name;
            other.Contant = info.Count;
            other.head = info.image;
            other.mc = info.mc;
            other.coin = info.coin;
            other.uid = info.id;
            other.transform.parent = father;
            other.transform.localScale = new Vector3(1,1,1);
            other.trySetSize();
        }
    }

    void InstantiateAllChat(ChatInfo info,Transform father)
    {
        Text newchat = Instantiate(Allchatlist);
        newchat.transform.parent = father;
        newchat.transform.localScale = new Vector3(1,1,1);
        string Type = null;
        string Name = info.Name;
        string Contant = info.Count;

        switch (info.type)
        {
            case "1":
                Type = "【私聊】";
                newchat.color = HexToColor("D1FF00FF");
                break;
            case "2":
                Type = "【世界】";
                newchat.color = HexToColor("BDFCD3FF");
                break;
        }
        newchat.text = Type + Name + ":" + Contant;
        

    }

    [SerializeField]
    public HttpModel current_chat;
    public void refresh_chat_start(float start_time)
    {
        InvokeRepeating("refresh_chat", start_time, 5);
    }

    public void refresh_chat_stop()
    {
        CancelInvoke("refresh_chat");
    }

    void refresh_chat()
    {
        current_chat.Get();
    }

    bool isopen = false;

    public void PlayAnimationGo(Animator GetAnimtor)
    {
        if (!isopen)
        {
            refresh_chat_stop();
            GetAnimtor.SetBool("Go", true);
            isopen = true;
            ClickFuntion[0].AllEvent();
        }
    }

    public void PlayAnimationBack(Animator GetAnimtor)
    {
        if (isopen)
        {
            GetAnimtor.SetBool("Back", true);
            isopen = false;
            refresh_chat_stop();
            ALL_chatListClick();
            AllofChatList();
        }
    }

    public Color HexToColor(string hex)
    {
        byte br = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        byte bg = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        byte bb = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
        byte cc = byte.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
        float r = br / 255f;
        float g = bg / 255f;
        float b = bb / 255f;
        float a = cc / 255f;
        return new Color(r, g, b, a);
    }

    [SerializeField]
    public Button closeprivatechat;
}
