using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using UnityEngine.Events;
using UnityEngine.UI;
using System;

public class report_Event : MonoBehaviour {

    [SerializeField]
    UnityEvent Event;

    [HideInInspector]
    public List<string> X_Value = new List<string>();
    [HideInInspector]
    public List<Vector2> Y1_Value = new List<Vector2>();
    [HideInInspector]
    public List<Vector2> Y2_Value = new List<Vector2>();

    [SerializeField]
    Transform days;

    [SerializeField]
    WMG_Axis_Graph Axis;
    [SerializeField]
    WMG_Series y_Axis, y2_Axis;

    WMG_Axis X_Axis;
    WMG_Axis Y_Axis;

    string stolenmc, yesterday, reqmc, stealmc;
    [SerializeField]
    Text stolenmc_text, yesterday_text, reqmc_text, stealmc_text;

    public string Stolenmc
    {
        get { return stolenmc; }
        set { stolenmc = value;
            stolenmc_text.text = Stolenmc;
        }
    }

    public string Yesterday
    {
        get { return yesterday; }
        set { yesterday = value;
            yesterday_text.text = Yesterday;
        }
    }

    public string Reqmc
    {
        get { return reqmc;}
        set { reqmc = value;
            reqmc_text.text = Reqmc;
        }
    }

    public string Stealmc
    {
        get { return stealmc; }
        set { stealmc = value;
            stealmc_text.text = Stealmc;
        }
    }

    private void Awake()
    {
        X_Axis = Axis.xAxis;
        Y_Axis = Axis.yAxis;
    }


    public void Succece()
    {
        X_Axis.axisLabels.SetList(X_Value);

        y_Axis.pointValues.SetList(Y1_Value);

        y2_Axis.pointValues.SetList(Y2_Value);

        Axis.yAxis.AxisMaxValue = GetMaxs();
    }
    public void OnEnables()
    {
        Event.Invoke();
    }

    public void GetJsonDate(JsonData data)
    {
        JsonData report = data["report"][0];
        JsonData total = data["report"][1];

        List<JsonData> Getjson = new List<JsonData>();
        for (int i = 0; i < report.Count; i++)
        {
            Getjson.Add(report[i]);
        }
        float num = 0;
        X_Value.Clear();
        Y1_Value.Clear();
        Y2_Value.Clear();
        
        foreach (JsonData i in Getjson)
        {

            X_Value.Add(JsonMapper.ToJson(i["time"]).ToString());
            Y1_Value.Add(new Vector2(num,float.Parse(JsonMapper.ToJson(i["daily"]["stealmc"]).ToString())/1000));
            Y2_Value.Add(new Vector2(num,float.Parse(JsonMapper.ToJson(i["daily"]["reqmc"]).ToString())/1000));
            num++;
        }

        Stolenmc  =((float.Parse(JsonMapper.ToJson(total["total"]))/1000)).ToString();
        Yesterday = (float.Parse(JsonMapper.ToJson(total["yesterdaytotal"]))/1000).ToString();
        Reqmc = (float.Parse(JsonMapper.ToJson(total["reqmc"])) /1000).ToString();
        Stealmc = (float.Parse(JsonMapper.ToJson(total["stealmc"]))/1000).ToString();

        for (int i = 0;i<days.childCount;i++)
        {
            days.GetChild(i).GetComponent<Text>().text = X_Value[i].Replace("\"","").Remove(0,4);
        }

        Succece();
    }

    float GetMaxs()
    {
        float max = 0;

        foreach (Vector2 i in Y1_Value)
        {
            if (i.y >= max)
                max = i.y;
        }
        foreach (Vector2 i in Y2_Value)
        {
            if (i.y >= max)
                max = i.y;
        }
       
            return max;
    }

}
