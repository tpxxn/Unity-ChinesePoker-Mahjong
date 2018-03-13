using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WT.UI;

public class PokerTable : WTUIPage
{

    #region 玩家控制变量
    public Button btnForlord;
    public Button btnNotlord;
    public Button btnPlay;
    public Button btnNotPlay;
    public Button btnStart;
    public Button btnRestart;
    public Button btnSetting;
    public GameObject panelControl;
    public GameObject arm;
    public GameObject lordImg;
    public GameObject[] hint = new GameObject[3];
    #endregion

    //桌面
    public GameObject table;
    //牌界面(UI)
    public List<Card> myUiCard = new List<Card>();

    /// <summary>
    /// 构造函数
    /// </summary>
    public PokerTable() : base(UIType.Normal, UIMode.DoNothing, UICollider.None)
    {
        uiIndex = R.Prefab.POKERTABLE;
    }

    #region Action

    //开始游戏
    public Action StartGameEvent = null;
    public void AddStartGameEvent(Action action)
    {
        StartGameEvent = action;
    }

    //重新开始游戏
    public Action ReStartGameEvent = null;
    public void AddReStartGameEvent(Action action)
    {
        ReStartGameEvent = action;
    }

    //打开设置
    public Action SettingEvetn = null;
    public void AddSettingEvent(Action action)
    {
        SettingEvetn = action;
    }

    //玩家抢地主按钮
    public Action PlayerLordEvent = null;
    public void AddPlayerLordEvent(Action action)
    {
        PlayerLordEvent = action;
    }

    //玩家不抢地主按钮
    public Action PlayerNotLordEvent = null;
    public void AddPlayerNotLordEvent(Action action)
    {
        PlayerNotLordEvent = action;
    }

    //玩家出牌按钮
    public Action PlayerPlayEvent = null;
    public void AddPlayerPlayEvent(Action action)
    {
        PlayerPlayEvent = action;
    }

    //玩家不出牌按钮
    public Action PlayerNotPlayEvent = null;
    public void AddPlayerNotPlayEvent(Action action)
    {
        PlayerNotPlayEvent = action;
    }

    #endregion

    /// <summary>
    /// awake方法
    /// </summary>
    /// <param name="go"></param>
    public override void Awake(GameObject go)
    {
        #region GameObject Find
        GameObject.Find("Content/Button_back").GetComponent<Button>().onClick.AddListener(Btn_Back);
        btnRestart = GameObject.Find("Content/Button_restart").GetComponent<Button>();
        btnForlord = GameObject.Find("Content/Panel_player/Button_forlord").GetComponent<Button>();
        btnNotlord = GameObject.Find("Content/Panel_player/Button_notlord").GetComponent<Button>();
        btnPlay = GameObject.Find("Content/Panel_player/Button_play").GetComponent<Button>();
        btnNotPlay = GameObject.Find("Content/Panel_player/Button_notplay").GetComponent<Button>();
        btnStart = GameObject.Find("Content/Button_start").GetComponent<Button>();
        btnSetting = GameObject.Find("Content/Button_tablesetting").GetComponent<Button>();

        //玩家控制界面的获取
        panelControl = GameObject.Find("Content/Panel_player");
        arm = GameObject.Find("Content/Panel_player/Alarm");
        table = GameObject.Find("Content/Table");
        lordImg = GameObject.Find("Content/Lord");

        //提示信息
        hint[0] = GameObject.Find("Content/Hint/Text_hint");
        hint[1] = GameObject.Find("Content/Hint/Text_hint1");
        hint[2] = GameObject.Find("Content/Hint/Text_hint2");

        #endregion

        #region 按钮事件绑定
        btnStart.onClick.AddListener(Btn_StartGame);
        btnRestart.onClick.AddListener(Btn_Restart);
        btnSetting.onClick.AddListener(Btn_Setting);
        btnForlord.onClick.AddListener(Btn_ForLord);
        btnNotlord.onClick.AddListener(Btn_NotLord);
        btnPlay.onClick.AddListener(Btn_PlayCards);
        btnNotPlay.onClick.AddListener(Btn_NotPlay);
        #endregion

        //一些初始化
        lordImg.SetActive(false);
        btnRestart.gameObject.SetActive(false);
        panelControl.SetActive(false);


    }

    #region 按钮事件

    /// <summary>
    /// 返回大厅
    /// </summary>
    public void Btn_Back()
    {
        Hide();
    }

    /// <summary>
    /// 开始游戏
    /// </summary>
    public void Btn_StartGame()
    {
        if (StartGameEvent != null)
        {
            StartGameEvent();
        }
        //点击开始游戏后再显示重新开始按钮
        btnRestart.gameObject.SetActive(true);
    }

    /// <summary>
    /// 重新开始游戏
    /// </summary>
    public void Btn_Restart()
    {
        if (ReStartGameEvent != null)
        {
            ReStartGameEvent();
        }
    }

    /// <summary>
    /// 桌面设置界面
    /// </summary>
    public void Btn_Setting()
    {
        if (SettingEvetn != null)
        {
            SettingEvetn();
        }
    }

