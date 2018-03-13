using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WT.UI;

/// <summary>
/// 桌面控制
/// </summary>
public class MajiangTableControl
{
    #region 变量
    //桌面
    private MajiangTable _majiangTable;
    //桌面牌数据
    private List<MCardInfo> tCards = new List<MCardInfo>();
    //玩家出的牌数据
    private MCardInfo pCard;
    //桌面上的玩家
    private List<MBasePlayer> players = new List<MBasePlayer>();
    //临时麻将
    private List<MCardInfo> temp = new List<MCardInfo>();
    //庄家索引
    private int startIndex;
    //玩家索引
    private int index;
    //赢家索引
    private int winIndex;
    //临时玩家索引
    private int tempIndex;
    //是否在发牌
    private bool isSend = true;
    //是否能点击麻将
    private bool canClick = true;
    #endregion

    public MajiangTableControl()
    {
        _majiangTable = new MajiangTable();

        //生成玩家索引
        startIndex = MyUtil.GetRange(0, 4);
        index = startIndex;

        //生成四个玩家 Mplayer（控制器控制）
        for (int i = 0; i < 4; i++)
        {
            players.Add(new MBasePlayer());
        }

        //添加按钮代理 Action (绑定界面按钮事件)
        _addAction();
    }

    //Action，碰，杠，胡，过, 开始游戏， 重新开始游戏
    #region Action
    //设置
    private Action _settingEvent = null;
    public void AddSettingEvent(Action action)
    {
        _settingEvent = action;
    }

    //输赢界面
    private Action<string> _gameOverEvent = null;
    public void AddGameOverEvent(Action<string> action)
    {
        _gameOverEvent = action;
    }
    #endregion

    /// <summary>
    /// 显示桌面UI
    /// </summary>
    public void ShowMajiangUI()
    {
        WTUIPage.ShowPage("MajiangUI", _majiangTable);
    }

    /// <summary>
    /// 显示设置界面
    /// </summary>
    private void _showSetting()
    {
        if (_settingEvent != null)
        {
            _settingEvent();
        }
    }

    /// <summary>
    /// 绑定代理事件
    /// </summary>
    private void _addAction()
    {
        _majiangTable.AddStartGameEvent(_startGame);
        _majiangTable.AddReStartGameEvent(_restartGame);
        _majiangTable.AddSettingEvent(_showSetting);

        //玩家点击麻将出牌事件
        _majiangTable.AddPlayerClickEven(_playerClickMaJiang);
        //完成发牌动画后出牌
        _majiangTable.AddFinshSendAnitmaEvent(_startPlayMajiang);

        //玩家按钮按钮事件碰胡杠过
        _majiangTable.AddPengEvent(delegate ()
        {
            canClick = true;
            _buttonPeng();
        });
        _majiangTable.AddGangEvent(delegate ()
        {
            canClick = true;
            _buttonGang();
        });
        _majiangTable.AddHuEvent(delegate ()
        {
            _setButton(false, false, false, false);
            _huMajiang();
        });
        _majiangTable.AddPassEvent(delegate ()
        {
            canClick = true;
            _setButton(false, false, false, false);
            index = tempIndex;
            index++;
            index = index % 4;
            _computerAIPlayMajiang();
        });


    }

    /// <summary>
    /// 开始游戏
    /// </summary>
    private void _startGame()
    {
        //生成麻将数据(包括洗牌)
        _initCards();

        //实例化到桌 桌调用UI（传数据）
        _majiangTable.InstanceCards(tCards);

        //发牌给四个玩家（只发数据）
        _sendMajiang();

        //排序
        _playerSortCards();

        //显示发牌动画(发牌动画完成后UI回调开始出麻将)
        isSend = true;
        _majiangTable.SendMajiangAnimation(players);

    }

    /// <summary>
    /// 重新开始游戏
    /// </summary>
    private void _restartGame()
    {
        //关闭所有协程
        UnitTool.ToolStopAllCoroutines();

        //清除玩家和桌控制器的数据，重置UI的显示
        _clearDatas();

        //调用开始游戏
        _startGame();
    }

