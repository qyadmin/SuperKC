using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FriendChatList : MonoBehaviour
{

    [SerializeField]
    Text Name, f_id,head_id;
    [SerializeField]
    Image Head;
    [SerializeField]
    Transform father;
    private privateChat self = new privateChat();

    [SerializeField]
    Button Chat,Black,TouQu;

    [SerializeField]
    private HttpModel Http_touuq;
	[SerializeField]
	private friendcheckunread selffriendcheckunread;
    private void Start()
    {
		int mun = -1;
		bool isget = int.TryParse (head_id.text, out mun);
		if (isget)
			ModelManager.GetModelManager.SetSmallIamge (Head, mun);
		else
			Debug.LogError ("数据类型未认可"+"--mun"+"--"+mun);
        self.Name = Name.text;
        self.f_id = f_id.text;
        self.Head = Head.sprite;
        Chat.onClick.AddListener(delegate () { ChatFuntion(); });
        Black.onClick.AddListener(delegate() { AddBlack(); });
        TouQu.onClick.AddListener(delegate ()
        {
            //记录当前偷取的好友ID
            GameManager.GetGameManager.fuid = self.f_id;
            Http_touuq.Data.AddData("fuid", self.f_id);
            Http_touuq.Get();
        });
    }

    private void ChatFuntion()
    {
		selffriendcheckunread.ClickChat ();
        father.SendMessage("PrivateChat", self);
    }
    private void AddBlack()
    {
        father.SendMessage("addBlacklist", self);
    }


    
}