    /// <summary>
    /// 抢地主监听事件
    /// </summary>
    public void Btn_ForLord()
    {
        if (PlayerLordEvent != null)
        {
            PlayerLordEvent();
        }
    }

    /// <summary>
    /// 不抢地主监听事件
    /// </summary>
    public void Btn_NotLord()
    {
        if (PlayerNotLordEvent != null)
        {
            PlayerNotLordEvent();
        }
    }

    /// <summary>
    /// 出牌监听事件
    /// </summary>
    public void Btn_PlayCards()
    {
        if (PlayerPlayEvent != null)
        {
            PlayerPlayEvent();
        }
    }

    /// <summary>
    /// 不出牌监听事件
    /// </summary>
    public void Btn_NotPlay()
    {
        if (PlayerNotPlayEvent != null)
        {
            PlayerNotPlayEvent();
        }
    }

    #endregion


    /// <summary>
    /// 实例化扑克牌到桌面（UI）
    /// </summary>
    public List<CardInfo> InstaceCards(List<CardInfo> list)
    {
        //实例化到桌面
        GameObject obj = WTUIPage.delegateSyncLoadUIByLocal(R.Prefab.POKER) as GameObject;
        for (int i = 0; i < list.Count; i++)
        {
            GameObject o = GameObject.Instantiate(obj, table.transform.position, Quaternion.identity, transform) as GameObject;
            Card c = o.GetComponent<Card>();
            list[i].pos = table.transform.position;
            list[i].parent = table.transform;
            c.cardName = list[i].cardName;
            myUiCard.Add(c);
        }

        return list;

    }


    /// <summary>
    /// 初始化图片
    /// </summary>
    /// <param name="cardInfo"></param>
    public Sprite InitImage(CardInfo cardInfo, bool isFront)
    {
        string path;
        Sprite sprite = null;

        if (isFront)
        {
            sprite = FileIO.LoadSprite(54);
        }
        else
        {
            //由于没有提供根据名字来加载的方法，所以只能自己通过名字取index来加载
            int i = 0;
            path = "lfx/table/spritepack/cards/" + cardInfo.cardName;
            for (i = 0; i < R.SpritePack.path.Length; i++)
            {
                if (path == R.SpritePack.path[i])
                {
                    break;
                }
            }
            sprite = FileIO.LoadSprite(i);
        }

        //设置图片
        GetCardObject(cardInfo).image.sprite = sprite;

        if (sprite == null)
            return null;
        else
            return sprite;

    }

    /// <summary>
    /// 显示玩家排序好的手牌（TableUI中完成）
    /// </summary>
    public void ShowCards(List<CardInfo> myCardInfo)
    {
        if (myCardInfo.Count == 0)
        {
            Debug.Log("MyCards is Empty!");
            return;
        }

        //牌的显示位置
        Transform myTrans = myCardInfo[0].parent;
        List<Vector3> pos = new List<Vector3>();
        //牌所在的位置
        Vector3 basePos = Vector3.zero;
        basePos = myTrans.position + new Vector3(-myCardInfo.Count * 0.43f, 0, 0);

        pos.Clear();

        for (int i = 0; i < myCardInfo.Count; i++)
        {
            basePos.x += 0.8f;
            pos.Add(basePos);
        }

        for (int i = 0; i < myCardInfo.Count; i++)
        {
            //加载对应的图片
            InitImage(myCardInfo[i], false);
            //移动到指定位置
            MoveCard(myCardInfo[i], pos[i], 0.5f);
        }


    }

    /// <summary>
    /// 显示地主牌
    /// </summary>
    public void ShowLordCards(List<CardInfo> tableCards)
    {
        Vector3 pos = new Vector3(2, 0, 0);
        Vector3 pos1 = GetCardObject(tableCards[51]).GetComponent<Transform>().position + pos;
        Vector3 pos2 = GetCardObject(tableCards[51]).GetComponent<Transform>().position - pos;

        MoveCard(tableCards[51], pos1, 0.5f);
        MoveCard(tableCards[53], pos2, 0.5f);
    }

    /// <summary>
    /// 玩家操作选项和计时器的显示控制
    /// </summary>
    /// <param name="isLord"></param>
    /// <param name="isPlay"></param>
    /// <param name="player"></param>
    public void ShowOption(bool isLord, bool isPlay, BasePlayer player, List<CardInfo> lastCards)
    {
        panelControl.SetActive(true);

        SetButton(false, false, true, lastCards);
        //抢地主
        if (isLord)
        {
            if (player.ToString() == "Player")
            {
                SetButton(true, false, true, lastCards);
                panelControl.transform.position = player.myTrans.position + new Vector3(0, 2.5f, 0);
            }
        }
        //出牌
        if (isPlay)
        {
            if (player.ToString() == "Player")
            {
                SetButton(false, true, true, lastCards);
                panelControl.transform.position = player.myTrans.position + new Vector3(0, 2.5f, 0);
            }
        }

        if (player.ToString() == "PlayerComputer1")
        {
            panelControl.transform.position = player.myTrans.position + new Vector3(-2, 0.3f, 0);
        }
        if (player.ToString() == "PlayerComputer2")
        {
            panelControl.transform.position = player.myTrans.position + new Vector3(2, 0.3f, 0);
        }
    }

