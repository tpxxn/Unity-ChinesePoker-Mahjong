using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 卡牌信息
/// </summary>

public class CardInfo 
{
    //大小王，黑桃，红心，梅花，方块
    public enum CardTypes { Joker, Spades, Hearts, Clubs, Diamonds };

    public string cardName;     //卡牌图片名
    public CardTypes cardType;  //牌的类型
    public int cardIndex;       //牌在所在类型的索引
    public Vector3 pos;         //卡牌所在的位置
    public Transform parent;    //卡牌的父节点
    public bool isSelected;     //是否被选中

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="cardName"></param>
    public CardInfo(string cardName)
    {
        this.cardName = cardName;
        this.isSelected = false;
        var splits = cardName.Split('_');

        switch (splits[0])
        {
            case "1":
                cardType = CardTypes.Spades;
                cardIndex = int.Parse(splits[1])-3;
                break;
            case "2":
                cardType = CardTypes.Hearts;
                cardIndex = int.Parse(splits[1])-3;
                break;
            case "3":
                cardType = CardTypes.Clubs;
                cardIndex = int.Parse(splits[1])-3;
                break;
            case "4":
                cardType = CardTypes.Diamonds;
                cardIndex = int.Parse(splits[1])-3;
                break;
            case "0":
                cardType = CardTypes.Joker;
                cardIndex = int.Parse(splits[1]);
                break;
            default:
                throw new Exception(string.Format("卡牌文件名{0}非法！", cardName));
        }


    }
    

}
