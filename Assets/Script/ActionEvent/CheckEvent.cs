using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckEvent : MonoBehaviour {


	public void CheckChongZhi(Transform window)
	{
		if (!Static.Instance.GetEmptyBool ("wallet"))
			GameManager.GetGameManager.GetWindown (window);
		else
			MessageManager._Instantiate.Show ("充值前，请先绑定您的钱包地址");
	}

	public void Creat(string data)
    {
        string STR=	Load.falselist.Creat(Static.Instance.GetValue("code"), "20180730GLH");
    }
}
