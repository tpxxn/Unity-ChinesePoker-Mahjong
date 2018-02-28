using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 玩家类
/// </summary>
public class Player : BasePlayer {


    /// <summary>
    /// 添加牌到玩家手牌中
    /// </summary>
    /// <param name="card"></param>
    public override void AddCard(CardInfo card)
    {
        card.pos = myTrans.position;
        card.parent = myTrans;
        myCardInfo.Add(card);
    }

    /// <summary>
    /// 获取玩家的Transform
    /// </summary>
    /// <returns></returns>
    public override Transform GetPlayer()
    {
        if (myTrans != null)
            return myTrans;
        myTrans = GameObject.FindGameObjectWithTag("Player").transform;
        return myTrans;
    }

    /// <summary>
    /// 开始叫地主
    /// </summary>
    public override void ToLord()
    {
        clickLord = false;
        //开启思考协程
        UnitTool.ToolStartCoroutine(Considerating(0));      
    }

    /// <summary>
    /// 抢地主
    /// </summary>
    public override void ForLord()
    {
        loadLord = true;
        ToLordEvent();
    }

    /// <summary>
    /// 不抢地主
    /// </summary>
    public override void NotLord()
    {
        loadLord = false;
        NotLordEvent();
    }

    /// <summary>
    /// 开始出牌
    /// </summary>
    public override void StartPlay()
    {
        StartPlayEvent();
        //开始倒计时
        UnitTool.ToolStartCoroutine(Considerating(1));

    }

    /// <summary>
    /// 玩家出牌
    /// </summary>
    public override void PlayCards()
    {
        if (myCardInfo.Count == 0)
            return;

        //检查合法性
        if (!PlayEvent())
            return;
        
        //能出牌
        
        CanPlayEvent();
    }

    /// <summary>
    /// 不出牌
    /// </summary>
    public override void NotPlay()
    {
        NotPlayEvent();
    }

    /// <summary>
    /// 计时器
    /// </summary>
    /// <returns></returns>
    private IEnumerator Considerating(int type)
    {
        //倒计时
        var time = 0f;
        if (type == 0)
            time = 5f;
        else
            time = 16f;

        //Debug.Log(lordTimes);
        while (time > 0)
        {
 
            if (loadLord && clickLord)
            {
                yield break;        
            }

            ShowTimeEvent(time.ToString());

            yield return new WaitForSeconds(1);
            time--;
        }
        
        //计时结束,默认不抢地主
        if (type == 0  && !clickLord )
        {
            NotLord();
        }
        
        if(type == 1)
        {
            NotPlay();
        }
 
    }
}
