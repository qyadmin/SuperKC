using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class rank : MonoBehaviour {

    [SerializeField]
    private Text f_s;
    [SerializeField]
    private Text fs_title;
    public void ChangeFenSi()
    {
        f_s.text = "我的粉丝";
        fs_title.text = "粉丝";
    }

    public void ChangeSuanLi()
    {
        f_s.text = "我的算力";
        fs_title.text = "算力";

    }

}
