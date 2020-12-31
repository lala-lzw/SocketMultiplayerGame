using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomItem : MonoBehaviour
{
    public Button join;
    public Text title, num, statc;

    public RoomListPanel roomListPanel;
    // Start is called before the first frame update
    void Start()
    {
        join.onClick.AddListener(OnJoinClick);
    }


    private void OnJoinClick()
    {
        roomListPanel.JoinRoom(title.text);
    }

    public void SetRoomInfo(string title,int curnum,int maxnum,int statc)
    {
        this.title.text = title;
        this.num.text = curnum + "/" + maxnum;
        switch (statc)
        {
            case 0:
                this.statc.text = "等待加入";
                break;
            case 1:
                this.statc.text = "房间已满人";
                break;
            case 2:
                this.statc.text = "游戏中";
                break;
        }
    }
    
}
