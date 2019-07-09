using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Model_RunMine : MonoBehaviour {


    public GameObject Have;

    public GameObject NOHave;

    public Mine_Pos minepos;

    public Button ReCallButton;

    public void SetState(Mine_Pos Getminepos)
    {
        minepos = Getminepos;
        Have.SetActive(false);
        NOHave.SetActive(false);
        if (Getminepos.State)
            Have.SetActive(true);
        else
            NOHave.SetActive(true);
        WokerBody.ShowPaiQianList(Getminepos);
    }

    //获取当前所有矿工信息
    [SerializeField]
    private Woker_Message WokerBody;

    public Text Name, lvl, SuanLi, prevmc, runtimemc;

    public Text power, totoppower;
    public Image PowerImage, IamgeBody;

    public Transform StartBody;

    //撤回成功调用 清除面板
    public void Close()
    {
        Have.SetActive(false);
        NOHave.SetActive(true);
    }


}
