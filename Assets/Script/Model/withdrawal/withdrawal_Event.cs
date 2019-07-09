using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class withdrawal_Event : MonoBehaviour {

    public static withdrawal_Event Instance;



    [SerializeField]
    public Sprite disable, pressed;

    [System.Serializable]
    public class classification
    {
        public UnityEvent Event;
        public Button ClickButton;

        public void Start()
        {
            ClickButton.onClick.AddListener(delegate ()
            {
                withdrawal_Event.Instance.ClickAnimationReset();
                Event.Invoke();
                Click();
            }
            );
        }

        public void Reset()
        {
            ClickButton.GetComponent<Image>().sprite = withdrawal_Event.Instance.disable;
        }
        public void Click()
        {
            ClickButton.GetComponent<Image>().sprite = withdrawal_Event.Instance.pressed;
        }
    }


    [SerializeField]
    classification[] ClickFuntion;

    // Use this for initialization
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        foreach (classification i in ClickFuntion)
        {
            i.Start();
        }
    }

    public void OnEnable()
    {

        ClickAnimationReset();
        ClickFuntion[0].Click();
        ClickFuntion[0].Event.Invoke();
    }



    public void ClickAnimationReset()
    {
        foreach (classification i in ClickFuntion)
        {
            i.Reset();
        }
    }
}
