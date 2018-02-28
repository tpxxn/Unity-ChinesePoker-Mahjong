using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using WT.UI;

public class Card : MonoBehaviour {

    #region 定义变量
    public string cardName;             //索引
    public Image image;                 //图片
    public bool isSelected = false;     //是否选中
    private bool isDown = false;        //鼠标是否按下
    private bool canClick = true;       //能够点击

    #endregion

    //点击事件
    public Action<string, bool> SetSelectEvent;
    public void AddSetSelectEvent(Action<string, bool> action)
    {
        SetSelectEvent = action;
    }

    //是否在叫地主
    public Func<bool> IsLordEvent;
    public void AddIsLordEvent(Func<bool> func)
    {
        IsLordEvent = func;
    }

    void Awake()
    {
        image = GetComponent<Image>();
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            isDown = true;
        }
        if(Input.GetMouseButtonUp(0))
        {
            isDown = false;
        }
    }

    /// <summary>
    /// 设置选择状态,组合鼠标点击和EventTrigger的点击事件
    /// 暂时没想到好办法所以使用两个函数
    /// </summary>
    public void SetSelectState()
    {
        if (IsLordEvent != null)
        {
            if (IsLordEvent())
                return;
        }
        
        if (transform.parent.name != "Player0"  || isDown == false || canClick == false) 
            return;

        if (isSelected )
        {
            StartCoroutine(MoveObject(transform.position, transform.position - Vector3.up * 0.5f, 0.1f));       
        }
        else if(!isSelected)
        {
            StartCoroutine(MoveObject(transform.position, transform.position + Vector3.up * 0.5f, 0.1f));
        }

        isSelected = !isSelected;

        if (SetSelectEvent != null)
        {
            SetSelectEvent(cardName, isSelected);
        }
    }

    public void SetSelectState1()
    {
        if (IsLordEvent != null)
        {
            if (IsLordEvent())
                return;
        }

        if (transform.parent.name != "Player0"  || canClick == false)
            return;

        if (isSelected)
        {
            StartCoroutine(MoveObject(transform.position, transform.position - Vector3.up * 0.5f, 0.1f));
        }
        else if (!isSelected)
        {
            StartCoroutine(MoveObject(transform.position, transform.position + Vector3.up * 0.5f, 0.1f));
        }

        isSelected = !isSelected;


        if (SetSelectEvent != null)
        {
            SetSelectEvent(cardName, isSelected);
        }

    }

    /// <summary>
    /// 移动协程
    /// </summary>
    /// <param name="startPos"></param>
    /// <param name="endPos"></param>
    /// <param name="time"></param>
    /// <returns></returns>
    IEnumerator MoveObject(Vector3 startPos, Vector3 endPos, float time)
    {
        var dur = 0.0f;
        canClick = false;
        while (dur <= time)
        {
            dur += Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, endPos, dur / time);
            yield return null;
        }
        canClick = true;
    }
}
