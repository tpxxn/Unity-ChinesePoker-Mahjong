using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 卡牌信息
/// </summary>

public class MCardInfo
{
    private string _cardName;                     //卡牌图片名
    private int _cardIndex;                       //牌在所在类型的索引

    public string cardName
    {
        get { return _cardName; }
        set { _cardName = value; }
    }

    public int cardIndex
    {
        get { return _cardIndex; }
        set { _cardIndex = value; }
    }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="cardName"></param>
    public MCardInfo(string cardName, int n)
    {
        var splits = cardName.Split('_');
        this._cardName = splits[1].ToString() + "_" + n.ToString();
        this._cardIndex = int.Parse(splits[1]);
    }


}
