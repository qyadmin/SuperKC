using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using LitJson;

public class privateChat
{
    public string f_id;
    public Sprite Head;
    public string Name;

}
public class Friend_Event : MonoBehaviour {

    public static Friend_Event Instance;



    [SerializeField]
    public Sprite disable, pressed;

    [System.Serializable]
    public class classification
    {
        public UnityEvent Event;
        public Button ClickButton;

        public void Start()
        {
            ClickButton.onClick.AddListener(delegate () 
            {
                Friend_Event.Instance.ClickAnimationReset();
                Event.Invoke();
                Click(); }
            );
        }

        public void Reset()
        {
            ClickButton.GetComponent<Image>().sprite = Friend_Event.Instance.disable;
        }
        public void Click()
        {
            ClickButton.GetComponent<Image>().sprite = Friend_Event.Instance.pressed;
        }

        
    }

    

    [SerializeField]
    UnityEvent chat_Event;
    [SerializeField]
    UnityEvent steal_Event;
    [SerializeField]
    UnityEvent blacklist_Event;

    [SerializeField]
    classification[] ClickFuntion;

    // Use this for initialization
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        foreach (classification i in ClickFuntion)
        {
            i.Start();
        }
    }

    public void OnEnable() {

        ClickAnimationReset();
        ClickFuntion[0].Click();
        ClickFuntion[0].Event.Invoke();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ClickAnimationReset()
    {
        foreach (classification i in ClickFuntion)
        { 
            i.Reset();
        }
    }

    [SerializeField]
    GameObject privateChatHead,FaterView;

    [SerializeField]
    List<Friend> objGroup = new List<Friend>();

    [HideInInspector]
    public Friend clickObj;

    [SerializeField]
    Text Name, f_id;

    public void PrivateChat(privateChat person)
    {
        
        foreach (Friend i in objGroup)
        {
            if (i.Self.f_id == person.f_id)
            {
                Click(i);
          //      chat_Event.Invoke();
                return;
            }
               
        }
        Debug.Log("PrivateChat");
        //chat_Event.Invoke();
        GameObject obj = Instantiate(privateChatHead);
        
        obj.transform.parent = FaterView.transform;
        obj.transform.localScale = new Vector3(1,1,1);
        Friend obj_friend = obj.GetComponent<Friend>();
        obj_friend.Self = person;
        objGroup.Add(obj_friend);
        Click(obj_friend);
        obj.GetComponent<Button>().onClick.AddListener(delegate () 
        {
            Click(obj_friend);

        });
        closeprivateContal();
    }

    public void addBlacklist(privateChat person)
    {
        Static.Instance.AddValue("black_id", person.f_id);
        blacklist_Event.Invoke();
    }



    void Click(Friend person)
    {
        Name.text = person.Self.Name;
        f_id.text = person.Self.f_id;
        clickObj = person;
        Static.Instance.AddValue("cid",Chat_Event.Instance.getCid(person.Self.f_id));
        Chat_Event.Instance.resetPrivateChat(person.Self.f_id);
        chat_Event.Invoke();
        Debug.Log("Click");
    }


    public void RemovePrivateChat(Friend person)
    {
        if (!person)
            return;
        objGroup.Remove(person);
        for (int i = 0; i < FaterView.transform.childCount; i++)
        {
            if (FaterView.transform.GetChild(i).GetComponent<Friend>() == person)
                Destroy(FaterView.transform.GetChild(i).gameObject);
        }
        Chat_Event.Instance.privateChat.Remove(person.Self.f_id);
        Chat_Event.Instance.privateChatCid.Remove(person.Self.f_id);
        if (objGroup.Count == 0)
        {
            Chat_Event.Instance.removechatlist();
            Name.text = null;
            f_id.text = null;
            Chat_Event.Instance.refresh_chat_stop();
        }
        else
        {
            Click(objGroup[0]);
        }
        closeprivateContal();
    }

    public void closeprivateContal()
    {
        if (objGroup.Count == 0)
            Chat_Event.Instance.closeprivatechat.interactable = false;
        else
            Chat_Event.Instance.closeprivatechat.interactable = true;
    }
    [SerializeField]
    GameObject applynum;
    public void CheckApplyNum(JsonData obj)
    {
        JsonData getData = obj["data"];
        applynum.GetComponentInChildren<Text>().text = getData.ToString();
        if (getData.ToString() == "0")
            applynum.SetActive(false);
        else
            applynum.SetActive(true);
    }
}
