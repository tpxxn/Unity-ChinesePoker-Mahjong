using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WT.UI;
using System;

public class PokerLoginControl
{
    private PokeLogin pokeLogin;

    //构造函数
    public PokerLoginControl()
    {
        pokeLogin = new PokeLogin();

        pokeLogin.AddLoginEvent(BtnLoginCallBack);
    }

    #region  事件
    public Action MainEvent = null;
    public void AddMainEvent(Action method)
    {
        MainEvent = method;
    }
    #endregion

    //显示LoginUI
    public void ShowLoginUI()
    {
        WTUIPage.ShowPage("PokerLoginUI", pokeLogin);
    }

    //登陆回调函数
    private string BtnLoginCallBack(string usr, string pwd)
    {
        if (MainEvent != null)
        {
            MainEvent();
        }

        //返回登陆成功的信息
        return "LOGIN_OK";
    }

}
