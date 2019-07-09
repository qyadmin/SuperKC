using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class mail_Event : MonoBehaviour
{

    public static mail_Event Instance;



    [SerializeField]
    public Sprite disable, pressed;

    [System.Serializable]
    public class classification
    {
        public UnityEvent Event;
        public Button ClickButton;

        public UnityEvent httpEvent;


        public void Start()
        {
            ClickButton.onClick.AddListener(delegate ()
            {
                mail_Event.Instance.ClickAnimationReset();
                Event.Invoke();
                Click();
                HttpButton();
            }
            );
        }

        public void Reset()
        {
            ClickButton.GetComponent<Image>().sprite = mail_Event.Instance.disable;
        }
        public void Click()
        {
            ClickButton.GetComponent<Image>().sprite = mail_Event.Instance.pressed;
        }

        public void HttpButton()
        {
            mail_Event.Instance.httpEventButton.onClick.RemoveAllListeners();
            mail_Event.Instance.httpEventButton.onClick.AddListener(delegate () { httpEvent.Invoke(); });
        }
    }




    [SerializeField]
    classification[] ClickFuntion;

    public Button httpEventButton;
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

    public void OnEnable()
    {

        ClickAnimationReset();
        ClickFuntion[0].Click();
        ClickFuntion[0].Event.Invoke();
        ClickFuntion[0].HttpButton();
    }



    public void ClickAnimationReset()
    {
        foreach (classification i in ClickFuntion)
        {
            i.Reset();
        }
    }

    public void Show(GameObject obj)
    {
        GameManager.GetGameManager.GetWindown(obj.transform);
    }
    public void Hide(GameObject obj)
    {
        GameManager.GetGameManager.CloseWindown(obj.transform);
    }

    [SerializeField]
    GameObject unreadnum, unreadsysnum;

    public void unreadmailnum(string obj)
    {
        if (obj == "0")
            unreadnum.SetActive(false);
        else
        {
            unreadnum.SetActive(true);
            unreadnum.GetComponentInChildren<Text>().text = obj;
        }

    }
    public void unreadsysmailnum(string obj)
    {
        if (obj == "0")
            unreadsysnum.SetActive(false);
        else
        {
            unreadsysnum.SetActive(true);
            unreadsysnum.GetComponentInChildren<Text>().text = obj;
        }

    }

    public void Show1(GameObject obj)
    {
        obj.gameObject.SetActive(true);
    }
    public void Hide1(GameObject obj)
    {
        obj.gameObject.SetActive(false);
    }


    [SerializeField]
    Transform CountFather, CountFather_2;

    string selecte_uid = null;

    public void selecte(Toggle tog)
    {
        SetClick(tog);
    }

    public void selecte_2(Toggle tog)
    {
        SetClick_2(tog);
    }


    public void Reset(Toggle ison)
    {
        ison.isOn = true;
        ison.isOn = false;
    }


    public void selecteAll(Toggle ison)
    {
        foreach (Transform i in CountFather)
        {
            if (ison.isOn)
                i.GetComponentInChildren<Toggle>().isOn = true;
            else
                i.GetComponentInChildren<Toggle>().isOn = false;
        }

    }

    public void selecteAll_2(Toggle ison)
    {
        foreach (Transform i in CountFather_2)
        {
            if (ison.isOn)
                i.GetComponentInChildren<Toggle>().isOn = true;
            else
                i.GetComponentInChildren<Toggle>().isOn = false;
        }

    }

    void SetClick(Toggle tog)
    {
        bool ison = Clickenable();
        Button b = tog.GetComponentInParent<Button>();
        if (b != null)
            b.interactable = !tog.isOn;
        //foreach(Transform i in CountFather)
        //{
        //    i.GetComponent<Button>().interactable = ison;
        //}

    }

    void SetClick_2(Toggle tog)
    {
        bool ison = Clickenable();
        Button b = tog.GetComponentInParent<Button>();
        if (b != null)
            b.interactable = !tog.isOn;
        //foreach(Transform i in CountFather)
        //{
        //    i.GetComponent<Button>().interactable = ison;
        //}

    }


    bool Clickenable()
    {
        bool ison = true;
        foreach (Transform i in CountFather)
        {
            if (i.GetComponentInChildren<Toggle>().isOn)
            {
                ison = false;
                break;
            }

        }
        return ison;
    }

    bool Clickenable_2()
    {
        bool ison = true;
        foreach (Transform i in CountFather)
        {
            if (i.GetComponentInChildren<Toggle>().isOn)
            {
                ison = false;
                break;
            }

        }
        return ison;
    }


    public void GetDelatesMails(HttpModel http)
    {
        delatesMail = http;
    }

    HttpModel delatesMail;

    public void Delateselecte(Transform CountFather)
    {
        selecte_uid = "";
        foreach (Transform i in CountFather)
        {
            if (i.GetComponentInChildren<Toggle>().isOn)
                foreach (Transform j in i)
                    if (j.name == "uid")
                        if (selecte_uid == null)
                            selecte_uid += j.GetComponent<Text>().text;
                        else
                            selecte_uid += "a" + j.GetComponent<Text>().text;
        }
        Static.Instance.AddValue("mailids", selecte_uid);

        if (selecte_uid == "" || selecte_uid == null)
            return;
        else
            delatesMail.Get();
    }

}
