using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WT.UI;

public class MajiangTable : WTUIPage
{
    #region 变量
    //桌面上的麻将
    private List<MCard> uiCards = new List<MCard>();
    //桌面上玩家的位置
    private List<Transform> playerTrans = new List<Transform>();
    //4个玩家碰杠的位置
    private List<Vector3> playerPengPos = new List<Vector3>();
    //一些按钮
    private Button btnStart;
    private Button btnRestart;
    private Button btnSetting;
    private Button btnBack;
    private Button btnPeng;
    private Button btnGang;
    private Button btnHu;
    private Button btnPass;
    private Button temp;
    //桌物体
    private Transform table;
    #endregion

    public MajiangTable() : base(UIType.Normal, UIMode.DoNothing, UICollider.None)
    {
        uiIndex = R.Prefab.MAJIANGTABLE;
    }

    public override void Awake(GameObject go)
    {
        #region GameObject Find
        table = GameObject.Find("Content/Table").transform;
        btnStart = GameObject.Find("Content/Button_start").GetComponent<Button>();
        btnRestart = GameObject.Find("Content/Button_restart").GetComponent<Button>();
        btnSetting = GameObject.Find("Content/Button_tablesetting").GetComponent<Button>();
        btnBack = GameObject.Find("Content/Button_back").GetComponent<Button>();

        btnPeng = GameObject.FindGameObjectWithTag("Peng").GetComponent<Button>();
        btnGang = GameObject.FindGameObjectWithTag("Gang").GetComponent<Button>();
        btnHu = GameObject.FindGameObjectWithTag("Hu").GetComponent<Button>();
        btnPass = GameObject.FindGameObjectWithTag("Pass").GetComponent<Button>();

        playerTrans.Add(GameObject.Find("Content/Player0").transform);
        playerTrans.Add(GameObject.Find("Content/PlayerAI1").transform);
        playerTrans.Add(GameObject.Find("Content/PlayerAI2").transform);
        playerTrans.Add(GameObject.Find("Content/PlayerAI3").transform);

        playerPengPos.Add(GameObject.Find("Content/Player0/Peng").transform.position);
        playerPengPos.Add(GameObject.Find("Content/PlayerAI1/Peng").transform.position);
        playerPengPos.Add(GameObject.Find("Content/PlayerAI2/Peng").transform.position);
        playerPengPos.Add(GameObject.Find("Content/PlayerAI3/Peng").transform.position);


        btnStart.onClick.AddListener(_btn_StartGame);
        btnRestart.onClick.AddListener(_btn_Restart);
        btnSetting.onClick.AddListener(_btn_Setting);
        btnBack.onClick.AddListener(_btn_Back);

        btnPeng.onClick.AddListener(_btn_Peng);
        btnGang.onClick.AddListener(_btn_Gang);
        btnHu.onClick.AddListener(_btn_Hu);
        btnPass.onClick.AddListener(_btn_Pass);

        btnRestart.gameObject.SetActive(false);
        #endregion


    }

    #region Action

    //开始游戏
    private Action _startGameEvent = null;
    public void AddStartGameEvent(Action action)
    {
        _startGameEvent = action;
    }

    //重新开始游戏
    private Action _reStartGameEvent = null;
    public void AddReStartGameEvent(Action action)
    {
        _reStartGameEvent = action;
    }

    //打开设置
    private Action _settingEvetn = null;
    public void AddSettingEvent(Action action)
    {
        _settingEvetn = action;
    }

    //完成发牌动画，开始出牌
    private Action _finshSendAnitmaEvent;
    public void AddFinshSendAnitmaEvent(Action action)
    {
        _finshSendAnitmaEvent = action;
    }

    //碰
    private Action _pengEvent;
    public void AddPengEvent(Action action)
    {
        _pengEvent = action;
    }

    //杠
    private Action _gangEvent;
    public void AddGangEvent(Action action)
    {
        _gangEvent = action;
    }

    //胡
    private Action _huEvent;
    public void AddHuEvent(Action action)
    {
        _huEvent = action;
    }

    //过
    private Action _passEvent;
    public void AddPassEvent(Action action)
    {
        _passEvent = action;
    }

    //出牌
    private Action<int, string> _playerClickEvent;
    public void AddPlayerClickEven(Action<int, string> action)
    {
        _playerClickEvent = action;
    }

    #endregion

