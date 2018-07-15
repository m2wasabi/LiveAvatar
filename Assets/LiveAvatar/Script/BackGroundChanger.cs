using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundChanger : MonoBehaviour {

public GameObject BackGroundObject;
	void Start () {
		
	}
	
	void Update () {
        if (Input.GetKeyDown(KeyCode.G))
        {
            BackGroundObject.SetActive(!BackGroundObject.activeSelf);
        }
	}
}
