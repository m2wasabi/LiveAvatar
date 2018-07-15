using UnityEngine;

using System.Collections;



public class StartMultiDisplay : MonoBehaviour {



	void Start () 

	{

		int maxDisplayCount = 2;

		for (int i=0; i<maxDisplayCount && i<Display.displays.Length; i++) {

			Display.displays[i].Activate();

		}

	}

}