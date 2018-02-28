using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WT.UI;

public class PokerFastRoom : WTUIPage
{ 
    public PokerFastRoom():base(UIType.Normal,UIMode.DoNothing,UICollider.None)
    {
        uiIndex = R.Prefab.POKERFASTROOM;
    }


    public override void Awake(GameObject go)
    {
        GameObject.Find("Content/Button_back").GetComponent<Button>().onClick.AddListener(Btn_Back);
    }

    /// <summary>
    /// 返回大厅
    /// </summary>
    public void Btn_Back()
    {
        Hide();
    }

}
