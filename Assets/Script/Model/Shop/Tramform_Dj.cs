using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Tramform_Dj : MonoBehaviour {

    public Text SuanLi, Nmae, Message, Icon;
    public Image HeadImage;
    public Button Buy_Button;
    public GameObject Mrak;

    public string lvl;
    public void Add()
    {
        GameManager.GetGameManager.LVL_Event.Addlistener(UpdateStatus);
    }
    private void OnEnable()
    {
        if (GameManager.GetGameManager)
        GameManager.GetGameManager.LVL_Event.Addlistener(UpdateStatus);
    }

    private void OnDisable()
    {
        GameManager.GetGameManager.LVL_Event.Removelistener(UpdateStatus);
    }

    public void UpdateStatus(string maxlvl)
    {
        if(Mrak)
        Mrak.SetActive(GetCL.Compare(lvl, maxlvl));
    }
}
