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
        private Toggle[] Toggles;

        public SteamVR_TrackedObject SteamController;
        private SteamVR_Controller.Device _device;
    
        void Start ()
        {
            if (ToggleGroup == null) ToggleGroup = GetComponent<ToggleGroup>();
            Toggles = GetComponentsInChildren<Toggle>();

            if (SteamController != null)
            {
                _device = SteamVR_Controller.Input((int) SteamController.index);
                this.ObserveEveryValueChanged(t => _device.GetTouch(SteamVR_Controller.ButtonMask.Trigger))
                    .Subscribe(x => SetToggleActive(x?0:1));
            }
        }

        void SetToggleActive(int index)
        {
            Toggles[index].isOn = true;
        }
    }
}

