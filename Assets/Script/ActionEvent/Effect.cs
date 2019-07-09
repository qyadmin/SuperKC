using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Effect : MonoBehaviour {

    [SerializeField]
    private Sprite[] AllSprite;
    [SerializeField]
    private float times=0.1f;
    private Image effect;
    [SerializeField]
    private bool Loop;
    [SerializeField]
    private bool NoStartLoop=false;
    [SerializeField]
    private float waittime;

    [SerializeField]
    private bool BeControl;
    private void Start()
    {
        if (Loop)
        {
            if (!NoStartLoop)
                StartCoroutine("PlayAnimatorLoop");
        }
        else
        {       
            transform.localScale = new Vector3(0, 0, 0);         
        }
        if(BeControl)
            GameManager.GetGameManager.SyncEffectStatus.Addlistener(PlayStatus);
    }

    private bool IsPaly;
    Coroutine OpenEffect;

    public void Statis()
    {

    }

    public void PlayStatus(Effect tag)
    {
        if (tag != this)
            Hide();
        else
            Show();
    }

    public void Play()
    {
        if (IsPaly)
            return;
        IsPaly = true;
        if (OpenEffect != null)
            StopCoroutine(OpenEffect);
        transform.localScale = new Vector3(1, 1, 1);
        OpenEffect=StartCoroutine("PlayAnimator");
    }


    IEnumerator PlayAnimator()
    {    
        int i= 0;
        while (i<AllSprite.Length&&IsPaly)
        {
            if (AllSprite[i] == null)
                continue;
            effect.sprite = AllSprite[i];
            yield return new WaitForSeconds(times);
            i++; 
        }
        transform.localScale = new Vector3(0, 0, 0);
        effect.sprite = AllSprite[0];
        IsPaly = false;
    }

    public bool LoopStop=false;
    public void StartLoop()
    {
        if(this.gameObject.activeInHierarchy)
        StartCoroutine("PlayAnimatorLoop");
    }
    public void StopLoop()
    {
        LoopStop = false;
        effect.sprite = AllSprite[10];
    }
    IEnumerator PlayAnimatorLoop()
    {
        LoopStop = true;
        int i = 0;
        while (LoopStop)
        {
            if (i == 0)
                yield return new WaitForSeconds(waittime);
            if (AllSprite[i] == null)
                continue;
            effect.sprite = AllSprite[i];
            yield return new WaitForSeconds(times);
            i++;
            if (i == AllSprite.Length)
                i = 0;
        }
       
    }

    public void Show()
    {
        transform.localScale = new Vector3(1,1,1);
    }
    public void Hide()
    {
        transform.localScale = Vector3.zero;
        IsPaly = false;
    }

    private void OnEnable()
    {
        effect = GetComponent<Image>();
        StartCoroutine("PlayAnimatorLoop"); 
    }

    public void SetSprite(Sprite[] all)
    {
        AllSprite = all;
    }
}
