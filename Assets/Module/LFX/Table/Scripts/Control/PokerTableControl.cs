using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WT.UI;

/// <summary>
/// 桌面控制
/// </summary>
public class PokerTableControl
{

    #region 变量定义
    //桌面
    public PokerTable pokerTable;
    //桌面跑数据
    public List<CardInfo> tableCards = new List<CardInfo>();
    //桌面上的玩家
    public List<BasePlayer> players = new List<BasePlayer>();
    //上个玩家出的牌
    public List<CardInfo> lastCards = new List<CardInfo>();
    //上玩家的牌型
    public DDZ_POKER_TYPE lastType;
    //玩家索引
    public int startIndex;
    public int playerIndex;
    //叫地主次数
    public int lordTimes = 0;
    //是否重来
    public bool isRestart = false;
    //是否在叫地主
    public bool isLord = true;
    #endregion

    /* 
     * # 流程
     * 1.桌控制器生成桌面牌数据，UI根据该数据生成卡牌到桌面(Done)
     * 2.桌控制器将tableCards发给三个玩家，添加到三个玩家的手牌数据List中，UI完成发牌的动画
     * 3.需要排序手牌（桌控制器）然后显示手牌(UI)
     * 4.叫地主（玩家只能通过桌控制器提供的特定的接口来叫地主，桌控制器记录叫地主信息，之后决定谁是地主还是重新开）
     * 5.叫地主完成，将地主牌数据添加到地主，UI更新卡牌动画
     * 6.开始出牌，地主先出（必须出，如果玩家在规定时间内没选择，系统默认出第一张）,选择要出的牌的数据，提交控桌控制器验证，通过，添加数据到桌面，UI更新动画
     * 7.玩家不出牌或提交卡牌数据未通过，时间结束或点击不出牌，桌控制器让下一个玩家出牌
     * 8.电脑玩家出牌由桌控制器的AI自动出牌，不需要要二次验证
     * 9.玩家出牌时提交出牌数据到桌控制器，控制器验证
     * 10.每完成一次出牌，桌控制器验证该玩家是否胜利，如胜利，向MainCavas控制器请求打开胜利界面
     * 11.用户选择重新开始或退出
     * 
     * # 说明
     * 1.桌控制器和玩家只有CardInfo数据List，UI中使用一个List<Card>用来对应卡牌数据的更新
     * 2.所有的卡牌移动、添加图片、提示信息的显示、玩家操作选项显示都是在桌控制器中控制，控制器调用UI中相应模块完成显示
     * 3.玩家和电脑、地主和出牌都只能向桌控制器发送相应的请求，桌控制器得到请求信息后控制叫地主、出牌的控制
     * 4.电脑AI出牌部分放在了桌控制器中，桌控制器也相当于电脑AI，这样可以不需要进行二次桌验证
     * 5.对于PlayeInfo类，应该是在网络版斗地主中保存玩家的个人信息，而BasePlayer类保存的是当前桌面上该玩家游戏的数据（如玩家手牌，在离开该局游戏后就销毁了）
    */

    /// <summary>
    /// 构造函数
    /// </summary>
    public PokerTableControl()
    {

        pokerTable = new PokerTable();

        //随机玩家索引
        startIndex = MyUtil.GetRange(0, 3);
        playerIndex = startIndex;

        //实例化三个玩家
        players.Add(new Player());
        players.Add(new PlayerComputer1());
        players.Add(new PlayerComputer2());

        //代理事件添加
        AddAction();
        
    }

    #region Action
    //设置
    public Action SettingEvent = null;
    public void AddSettingEvent(Action action)
    {
        SettingEvent = action;
    }
    //输赢界面
    public Action<string> WinEvent = null;
    public void AddWinEvent(Action<string> action)
    {
        WinEvent = action;
    }
    #endregion

    /// <summary>
    /// 显示桌面界面
    /// </summary>
    public void ShowTable()
    {
        WTUIPage.ShowPage("PokerTable", pokerTable);
    }

