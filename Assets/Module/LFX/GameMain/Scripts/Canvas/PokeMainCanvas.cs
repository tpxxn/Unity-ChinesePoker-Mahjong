using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WT.UI;

public class PokeMainCanvas : BaseCanvas, IHandlerReceive
{
    //登陆界面
    private PokerLoginControl pokerLoginControl;
    //主界面
    private PokerMainControl pokerMainControl;
    //斗地主桌界面
    private PokerTableControl pokerTableControl;
    //麻将卓界面
    private MajiangTableControl majiangTableControl;
    //快速开始界面
    private PokerFastRoomControl pokerFastRoomControl;
    //设置界面
    private PokerSettingControl pokerSettingControl;
    //输赢界面
    private PokerWinControl pokerWinControl;


    void Awake()
    {
        AddHandlerReceiveEvent(this);//注册

        #region 协程

        UnitTool.AddToolStartCoroutine(U3dCoroutine);
        UnitTool.AddToolStopAllCoroutines(StopAllCoroutines);

        #endregion

        #region 控制器

        //登陆界面控制器
        pokerLoginControl = new PokerLoginControl();
        //主界面控制器
        pokerMainControl = new PokerMainControl();
        //斗地主桌面控制器
        pokerTableControl = new PokerTableControl();
        //麻将桌控制器
        majiangTableControl = new MajiangTableControl();
        //快速开始界面控制器
        pokerFastRoomControl = new PokerFastRoomControl();
        //设置界面控制器
        pokerSettingControl = new PokerSettingControl();
        //输赢界面控制器
        pokerWinControl = new PokerWinControl();

        #endregion


        //打开登陆界面
        ShowLoginUI();

        //打开主界面
        pokerLoginControl.AddMainEvent(OpenMainCallBack);

        //打开桌界面
        pokerMainControl.AddTableEvent(OpenTableCallBack);

        //打开麻将界面
        pokerMainControl.AddMajiangEvent(OpenMajiangCallBack);

        //打开快速开始界面
        pokerMainControl.AddFastRoomEvent(OpenFastRoomCallBack);

        //打开输赢界面
        pokerTableControl.AddWinEvent(OpenWinCallBack);
        majiangTableControl.AddGameOverEvent(OpenWinCallBack);

        //打开设置界面
        pokerMainControl.AddSettingEvent(OpenSettingCallBack);
        pokerTableControl.AddSettingEvent(OpenSettingCallBack);
        majiangTableControl.AddSettingEvent(OpenSettingCallBack);
        

    }

    public void U3dCoroutine(IEnumerator enumerator)
    {
        StartCoroutine(enumerator);
    }

    public Response RunServerReceive()
    {
        return null;
    }

    protected override void RunUpdate()
    {
        
    }

    public Response RunServerReceive(Response response)
    {
        return null;
    }

    //打开登陆界面
    void ShowLoginUI()
    {
        pokerLoginControl.ShowLoginUI();
 
    }
    //Main界面的回调函数
    public void OpenMainCallBack()
    {
        pokerMainControl.ShowPokerMain();
    }
    
    //Table界面的回调函数
    public void OpenTableCallBack()
    {
        pokerTableControl.ShowTable();
    }

    //FastRoom界面的回调函数
    public void OpenFastRoomCallBack()
    {
        pokerFastRoomControl.ShowFastRoom();
    }

    //Setting界面的回调函数
    public void OpenSettingCallBack()
    {
        pokerSettingControl.ShowSetting();
    }

    //Win界面的回调函数
    public void OpenWinCallBack(string info)
    {
        pokerWinControl.ShowWinUI(info);
    }

    //麻将界面回调函数
    public void OpenMajiangCallBack()
    {
        majiangTableControl.ShowMajiangUI();
    }


}
