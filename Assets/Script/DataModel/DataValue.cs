using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TypeClass;
namespace TypeClass
{
	public enum SaveNameType
	{
		None,
		SaveMySelfName,
		SaveOtherName,
		ActionEvent,
		Chosetype

	}

	public enum GetTypeValue
	{
		GetFromValue,
		GetFormText,
		GetFromInputField,
		GetFromList,
		GetFromListOther
	}

	public enum GetBackTypeValue
	{
		GetValue,
		NoGetValue,
	}


	public enum Valuetype
	{
		GetInt,
		GetString
	}
}

public class EmptyItem
{
    public EmptyItem()
    {
        Items.Add("tuijianren");
    }

    private List<string> Items = new List<string>();

    public  bool IsGone(string itemename)
    {
        if (Items.Contains(itemename))
            return true;
        return false;
    }

}

[System.Serializable]
public class DataValue
{
	public bool IsSave;
	public string Name;
	public string OtherName;

	public GetTypeValue MyType;

	public Text SetText;
	public InputField SetInputField;
	public string SetValue;

	//public TagerType MyTager;
	public string MakeValue;

    public bool IsCheck;
    public InputField InputText;
    public string WarmMessage;
    public int MinNub;
    public bool NeedMaxm;
    //可为空值的过滤器
    private EmptyItem EmptyBody=new EmptyItem();

	public string GetString()
	{
		string ValueData = null;
		switch (MyType)
		{
		case GetTypeValue.GetFromValue:
			ValueData = SetValue;
		break;
		case GetTypeValue.GetFormText:
			ValueData = SetText.text;
		break;
		case GetTypeValue.GetFromInputField:
			ValueData = SetInputField.text;
		break;
		case GetTypeValue.GetFromList:
			ValueData = Static.Instance.GetValue(Name);
		break;
		case GetTypeValue.GetFromListOther:
			ValueData = Static.Instance.GetValue(OtherName);
		break;
		}
		if(IsSave)
			Static.Instance.AddValue(Name,ValueData);
        if (IsCheck)
        {          
            if (InputText == null)
            {
                ValueData = ValueData == string.Empty ? "0" : ValueData;
                if (int.Parse(ValueData) < MinNub)
                {
                    MessageManager._Instantiate.Show(WarmMessage);
                    return "Error";
                }
            }
            else
            {
                if (ValueData != InputText.text)
                {
                    MessageManager._Instantiate.Show(WarmMessage);
                    return "Error";
                }
            }
        }
        if (ValueData == string.Empty)
        {
            if (!EmptyBody.IsGone(Name))
            {
                Debug.Log("不包含"+Name);
                MessageManager._Instantiate.Show("参数不能为空");
                return "Error";
            }
            else
                return "Gone";
        }
        if (NeedMaxm)
        {
            return Load.falselist.Creat(ValueData,Static.Instance.LaodConfig.maxm);
        }
        else
		return ValueData;

	}
}

[System.Serializable]
public class BackDataValue
{
	public GetBackTypeValue MyType;
	public bool IsSave;
	public string Name;
	public Text SetText;


	public void SetString(string BackValue)
	{
		switch (MyType)
		{
		case GetBackTypeValue.GetValue:
			SetText.text = BackValue;
		break;
		}
		if(IsSave)
			Static.Instance.AddValue(Name,BackValue);
	}
}

public class EventClass
{
	public UnityEvent Fuc;
	public UnityEvent Fal;
}