    /// <summary>
    /// 显示设置界面
    /// </summary>
    public void ShowSetting()
    {
        if(SettingEvent != null)
        {
            SettingEvent();
        }
    }

    /// <summary>
    /// 绑定代理事件
    /// </summary>
    private void AddAction()
    {
        pokerTable.AddStartGameEvent(StartGame);
        pokerTable.AddReStartGameEvent(RestartGame);
        pokerTable.AddSettingEvent(ShowSetting);

        //玩家按钮按钮事件
        pokerTable.AddPlayerLordEvent(PlayerClickLord);
        pokerTable.AddPlayerNotLordEvent(PlayerNotClickLord);
        pokerTable.AddPlayerPlayEvent(players[0].PlayCards);
        pokerTable.AddPlayerNotPlayEvent(players[0].NotPlay);
        
        for (int i = 0; i < players.Count; i++)
        {
            //显示时间
            players[i].AddShowTimeEvent(SetTimeText);
            //玩家不叫地主
            players[i].AddNotLordEvent(NotLord);
            //玩家叫地主
            players[i].AddToLordEvent(ToLord);
            //开始出牌
            players[i].AddStartPlayEvent(PlayerStartPlay);
            //桌控制器验证是否能出牌
            players[i].AddPlayEvent(CheckToCards);
            //玩家可以出牌
            players[i].AddCanPlayEvent(CanPlayCards);
            //玩家不出牌
            players[i].AddNotPlayEvent(NotPlayCards);
            //AI出牌
            players[i].AddAIPlayEvent(AIPlayCards);

        }
    }

    /// <summary>
    /// 初始化牌到桌面
    /// </summary>
    public void StartGame()
    {
        //重新开始清除数据
        ClearDatas();

        //第一个发牌的玩家
        playerIndex = startIndex % 3;
        //初始化扑克牌信息
        tableCards = InitCards();
        //洗牌
        tableCards = GetRandomList<CardInfo>(tableCards);
        //实例化到桌面
        tableCards = pokerTable.InstaceCards(tableCards);
        //发牌协程
        UnitTool.ToolStartCoroutine(SendCards());

    }

    /// <summary>
    /// 初始化扑克牌(Control)
    /// </summary>
    public List<CardInfo> InitCards()
    {
        List<CardInfo> cardInfos = new List<CardInfo>();
        for (int i = 0; i < 54; i++)
        {
            //这里面也可以先不用读图集获取名字，直接设置好名字也可以
            Sprite o = FileIO.LoadSprite(i);
            CardInfo cardInfo = new CardInfo(o.name);
            cardInfos.Add(cardInfo);
            
        }
        return cardInfos;
    }

    /// <summary>
    /// 将桌面上的牌发给三个玩家
    /// </summary>
    public IEnumerator SendCards()
    {
        SetStartButton(false, true);

        //发51张牌给三个玩家
        for (int i = 0; i < 51; i++)
        {
            players[(i + playerIndex) % 3].GetPlayer();
            players[(i + playerIndex) % 3].AddCard(tableCards[i]);
            //玩家点击扑克牌事件
            pokerTable.GetCardObject(tableCards[i]).AddSetSelectEvent(SetSelect);
            pokerTable.GetCardObject(tableCards[i]).AddIsLordEvent(SetIsLord);
            //UI同步更新发牌动画
            yield return new WaitForSeconds(0.1f);
            pokerTable.MoveCard(tableCards[i], 0.5f);
        }
        yield return new WaitForSeconds(2f);
        //设置玩家
        InitPlayers();
        //显示牌
        ShowCards();
        //显示完成,开始抢地主
        yield return new WaitForSeconds(2f);
        StartForLord();
    }

    /// <summary>
    /// 显示牌
    /// </summary>
    public void ShowCards()
    {
        //显示玩家的手牌
        pokerTable.ShowCards(players[0].myCardInfo);
        //显示地主牌
        pokerTable.ShowLordCards(tableCards);
    }

