using UnityEngine;
using UniRx;
using UniRx.Triggers;

namespace LiveAvatar.UI
{
    public class ConfigMenu : MonoBehaviour
    {
        [SerializeField]
        private GameObject ConfigUI;

        void Start () {
            this.UpdateAsObservable()
                .Where(_ => Input.GetKeyDown(KeyCode.Escape))
                .Subscribe(_ => ToggleUI());
        }

        public void ToggleUI()
        {
            ConfigUI.SetActive(!ConfigUI.activeSelf);
        }
    }
}
