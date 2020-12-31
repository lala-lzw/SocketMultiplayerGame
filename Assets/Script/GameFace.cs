using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketGameProtocol;

public class GameFace : MonoBehaviour
{
    private ClientManager clientManager;
    private RequestManager requestManager;
    private UIManager uIManager;
    private PlayerManager playerManager;

    
    
    public string UserName
    {
        set;
        get;
    }

    private static GameFace face;
    public static GameFace Face
    {
        get
        {
            if (face == null)
            {
                face= GameObject.Find("GameFace").GetComponent<GameFace>();
            }
            return face;
        }
    }


    // Start is called before the first frame update
    void Awake()
    {
        
        
        uIManager = new UIManager(this);
        clientManager = new ClientManager(this);
        requestManager = new RequestManager(this);
        playerManager=new PlayerManager(this);


        uIManager.OnInit();
        clientManager.OnInit();
        requestManager.OnInit();
        playerManager.OnInit();
        
    }

    private void OnDestroy()
    {
        clientManager.OnDestroy();
        requestManager.OnDestroy();
        uIManager.OnDestroy();
        playerManager.OnDestroy();
    }

    private float recTime = 0;
    public bool isRec = false;
    private void Update()
    {
        if (isRec)
        {
            Debug.Log("接收消息时间间隔： "+(Time.time-recTime));
            recTime = Time.time;
            isRec = false;
        }
    }


    public void Send(MainPack pack)
    {
        clientManager.Send(pack);
    }

    public void SendTo(MainPack pack)
    {
        pack.User = UserName;
        clientManager.SendTo(pack);
    }

    public void HandleResponse(MainPack pack)
    {
        //处理
        requestManager.HandleResponse(pack);
        //Debug.Log("face处理");
    }

    public void AddRequest(BaseRequest request)
    {
        requestManager.AddRequest(request);
    }

    public void RemoveRequest(ActionCode action)
    {
        requestManager.RemoveRequest(action);
    }

    public void ShowMessage(string str,bool sync=false)
    {
        uIManager.ShowMessage(str,sync);
    }

    public void SetSelfID(string id)
    {
        playerManager.CurPlayerID = id;
    }

    public void addPlayer(MainPack packs)
    {
        playerManager.addPlayer(packs);
    }

    public void removePlayer(string id)
    {
        playerManager.removePlayer(id);
    }

    public void GameExit()
    {
        playerManager.GameExit();
        uIManager.PopPanel();
        uIManager.PopPanel();
    }

    public void UpPos(MainPack pack)
    {
        playerManager.UpPos(pack);
    }

    public void spawnBullet(MainPack pack)
    {
        playerManager.spawnBullet(pack);
    }

    public void Damage(MainPack pack)
    {
        playerManager.Damage(pack);
    }

    public void UpdategamePanelList(MainPack pack)
    {
        GamePanel panel= uIManager.GetPanel(PanelType.Game) as GamePanel;
        panel.UpdateList(pack);
    }
}
