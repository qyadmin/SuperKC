using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ControlOPC : MonoBehaviour {

    private long startSec;
    public void Open(Transform obj)
    {
        GameManager.GetGameManager.GetWindown(obj);
    }

    public void Close(Transform obj)
    {
        GameManager.GetGameManager.CloseWindown(obj);

    }

    public void Wait(Text time)
    {
        startSec = System.DateTime.Now.Second + System.DateTime.Now.Minute * 60 + System.DateTime.Now.Hour * 3600 + System.DateTime.Now.Day * 86400;
        StopAllCoroutines();
        StartCoroutine(Loop(TimeBtn));
        Debug.Log("=====================");
    }

    [SerializeField]
    private Button TimeBtn;
    [SerializeField]
    private GameObject mark;
    IEnumerator Loop(Button obj)
    {
        obj.interactable = false;
        Text a = obj.GetComponentInChildren<Text>();
        while (System.DateTime.Now.Second + System.DateTime.Now.Minute * 60 + System.DateTime.Now.Hour * 3600 + System.DateTime.Now.Day * 86400 - startSec <= 60)
        {
            a.text = (60 - (System.DateTime.Now.Second + System.DateTime.Now.Minute * 60 + System.DateTime.Now.Hour * 3600 + System.DateTime.Now.Day * 86400 - startSec)).ToString() + "秒";
            yield return new WaitForSeconds(1);
        }

        a.text = "获取验证码";
        obj.interactable = true;
    }

    //[SerializeField]
    //private GameObject PAIQIAN;
    //[SerializeField]
    //private Woker_Message getworker;
    //public void CheckUpdatePQ()
    //{
    //    if (PAIQIAN.activeSelf)
    //        getworker.ShowPaiQian();
    //}

    //public void Restart(Text obj)
    //{
    //    startSec = 0;
    //    mark.SetActive(false);
    //}
}
