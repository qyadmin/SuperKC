using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
public class CreatIP : MonoBehaviour {

    [SerializeField]
    private Text IP, PORT;
    public UnityEvent ActionEvent;
    [SerializeField]
    private HttpModel HTTP_ip;
    private void Start()
    {
        HTTP_ip.Get();
    }
    public void Creat()
    {
        Static.Instance.URL = "http://" + IP.text + ":" + PORT.text + "/";
    }

    public void SaveJwt()
    {
        Static.Instance.DeleteFile(Application.persistentDataPath, "jwt.text");
        Static.Instance.CreateFile(Application.persistentDataPath, "jwt.text", Static.Instance.GetValue("jwt"));
    }


    public HttpModel JwtLogin,PasswordLogin;
    public void Login()
    {
         PasswordLogin.Get();
    }

  
    public void LoadSuS()
    {
        SaveJwt();
        SceneManager.LoadScene("LabbyScene");

    }

}
