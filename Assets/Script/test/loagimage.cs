using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class loagimage : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{

		StartCoroutine (GetMessage("http://192.168.88.227:19001/newcode"));
	}
	

	public Image icon;

	IEnumerator GetMessage(string url)
	{

		WWW www = new WWW (url);
		yield return www;
		Texture2D texture = www.texture;
		Sprite sprites = Sprite.Create(texture,new Rect(0,0,texture.width,texture.height),new Vector2(0.5f,0.5f));
		icon.sprite = sprites;
	}
}
