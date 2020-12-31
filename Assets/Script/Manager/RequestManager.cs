using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketGameProtocol;

public class RequestManager : BaseManager
{
    public RequestManager(GameFace face) : base(face){}

    private Dictionary<ActionCode, BaseRequest> requestDict = new Dictionary<ActionCode, BaseRequest>();

    public void AddRequest(BaseRequest request)
    {
        requestDict.Add(request.GetActionCode, request);
        Debug.Log(requestDict.Count);
    }

    public void RemoveRequest(ActionCode action)
    {
        requestDict.Remove(action);
    }

    public void HandleResponse(MainPack pack)
    {
        if(requestDict.TryGetValue(pack.Actioncode,out BaseRequest request))
        {
            request.OnResponse(pack);
            //Debug.Log("re处理"+request.GetActionCode.ToString());
        }
        else
        {
            Debug.LogWarning("不能找到对应的处理");
        }
    }
}
