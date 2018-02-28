using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WT.UI;
using System;

public class PokerMainControl  {

    private PokerMain pokerMain;
    
	//构造函数
    public PokerMainControl()
    {
        pokerMain = new PokerMain();

        //桌面
        pokerMain.AddTableEventEvernt(ShowTable);
        //快速开始
        pokerMain.AddFastEvent(ShowFastRoom);
        //设置
        pokerMain.AddSettingEvent(ShowSetting);

    }

    #region  事件

    //打开桌界面事件
    public Action TableEvent = null;
    public void AddTableEvent(Action method)
    {
        TableEvent = method;
    }

    //打开快速房间界面
    public Action FastRoomEvent = null;
    public void AddFastRoomEvent(Action action)
    {
        FastRoomEvent = action;
    }

    //打开设置界面
    public Action SettingEvent = null;
    public void AddSettingEvent(Action action)
    {
        SettingEvent = action;
    }

    #endregion

    /// <summary>
    /// 显示主界面
    /// </summary>
    public void ShowPokerMain()
    {
        WTUIPage.ShowPage("PokerMain", pokerMain);
    }


    /// <summary>
    /// 打开设置界面
    /// </summary>
    public void ShowSetting()
    {
        if(SettingEvent != null)
        {
            SettingEvent();
        }
    }

    /// <summary>
    /// 打开快速开始房间界面
    /// </summary>
    public void ShowFastRoom()
    {
       if(FastRoomEvent != null)
        {
            FastRoomEvent();
        }
    }

    /// <summary>
    /// 打开桌面界面
    /// </summary>
    public void ShowTable()
    {
        if (TableEvent != null)
        {
            TableEvent();
        }            
    }
}