    /// <summary>
    /// 发牌
    /// </summary>
    /// <returns></returns>
    private void _sendMajiang()
    {
        int n = 0;
        //每个玩家发13个麻将
        for (int i = 0; i < tCards.Count; i++)
        {
            players[n].AddCard(tCards[i]);
            tCards.RemoveAt(i);
            if (i % 13 == 12 && i != 0)
                n++;
            if (n == 4)
                break;
        }
    }

    /// <summary>
    /// 完成动画后开始出牌回调函数
    /// </summary>
    private void _startPlayMajiang()
    {
        //庄家先摸牌
        _drawMajiang();
        //摸牌后检测胡牌
        if (_checkHu(players[index].myCards, null, index))
            return;

        isSend = false;

        if (index % 4 == 0)
        {
            //庄家是玩家
            return;
        }
        else
        {
            //庄家是电脑
            _computerAIPlayMajiang();
        }
    }

    /// <summary>
    /// 玩家摸牌
    /// </summary>
    private void _drawMajiang()
    {
        //该玩家摸牌
        if (tCards.Count == 0)
        {
            UnitTool.ToolStopAllCoroutines();
            Debug.Log("牌抓完了...平局");
            //打开平局面板
            UnitTool.ToolStopAllCoroutines();
            if (_gameOverEvent != null)
            {
                _gameOverEvent("平局");
            }
            return;
        }

        players[index].AddCard(tCards[0]);
        var moCard = tCards[0];
        _majiangTable.DrawMajiangAnimation(tCards[0], index);
        _sortCards(players[index].myCards);
        tCards.RemoveAt(0);

    }

    /// <summary>
    /// 人家玩家点击麻将出牌
    /// </summary>
    /// <param name="cardIndex"></param>
    /// <param name="cardName"></param>
    private void _playerClickMaJiang(int cardIndex, string cardName)
    {
        if (index % 4 != 0 || isSend || !canClick)
            return;

        _playerPlay(cardIndex, cardName, true);
    }

    /// <summary>
    /// 电脑玩家AI出麻将
    /// </summary>
    private void _computerAIPlayMajiang()
    {
        if (index % 4 == 0)
            return;

        var card = MajiangAI.AIPlayCard(players[index].myCards);
        _playerPlay(card.cardIndex, card.cardName, false);
    }

    /// <summary>
    /// 出牌等待时间
    /// </summary>
    /// <param name="cardIndex"></param>
    /// <param name="cardName"></param>
    /// <param name="go"></param>
    /// <returns></returns>
    private IEnumerator _waitCoroutine(int cardIndex, string cardName, bool go)
    {
        yield return new WaitForSeconds(1f);
        go = true;

        _playerPlay(cardIndex, cardName, go);
    }

    /// <summary>
    /// 玩家出麻将
    /// </summary>
    /// <param name="cardIndex"></param>
    /// <param name="cardName"></param>
    /// <returns></returns>
    private void _playerPlay(int cardIndex, string cardName, bool go)
    {
        if (!go)
        {
            UnitTool.ToolStartCoroutine(_waitCoroutine(cardIndex, cardName, false));
            return;
        }

        //当前玩家的出牌
        Debug.Log("Player " + index + " 出 " + cardIndex);
        int n = 0;
        for (int i = 0; i < players[index].myCards.Count; i++)
        {
            if (players[index].myCards[i].cardName == cardName)
            {
                n = i;
                break;
            }
        }
        _majiangTable.PlayMajiangAnimation(cardName, players[index].myCards, index);
        //保存这个玩家出的牌
        pCard = players[index].myCards[n];
        //清除
        players[index].myCards.RemoveAt(n);
        //当前玩家重新排序
        _sortCards(players[index].myCards);
        _majiangTable.ShowMajiang(players[index].myCards, index);

        //当前玩家出牌后，其他三个玩家进行检测胡、碰、杠操作
        _checkMajiang();

    }

