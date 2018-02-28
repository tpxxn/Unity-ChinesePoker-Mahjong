using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 电脑玩家1
/// </summary>
public class PlayerComputer1 : BasePlayer {

    /// <summary>
    /// 玩家接收桌控制器发牌接口
    /// </summary>
    /// <param name="card"></param>
    public override void AddCard(CardInfo card)
    {
        card.pos = myTrans.position;
        card.parent = myTrans;
        myCardInfo.Add(card);
    }

    /// <summary>
    /// 获取自己的位置
    /// </summary>
    /// <returns></returns>
    public override Transform GetPlayer()
    {
        if (myTrans != null)
            return myTrans;
        myTrans = GameObject.FindGameObjectWithTag("Player1").transform;
        return myTrans;
    }
}
