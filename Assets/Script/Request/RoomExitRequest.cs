﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketGameProtocol;

public class RoomExitRequest : BaseRequest
{

    private bool isexit = false;

    public RoomPanel roomPanel;
    public override void Awake()
    {
        requestCode = RequestCode.Room;
        actionCode = ActionCode.Exit;
        base.Awake();
    }

    public void SendRequest()
    {
        MainPack pack=new MainPack();
        pack.Requestcode = requestCode;
        pack.Actioncode = actionCode;
        pack.Str = "r";
        base.SendRequest(pack);
    }

    private void Update()
    {
        if (isexit)
        {
            roomPanel.ExitRoomResponse();
            isexit = false;
        }
    }

    public override void OnResponse(MainPack pack)
    {
        isexit = true;
        Debug.Log("离开房间");
    }
}
