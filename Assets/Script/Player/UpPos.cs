using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpPos : MonoBehaviour
{
    private UpPosRequest upPosRequest;

    private Transform gunTransform;
    // Start is called before the first frame update
    void Start()
    {
        upPosRequest = GetComponent<UpPosRequest>();
        gunTransform = transform.Find("HandGun");
        InvokeRepeating("UpPosFun",1,1f/10f);
    }

    private void UpPosFun()
    {
        Vector2 pos = transform.position;
        float characterRot = transform.eulerAngles.z;
        float gunRot = gunTransform.eulerAngles.z;
        upPosRequest.SendRequest(pos,characterRot,gunRot);
    }
    
}
