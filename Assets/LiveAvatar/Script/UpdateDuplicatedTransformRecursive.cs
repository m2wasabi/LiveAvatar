using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateDuplicatedTransformRecursive : MonoBehaviour
{
    public GameObject TargetObject;

    public bool Enabled = false;

	// Update is called once per frame
	void LateUpdate () {
	    if (Enabled && TargetObject != null)
	    {
	        var src = GetComponentsInChildren<Transform>();
            var dest = TargetObject.GetComponentsInChildren<Transform>();
            if (src != null)
            {
                int i;
                for (i = 0; i < src.Length; i++)
                {
                    if(i == 0) continue;
//                    dest[i].transform.position = src[i].transform.position;
                    dest[i].transform.localRotation = src[i].transform.localRotation;
                }
            }
	    }
	}
}