    /// <summary>
    /// 实例化麻将
    /// </summary>
    /// <param name="list"></param>
    public void InstanceCards(List<MCardInfo> list)
    {
        //根据数据实例化麻将到桌面

        GameObject obj = WTUIPage.delegateSyncLoadUIByLocal(R.Prefab.MAJIANG) as GameObject;

        for (int i = 0; i < list.Count; i++)
        {
            GameObject o = GameObject.Instantiate(obj, new Vector3(0, 3, 0), Quaternion.identity, transform) as GameObject;
            MCard c = o.GetComponent<MCard>();
            c.name = list[i].cardName;
            c.cardIndex = list[i].cardIndex;
            //绑定麻将点击事件
            c.AddSetSelectEvent(_clickMajiang);
            uiCards.Add(c);
        }

    }

    /// <summary>
    /// 加载麻将图片
    /// </summary>
    /// <param name="mCardInfo"></param>
    /// <returns></returns>
    private Sprite _initImage(MCardInfo mCardInfo, int type)
    {
        //根据卡牌信息生成图片
        string path;

        Sprite sprite = null;

        //由于没有提供根据名字来加载的方法，所以只能自己通过名字取index来加载

        if (type == 0)
        {
            int i = 0;
            string name = "n_" + mCardInfo.cardIndex.ToString();
            path = "lfx/tablemajiang/spritepack/majiang/" + name;
            for (i = 0; i < R.SpritePack.path.Length; i++)
            {
                if (path == R.SpritePack.path[i])
                {
                    break;
                }
            }
            sprite = FileIO.LoadSprite(i);
        }
        else if (type == 1)
            sprite = FileIO.LoadSprite(125);
        else if (type == 2)
            sprite = FileIO.LoadSprite(123);
        else if (type == 3)
            sprite = FileIO.LoadSprite(124);

        if (sprite == null)
        {
            Debug.Log("Sprite Null");
            return null;
        }
        else
            return sprite;


    }

    /// <summary>
    /// 加载出出去的麻将的图片
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    private Sprite _initSendImg(string name)
    {
        int i = 0;
        Sprite sprite = null;
        string path;
        name = name.Split('_')[0];
        name = "b_" + name;
        path = "lfx/tablemajiang/spritepack/majiang/" + name;
        for (i = 0; i < R.SpritePack.path.Length; i++)
        {
            if (path == R.SpritePack.path[i])
            {
                break;
            }
        }
        sprite = FileIO.LoadSprite(i);

        if (sprite == null)
        {
            Debug.Log("Sprite Null");
            return null;
        }
        else
            return sprite;

    }

    /// <summary>
    /// 发牌动画
    /// </summary>
    /// <param name="players"></param>
    /// <param name="startIndex"></param>
    /// <returns></returns>
    public void SendMajiangAnimation(List<MBasePlayer> players)
    {
        UnitTool.ToolStartCoroutine(_sendMajiangCoroutine(players));
    }

    /// <summary>
    /// 发牌协程
    /// </summary>
    /// <param name="players"></param>
    /// <returns></returns>
    IEnumerator _sendMajiangCoroutine(List<MBasePlayer> players)
    {
        List<List<Vector3>> pos = new List<List<Vector3>>();
        for (int i = 0; i < 4; i++)
        {
            pos.Add(_getMajiangPos(players[i].myCards, i));
        }

        for (int i = 0; i < 13; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                yield return new WaitForSeconds(0.1f);
                _majiangAnimation(players[j].myCards[i], pos[j][i], j, false);
            }
        }
        yield return new WaitForSeconds(0.5f);

