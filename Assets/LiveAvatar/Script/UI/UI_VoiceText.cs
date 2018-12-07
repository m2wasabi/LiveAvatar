using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;

namespace LiveAvatar.UI
{
    public class UI_VoiceText : MonoBehaviour
    {
        [SerializeField]
        private GameObject IsOn;

        private Image target;
        private void Start()
        {
            target = IsOn.GetComponent<Toggle>().transform.Find("Background").GetComponent<Image>();
        }

        public void SetStatus(SpeechSystemStatus stat)
        {
            if (stat == SpeechSystemStatus.Stopped)
            {
                target.color = Color.white;
            }
            else if (stat == SpeechSystemStatus.Running)
            {
                target.color = Color.green;
            }
            else
            {
                target.color = Color.red;
            }
        }
    }
}
