using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WT.UI;

public class PokerMain : WTUIPage {

    /// <summary>
    /// 所有的按钮和计时器显示
    /// </summary>

    //构造函数
	public PokerMain():base(UIType.Normal,UIMode.DoNothing,UICollider.None)
    {
        uiIndex = R.Prefab.POKERMAIN;
    }

    #region
    //进入设置界面
    public event Action SettingEvetn;
    public void AddSettingEvent(Action method)
    {
        SettingEvetn = method;
    }

    //进入快速开始房间
    public Action FastEvent = null;
    public void AddFastEvent(Action action)
    {
        FastEvent = action;
    }

    //进入加入游戏界面
    public Action JoinEvent = null;
    public void AddJoinEvent(Action action)
    {
        JoinEvent = action;
    }

    //进入创建房间界面
    public Action TableEvent = null;
    public void AddTableEventEvernt(Action action)
    {
        TableEvent = action;
    }

    #endregion

    public override void Awake(GameObject go)
    {
        #region GameObject Find
        GameObject.Find("Content/Button_setting").GetComponent<Button>().onClick.AddListener(Btn_Setting);
        GameObject.Find("Content/Panel_poker/Button_fast").GetComponent<Button>().onClick.AddListener(Btn_In);
        GameObject.Find("Content/Panel_join/Button").GetComponent<Button>().onClick.AddListener(Btn_Join);
        GameObject.Find("Content/Panel_create/Button").GetComponent<Button>().onClick.AddListener(Btn_Create);
        //GameObject.Find("Content/Button_restart").GetComponent<Button>().onClick.AddListener(Btn_Restart);
        #endregion
    }

    /// <summary>
    /// 打开设置面板
    /// </summary>
    public void Btn_Setting()
    {
        if (SettingEvetn != null)
        {
            SettingEvetn();
        }
    }


    /// <summary>
    /// 进入快速开始界面
    /// </summary>
    public void Btn_In()
    {
        if(FastEvent != null)
        {
            FastEvent();
        }
    }

    /// <summary>
    /// 打开加入游戏界面
    /// </summary>
    public void Btn_Join()
    {
        if(JoinEvent != null)
        {
            JoinEvent();
        }
    }

    /// <summary>
    /// 创建游戏界面
    /// </summary>
    public void Btn_Create()
    {
        if(TableEvent != null)
        {
            TableEvent();
        }
    }
}
