using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
public class Model_UpdateBounds : MonoBehaviour
{


    [SerializeField]
    private Model_Mine BodyMine;
    public void SetData(JsonData jd)
    {

        JsonData GetData = jd["mine"];

        if (GetData == null)
            return;

        foreach (JsonData child in GetData)
        {

        }

    }
}
