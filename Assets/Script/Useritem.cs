#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Useritem : MonoBehaviour
{
    [SerializeField]
    private Text playername;
    
    public void SetPlayerInFo(string name)
    {
        playername.text = name;
    }
}
