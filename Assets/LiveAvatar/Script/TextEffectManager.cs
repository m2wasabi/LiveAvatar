using System;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

namespace LiveAvatar
{
    public class TextEffectManager : MonoBehaviour
    {

        [SerializeField] private Text frontText;
        public bool speech = false;

        public void SetText(string text)
        {
            if (speech)
            {
                // ToDo : SpeechEngine 
            }
            TriggerFrontText(text);
        }

        void TriggerFrontText(string text)
        {
            frontText.text = text;
            Observable.Timer(TimeSpan.FromSeconds(10))
                .Where(_ => frontText.text == text)
                .Subscribe(_ => frontText.text = "")
                .AddTo(gameObject);
        }
    }
}
