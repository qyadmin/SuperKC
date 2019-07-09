using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using UnityEngine.UI;
public class Model_Announcement : MonoBehaviour {

    [SerializeField]
    private Transform FatherObj;
    [SerializeField]
    private GameObject listObj;
    [SerializeField]
    private Tramforms_Message ShowAnnouncement;
    [SerializeField]
    private HttpModel ReadAnn;

    public void SetAnnouncemenData(JsonData GetData)
    {
        foreach (Transform child in FatherObj)
        {
           Destroy(child.gameObject);
        }
        if (GetData != null)
        {
            foreach (JsonData child in GetData)
            {
                GameObject NewList = GameObject.Instantiate(listObj);
                NewList.transform.SetParent(FatherObj);
                NewList.transform.localScale = new Vector3(1, 1, 1);
                NewList.SetActive(true);
                NewList.name = listObj.name;
                Tramforms_Announcenment obj = NewList.GetComponent<Tramforms_Announcenment>();
                obj.Title.text = child["title"].ToString();
                if (child["content"].ToString().Length >= 5)
                {
                    obj.Message.text = child["content"].ToString().Substring(0, 5);
                }
                else
                {
                    obj.Message.text = child["content"].ToString();
                }
                obj.Time.text = child["sj"].ToString();
                if (child["status"].ToString() == "0")
                    obj.mark.SetActive(true);
                else
                    obj.mark.SetActive(false);
                obj.ActionButton.onClick.AddListener(delegate ()
                {
                    GameManager.GetGameManager.GetWindown(ShowAnnouncement.transform);
                    ShowAnnouncement.Title.text = obj.Title.text;
                    ShowAnnouncement.Message.text = child["content"].ToString();
                    ReadAnn.Data.AddData("gid", child["id"].ToString());
                    ReadAnn.EventObj.Addlistener(delegate ()
                    {
                        GetComponent<HttpModel>().Get();
                    });
                    ReadAnn.Get();

                });
            }
        }
    }
}
