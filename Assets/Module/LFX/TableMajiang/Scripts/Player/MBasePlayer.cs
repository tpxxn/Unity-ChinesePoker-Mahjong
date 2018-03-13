using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 桌上玩家类
/// </summary>
public class MBasePlayer
{
    //玩家手牌
    private List<MCardInfo> _myCards = new List<MCardInfo>();
    public List<MCardInfo> myCards
    {
        get { return _myCards; }
        set { _myCards = value; }
    }

    /// <summary>
    /// 添加麻将到玩家的myCards列表中
    /// </summary>
    /// <param name="mCardInfo"></param>
    public void AddCard(MCardInfo mCardInfo)
    {
        _myCards.Add(mCardInfo);
    }


}