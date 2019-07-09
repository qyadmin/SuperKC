using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SetIP : MonoBehaviour {

	[SerializeField]
	private HttpModel ser;
	public void SetIPd(InputField t)
	{
		Static.Instance.LocalURL=t.text+":19001/";
		ser.Get ();
	}
}
