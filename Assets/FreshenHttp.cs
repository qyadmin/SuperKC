// ==================================================================
// 作    者：Pablo.风暴洋-宋杨
// 説明する：在这里输入类的功能
// 作成時間：#CreateTime
// 類を作る：FreshenHttp.cs
// 版    本：v 1.0
// 会    社：大连仟源科技
// QQと微信：731483140
// ==================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreshenHttp : MonoBehaviour {

    public HttpModel ajax_sys_mails;

    private void Start()
    {
        InvokeRepeating("Freshen", 1, 5);
    }

    private void Freshen()
    {
        ajax_sys_mails.Get();
    }
}
