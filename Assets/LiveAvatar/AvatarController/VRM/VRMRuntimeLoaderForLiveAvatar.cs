#pragma warning disable 0414
using System;
using System.IO;
using System.Runtime.InteropServices;
using RootMotion.FinalIK;
using UnityEngine;
using VRM;


namespace LiveAvatar.AvatarController.VRM
{
    public class VRMRuntimeLoaderForLiveAvatar : MonoBehaviour
    {
        [SerializeField]
        bool m_loadAsync;

        [SerializeField, Header("GUI")]
        CanvasManager m_canvas;

        [SerializeField]
        LookTarget m_faceCamera;

        [SerializeField, Header("loader")]
        UniHumanoid.HumanPoseTransfer m_source;

        [SerializeField]
        UniHumanoid.HumanPoseTransfer m_target;

        [SerializeField]
        RuntimeAnimatorController m_animationController;

        [SerializeField, Header("runtime")]
        VRMFirstPerson m_firstPerson;

        [SerializeField, Header("Tracker")]
        Transform m_headTarget;
        [SerializeField]
        Transform m_leftHandTarget;
        [SerializeField]
        Transform m_rightHandTarget;

        VRMBlendShapeProxy m_blendShape;

        private HandPoseController _handPoseController;

        void SetupTarget()
        {
            if (m_target != null)
            {
                m_target.Source = m_source;
                m_target.SourceType = UniHumanoid.HumanPoseTransfer.HumanPoseTransferSourceType.HumanPoseTransfer;

                m_blendShape = m_target.GetComponent<VRMBlendShapeProxy>();

                m_firstPerson = m_target.GetComponent<VRMFirstPerson>();

                var animator = m_target.GetComponent<Animator>();
                if (animator != null)
                {
                    animator.runtimeAnimatorController = m_animationController;
                    VRIK m_vrik = m_target.gameObject.AddComponent<VRIK>();
                    m_vrik.solver.spine.headTarget = m_headTarget;
                    m_vrik.solver.leftArm.target = m_leftHandTarget;
                    m_vrik.solver.rightArm.target = m_rightHandTarget;
                    m_vrik.solver.leftArm.stretchCurve = new AnimationCurve();
                    m_vrik.solver.rightArm.stretchCurve = new AnimationCurve();
                    IKSolverVR.Locomotion m_vrikLoco = m_vrik.solver.locomotion;
                    m_vrikLoco.footDistance = 0.1f;
                    m_vrikLoco.stepThreshold = 0.2f;

                    VRIKCalibrator.Calibrate(m_vrik, null, m_headTarget, null, m_leftHandTarget, m_rightHandTarget, null, null);

                    m_firstPerson.Setup();

                    _handPoseController.InitAnimator();
                    if (m_faceCamera != null)
                    {
                        m_faceCamera.Target = animator.GetBoneTransform(HumanBodyBones.Head);
                    }
                }
            }
        }

        private void Awake()
        {
            SetupTarget();
        }

        private void Start()
        {
            if (m_canvas == null)
            {
                Debug.LogWarning("no canvas");
                return;
            }

            m_canvas.LoadVRMButton.onClick.AddListener(LoadVRMClicked);

            _handPoseController = GetComponent<HandPoseController>();
        }

        void LoadVRMClicked()
        {
#if UNITY_STANDALONE_WIN
            var path = FileDialogForWindows.FileDialog("open VRM", ".vrm");
#else
            var path = Application.dataPath + "/default.vrm";
#endif
            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            var bytes = File.ReadAllBytes(path);
            // なんらかの方法でByte列を得た

            var context = new VRMImporterContext(UniGLTF.UnityPath.FromFullpath(path));

            // GLB形式でJSONを取得しParseします
            context.ParseGlb(bytes);


            // metaを取得(todo: thumbnailテクスチャのロード)
            var meta = context.ReadMeta();
            Debug.LogFormat("meta: title:{0}", meta.Title);


            // ParseしたJSONをシーンオブジェクトに変換していく
            if (m_loadAsync)
            {
                LoadAsync(context);
            }
            else
            {
                VRMImporter.LoadFromBytes(context);
                OnLoaded(context.Root);
            }
        }

        /// <summary>
        /// メタが不要な場合のローダー
        /// </summary>
        void LoadVRMClicked_without_meta()
        {
#if UNITY_STANDALONE_WIN
            var path = FileDialogForWindows.FileDialog("open VRM", ".vrm");
#else
            var path = Application.dataPath + "/default.vrm";
#endif
            if (string.IsNullOrEmpty(path))
            {
                return;
            }

#if true
            var bytes = File.ReadAllBytes(path);
            // なんらかの方法でByte列を得た

            if (m_loadAsync)
            {
                // ローカルファイルシステムからロードします
                VRMImporter.LoadVrmAsync(bytes, OnLoaded);
            }
            else
            {
                var root=VRMImporter.LoadFromBytes(bytes);
                OnLoaded(root);
            }

#else
            // ParseしたJSONをシーンオブジェクトに変換していく
            if (m_loadAsync)
            {
                // ローカルファイルシステムからロードします
                VRMImporter.LoadVrmAsync(path, OnLoaded);
            }
            else
            {
                var root=VRMImporter.LoadFromPath(path);
                OnLoaded(root);
            }
#endif
        }


        void LoadAsync(VRMImporterContext context)
        {
#if true
            var now = Time.time;
            VRMImporter.LoadVrmAsync(context, go=> {
                var delta = Time.time - now;
                Debug.LogFormat("LoadVrmAsync {0:0.0} seconds", delta);
                OnLoaded(go);
            });
#else
            // ローカルファイルシステムからロードします
            VRMImporter.LoadVrmAsync(path, OnLoaded);
#endif
        }

        void OnLoaded(GameObject root)
        {
            root.transform.SetParent(transform, false);

            // add motion
            var humanPoseTransfer = root.AddComponent<UniHumanoid.HumanPoseTransfer>();
            if (m_target != null)
            {
                GameObject.Destroy(m_target.gameObject);
            }
            m_target = humanPoseTransfer;
            SetupTarget();
        }
    }
}
