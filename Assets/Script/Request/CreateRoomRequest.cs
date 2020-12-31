using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketGameProtocol;

public class CreateRoomRequest : BaseRequest
{
    public RoomListPanel roomListPanel;
    private MainPack pack = null;

    public override void Awake()
    {
        requestCode = RequestCode.Room;
        actionCode = ActionCode.CreateRoom;
        base.Awake();
    }

    public void SendRequest(string roomname,int maxnum)
    {
        MainPack pack = new MainPack();
        pack.Requestcode = requestCode;
        pack.Actioncode = actionCode;
        RoomPack room = new RoomPack();
        room.Roomname = roomname;
        room.Maxnum = maxnum;
        pack.Roompack.Add(room);
        base.SendRequest(pack);
    }

    private void Update()
    {
        if (pack != null)
        {
            roomListPanel.CreateRoomResponse(pack);
            pack = null;
        }
    }

    public override void OnResponse(MainPack pack)
    {
        this.pack = pack;
    }
}
