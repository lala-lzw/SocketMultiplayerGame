using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketGameProtocol;

public class DamageRequest : BaseRequest
{
    private MainPack pack = null;
    public override void Awake()
    {
        requestCode = RequestCode.Game;
        actionCode = ActionCode.Damage;
        base.Awake();
    }

    private void Update()
    {
        if (pack != null)
        {
            face.UpdategamePanelList(pack);
            face.Damage(pack);
            pack = null;
        }
    }


    public override void OnResponse(MainPack pack)
    {
        this.pack = pack;
    }
    
    
    public void SendRequest(float PosX,float PosY,string hituser,float startX,float startY,float endX,float endY)
    {
        MainPack pack=new MainPack();
        PosPack posPack=new PosPack();
        PlayerPack playerPack=new PlayerPack();
        BulletHitPack bulletHitPack=new BulletHitPack();
        pack.Requestcode = requestCode;
        pack.Actioncode = actionCode;
        bulletHitPack.PosX = PosX;
        bulletHitPack.PosY = PosY;
        bulletHitPack.Hituser = hituser;
        playerPack.Playername = hituser;
        posPack.PosX = startX;
        posPack.PosY = startY;
        posPack.RotZ = endX;
        posPack.GunRotZ = endY;
        playerPack.Pospack = posPack;
        pack.Bullethitpack = bulletHitPack;
        base.SendRequest(pack);
    }
}
