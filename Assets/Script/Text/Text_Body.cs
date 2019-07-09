using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Text_Body : MonoBehaviour {

    public void ClearText()
    {
        Tag_Clear[] group = GetComponentsInChildren<Tag_Clear>();
        foreach (Tag_Clear child in group)
            child.Clear();
    }
}
