using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelpUI : MonoBehaviour
{
    private Image helpUI;
    private RectTransform helpRect;
    private enum state
    {
        Hide,Small,Big
    }
    private state status = state.Small;

    void Start ()
    {
        helpUI = GetComponent<Image>();
        helpRect = GetComponent<RectTransform>();
    }
    
    void OnGUI () {
        if (Input.GetKeyDown(KeyCode.H))
        {
            toggleHelpUI();
        }
    }

    private void toggleHelpUI()
    {
        var duration = 0.1f;

        switch (status)
        {
            case state.Hide:
                status = state.Small;
                helpUI.CrossFadeAlpha(1, duration, true);
                helpRect.sizeDelta = new Vector2(200,200);
                break;
            case state.Small:
                status = state.Big;
                helpUI.CrossFadeAlpha(1, duration, true);
                helpRect.sizeDelta = new Vector2(400,400);
                break;
            case state.Big:
                status = state.Hide;
                helpUI.CrossFadeAlpha(0, duration, true);
                helpRect.sizeDelta = new Vector2(200, 200);
                break;
        }
    }
}
