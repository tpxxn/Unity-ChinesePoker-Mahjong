using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 桌上玩家基类
/// </summary>
public abstract class BasePlayer
{
    #region 定义变量
    //玩家ID(对应PlayerInfo类中ID,用来获取该玩家个人信息)
    public int ID;
    //玩家手牌
    public List<CardInfo> myCardInfo = new List<CardInfo>();
    //玩家父节点
    public Transform myTrans = null;
    //出牌牌型
    public DDZ_POKER_TYPE myType;
    //是否叫地主
    public bool loadLord = false;
    //是不是我的轮回
    public bool myTerm = false;
    public bool clickLord = false;
    #endregion

    #region Action
    //显示时间
    public Action<string> ShowTimeEvent;
    public virtual void AddShowTimeEvent(Action<string> action)
    {
        ShowTimeEvent = action;
    }

    //不叫地主
    public Action NotLordEvent;
    public void AddNotLordEvent(Action action)
    {
        NotLordEvent = action;
    }

    //叫地主
    public Action ToLordEvent;
    public void AddToLordEvent(Action action)
    {
        ToLordEvent = action;
    }

    //开始出牌
    public Action StartPlayEvent;
    public void AddStartPlayEvent(Action action)
    {
        StartPlayEvent = action;
    }

    //AI出牌
    public Func<bool> AIPlayEvent;
    public void AddAIPlayEvent(Func<bool> func)
    {
        AIPlayEvent = func;
    }

    //验证出牌并返回结果
    public Func<bool> PlayEvent;
    public void AddPlayEvent(Func< bool> action)
    {
        PlayEvent = action;
    }

    //能出牌
    public Action CanPlayEvent;
    public void AddCanPlayEvent(Action action)
    {
        CanPlayEvent = action;
    }

    //不出牌
    public Action NotPlayEvent;
    public void AddNotPlayEvent(Action action)
    {
        NotPlayEvent = action;
    }

    #endregion

    /// <summary>
    /// 添加牌到玩家
    /// </summary>
    /// <param name="card"></param>
    public virtual void AddCard(CardInfo card) { }

    /// <summary>
    /// 获取自己的父亲，即玩家
    /// </summary>
    /// <returns></returns>
    public virtual Transform GetPlayer()
    {
        return null;
    }

    /// <summary>
    /// 叫地主
    /// </summary>
    public virtual void ToLord()
    {
        //电脑开始思考
        UnitTool.ToolStartCoroutine(Considerating(0));
    }

    /// <summary>
    /// 抢地主
    /// </summary>
    public virtual void ForLord()
    {
        //向控制器请求叫地主
        loadLord = true;
        ToLordEvent();
    }

    /// <summary>
    /// 不抢地主
    /// </summary>
    public virtual void NotLord()
    {
        //先关闭所有协程
        UnitTool.ToolStopAllCoroutines();

        loadLord = false;
        NotLordEvent();
    }

    /// <summary>
    /// 开始出牌
    /// </summary>
    public virtual void StartPlay()
    {
        StartPlayEvent();
        //电脑开始思考
        UnitTool.ToolStartCoroutine(Considerating(1));
    }

    /// <summary>
    /// 电脑出牌
    /// </summary>
    public virtual void PlayCards()
    {
        if (myCardInfo.Count == 0)
            return;

        //AI是否能出牌
        if (!AIPlayEvent())
             return;

        ////验证
        //if (!PlayEvent())
        //    return;

        //确定出牌
        CanPlayEvent();
        
    }

    /// <summary>
    /// 不出牌
    /// </summary>
    public virtual void NotPlay()
    {
        NotPlayEvent();
    }
 
    /// <summary>
    /// 电脑思考协程
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    IEnumerator Considerating(int type)
    {
        //倒计时
        float time;
        if (type == 0)
            time = 6f;
        else
            time = 16f;

        while (time > 0)
        {
            if (time == 14f)
                break;

            ShowTimeEvent(time.ToString());

            yield return new WaitForSeconds(1);
            time--;
        }
        //计时结束,随机概率让电脑抢地主
        if (type == 0)
        {
            int n = MyUtil.GetRange(0, 4);
            if (n > 1)
                ForLord();
            else
                NotLord();
        }
        else
        {
            PlayCards();
        }
    }
}