    /// <summary>
    /// 开始叫地主
    /// </summary>
    public void StartForLord()
    {
        isLord = true;
        //判断谁是地主，如果没有出来那么就继续叫地主
        if (!JudgeLord())
            return;

        playerIndex++;
        playerIndex = playerIndex % 3;
        //Debug.Log(playerIndex);
        //抢地主前先关闭所有协程
        UnitTool.ToolStopAllCoroutines();
        //当前玩家开始叫地主
        players[playerIndex].ToLord();
        //显示叫地主图标
        ShowOption(true, false, players[playerIndex]);

    }

    /// <summary>
    /// 玩家不叫地主
    /// </summary>
    /// <param name="player"></param>
    public void NotLord()
    {
        //玩家不叫地主，开始下一个玩家叫地主
        SetHintText("不抢", playerIndex % 3);
        StartForLord();
    }

    /// <summary>
    /// 玩家叫地主
    /// </summary>
    /// <param name="player"></param>
    public void ToLord()
    {
        //玩家叫地主，开始下一个玩家叫地主
        SetHintText("抢地主", playerIndex % 3);
        StartForLord();
    }

    /// <summary>
    /// 玩家抢到地主
    /// </summary>
    public void ForLord()
    {
        //清除抢地主留下的信息，以免重开时未重置
        ClearLordDatas();
        Transform parent;

        //抢地主成功,当前playerIndex索引玩家为地主
        UnitTool.ToolStopAllCoroutines();
        SetButton(false, false, false);


        //拿到地主的位置
        parent = players[playerIndex % 3].GetPlayer();
        //重新排序地主的牌
        players[playerIndex % 3].myCardInfo = SortCards(players[playerIndex % 3].myCardInfo);
        
        if (playerIndex % 3 == 0)
        {
            //地主是玩家
            for (int i = 51; i < 54; i++)
            {
                players[0].AddCard(tableCards[i]);
                //玩家点击扑克牌市斤添加
                pokerTable.GetCardObject(tableCards[i]).AddSetSelectEvent(SetSelect);
            }

            //UI同步更新
            pokerTable.ClearCardParent(players[0].myCardInfo);
            InitPlayers();
            pokerTable.ShowCards(players[0].myCardInfo);
        }
        else
        {
            //地主是电脑
            for (int i = 51; i < 54; i++)
            {
                players[playerIndex % 3].AddCard(tableCards[i]);
                pokerTable.MoveCard(tableCards[i], 0.5f);
            }
        }

        tableCards.Clear();
        SetLordImg();

        //地主开始出牌
        //标志谁最先出牌
        startIndex = playerIndex;

        //由于下面的开始游戏前会自动增加玩家索引，所以这里玩家索引递减一个
        if (playerIndex != 0)
            playerIndex--;
        else
            playerIndex += 2;
        isLord = false;

        //开始出牌
        StartPlay();
    }

    /// <summary>
    /// 玩家点击叫地主
    /// </summary>
    public void PlayerClickLord()
    {
        SetHintText("抢地主", 0);
        players[0].loadLord = true;
        players[0].clickLord = true;
        StartForLord();
    }

    /// <summary>
    /// 玩家点击不叫地主
    /// </summary>
    public void PlayerNotClickLord()
    {
        UnitTool.ToolStopAllCoroutines();
        SetHintText("不抢", 0);
        StartForLord();
    }

    /// <summary>
    /// 是否在叫地主
    /// </summary>
    /// <returns></returns>
    public bool SetIsLord()
    {
        return isLord;
    }

