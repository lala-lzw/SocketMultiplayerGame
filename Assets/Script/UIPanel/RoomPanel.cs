using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SocketGameProtocol;

public class RoomPanel : BasePanel
{
    public Button back, send, start;
    public InputField inputtext;
    public Scrollbar scrollbar;

    public Text chattext;
    public Transform content;

    public GameObject UserItemobj;

    public RoomExitRequest roomExitRequest;
    public ChatRequest chatRequest;
    public StartGameRequest startGameRequest;

    private void Start()
    {
        back.onClick.AddListener(OnBackClick);
        send.onClick.AddListener(OnSendClick);
        start.onClick.AddListener(OnStartClick);
    }

    private void OnBackClick()
    {
        roomExitRequest.SendRequest();
    }

    private void OnSendClick()
    {
        if (inputtext.text == "")
        {
            uiMag.ShowMessage("发送内容不能为空！");
            return;;
        }
        chatRequest.SendRequest(inputtext.text);
        chattext.text += "我：" + inputtext.text+"\n";
        inputtext.text = "";
    }

    private void OnStartClick()
    {
        startGameRequest.SendRequest();
    }

    /// <summary>
    /// 刷新玩家列表
    /// </summary>
    /// <param name="pack"></param>
    public void UpdatePlayerList(MainPack pack)
    {
        for(int i = 0; i < content.childCount; i++)
        {
            Destroy(content.GetChild(i).gameObject);
        }

        foreach(PlayerPack player in pack.Playerpack)
        {
            if (PlayerPrefs.GetString("username").Equals(player.Playername))
            {
                PlayerPrefs.SetString("id",pack.Playerpack.IndexOf(player).ToString());
            }
            Useritem useritem = Instantiate(UserItemobj, Vector3.zero, Quaternion.identity).GetComponent<Useritem>();
            useritem.gameObject.transform.SetParent(content);
            useritem.SetPlayerInFo(player.Playername);
        }
    }

    public void ExitRoomResponse()
    {
        uiMag.PopPanel();
    }

    public void ChatResponse(string str)
    {
        chattext.text += str + "\n";
    }

    public void StartGameResponse(MainPack pack)
    {
        switch (pack.Returncode)
        {
            case ReturnCode.Fail:
                uiMag.ShowMessage("开始游戏失败！您不是房主");
                start.interactable = false;
                break;
            case ReturnCode.Succeed:
                uiMag.ShowMessage("游戏已启动！");
                start.interactable = false;
                break;
        }
    }

    public void GameStarting(MainPack packs)
    {
        GamePanel gamePanel= uiMag.PushPanel(PanelType.Game).GetComponent<GamePanel>();
        gamePanel.UpdateList(packs);
    }


    public override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("进入房间");
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
        chattext.text = "";
        start.interactable = true;
        Debug.Log("enterroom");
    }

    private void Exit()
    {
        gameObject.SetActive(false);
    }
}
