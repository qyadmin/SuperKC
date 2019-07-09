using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Friend : MonoBehaviour {

    [SerializeField]
    Image Head;
    [SerializeField]
    Text Name, f_id;

    private privateChat self;
    public privateChat Self
    {
        get { return self; }
        set { self = value;
                Head.sprite = Self.Head;
                Name.text = Self.Name;
                f_id.text = self.f_id;
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
