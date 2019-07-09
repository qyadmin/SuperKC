using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;


public class ActionEvent_Prop : MonoBehaviour {

    public Text suanli, Message, Name,wid;

    [SerializeField]
    public Image HeadIamge;

    //装在道具参数
    public Button LoadButton;
    public GameObject Mark;

    //更新道具状态
    public void SendState(bool state)
    {
        if (Mark.activeSelf)
        {
            LoadButton.gameObject.SetActive(false);
            return;
        }
        LoadButton.gameObject.SetActive(state);
    }

    //移除绑定关系
    private void OnDisable()
    {
        ModelManager.GetModelManager.SyncState.Removelistener(SendState);
    }

    //绑定关系
    private void OnEnable()
    {
        ModelManager.GetModelManager.SyncState.Addlistener(SendState);
    }

}

