using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PowerDisplay : MonoBehaviour
{
    public Power target;
    private TextMeshPro text;

    private void Awake()
    {
        text = GetComponent<TextMeshPro>();
    }
    public void Init(Power newTarget)
    {
        target = newTarget;

        if (text != null && target != null)
        {
            text.text = target.power.ToString();
        }
    }
    // private void Start()
    // {
    //     text.text = target.power.ToString();
    // }

}