        //告诉控制器发牌动画完成了.可以出牌了
        if (_finshSendAnitmaEvent != null)
        {
            _finshSendAnitmaEvent();
        }
    }

    /// <summary>
    /// 麻将动画和图片
    /// </summary>
    /// <param name="mCardInfo"></param>
    /// <param name="pos"></param>
    /// <param name="index"></param>
    /// <param name="isSend"></param>
    private void _majiangAnimation(MCardInfo mCardInfo, Vector3 pos, int index, bool isSend)
    {
        var card = GetCardObject(mCardInfo);
        card.transform.localScale = new Vector3(1, 1, 1);
        card.image.sprite = _initImage(mCardInfo, index);

        if (!isSend)
        {
            card.transform.SetParent(playerTrans[index]);
            card.transform.SetAsFirstSibling();
        }
        else
        {
            card.transform.SetParent(table);
        }

        _moveMajiang(mCardInfo, pos);
    }

    /// <summary>
    /// 摸牌动画
    /// </summary>
    /// <param name="mCardInfo"></param>
    /// <param name="index"></param>
    public void DrawMajiangAnimation(MCardInfo mCardInfo, int index)
    {
        Vector3 pos = Vector3.zero;
        if (index == 0)
        {
            pos = playerTrans[index].position + new Vector3(3f, 0, 0);
        }
        if (index == 1)
        {
            pos = playerTrans[index].position + new Vector3(-2f, 3.3f, 0);
        }
        if (index == 2)
        {
            pos = playerTrans[index].position + new Vector3(-9f, 0, 0);
        }
        if (index == 3)
        {
            pos = playerTrans[index].position + new Vector3(-2f, -3.8f, 0);
        }

        _majiangAnimation(mCardInfo, pos + new Vector3(2, 0, 0), index, false);
    }

    /// <summary>
    /// 玩家出麻将动画
    /// </summary>
    /// <param name="name"></param>
    /// <param name="list"></param>
    /// <param name="index"></param>
    public void PlayMajiangAnimation(string name, List<MCardInfo> list, int index)
    {
        Vector3 pos = new Vector3();
        if (index == 0)
        {
            pos = playerTrans[0].position + new Vector3(4.5f, 1.3f, 0);
        }
        if (index == 1)
        {
            pos = playerTrans[1].position + new Vector3(-1.5f, 2f, 0);
        }
        if (index == 2)
        {
            pos = playerTrans[2].position + new Vector3(-5.5f, -1.2f, 0);
        }
        if (index == 3)
        {
            pos = playerTrans[3].position + new Vector3(2f, -2.5f, 0);
        }


        for (int i = 0; i < list.Count; i++)
        {
            var card = GetCardObject(list[i]);
            if (card.name == name)
            {
                card.image.sprite = _initSendImg(name);
                card.transform.position = pos;
                card.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
                card.transform.SetParent(table);
            }
        }
    }

    /// <summary>
    /// 显示玩家手里的麻将
    /// </summary>
    /// <param name="list"></param>
    /// <param name="index"></param>
    /// <param name="isSend"></param>
    /// <returns></returns>
    public void ShowMajiang(List<MCardInfo> list, int index)
    {
        if (list.Count == 0)
        {
            Debug.Log("MyCards is Empty!");
            return;
        }

        List<Vector3> pos = _getMajiangPos(list, index);
        for (int i = 0; i < list.Count; i++)
        {
            _majiangAnimation(list[i], pos[i], index, false);
        }
    }

    /// <summary>
    /// 显示玩家碰、杠的牌
    /// </summary>
    /// <param name="list"></param>
    /// <param name="index"></param>
    public void ShowPengMajiang(List<MCardInfo> list, int index)
    {

        if (index == 0)
            playerPengPos[0] += new Vector3(2.1f, 0, 0);
        if (index == 1)
            playerPengPos[1] += new Vector3(0, 1f, 0);
        if (index == 2)
            playerPengPos[2] += new Vector3(-2.1f, 0, 0);
        if (index == 3)
            playerPengPos[3] += new Vector3(0f, -1f, 0);

        Vector3 pos = playerPengPos[index];

        for (int i = 0; i < list.Count; i++)
        {
            pos.x += 0.5f;
            GetCardObject(list[i]).image.sprite = _initSendImg(list[i].cardName);
            GetCardObject(list[i]).transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
            _moveMajiang(list[i], pos);
            GetCardObject(list[i]).transform.SetParent(table);
        }

    }

    /// <summary>
    /// 显示胡牌麻将
    /// </summary>
    /// <param name="list"></param>
    public void ShowHuMajiang(List<MCardInfo> list)
    {
        //牌的显示位置
        List<Vector3> pos = new List<Vector3>();
        //牌所在的位置
        Vector3 basePos = Vector3.zero;

        basePos = table.position + new Vector3(list.Count * -0.45f, 0, 0);
        for (int i = 0; i < list.Count; i++)
        {
            basePos.x += 0.8f;
            pos.Add(basePos);

        }

        for (int i = 0; i < pos.Count; i++)
        {
            var card = GetCardObject(list[i]);
            card.image.sprite = _initSendImg(card.name);
            card.transform.SetParent(table);
            card.transform.SetAsLastSibling();
            card.transform.localScale = new Vector3(1, 1, 1);
            card.transform.position = pos[i];
        }

    }

    /// <summary>
    /// 获取麻将的位置
    /// </summary>
    /// <param name="list"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    private List<Vector3> _getMajiangPos(List<MCardInfo> list, int type)
    {
        //牌的显示位置
        List<Vector3> pos = new List<Vector3>();
        //牌所在的位置
        Vector3 basePos = Vector3.zero;

        if (type == 0 || type == 2)
        {
            basePos = playerTrans[type].position + new Vector3(list.Count * -0.5f, 0, 0);
            for (int i = 0; i < list.Count; i++)
            {
                basePos.x += 0.8f;
                pos.Add(basePos);
            }
        }
        if (type == 1 || type == 3)
        {
            basePos = playerTrans[type].position + new Vector3(0, list.Count * -0.25f, 0);
            for (int i = 0; i < list.Count; i++)
            {
                basePos.y += 0.4f;
                pos.Add(basePos);
            }
        }

        return pos;
    }

    /// <summary>
    /// 移动麻将到指定的位置
    /// </summary>
    /// <param name="cardInfo"></param>
    /// <param name="time"></param>
    private void _moveMajiang(MCardInfo mCardInfo, Vector3 endPos)
    {
        var tran = GetCardObject(mCardInfo).GetComponent<Transform>();
        tran.position = endPos;
    }

    /// <summary>
    /// 根据数据返回卡牌物体
    /// </summary>
    /// <param name="cardInfo"></param>
    /// <returns></returns>
    public MCard GetCardObject(MCardInfo cardInfo)
    {
        MCard card = null;
        foreach (var c in uiCards)
        {
            if (c.name == cardInfo.cardName)
            {
                card = c;
            }
        }

        return card;
    }

    /// <summary>
    /// 清除UI数据
    /// </summary>
    public void ClearUi()
    {
        for (int i = 0; i < uiCards.Count; i++)
        {
            uiCards[i].transform.position = new Vector3(0, 0, 0);
            GameObject.Destroy(uiCards[i].gameObject);
        }

        playerPengPos.Clear();
        playerPengPos.Add(GameObject.Find("Content/Player0/Peng").transform.position);
        playerPengPos.Add(GameObject.Find("Content/PlayerAI1/Peng").transform.position);
        playerPengPos.Add(GameObject.Find("Content/PlayerAI2/Peng").transform.position);
        playerPengPos.Add(GameObject.Find("Content/PlayerAI3/Peng").transform.position);

        uiCards.Clear();
    }

    /// <summary>
    /// 清除玩家麻将父节点
    /// </summary>
    /// <param name="list"></param>
    public void ClearPartent(List<MCardInfo> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            GetCardObject(list[i]).transform.SetParent(table);
        }
    }

    /// <summary>
    /// 设置麻将按钮状态
    /// </summary>
    /// <param name="peng"></param>
    /// <param name="gang"></param>
    /// <param name="hu"></param>
    /// <param name="pass"></param>
    public void SetButton(bool peng, bool gang, bool hu, bool pass)
    {
        btnPeng.interactable = peng;
        btnGang.interactable = gang;
        btnHu.interactable = hu;
        btnPass.interactable = pass;
    }

    /// <summary>
    /// 返回大厅
    /// </summary>
    private void _btn_Back()
    {
        Hide();
    }

    /// <summary>
    /// 开始游戏
    /// </summary>
    private void _btn_StartGame()
    {
        if (_startGameEvent != null)
        {
            _startGameEvent();
        }
        //点击开始游戏后再显示重新开始按钮
        btnRestart.gameObject.SetActive(true);
        btnStart.gameObject.SetActive(false);
    }

    /// <summary>
    /// 重新开始游戏
    /// </summary>
    private void _btn_Restart()
    {
        if (_reStartGameEvent != null)
        {
            _reStartGameEvent();
        }
    }

    /// <summary>
    /// 桌面设置界面
    /// </summary>
    private void _btn_Setting()
    {
        if (_settingEvetn != null)
        {
            _settingEvetn();

        }
    }

    /// <summary>
    /// 麻将点击事件
    /// </summary>
    /// <param name="name"></param>
    private void _clickMajiang(string name)
    {
        if (_playerClickEvent != null)
        {
            int n = 0;
            for (int i = 0; i < uiCards.Count; i++)
            {
                if (uiCards[i].name == name)
                {
                    n = uiCards[i].cardIndex;
                }
            }
            _playerClickEvent(n, name);
        }

    }

    /// <summary>
    /// 玩家点击碰
    /// </summary>
    private void _btn_Peng()
    {
        if (_pengEvent != null)
        {
            _pengEvent();
        }
    }

    /// <summary>
    /// 玩家点击杠
    /// </summary>
    private void _btn_Gang()
    {
        if (_gangEvent != null)
        {
            _gangEvent();
        }
    }

    /// <summary>
    /// 玩家点击胡
    /// </summary>
    private void _btn_Hu()
    {
        if (_huEvent != null)
        {
            _huEvent();
        }
    }

    /// <summary>
    /// 玩家点击过
    /// </summary>
    private void _btn_Pass()
    {
        if (_passEvent != null)
        {
            _passEvent();
        }
    }



}
