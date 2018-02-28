using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WT.UI;

public class PokerSetting : WTUIPage {

	public PokerSetting():base(UIType.PopUp,UIMode.HideOther,UICollider.None)
    {
        uiIndex = R.Prefab.SETTING;
    }

    public override void Awake(GameObject go)
    {
        GameObject.Find("Content/Panel/Button_exit").GetComponent<Button>().onClick.AddListener(BtnSettingBack);
    }


    private void BtnSettingBack()
    {
        //直接调用，不受控制器决定
        Hide();
    }
}
