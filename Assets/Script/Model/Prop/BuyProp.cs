using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BuyProp : MonoBehaviour {

    [SerializeField]
    private string id;
    [SerializeField]
    private HttpModel http;
    [SerializeField]
    private Button SendButton;
    [SerializeField]
    private Text MessageText;
    [SerializeField]
    private string djMessage;
    [SerializeField]
    private Text money;

    public int lvl;
    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(Buyobj);
    }

    private void Buyobj()
    {
        //System.Convert.ToInt32((lvl*0.2f)) * 5).ToString()
        http.Data.AddData("lvl",lvl.ToString());
        http.Data.AddData("id", id);
        MessageText.text = djMessage.Replace("s", money.text);
        SendButton.onClick.AddListener(http.Get);
    }
}
