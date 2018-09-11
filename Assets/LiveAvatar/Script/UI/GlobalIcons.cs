using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;

namespace LiveAvatar.UI
{
    public class GlobalIcons : MonoBehaviour
    {

        private ToggleGroup ToggleGroup;
        private IEnumerable<Toggle> Toggles;

        public SteamVR_TrackedObject SteamController;
        private SteamVR_Controller.Device _device;
    
        void Start ()
        {
            if (ToggleGroup == null) ToggleGroup = GetComponent<ToggleGroup>();
            Toggles = GetComponentsInChildren<Toggle>();

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
            if(!Toggles.Any()) return;
            var target = Toggles.SkipWhile(toggle => !toggle.isOn).Skip(1).FirstOrDefault();
            if (target == null || target.isOn)
            {
                target = Toggles.FirstOrDefault(toggle => !toggle.isOn);
            }
            target.isOn = true;
        }
    }
}

