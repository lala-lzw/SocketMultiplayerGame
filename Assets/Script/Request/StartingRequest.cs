using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using SocketGameProtocol;

public class StartingRequest : BaseRequest
{
    private MainPack isstart = null;
    public RoomPanel roomPanel;
    public override void Awake()
    {
        actionCode = ActionCode.Starting;
        base.Awake();
    }

    private void Update()
    {
        if (isstart!=null)
        {
            Debug.Log("游戏正式开始！");
            face.addPlayer(isstart);
            roomPanel.GameStarting(isstart);
            isstart = null;
        }
    }

    public override void OnResponse(MainPack pack)
    {
        isstart = pack;
    }
}
