using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;

public class GlobalIcons : MonoBehaviour
{

    private ToggleGroup ToggleGroup;
    private IEnumerable<Toggle> Toggles;

    public SteamVR_TrackedObject SteamController;
    private SteamVR_Controller.Device _device;
    
	void Start ()
	{
	    if (ToggleGroup == null) ToggleGroup = GetComponent<ToggleGroup>();
	    if (ToggleGroup != null)
	    {
	        Toggles = ToggleGroup.ActiveToggles();
	    }

	    if (SteamController != null)
	    {
	        _device = SteamVR_Controller.Input((int) SteamController.index);
	        this.UpdateAsObservable()
	            .Where(_ => _device.GetPressDown(SteamVR_Controller.ButtonMask.Grip))
	            .Subscribe(_ => ToggleForward());
	    }
	}
	
    void ToggleForward()
    {
        Debug.Log("Toggle Forward");
    }
}
