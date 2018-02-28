using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WT.UI;
using System;
using UnityEngine.UI;

/// <summary>
/// 输赢界面UI
/// </summary>
public class PokerWin : WTUIPage {

    public Text winText;

    public PokerWin():base(UIType.PopUp,UIMode.DoNothing,UICollider.None)
    {
        uiIndex = R.Prefab.POKERWIN;
    }

    public override void Awake(GameObject go)
    {
        #region GameObject Find
        GameObject.Find("Content/Button_res").GetComponent<Button>().onClick.AddListener(Btn_Restart);
        GameObject.Find("Content/Button_exit").GetComponent<Button>().onClick.AddListener(Btn_Exit);
        GameObject.Find("Content/Button").GetComponent<Button>().onClick.AddListener(Btn_Back);
        winText = GameObject.Find("Content/Text_win").GetComponent<Text>();
        #endregion
    }

    #region Action
    public Action RestartEvent = null;
    public void AddRestartEvent(Action action)
    {
        RestartEvent = action;
    }

    public Action ExitEvent = null;
    public void AddExitEvent(Action action)
    {
        ExitEvent = action;
    }
    #endregion

    public void Btn_Restart()
    {
        Debug.Log("重新开始");
       
        Hide();
        //PokerTableControl.instance.RestartGame();
        
    }

    public void Btn_Exit()
    {
        Debug.Log("Exit");
        if(ExitEvent != null)
        {
            ExitEvent();
        }
    }

    public void Btn_Back()
    {
        Debug.Log("Hide");
        Hide();
    }

   

}