    /// <summary>
    /// 验证玩家准备出的牌
    /// </summary>
    /// <param name="toPlayCards"></param>
    /// <returns></returns>
    public bool CheckToCards()
    {
        //获取当前手牌的大小
        List<int> myList = new List<int>();
        for (int i = 0; i < players[playerIndex%3].myCardInfo.Count; i++)
        {
            if(players[playerIndex % 3].myCardInfo[i].isSelected)
                myList.Add(players[playerIndex % 3].myCardInfo[i].cardIndex);
        }

        //获取上个玩家手牌的大小
        List<int> last = new List<int>();
        if (lastCards.Count != 0)
        {
            for (int i = 0; i < lastCards.Count; i++)
            {
                last.Add(lastCards[i].cardIndex);
            }
        }
        //要排序
        myList.Sort();
        last.Sort();

        //不符合重新出
        if (!PokerRules.PopEnable(myList, out players[playerIndex%3].myType))
        {
            Debug.Log("出的牌不符合规则");
            return false;
        }
        //合法的话，如果不是第一次出，判断是不是比上家的牌大
        if (lastCards.Count != 0)
        {
            if (!PokerRules.isSelectCardCanPut(myList, players[playerIndex % 3].myType, last, lastType))
            {
                //比上家小，不出
                if(playerIndex % 3 != 0)
                    NotPlayCards();
                Debug.Log("出的牌比上家小或者牌型不对");
                return false;
            }

        }
        return true;
    }

    /// <summary>
    /// 确定能够出牌
    /// </summary>
    public void CanPlayCards()
    {
        //清除桌面上的牌
        if (tableCards.Count != 0)
        {
            for (int i = 0; i < tableCards.Count; i++)
            {
                pokerTable.MoveCard(tableCards[i], new Vector3(0, 0, 0), 0.01f);
            }
        }

        tableCards.Clear();

        List<int> list = new List<int>();
        list.Clear();
        //将要出的牌添加到桌面List中并排序
        for (int i = 0; i < players[playerIndex % 3].myCardInfo.Count; i++)
        {
            if (players[playerIndex % 3].myCardInfo[i].isSelected)
            {
                tableCards.Add(players[playerIndex % 3].myCardInfo[i]);
                list.Add(players[playerIndex % 3].myCardInfo[i].cardIndex);
            }
        }

        //tableCards = SortCards(tableCards);

        //保存上个玩家出的牌的信息
        lastCards.Clear();

        PokerRules.PopEnable(list, out players[playerIndex % 3].myType);
        lastType = players[playerIndex % 3].myType;

        for (int i = 0; i < tableCards.Count; i++)
        {
            lastCards.Add(tableCards[i]);
        }

        //移动要出的牌到指定的位置，并清除myCards中出去的牌
        GameObject table = pokerTable.table;
        Vector3 pos = table.transform.position + new Vector3(-tableCards.Count * 0.48f, 0, 0);
        for (int i = 0 ; i < tableCards.Count; i++)
        {
            tableCards[i].parent = table.transform;
            pokerTable.SetCardParent(tableCards[i]);
            pokerTable.InitImage(tableCards[i], false);
            pokerTable.MoveCard(tableCards[i], pos += new Vector3(0.78f, 0, 0), 0.5f);
            players[playerIndex % 3].myCardInfo.Remove(tableCards[i]);
        }

        //判断是否获胜
        if (players[playerIndex % 3].myCardInfo.Count == 0)
        {
            UnitTool.ToolStartCoroutine(Win(players[playerIndex % 3]));
            return;
        }

        //是玩家出牌
        if(playerIndex % 3 == 0)
        {
            //对当前手牌进行重新显示
            pokerTable.ShowCards(players[0].myCardInfo);
        }

        //当前玩家轮回结束
        players[playerIndex % 3].myTerm = true;

        //开始下一个玩家的出牌
        StartPlay();
    }

    /// <summary>
    /// 玩家不出牌
    /// </summary>
    public void NotPlayCards()
    {
        if (playerIndex % 3 == 0)
        {
            //玩家不出牌
            if (lastCards.Count != 0)
            {
                //上个玩家出牌了
                Debug.Log(players[0].ToString() + "不要");
                players[playerIndex % 3].myTerm = false;
                //lastType = players[playerIndex % 3].myType;

                //不出牌的时候，将选中出列的牌归位
                for (int i = 0; i < players[0].myCardInfo.Count; i++)
                {
                    if (players[0].myCardInfo[i].isSelected)
                        pokerTable.GetCardObject(players[0].myCardInfo[i]).SetSelectState();
                }

                UnitTool.ToolStopAllCoroutines();
                StartPlay();
            }
            else
            {
                //自己的出牌回合，时间结束，默认系统出玩家的第一张牌
                lastType = DDZ_POKER_TYPE.SINGLE;
                players[0].myCardInfo[players[0].myCardInfo.Count-1].isSelected = true;

                CanPlayCards();
            }

        }
        else
        {
            //电脑不出牌
            Debug.Log(players[playerIndex % 3].ToString() + "不要");
            players[playerIndex % 3].myTerm = false;
            //先关闭所有协程
            UnitTool.ToolStopAllCoroutines();
            //回到开始出牌主函数
            StartPlay();
        }

    }

