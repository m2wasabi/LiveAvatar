using UnityEngine;
using UnityEngine.UI;
using LiveAvatar.AvatarController.VRM;

namespace LiveAvatar
{
    public class LipSyncManager : MonoBehaviour
    {
        [SerializeField]
        private Dropdown LipsyncConfig;

        [SerializeField]
        private BlendShapeController _blendShapeController;

        [SerializeField]
        private VRMLipSyncContextMorphTarget _vrmLipSync;

        void Start ()
        {
            LipsyncConfig.onValueChanged.AddListener(OnChangeConfig);
        }

        private void OnChangeConfig(int config)
        {
            switch (config)
            {
                case 0:
                    _blendShapeController.CameraMouth = true;
                    _vrmLipSync.enabled = false;
                    break;
                case 1:
                    _blendShapeController.CameraMouth = false;
                    _vrmLipSync.enabled = true;
                    break;
                case 2:
                    _blendShapeController.CameraMouth = false;
                    _vrmLipSync.enabled = false;
                    break;
            }
        }
    }
}
