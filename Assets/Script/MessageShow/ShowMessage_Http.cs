using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class ShowMessage_Http : MonoBehaviour
{
    [SerializeField]
    private Text ShowMessageSize;
    [SerializeField]
    private Text ShowMessage;

    Coroutine OpenM;
    private float waittime;
    public void SetMessage(string Message)
    {
        waittime = 0;
        ActionMesaaage(Message);
    }

    public void SetMessage(string Message, float wait)
    {
        waittime = wait;
        ActionMesaaage(Message);
    }

    private void ActionMesaaage(string Message)
    {
        if (Message == "(-1)")
            return;
        if (OpenM != null)
        {
            StopCoroutine(OpenM);
        }
        ShowMessage.text = Message;
        transform.localScale = new Vector3(0, 0, 0);

		transform.DOScale (new Vector3(1.2f,1.2f,1.2f), 0.15f).OnComplete (delegate() 
		{
				transform.DOScale (Vector3.one, 0.1f).OnComplete (delegate() 
			    {
					OpenM = StartCoroutine("Open");
				});			
		});
     
    }

    IEnumerator Open()
    {
        yield return new WaitForSeconds(1.5f + waittime);

        while (transform.localScale.y > 0)
        {
            transform.localScale -= new Vector3(0, 0.06f, 0);
            yield return 0;
        }
        transform.localScale = new Vector3(0, 0, 0);
        ShowMessage.text = string.Empty;
    }

    private void Update()
    {
        ShowMessageSize.text = ShowMessage.text; 
    }
}
