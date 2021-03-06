﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SocketGameProtocol;

public class LogonPanel : BasePanel
{
    public LogonRequest logonRequest;
    public InputField user, pass;
    public Button logonBtn, switchBtn;

    private void Start()
    {
        logonBtn.onClick.AddListener(OnLogonClick);
        switchBtn.onClick.AddListener(SwitchLogin);
    }

    private void OnLogonClick()
    {
        if (user.text == "" || pass.text == "")
        {
            Debug.LogWarning("用户名或密码不能为空！");
            return;
        }
        logonRequest.SendRequest(user.text, pass.text);
    }

    private void SwitchLogin()
    {
        uiMag.PopPanel();
    }

    public override void OnEnter()
    {
        base.OnEnter();
        Enter();
    }

    public override void OnExit()
    {
        base.OnExit();
        Exit();
    }

    public override void OnPause()
    {
        base.OnPause();
        Exit();
    }

    public override void OnRecovery()
    {
        base.OnRecovery();
        Enter();
    }

    private void Enter()
    {
        gameObject.SetActive(true);
    }

    private void Exit()
    {
        gameObject.SetActive(false);
    }

    public void OnResponse(MainPack pack)
    {
        switch (pack.Returncode)
        {
            case ReturnCode.Succeed:
                uiMag.ShowMessage("注册成功！");
                uiMag.PushPanel(PanelType.Login);
                break;
            case ReturnCode.Fail:
                uiMag.ShowMessage("注册失败！");
                break;
            default:
                Debug.Log("def");
                break;
        }

    }
}
