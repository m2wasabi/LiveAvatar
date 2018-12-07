using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundChanger : MonoBehaviour {

    [SerializeField]
    private List<Camera> Cameras;
    [SerializeField]
    private List<GameObject> BackGroundObjects;

    void SetObjectOff()
    {
        foreach (var bgo in BackGroundObjects)
        {
            bgo.SetActive(false);
        }
    }

    void SetBGColor(Color color)
    {
        foreach (var camera in Cameras)
        {
            camera.backgroundColor = color;
        }
        SetObjectOff();
    }

    void SetBGObject(int index)
    {
        SetObjectOff();
        BackGroundObjects[index].SetActive(true);
    }

    public void OnChangeSetting(int index)
    {
        switch (index)
        {
                case 0:
                    SetBGObject(0);
                    break;
                case 1:
                    SetBGColor(Color.green);
                    break;
                case 2:
                    SetBGColor(Color.white);
                    break;
                default:
                    break;
        }
    }
}