    /// <summary>
    /// 玩家拿到牌后的初始化
    /// </summary>
    public void InitPlayers()
    {
        for (int j = 0; j < players.Count; j++)
        {
            players[j].myCardInfo = SortCards(players[j].myCardInfo);
            for (int i = 0; i < players[j].myCardInfo.Count; i++)
            {
                pokerTable.SetCardParent(players[j].myCardInfo[i]);
            }
        }

    }

    /// <summary>
    /// 开始出牌
    /// </summary>
    public void StartPlay()
    {
        UnitTool.ToolStartCoroutine(StartPlayCoroutine());
    }

    /// <summary>
    /// 出牌协程
    /// </summary>
    /// <returns></returns>
    private IEnumerator StartPlayCoroutine()
    {
        SetButton(false, false, false);
        //这里等待一秒钟，是为了部分协程执行执行结束，不然就会同时对玩家索引进行操作
        yield return new WaitForSeconds(1f);
        UnitTool.ToolStopAllCoroutines();

        if (isRestart)
        {
            isRestart = false;
            playerIndex = startIndex;
        }
        else
        {
            playerIndex++;
        }
        players[playerIndex % 3].StartPlay();

    }

    /// <summary>
    /// 所有玩家开始出牌接口
    /// </summary>
    public void PlayerStartPlay()
    {
        //显示选项
        SetHintText(" ", 0);
        SetHintText(" ", 1);
        SetHintText(" ", 2);
        ShowOption(false, true, players[playerIndex % 3]);
        if (players[playerIndex % 3].myCardInfo.Count != 20 && (playerIndex % 3) == 0)
            SetNotPlayBtn(true);
        //我出牌，一轮下来，又到我出牌
        if (players[playerIndex % 3].myTerm)
        {
            if (players[(playerIndex + 1) % 3].myTerm == false
                && players[(playerIndex + 2) % 3].myTerm == false)
            {
                Debug.Log(players[playerIndex%3].ToString() + " 开始出牌");
                players[playerIndex % 3].myTerm = false;
                SetNotPlayBtn(false);
                lastCards.Clear();
                lastType = DDZ_POKER_TYPE.DDZ_PASS;
                for (int i = 0; i < tableCards.Count; i++)
                {
                    pokerTable.MoveCard(tableCards[i], new Vector3(0, 0, 0), 0.01f);
                }
                tableCards.Clear();
            }

        }
    }

