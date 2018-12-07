using LiveAvatar.Speech;
using UnityEngine;

namespace LiveAvatar.UI
{
    public class ScenarioModeSelector : MonoBehaviour
    {
        [SerializeField]
        private ScenarioController _scenarioController;
        [SerializeField]
        private SpeechRecognizer _speechRecognizer;

        public void SetScenarioMode(int index)
        {
            switch (index)
            {
                    case 0:
                        _speechRecognizer.SetEnabled(false);
                        _scenarioController.SetEnabled(true);
                        break;
                    case 1:
                        _scenarioController.SetEnabled(false);
                        _speechRecognizer.SetEnabled(true);
                        break;
            }
        }
    }
}
