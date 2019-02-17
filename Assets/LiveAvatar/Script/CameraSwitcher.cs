using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    [SerializeField]
    private Transform camerapos;

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftAlt))
        {
            if(Input.GetKeyDown(KeyCode.Alpha1)) SetCam1();
            if(Input.GetKeyDown(KeyCode.Alpha2)) SetCam2();
            if(Input.GetKeyDown(KeyCode.Alpha3)) SetCam3();
        } 
    }

    public void SetCam1()
    {
        camerapos.position = Vector3.zero;
        camerapos.rotation = Quaternion.identity;
    }
    public void SetCam2()
    {
        camerapos.position = new Vector3(0,0,-1.64f);
        camerapos.rotation = Quaternion.identity;
    }
    public void SetCam3()
    {
        camerapos.position = new Vector3(0.36f,0.3f,-0.27f);
        camerapos.rotation = Quaternion.identity;
        camerapos.Rotate(0,31.61f,0);
    }
}
