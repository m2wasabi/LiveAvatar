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
        private SpeechApi _speechApi;
        public bool speech = false;

        private void Start()
        {
            _speechApi = new SpeechApi();
        }

        public void SetText(string text)
        {
            if (speech)
            {
                Voice(text); 
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

        void Voice(string text)
        {
            StartCoroutine(_speechApi.Speech(text));
        }

        public void SetSpeechEnabled(bool flag)
        {
            speech = flag;
        }

        public void SetSpeechUrl(string url)
        {
            _speechApi.SetUrl(url);
        }
    }
}