    /// <summary>
    /// 控制器检测胡、杠、碰
    /// </summary>
    /// <returns></returns>
    private void _checkMajiang()
    {
        //检测其他三个玩家是否能胡牌
        if (_checkThreePlayerHu())
            return;

        //检测碰、杠
        if(_checkPengGang())
            return;

        //下一个玩家摸牌、检测、出牌
        index++;
        index = index % 4;
        _drawMajiang();

        if (_checkHu(players[index].myCards, null, index))
            return;

        //电脑出牌
        if (index % 4 != 0)
        {
            _computerAIPlayMajiang();
        }
    }



    /// <summary>
    /// 碰按钮调用
    /// </summary>
    private void _buttonPeng()
    {
        _setButton(false, false, false, false);
        UnitTool.ToolStopAllCoroutines();
        _peng(temp);
    }

    /// <summary>
    /// 杠按钮调用
    /// </summary>
    private void _buttonGang()
    {
        _setButton(false, false, false, false);
        UnitTool.ToolStopAllCoroutines();
        _gang(temp);
    }

    /// <summary>
    /// 检测碰、杠
    /// </summary>
    /// <returns></returns>
    private bool _checkPengGang()
    {
        int result = -1;
        tempIndex = index;
        //检测其他三个玩家的碰、杠
        for (int i = index + 1; i < index + 4; i++)
        {
            result = MajiangRules.IsPeng(players[i % 4].myCards, pCard);
            if (result == -1)
                result = MajiangRules.IsGang(players[i % 4].myCards, pCard);
            if (result != -1)
            {
                index = i % 4;
                //有玩家碰、杠了
                temp.Clear();
                for (int j = 0; j < players[index].myCards.Count; j++)
                {
                    if (players[index].myCards[j].cardIndex == result)
                    {
                        temp.Add(players[index].myCards[j]);
                    }
                }
                temp.Add(pCard);

                //如果碰、杠的玩家是电脑玩家，直接碰、杠
                if (index != 0)
                {
                    if (temp.Count == 3)
                        _peng(temp);
                    if (temp.Count == 4)
                        _gang(temp);
                    temp.Clear();
                    return true;
                }

                //人玩家
                if (index == 0)
                {
                    canClick = false;
                    if (temp.Count == 3)
                        _setButton(true, false, false, true);
                    else
                        _setButton(false, true, false, true);
                    //等待过按钮按下
                    return true;

                }
            }
        }

        return false;
    }

    /// <summary>
    /// 碰
    /// </summary>
    /// <param name="list"></param>
    /// <param name="x"></param>
    /// <returns></returns>
    private void _peng(List<MCardInfo> list)
    {
        Debug.Log("Player " + index.ToString() + " 碰!");

        for (int i = 0; i < list.Count; i++)
            players[index].myCards.Remove(list[i]);

        _majiangTable.ShowMajiang(players[index].myCards, index);
        _majiangTable.ShowPengMajiang(list, index);

        //yield return new WaitForSeconds(1f);

        //电脑碰完直接出
        if (index % 4 != 0)
        {
            _computerAIPlayMajiang();
        }

    }

    /// <summary>
    /// 杠
    /// </summary>
    /// <param name="list"></param>
    /// <param name="x"></param>
    /// <returns></returns>
    private void _gang(List<MCardInfo> list)
    {
        Debug.Log("Player " + index.ToString() + " 杠!");

        for (int i = 0; i < list.Count; i++)
            players[index].myCards.Remove(list[i]);

        _majiangTable.ShowMajiang(players[index].myCards, index);
        _majiangTable.ShowPengMajiang(list, index);

        //杠完再摸一张、检测、再出
        _drawMajiang();
        if (_checkHu(players[index].myCards, null, index))
            return;

        //yield return new WaitForSeconds(1f);

        if (index % 4 != 0)
        {
            _computerAIPlayMajiang();
        }

    }

    /// <summary>
    /// 检查某个玩家是否胡牌
    /// </summary>
    /// <param name="list"></param>
    /// <param name="mCardInfo"></param>
    /// <returns></returns>
    private bool _checkHu(List<MCardInfo> list, MCardInfo mCardInfo, int win)
    {
        bool canHu = false;
        if (MajiangRules.IsCanHU(list, mCardInfo))
        {
            canHu = true;
            winIndex = win;
            if(mCardInfo != null)
                players[win].myCards.Add(pCard);
            if (winIndex % 4 != 0)
            {
                _huMajiang();
            }
            else
            {
                _setButton(false, false, true, false);
            }
        }

        return canHu;
    }

