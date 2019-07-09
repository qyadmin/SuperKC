using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateControl : MonoBehaviour
{


    [SerializeField]
    private Transform BuyObj;
    [SerializeField]
    private Transform EatObj;
    public void Buy()
    {
        if (Static.Instance.GetValue("drink") != "0")
        {
            return;
        }
        GameManager.GetGameManager.GetWindown(BuyObj);
        GameManager.GetGameManager.CloseWindown(EatObj);
    }

    public void Eat()
    {
        if (Static.Instance.GetValue("drink") == "0")
            return;
        GameManager.GetGameManager.GetWindown(EatObj);
        GameManager.GetGameManager.GetWindown(BuyObj);
    }
}