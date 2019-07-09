using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Text.RegularExpressions;

public class MoneyAdress : MonoBehaviour {


	public HttpModel Money;
	public void Check(InputField input)
	{
		Regex regex = new Regex ("^0x[0-9a-fA-F]{40}$");
		bool isgone=regex.IsMatch(input.text);
		Debug.Log (input.text+"****"+input.text.Length);
		if (isgone) {
			Money.Get ();
		} else {

			MessageManager._Instantiate.Show ("您输入的钱包地址无效！");
		}
	}
}

