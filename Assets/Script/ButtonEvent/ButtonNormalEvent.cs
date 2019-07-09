using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;
public class ButtonNormalEvent : ButtonEventBase {

    [SerializeField]
    private GameObject ShowObj, HideObj, ShowPanel, HidePanel;
    [TextArea(2,5)]
    public string Message;
    [SerializeField]
    private HttpModel Model;
    void Start()
    {
        base.Start();
        if (Model != null)
            ActionEvent += SendModel;
        ActionEvent += OnClick;     
    }

    void SendModel()
    {
        Model.Get();
    }

    void OnClick()
    {
        if(ShowObj!=null)
        GameManager.GetGameManager.GetWindown(ShowObj.transform);
        if(HideObj!=null)
            GameManager.GetGameManager.CloseWindown(HideObj.transform);
        if (ShowPanel != null)
            ShowPanel.SetActive(true);
        if (HidePanel != null)
            HidePanel.SetActive(false);
        if (Message!=string.Empty)
        MessageManager._Instantiate.WindowShoMessage(Message);
    }

    public void SetObjActive(GameObject Obj,bool State)
    {
        if(Obj!=null)
        Obj.SetActive(State);
    }

    public void PlayAnimationGo(Animator GetAnimtor)
    {
        GetAnimtor.SetBool("Go",true);
    }

    public void PlayAnimationBack(Animator GetAnimtor)
    {
        GetAnimtor.SetBool("Back", true);
    }

}
