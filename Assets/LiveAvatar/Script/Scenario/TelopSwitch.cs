using LiveAvatar;
using UnityEngine;
using UnityEngine.UI;

public class TelopSwitch : MonoBehaviour
{
    public TextEffectManager textManager;

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
            textManager.SetText(telopText);
        }
    }
}
