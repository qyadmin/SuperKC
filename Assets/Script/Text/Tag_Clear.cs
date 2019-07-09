using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tag_Clear : MonoBehaviour {

    public InputField TagText { get { return GetComponent<InputField>(); } }

    public void Clear()
    {
        TagText.text = string.Empty;
    }
}