    /// <summary>
    /// AI出牌
    /// </summary>
    /// <returns></returns>
    public bool AIPlayCards()
    {
        BasePlayer c = players[playerIndex % 3];
        //获取当前手牌的大小
        List<int> myList = new List<int>();
        for (int i = 0; i < c.myCardInfo.Count; i++)
        {
            myList.Add(c.myCardInfo[i].cardIndex);
        }

        //获取上个玩家手牌的大小
        List<int> last = new List<int>();
        if (lastCards.Count != 0)
        {
            for (int i = 0; i < lastCards.Count; i++)
            {
                last.Add(lastCards[i].cardIndex);
            }
        }
       
        //要排序
        myList.Sort();
        last.Sort();

        //所有符合该牌型的组合,存到字典中
        Dictionary<int, List<int>> result = PokerAI.FindPromptCards(myList, last, lastType);
        //是否能出牌
        if (result == null || result.Count == 0)
        {
            //Debug.Log("字典为空");
            //没有比上家大的牌,不出
            NotPlayCards();
            return false;
        }

        //所有的key
        List<int> keys = new List<int>();
        //某一个key的值
        List<int> value = new List<int>();
        //循环取到所有的key
        foreach (var item in result.Keys)
        {
            keys.Add(item);
        }

        //随机选择众多结果中的一个结果出 = =
        int vauleCount = MyUtil.GetRange(0, keys.Count);
        value = result[keys[vauleCount]];

        //如上家出333， 那么value为444，有三个4，所以在当前手牌中需要取3个4
        for (int i = 0; i < value.Count; i++)
        {
            for (int j = 0; j < c.myCardInfo.Count; j++)
            {
                if (value[i] == c.myCardInfo[j].cardIndex && c.myCardInfo[j].isSelected == false)
                {
                    c.myCardInfo[j].isSelected = true;
                    break;
                }
            }
        }

        //能出牌,获取出牌牌型
        PokerRules.PopEnable(value, out c.myType);
        lastType = c.myType;

        return true;
    }

    /// <summary>
    /// 重新开始游戏
    /// </summary>
    public void RestartGame()
    {
        SetStartButton(true, true);
        ClearDatas();
        StartGame();
    }

    /// <summary>
    /// 清除界面数据
    /// </summary>
    public void ClearDatas()
    {
        UnitTool.ToolStopAllCoroutines();
        //清除玩家相关数据
        for (int i = 0; i < players.Count; i++)
        {
            players[i].myCardInfo.Clear();
            players[i].myType = new DDZ_POKER_TYPE();
            players[i].myTerm = false;
        }

        lastType = DDZ_POKER_TYPE.DDZ_PASS;
        ClearLordDatas();
        //清除控制器相关数据
        this.tableCards.Clear();
        this.lastCards.Clear();
        this.lordTimes = 0;
        //销毁桌面上所有的扑克牌
        pokerTable.myUiCard.Clear();
        GameObject[] pokers = GameObject.FindGameObjectsWithTag("Poker");

        for (int i = 0; i < pokers.Length; i++)
        {
            GameObject.Destroy(pokers[i].gameObject);
        }

        startIndex = playerIndex % 3;
        SetLordImg(false);
        SetStartButton(false, true);
        //显示界面重置
        SetButton(false, false, false);
    }

    /// <summary>
    /// 玩家操作选项和计时器的显示控制
    /// </summary>
    /// <param name="isLord"></param>
    /// <param name="isPlay"></param>
    /// <param name="player"></param>
    public void ShowOption(bool isLord, bool isPlay, BasePlayer player)
    {
        pokerTable.ShowOption(isLord, isPlay, player, lastCards);
    }

    /// <summary>
    /// 设置玩家按钮的状态
    /// </summary>
    /// <param name="lord"></param>
    /// <param name="notlord"></param>
    /// <param name="play"></param>
    /// <param name="notplay"></param>
    public void SetButton(bool lord, bool play, bool arm)
    {
        pokerTable.SetButton(lord, play, arm, lastCards);
    }

    /// <summary>
    /// 显示抢地主、不要等文本信息
    /// </summary>
    /// <param name="hint"></param>
    public void SetHintText(string hint, int index)
    {
        pokerTable.SetHintText(hint, index);
    }

    /// <summary>
    /// 显示计时器的时间
    /// </summary>
    /// <param name="info"></param>
    public void SetTimeText(string info)
    {
        pokerTable.arm.GetComponentInChildren<Text>().text = info;
    }

    /// <summary>
    /// 设置地主图标显示位置
    /// </summary>
    public void SetLordImg()
    {
        pokerTable.SetLordImg(playerIndex, players);
    }

    /// <summary>
    /// 清除抢地主相关信息
    /// </summary>
    public void ClearLordDatas()
    {
        //清除抢地主相关信息
        for(int i=0;i<3;i++)
        {
            players[i].loadLord = false;
            SetHintText(" ", i);
        }
    }

