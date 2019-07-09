using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StatusListTX : MonoBehaviour {

	public void Status(string data)
	{
		if (data == "2")
			GetComponent<Text> ().text = "已拒绝";
		if (data == "4")
			GetComponent<Text> ().text = "已完成";
	}
}
