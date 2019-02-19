using System;
using LiveAvatar.Speech;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

namespace LiveAvatar
{
    public class TextEffectManager : MonoBehaviour
    {

        [SerializeField] private Text frontText;
        [SerializeField] private Text monitorText;
        private RemoteTalkManager _remoteTalkManager;
        private SeikaCenterManager _seikaCenterManager;
        public bool speech = false;
        public enum SpeechEngine
        {
            None,
            SeikaServer,
            RemoteTalk
        }

        [SerializeField]
        private Dropdown speechEngine;

        private void Start()
        {
            _remoteTalkManager = transform.GetComponent<RemoteTalkManager>();
            _seikaCenterManager = transform.GetComponent<SeikaCenterManager>();
        }

        public void SetText(string text)
        {
            var activeSpeechEngine = (SpeechEngine) Enum.ToObject(typeof(SpeechEngine), speechEngine.value);
            switch (activeSpeechEngine)
            {
                case SpeechEngine.SeikaServer:
                    SpeechSeikaServer(text); 
                    break;
                case SpeechEngine.RemoteTalk:
                    SpeechRemoteTalk(text);
                    break;
            }
            TriggerFrontText(text);
            TriggerMonitorText(text);
        }

        void TriggerFrontText(string text)
        {
            frontText.text = text;
            Observable.Timer(TimeSpan.FromSeconds(10))
                .Where(_ => frontText.text == text)
                .Subscribe(_ => frontText.text = "")
                .AddTo(gameObject);
        }

        void TriggerMonitorText(string text)
        {
            monitorText.text = text;
            Observable.Timer(TimeSpan.FromSeconds(10))
                .Where(_ => monitorText.text == text)
                .Subscribe(_ => monitorText.text = "")
                .AddTo(gameObject);
        }

        void SpeechSeikaServer(string text)
        {
            _seikaCenterManager.Speech(text);
        }

        void SpeechRemoteTalk(string text)
        {
            _remoteTalkManager.Speech(text);
        }

        public void SetSpeechEnabled(bool flag)
        {
            speech = flag;
        }
    }
}
