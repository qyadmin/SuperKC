using UnityEditor;
using UnityEditorInternal;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.Collections;
using Newtonsoft.Json;

public class EditorJsontoobj : EditorWindow
{
    [TextArea(5, 5)]
    public string str_json;
    [TextArea(5, 5)]
    public string str;
    [MenuItem("Tools/转公有成员")]
    public static void ShowEditorList()
    {
        EditorWindow window = GetWindow(typeof(EditorJsontoobj));
        window.Show();
    }
    private SerializedObject _serializedObject;

    private void OnEnable()
    {
        _serializedObject = new SerializedObject(this);
    }
    void OnGUI()
    {
        str_json = EditorGUI.TextField(new Rect(0, 30, 500, 100), "需要解析的JsonData:", str_json);
        if (GUILayout.Button("创建"))
        {
            str = Tool_JsonToObj.CreatObj(str_json);
        }
        str = EditorGUI.TextField(new Rect(6, 160, 500, 500), "生成的成员", str);
    }
}

public class kv
{
	public object key;
	public object value;
}

public class Tool_JsonToObj
{

    public static string CreatObj(string str_json)
    {
        string str = string.Empty;
        if (str_json == string.Empty)
            return str;
        str = string.Empty;
        JsonData jd = JsonMapper.ToObject(str_json);
		Dictionary<string,object> newtable =JsonConvert.DeserializeObject<Dictionary<string,object>>(str_json);
		foreach (var child in newtable)
		{
			if(child.Value.GetType()==typeof(System.Int64))
				str += "\n" + "public int " + child.Key + ";";
			if(child.Value.GetType()==typeof(string))
				str += "\n" + "public string " + child.Key + ";";
			if(child.Value.GetType()==typeof(double))
				str += "\n" + "public double " + child.Key + ";";
		}
			
        return str;
    }
}