using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WT.UI;

public class PokeLogin :  WTUIPage {

    private InputField inputField_usr;
    private InputField inputField_pwd;
    private Button btn_mute;
    private Text text_hint;

    //构造函数
    public PokeLogin():base(UIType.Normal,UIMode.DoNothing,UICollider.None)
    {
        //指定UI索引
        uiIndex = R.Prefab.POKERLOGIN;
    }

    #region 事件

    private Func<string, string, string> LoginEvent;
    //添加登陆事件
    public void AddLoginEvent(Func<string, string, string> method)
    {
        LoginEvent = method;
    }
    
    #endregion

    //重写Awake方法获取组件
    public override void Awake(GameObject go)
    {
        #region GameObject Find
        inputField_usr = GameObject.Find("Content/Panel/InputField_usr").GetComponent<InputField>();
        inputField_pwd = GameObject.Find("Content/Panel/InputField_pwd").GetComponent<InputField>();
        text_hint = GameObject.Find("Content/Panel/Text_hint").GetComponent<Text>();
        GameObject.Find("Content/Panel/Button_login").GetComponent<Button>().onClick.AddListener(Btn_Login);
        #endregion
    }

    //登陆按钮
    private void Btn_Login()
    {
        if(LoginEvent != null)
        {
            if(LoginEvent(inputField_usr.text, inputField_pwd.text) == "LOGIN_OK")
            {
                Debug.Log("登陆成功!");
                Hide();
            }
            else
            {
                text_hint.text = "用户名或密码不正确！";
            }
        }
    }

    

}
