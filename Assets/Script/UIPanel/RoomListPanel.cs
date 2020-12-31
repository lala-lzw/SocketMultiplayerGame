using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SocketGameProtocol;

public class RoomListPanel : BasePanel
{
    public Button back, find, create;
    public InputField roomname;
    public Slider num;

    public Transform roomListTransform;
    public GameObject roomitem;

    public CreateRoomRequest createRoomRequest;
    public FindRoomRequest findRoomRequest;
    public JoinRoomRequest joinRoomRequest;
    private void Start()
    {
        back.onClick.AddListener(OnBackClick);
        find.onClick.AddListener(OnFindClick);
        create.onClick.AddListener(OnCreateClick);
    }

    /// <summary>
    /// 注销登录
    /// </summary>
    private void OnBackClick()
    {
        uiMag.PopPanel();
    }
    /// <summary>
    /// 查询房间
    /// </summary>
    private void OnFindClick()
    {
        findRoomRequest.SendRequest();
    }
    /// <summary>
    /// 创建房间
    /// </summary>
    private void OnCreateClick()
    {
        if (roomname.text == "")
        {
            uiMag.ShowMessage("房间名不能为空！");
            return;
        }
        createRoomRequest.SendRequest(roomname.text,(int)num.value);
    }

    public void CreateRoomResponse(MainPack pack)
    {
        switch (pack.Returncode)
        {
            case ReturnCode.Succeed:
                uiMag.ShowMessage("创建成功！");
                RoomPanel roomPanel = uiMag.PushPanel(PanelType.Room).GetComponent<RoomPanel>();
                roomPanel.UpdatePlayerList(pack);
                break;
            case ReturnCode.Fail:
                uiMag.ShowMessage("创建失败！");
                break;
            default:
                Debug.Log("def");
                break;
        }
    }

    public void FindRoomResponse(MainPack pack)
    {
        switch (pack.Returncode)
        {
            case ReturnCode.Succeed:
                uiMag.ShowMessage("查询成功！一共有"+pack.Roompack.Count+"个房间");
                break;
            case ReturnCode.Fail:
                uiMag.ShowMessage("查询出错！");
                break;
            case ReturnCode.NotRoom:
                uiMag.ShowMessage("当前没有房间！");
                break;
            default:
                Debug.Log("def");
                break;
        }
        UpdateRoomList(pack);
    }

    private void UpdateRoomList(MainPack pack)
    {
        //清空房间列表
        for(int i = 0; i < roomListTransform.childCount; i++)
        {
            Debug.Log("清除"+i);
            Destroy(roomListTransform.GetChild(i).gameObject);
        }

        if (pack.Roompack.Count == 0) return;
        foreach(RoomPack room in pack.Roompack)
        {
            RoomItem item = Instantiate(roomitem,Vector3.zero,Quaternion.identity).GetComponent<RoomItem>();
            item.roomListPanel = this;
            item.gameObject.transform.SetParent(roomListTransform);
            item.SetRoomInfo(room.Roomname, room.Curnum, room.Maxnum, room.Statc);
        }

    }


    public void JoinRoomResponse(MainPack pack)
    {
        switch (pack.Returncode)
        {
            case ReturnCode.Succeed:
                uiMag.ShowMessage("加入房间成功！");
                RoomPanel roomPanel= uiMag.PushPanel(PanelType.Room).GetComponent<RoomPanel>();
                roomPanel.UpdatePlayerList(pack);
                break;
            case ReturnCode.Fail:
                uiMag.ShowMessage("加入房间失败！");
                break;
            default:
                uiMag.ShowMessage("房间不存在！");
                break;
        }
    }

    public void JoinRoom(string roomname)
    {
        joinRoomRequest.SendRequest(roomname);
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
}
