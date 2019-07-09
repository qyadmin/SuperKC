using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MessageManager : MonoBehaviour {

    public static MessageManager _Instantiate;

    private ShowMessage_Http ShowMessage;

    private GameObject ShowLoad;

    private void Awake()
    {
        _Instantiate = this;
    }

    void Start ()
    {
        ShowLoad = GameObject.Find("HttpBack");
        ShowMessage = GameObject.Find("MessageObj").GetComponent<ShowMessage_Http>();
    }
	
    //默认显示2秒
    public void Show(string GetMessage)
    {
        ShowMessage.SetMessage(GetMessage);
    }

    //增加显示2秒+waittime
    public void Show(string GetMessage,float waittime)
    {
        ShowMessage.SetMessage(GetMessage,waittime);
    }

    public void Show(Text GetMessage)
    {
        ShowMessage.SetMessage(GetMessage.text);
    }

    public GameObject Window;
    public Text Message_Window;
	public Button MessageButton;
	public Button CloseButton;
    public void WindowShoMessage(Text GetText)
    {
        Message_Window.text = GetText.text;
        Window.SetActive(true);
    }

    public void WindowShoMessage(string GetText)
    {
		if(MessageButton)
		MessageButton.gameObject.SetActive (false);
        Message_Window.text = GetText;
        Window.SetActive(true);
    }

	public void WindowShoMessage(string GetText,System.Action action ,bool closebutton=true,string title="确定")
	{
		if(MessageButton)
		MessageButton.gameObject.SetActive (true);
		CloseButton.gameObject.SetActive (closebutton);
		System.Action epp = MessageButton.GetComponent<ButtonEventBase>().ActionEvent;	
		while(epp!=null)
		{
			epp -= epp;
		}
		MessageButton.GetComponent<ButtonEventBase> ().ActionEvent += action;
		Message_Window.text = GetText;
		Window.SetActive(true);
	}

    public void QuiteGame()
    {
        SceneManager.LoadScene("mainmeun");
    }

    [SerializeField]

    private Text ErrorCode;
    public void ShowErrorCode(string GetError)
    {
        if(this.ErrorCode != null)
        ErrorCode.text = GetError;
    }

    public int LoadNub = 0;
    public void AddLockNub()
    {
        LoadNub++;
    }
    public void DisLockNub()
    {
        LoadNub--;
    }

    public int StatusNub = 0;
    private void Update()
    {
        if (LoadNub <= 0)
        {          
            ShowLoad.transform.localScale = new Vector3(0, 0, 0);
            StatusNub = 0;
            LoadNub = 0;	
        }
        else
        {         
            ShowLoad.transform.localScale = new Vector3(1, 1, 1);
            StatusNub++;
            if (StatusNub > 1000)
            {
                StopAllCoroutines();
                Show("请求超时！");
                LoadNub = 0;
            }
        }

    }
}
