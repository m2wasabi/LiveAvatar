using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TelopSwitch : MonoBehaviour
{
    public Text telopTarget;

    private string telopText = null;
    private Toggle toggle;

    void Start ()
    {
        telopText = GetComponentInChildren<Text>().text;
        toggle = GetComponent<Toggle>();
    }
    
    public void OnSwitch()
    {
        if (toggle.isOn)
        {
            telopTarget.text = telopText;
        }
        else
        {
            if (telopTarget.text == telopText)
            {
                telopTarget.text = "";
            }
        }
    }
}