    /// <summary>
    /// 玩家按钮的状态显示
    /// </summary>
    /// <param name="lord"></param>
    /// <param name="notlord"></param>
    /// <param name="play"></param>
    /// <param name="notplay"></param>
    public void SetButton(bool lord, bool play, bool alarm, List<CardInfo> lastCards)
    {
        btnForlord.gameObject.SetActive(lord);
        btnNotlord.gameObject.SetActive(lord);
        btnPlay.gameObject.SetActive(play);
        btnNotPlay.gameObject.SetActive(play);
        arm.gameObject.SetActive(alarm);

        //如果上一家没出，那么我必须要出，设置不出按钮不可按
        if (play && lastCards.Count == 0)
        {
            btnNotPlay.enabled = false;
        }
        else
        {
            btnNotPlay.enabled = true;
        }

    }

    /// <summary>
    /// 设置提示信息
    /// </summary>
    /// <param name="text"></param>
    /// <param name="index"></param>
    public void SetHintText(string text, int index)
    {
        hint[index].GetComponent<Text>().text = text;
    }

    /// <summary>
    /// 地主图标显示位置
    /// </summary>
    public void SetLordImg(int playerIndex, List<BasePlayer> players)
    {
        lordImg.SetActive(true);
        if (playerIndex % 3 == 0)
            lordImg.transform.position =
                players[playerIndex % 3].myTrans.position + new Vector3(-4, 2.3f, 0);
        if (playerIndex % 3 == 1 || playerIndex % 3 == 2)
            lordImg.transform.position =
                players[playerIndex % 3].myTrans.position + new Vector3(0, 2, 0);
    }

    /// <summary>
    /// 设置开始游戏重开游戏按钮
    /// </summary>
    public void SetStartButton(bool start, bool restart)
    {
        btnStart.gameObject.SetActive(start);
        btnRestart.gameObject.SetActive(restart);
    }

    /// <summary>
    /// 设置不出牌按钮状态
    /// </summary>
    /// <param name="show"></param>
    public void SetNotPlayBtn(bool show)
    {
        btnNotPlay.gameObject.SetActive(show);
    }

    /// <summary>
    /// 设置地主图片
    /// </summary>
    /// <param name="show"></param>
    public void SetLordImg(bool show)
    {
        lordImg.SetActive(show);
    }

    /// <summary>
    /// 根据数据返回卡牌物体
    /// </summary>
    /// <param name="cardInfo"></param>
    /// <returns></returns>
    public Card GetCardObject(CardInfo cardInfo)
    {
        Card card = null;
        foreach (var c in myUiCard)
        {
            if (c.cardName == cardInfo.cardName)
            {
                card = c;
            }
        }

        return card;
    }

    /// <summary>
    /// 设置卡牌的父节点
    /// </summary>
    /// <param name="cardInfo"></param>
    public void SetCardParent(CardInfo cardInfo)
    {
        Transform trans = GetCardObject(cardInfo).GetComponent<Transform>();
        trans.SetParent(cardInfo.parent);
    }

    /// <summary>
    /// 清除卡牌父节点
    /// </summary>
    /// <param name="cardInfo"></param>
    public void ClearCardParent(List<CardInfo> cardInfo)
    {
        for (int i = 0; i < cardInfo.Count; i++)
        {
            GetCardObject(cardInfo[i]).GetComponent<Transform>().SetParent(table.transform);
        }

    }

    /// <summary>
    /// 更新玩家的卡牌到玩家的位置
    /// </summary>
    /// <param name="cardInfo"></param>
    /// <param name="time"></param>
    public void MoveCard(CardInfo cardInfo, float time)
    {
        var endPos = cardInfo.parent.transform.position;
        var trans = GetCardObject(cardInfo).GetComponent<Transform>();

        UnitTool.ToolStartCoroutine(MoveObject(endPos, time, trans));
    }

    /// <summary>
    /// 更新卡牌到数据指定的位置
    /// </summary>
    /// <param name="cardInfo"></param>
    /// <param name="time"></param>
    public void MoveCard(CardInfo cardInfo, Vector3 endPos, float time)
    {
        var trans = GetCardObject(cardInfo).GetComponent<Transform>();

        UnitTool.ToolStartCoroutine(MoveObject(endPos, time, trans));
    }

    /// <summary>
    /// 移动协程
    /// </summary>
    /// <param name="startPos"></param>
    /// <param name="endPos"></param>
    /// <param name="time"></param>
    /// <returns></returns>
    IEnumerator MoveObject(Vector3 endPos, float time, Transform trans)
    {
        var dur = 0.0f;
        while (dur <= time)
        {
            dur += Time.deltaTime;
            trans.position = Vector3.Lerp(trans.position, endPos, dur / time);
            yield return null;
        }
    }





}
