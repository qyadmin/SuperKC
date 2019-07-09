using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Action_YinLiao : MonoBehaviour {

    [SerializeField]
    private HttpModel http_yinliao;

    [SerializeField]
    private Button ActionButton;
    [SerializeField]
    private Model_Mine ModelMine;

    private void Start()
    {
        ActionButton.onClick.AddListener(ActionSY);
    }


    private void ActionSY()
    {
        http_yinliao.Data.AddData("id",ModelManager.GetModelManager.kd_id);
        http_yinliao.EventObj.Addlistener(delegate()
        {
            ModelMine.UpdateMineMessage();
        });
        http_yinliao.Get();
    }
}
