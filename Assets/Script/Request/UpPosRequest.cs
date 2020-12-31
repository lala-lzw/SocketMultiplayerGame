using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketGameProtocol;

public class UpPosRequest : BaseRequest
{
    
    public override void Awake()
    {
        requestCode = RequestCode.Game;
        actionCode = ActionCode.UpPos;
        base.Awake();
    }

    public void SendRequest(Vector2 pos, float characterRot, float GunRot)
    {
        MainPack pack=new MainPack();
        PosPack posPack=new PosPack();
        PlayerPack playerPack=new PlayerPack();
        posPack.PosX = pos.x;
        posPack.PosY = pos.y;
        posPack.RotZ = characterRot;
        posPack.GunRotZ = GunRot;
        playerPack.Playername = face.UserName;
        playerPack.Pospack = posPack;
        pack.Playerpack.Add(playerPack);
        pack.Requestcode = requestCode;
        pack.Actioncode = actionCode;
        base.SendRequestUDP(pack);
    }



    public override void OnResponse(MainPack pack)
    {
        face.UpPos(pack);

    }
}
