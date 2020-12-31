#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInFoItem : MonoBehaviour
{
    [SerializeField]
    private Text nameText;
    [SerializeField]
    private Slider hpSlider;

    public void Set(string str,int v)
    {
        nameText.text = str;
        hpSlider.value = v;
    }

    public void Up(int v)
    {
        hpSlider.value = v;
    }
}
