using UnityEngine;
using UnityEngine.UI;

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
        }
    }
}
