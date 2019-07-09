using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;

//用于UI显示的容器
public class ActionEvent_RenWu : MonoBehaviour {

    [SerializeField]
    private Text Title,Message, buttonText,Current,coin;
    [SerializeField]
    private Image HeadIamge;
    [SerializeField]
    private Button Lingqu, ZhiXng;


    [SerializeField]
    private Model_RenWu Obj_RenWu;
    private RenWuMesage obj;

    [SerializeField]
    private HttpModel http_lingqu;

    [SerializeField]
    private Transform RenWuWindon;

    public void SetData(string GetData)
    {
        obj = Obj_RenWu.SetData(GetData);

        if (obj != null)
        {
            this.Title.text = obj.Title;
            this.Message.text = obj.Message.Replace("s",obj.count);
            this.buttonText.text = obj.ButtonText;
            this.HeadIamge.sprite = obj.HeadIamge;
            this.Current.text = obj.current+"/"+obj.count;
            this.coin.text = obj.coin;
            if (System.Convert.ToBoolean(obj.complete))
            {
                ZhiXng.gameObject.SetActive(false);
                Lingqu.gameObject.SetActive(true);
                Lingqu.onClick.AddListener(
                delegate ()
                {
                    http_lingqu.Data.AddData("id", obj.id);
                    http_lingqu.Get();
                });
            }
            else
            {
                Lingqu.gameObject.SetActive(false);
                ZhiXng.gameObject.SetActive(true);
                if (obj.ShowRenWu)
                {
                    ZhiXng.onClick.AddListener(
                    delegate ()
                    {
                      if(obj.IsFalse)
                            GameManager.GetGameManager.CloseWindown(obj.ShowRenWu);
                        else
                        GameManager.GetGameManager.GetWindown(obj.ShowRenWu);
                      //关闭任务窗口
                        GameManager.GetGameManager.CloseWindown(RenWuWindon);
                    });
                }
            }
            if (System.Convert.ToBoolean(obj.req))
                Destroy(this.gameObject);

        }
    }
}
