using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WT.UI;

public class PokerSettingControl  {

    private PokerSetting pokerSetting = new PokerSetting();


    //回调函数,PokerMainControl控制
    public void ShowSetting()
    {
        WTUIPage.ShowPage("PokerSetting", pokerSetting);
    }
}
