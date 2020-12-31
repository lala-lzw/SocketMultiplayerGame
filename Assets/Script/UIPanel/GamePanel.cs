using System;
using System.Collections;
using System.Collections.Generic;
using SocketGameProtocol;
using UnityEngine;
using UnityEngine.UI;

public class GamePanel : BasePanel
{
    public GameObject item;
    public Transform listTransform;
    public Text Timetext;
    public Button exitBtn;
    public GameExitRequest gameExitRequest;
    
    private float starttime;
    
    private Dictionary<string,PlayerInFoItem> itemList=new Dictionary<string, PlayerInFoItem>();

    private void Start()
    {
        starttime = Time.time;
        exitBtn.onClick.AddListener(OnExitClick);
    }

    public void UpdateList(MainPack packs)
    {
        for (int i = 0; i < listTransform.childCount; i++)
        {
            GameObject.Destroy(listTransform.GetChild(i).gameObject);
        }
        itemList.Clear();
        foreach (var p in packs.Playerpack)
        {
            GameObject g= Instantiate(item, Vector3.zero, Quaternion.identity);
            g.transform.SetParent(listTransform);
            PlayerInFoItem pinfo = g.GetComponent<PlayerInFoItem>();
            pinfo.Set(p.Playername,p.Hp);
            itemList.Add(p.Playername,pinfo);
        }
    }

    public void UpdateValue(string id,int v)
    {
        if (itemList.TryGetValue(id, out PlayerInFoItem pinfo))
        {
            pinfo.Up(v);
        }
        else
        {
            Debug.Log("获取不到对应的角色信息");
        }
    }

    private void FixedUpdate()
    {
        Timetext.text= Mathf.Clamp((int) (Time.time - starttime), 0, 300).ToString();
    }

    private void OnExitClick()
    {
        gameExitRequest.SendRequest();
        face.GameExit();
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
        Debug.Log("enterroom");
    }

    private void Exit()
    {
        gameObject.SetActive(false);
    }
}