    /// <summary>
    /// 设置开始游戏重开游戏按钮
    /// </summary>
    public void SetStartButton(bool start, bool restart)
    {
        pokerTable.SetStartButton(start, restart);
    }

    /// <summary>
    /// 设置地主图片
    /// </summary>
    /// <param name="show"></param>
    public void SetLordImg(bool show)
    {
        pokerTable.SetLordImg(show);
    }

    /// <summary>
    /// 设置不出牌按钮状态
    /// </summary>
    /// <param name="show"></param>
    public void SetNotPlayBtn(bool show)
    {
        pokerTable.SetNotPlayBtn(show);
    }

    /// <summary>
    /// 判断谁是地主和是否流局
    /// </summary>
    public bool JudgeLord()
    {
        lordTimes++;
        //叫到了第四次
        if (lordTimes == 5)
        {
            //如果是我先叫，那么不管其他的玩家，我肯定是地主
            if (players[playerIndex % 3].loadLord)
            {
                playerIndex = playerIndex % 3;
                ForLord();
                return false;
            }
            //如果我先叫，我不抢地主
            if (!players[playerIndex % 3].loadLord)
            {
                //上一家叫地主，上一家是地主
                if (players[(playerIndex + 1) % 3].loadLord)
                {
                    playerIndex = (playerIndex + 1) % 3;
                    ForLord();
                    return false;
                }
                //上一家不叫地主
                if (!players[(playerIndex + 1) % 3].loadLord)
                {
                    //下一家叫地主了,下一家是地主
                    if (players[(playerIndex + 2) % 3].loadLord)
                    {
                        playerIndex = (playerIndex + 2) % 3;
                        ForLord();
                        return false;
                    }
                    //都不叫地主，流局
                    else
                    {
                        Debug.Log("流局, 重新开始游戏");
                        RestartGame();
                        return false;
                    }
                }

            }

        }

        return true;
    }

    /// <summary>
    /// 排序当前手牌
    /// </summary>
    public List<CardInfo> SortCards(List<CardInfo> list)
    {
        if (list.Count == 0)
            return null;
        list.Sort(delegate (CardInfo x, CardInfo y)
        {
            return y.cardIndex.CompareTo(x.cardIndex);
        });

        return list;
    }

    /// <summary>
    /// list随机排序
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="inputList"></param>
    /// <returns></returns>
    public List<T> GetRandomList<T>(List<T> inputList)
    {
        //赋值数组
        T[] copyArray = new T[inputList.Count];
        inputList.CopyTo(copyArray);

        //添加
        List<T> copyList = new List<T>();
        copyList.AddRange(copyArray);

        //设置随机
        List<T> outputList = new List<T>();
        System.Random rd = new System.Random(System.DateTime.Now.Millisecond);

        while (copyList.Count > 0)
        {
            //选择一个index和item
            int rdIndex = rd.Next(0, copyList.Count - 1);
            T remove = copyList[rdIndex];
            //从赋值list删除添加到输出列表
            copyList.Remove(remove);
            outputList.Add(remove);
        }
        return outputList;
    }

    /// <summary>
    /// 同步玩家点击
    /// </summary>
    /// <param name="name"></param>
    /// <param name="isSelect"></param>
    public void SetSelect(string name, bool isSelect)
    {
        for(int i=0;i<players[0].myCardInfo.Count;i++)
        {
            if(players[0].myCardInfo[i].cardName == name)
            {
                players[0].myCardInfo[i].isSelected = isSelect;
            }
        }
    }

    /// <summary>
    /// 玩家获胜
    /// </summary>
    /// <param name="player"></param>
    public IEnumerator Win(BasePlayer player)
    {
        Debug.Log(player.ToString() + " 获胜！");
        yield return new WaitForSeconds(2f);

        //清除相关数据
        ClearDatas();

        isRestart = true;
        SetStartButton(true, false);

        //显示输赢界面
        if (WinEvent != null)
        {
            WinEvent(player.ToString() + " 获胜！");
        }
    }






}