    private bool _checkThreePlayerHu()
    {
        //检测三个玩家是否胡
        bool isHu = false;
        for (int i = index + 1; i < index + 4; i++)
        {
            if (_checkHu(players[i % 4].myCards, pCard, i % 4))
            {
                isHu = true;
                break;
            }
        }

        return isHu;
    }

    /// <summary>
    /// 胡牌
    /// </summary>
    private void _huMajiang()
    {
        //停止所有协程
        UnitTool.ToolStopAllCoroutines();

        //玩家胡,显示赢家的麻将
        _sortCards(players[winIndex].myCards);
        _majiangTable.ShowHuMajiang(players[winIndex].myCards);

        //控制器显示胜利
        _gameOver();
    }

    /// <summary>
    /// 游戏结束
    /// </summary>
    /// <returns></returns>
    private void _gameOver()
    {
        Debug.Log("Player " + winIndex + " Win");

        if (_gameOverEvent != null)
        {
            string info;
            if (winIndex % 4 == 0)
                info = "你赢了！！！";
            else
                info = "Player " + winIndex + "赢了\n" + "你输了...";
            _gameOverEvent(info);
        }

    }

    /// <summary>
    /// 设置玩家按钮
    /// </summary>
    /// <param name="peng"></param>
    /// <param name="gang"></param>
    /// <param name="hu"></param>
    /// <param name="pass"></param>
    private void _setButton(bool peng, bool gang, bool hu, bool pass)
    {
        _majiangTable.SetButton(peng, gang, hu, pass);
    }

    /// <summary>
    /// 生成卡牌数据
    /// </summary>
    private void _initCards()
    {
        for (int i = 89; i < 123; i++)
        {
            Sprite o = FileIO.LoadSprite(i);

            for (int j = 0; j < 4; j++)
            {
                MCardInfo cardInfo = new MCardInfo(o.name, j + 1);
                tCards.Add(cardInfo);
            }
        }

        //洗牌 
        tCards = _getRandomList<MCardInfo>(tCards);
    }

    /// <summary>
    /// 排序当前手牌
    /// </summary>
    private void _sortCards(List<MCardInfo> list)
    {
        if (list.Count == 0)
            return;

        list.Sort(delegate (MCardInfo x, MCardInfo y)
        {
            return x.cardIndex.CompareTo(y.cardIndex);
        });

       
    }

    /// <summary>
    /// list随机排序
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="inputList"></param>
    /// <returns></returns>
    private List<T> _getRandomList<T>(List<T> inputList)
    {
        //赋值数组
        T[] copyArray = new T[inputList.Count];
        inputList.CopyTo(copyArray);

        //添加
        List<T> copyList = new List<T>();
        copyList.AddRange(copyArray);

        //设置随机
        List<T> outputList = new List<T>();

        while (copyList.Count > 0)
        {
            //选择一个index和item
            int rdIndex = MyUtil.GetRange(0, copyList.Count - 1);
            T remove = copyList[rdIndex];
            //从赋值list删除添加到输出列表
            copyList.Remove(remove);
            outputList.Add(remove);
        }
        return outputList;
    }

    /// <summary>
    /// 玩家对手里的麻将排序
    /// </summary>
    private void _playerSortCards()
    {
        for (int i = 0; i < 4; i++)
        {
            _sortCards(players[i].myCards);
        }
    }

    /// <summary>
    /// 清除数据
    /// </summary>
    private void _clearDatas()
    {
        //清除桌数据
        pCard = null;
        tCards.Clear();
        temp.Clear();
        //清除玩家手牌
        for (int i = 0; i < 4; i++)
        {
            players[i].myCards.Clear();
        }
        //重置玩家索引
        index = MyUtil.GetRange(0, 4);
        //清除UI
        _majiangTable.ClearUi();
    }


}