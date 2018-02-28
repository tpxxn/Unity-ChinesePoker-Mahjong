using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WT.UI;

/// <summary>
/// 输赢界面控制器
/// </summary>
public class PokerWinControl  {

    public PokerWin pokerWin;

    public PokerWinControl()
    {
        pokerWin = new PokerWin();

        pokerWin.AddExitEvent(Exit);

    }


    /// <summary>
    /// 显示输赢界面
    /// </summary>
    public void ShowWinUI(string info)
    {
        WTUIPage.ShowPage("PokerWin", pokerWin);
        //显示获胜信息
        pokerWin.winText.text = info;
    }

    /// <summary>
    /// 退出程序
    /// </summary>
    public void Exit()
    {
        Application.Quit();
    }

    
    
	
}
