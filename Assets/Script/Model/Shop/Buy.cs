using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Buy : MonoBehaviour {

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

    private void Start()
    {
        //GetComponent<Button>().onClick.AddListener(Buyobj);
    }

    
}
