using LiveAvatar.Speech;
using UnityEngine;

namespace LiveAvatar.UI
{
    public class SpeechEngineSelector : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_seikaServerUI;
        [SerializeField]
        private GameObject m_remoteTalkUI;

        public void SetSpeechEngine(int index)
        {
            switch (index)
            {
                    case 0:
                        m_seikaServerUI.SetActive(false);
                        m_remoteTalkUI.SetActive(false);
                        break;
                    case 1:
                        m_seikaServerUI.SetActive(true);
                        m_remoteTalkUI.SetActive(false);
                        break;
                    case 2:
                        m_seikaServerUI.SetActive(false);
                        m_remoteTalkUI.SetActive(true);
                        break;
            }
        }
    }
}
