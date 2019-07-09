using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mails : MonoBehaviour {


    [SerializeField]
    Sprite isread, noread;

    [SerializeField]
    Image icon;
    public void checkread(string obj)
    {
        
        if (obj == "0")
        {
            icon.sprite = noread;
        }
        else
            icon.sprite = isread;
    }

    
}
