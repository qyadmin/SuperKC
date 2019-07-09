using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class chatList : MonoBehaviour {

    [SerializeField]
    Text name, contant;
    string headnum;
    [SerializeField]
    Image head_image;

    public string head
    {
        get { return headnum; }
        set {
            headnum = value;
            ModelManager.GetModelManager.SetSmallIamge(head_image,int.Parse(head));
        }
    }

    public string Name
    {
        get { return name.text; }
        set { name.text = value; }
    }
    public string Contant
    {
        get { return contant.text; }
        set { contant.text = value;
        }

    }
    public string mc;
    public string coin;
    public string uid;
    public void trySetSize()
    {
        
        this.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(this.gameObject.GetComponent<RectTransform>().sizeDelta.x, 
        name.gameObject.GetComponent<RectTransform>().sizeDelta.y+ 50f/*contant.gameObject.GetComponent<RectTransform>().sizeDelta.y*/);
    
    }


    public void ShowFirend()
    {
        Chat_Event.Instance.addFriend.gameObject.SetActive(true);
        Chat_Event.Instance.addFriend.head.sprite = head_image.sprite;
        Chat_Event.Instance.addFriend.name.text = Name;
        Chat_Event.Instance.addFriend.mc.text = mc;
        Chat_Event.Instance.addFriend.coin.text = coin;
        Chat_Event.Instance.addFriend.uid.text = uid;
    }
}
