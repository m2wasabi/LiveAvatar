using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LiveAvatar.UI
{
    public class WebCamSelector : MonoBehaviour
    {
        private Dropdown dropdown;
        void Start()
        {
            dropdown = GetComponent<Dropdown>();
            dropdown.ClearOptions();

            var devices = new List<string>();
            for (int i = 0; i < WebCamTexture.devices.Length; i++)
            {
                devices.Add(WebCamTexture.devices[i].name);
            }
            dropdown.AddOptions(devices);
        }
    }
}