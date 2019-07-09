using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class friendcheckunread : MonoBehaviour {

    [SerializeField]
    GameObject unread, unreadPlan;

    public void unreadnum(string obj)
    {
        if (obj == "0")
        {
            unreadPlan.SetActive(false);
            unread.SetActive(false);
            
        }
        else
        {
            unread.SetActive(true);
            unreadPlan.SetActive(true);
            unread.GetComponent<Text>().text = obj;
        }

    }

	public void ClickChat()
	{
		unreadPlan.SetActive(false);
		unread.SetActive(false);
	}
}
