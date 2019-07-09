using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class friendHead : MonoBehaviour {

    [SerializeField]
    Image head;

    public void sethead(string obj)
    {
        ModelManager.GetModelManager.SetSmallIamge(head,int.Parse(obj));
    }
}
