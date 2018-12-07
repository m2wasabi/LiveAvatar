using LiveAvatar.UI;
using UnityEngine;
using UnityEngine.Windows.Speech;

namespace LiveAvatar.Speech
{
    public class SpeechRecognizer : MonoBehaviour
    {
        public GameObject LeftHandController;
        private SteamVR_Controller.Device _device_L;

        private DictationRecognizer m_DictationRecognizer;

        [SerializeField] private UI_VoiceText m_ui;

        [SerializeField] private TextEffectManager _textEffectManager;
        [SerializeField] private bool IsEnabled;

        void Start () {
            m_DictationRecognizer = new DictationRecognizer();
            m_ui.SetStatus(SpeechSystemStatus.Stopped);

            m_DictationRecognizer.DictationResult += (text, confidence) =>
            {
                Debug.LogFormat("Dictation result: {0}", text);
                _textEffectManager.SetText(text);
            };

            m_DictationRecognizer.DictationHypothesis += (text) =>
            {
                Debug.LogFormat("Dictation hypothesis: {0}", text);
            };

            m_DictationRecognizer.DictationComplete += (completionCause) =>
            {
                m_ui.SetStatus(SpeechSystemStatus.Stopped);
                if (completionCause != DictationCompletionCause.Complete)
                    Debug.LogErrorFormat("Dictation completed unsuccessfully: {0}.", completionCause);
            };

            m_DictationRecognizer.DictationError += (error, hresult) =>
            {
                m_ui.SetStatus(SpeechSystemStatus.Failed);
                Debug.LogErrorFormat("Dictation error: {0}; HResult = {1}.", error, hresult);
            };


            if (LeftHandController != null)
            {
                var controller_r = LeftHandController.GetComponent<SteamVR_TrackedObject>();
                _device_L = SteamVR_Controller.Input((int)controller_r.index);
            }
        }

        void Update()
        {
            if (!IsEnabled) { return; }
            if (_device_L.GetPressDown(SteamVR_Controller.ButtonMask.Trigger) || Input.GetKeyDown(KeyCode.Space))
            {
                if (m_DictationRecognizer.Status == SpeechSystemStatus.Stopped)
                {
                    m_ui.SetStatus(SpeechSystemStatus.Running);
                    m_DictationRecognizer.Start();
                }
                else if(m_DictationRecognizer.Status == SpeechSystemStatus.Running)
                {
                    m_ui.SetStatus(SpeechSystemStatus.Stopped);
                    m_DictationRecognizer.Stop();
                }
            }
        }

        public void SetEnabled(bool flag)
        {
            IsEnabled = flag;
            if (!flag)
            {
                if (m_DictationRecognizer.Status == SpeechSystemStatus.Running)
                {
                    m_ui.SetStatus(SpeechSystemStatus.Stopped);
                    m_DictationRecognizer.Stop();
                }
            }
        }
    }
}

