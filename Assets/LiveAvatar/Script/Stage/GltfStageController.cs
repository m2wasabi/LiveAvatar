using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UniGLTF;
using VRM.Samples;

namespace LiveAvatar.Stage
{
    public class GltfStageController : MonoBehaviour
    {
        [SerializeField]
        private Button m_loadButton;

        [SerializeField]
        private Light m_defaultLight;

        GameObject m_target;

        private void Start()
        {
            m_loadButton.onClick.AddListener(OpenGlbLoadDialog);
        }

        void OpenGlbLoadDialog()
        {
#if UNITY_STANDALONE_WIN
            var path = FileDialogForWindows.FileDialog("open GLB", ".glb",".zip",".gltf");
#else
            var path = Application.dataPath + "/default.vrm";
#endif
            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            var bytes = File.ReadAllBytes(path);
            // なんらかの方法でByte列を得た

            var context = new ImporterContext();
            context.ParseGlb(bytes);
            var now = Time.time;
            gltfImporter.LoadVrmAsync(path, bytes, go=> {
                var delta = Time.time - now;
                Debug.LogFormat("LoadVrmAsync {0:0.0} seconds", delta);
                OnLoaded(go);
            });
        }

        void OnLoaded(GameObject root)
        {
            // Lightが無い場合にデフォルトライト点灯
            m_defaultLight.enabled = (root.transform.GetComponentsInChildren<Light>().Length == 0);
            root.transform.SetParent(transform, false);

            // add motion
            if (m_target != null)
            {
                Destroy(m_target);
            }
            m_target = root;
        }
    }
}