using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRM;

namespace LiveAvatar.AvatarController.VRM
{
    public class FaceLandmarkMorph : MonoBehaviour
    {
        public GameObject TrackedCamera;
        public GameObject BodyAnchor;
        public GameObject HeadAnchor;
        public BlendShapeController BlendShapeController;
        public FaceExpressionController FaceExpressionController;

        private Rect _rect;
        private List<Vector2> _landmarkList;
        private bool isActive = false;

        private Vector2Int WebCameraScreenSize = new Vector2Int(320,240);

        [SerializeField]
        private Vector3 BodyPos;
        [SerializeField]
        private Vector3 HeadAng;

        private Vector3[] BodyPosBuffer = new Vector3[5];
        private Vector3[] HeadAngBuffer = new Vector3[5];

        // キャラクター制御パラメーターの調整値
        protected float BodyPosX = -3;
        protected float BodyPosY = 3;
        protected float BodyPosRatioZ = 1.2f;
        protected float BodyPosOffsetZ = 0.0f;
        protected Vector3 HeadAngleOffset;
        protected float HeadRotateOffsetX = 0;
        protected float HeadRotateOffsetY = 0;
        protected float HeadRotateRatioY = 70;
        protected float HeadRotateOffsetZ = 90;
        protected float HeadRotateRatioX = -300;
        protected float LipOpenRatio = 2;

        [SerializeField]
        private float smileParam;

        // Use this for initialization
        void Start()
        {
            WebCamManager.Instance.OnFacelandmarkUpdated += fadeDetedtedEvent;
            WebCameraScreenSize.x = WebCamManager.Instance.requestedWidth;
            WebCameraScreenSize.y = WebCamManager.Instance.requestedHeight;
            if (HeadAnchor)
            {
                HeadAngleOffset = HeadAnchor.transform.localEulerAngles;
            }
            else
            {
                HeadAngleOffset = Vector3.zero;
            }
        }

        void OnDestroy()
        {
            if (WebCamManager.Instance != null)
            {
                WebCamManager.Instance.OnFacelandmarkUpdated -= fadeDetedtedEvent;
            }
        }

        public void setDetectResult(Rect rect, List<Vector2> landmarkList)
        {
            _rect = rect;
            _landmarkList = landmarkList;
            isActive = true;
        }

        public void setCameraParam(int width, int height)
        {
            WebCameraScreenSize = new Vector2Int(width, height);
        }

        private void calcParams()
        {
            if (!isActive) return;
            BodyPos = GetBodyPos(_rect);
            HeadAng = GetHeadAng(_landmarkList);
            UnshiftBuffer(BodyPosBuffer, BodyPos);
            UnshiftBuffer(HeadAngBuffer, HeadAng);

            if (BlendShapeController)
            {
                BlendShapeController.MouthOpen = GetMouthOpen(_landmarkList);
                BlendShapeController.LeftEyeOpen = GetLeftEyeOpenRatio(_landmarkList);
                BlendShapeController.RightEyeOpen = GetRightEyeOpenRatio(_landmarkList);
                FaceExpressionController.AutoDetectedFace = GetFaceExpression(_landmarkList);
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (isActive)
            {
                var _bodyPos = AvarageBuffer(BodyPosBuffer);
                BodyAnchor.transform.position = TrackedCamera.transform.rotation * _bodyPos + TrackedCamera.transform.position;
                // 体は水平に保つ移動(オイラー角のYのみ適用) + カメラに向かせる
                BodyAnchor.transform.rotation = Quaternion.AngleAxis(180.0f,Vector3.up) * Quaternion.Euler(0, TrackedCamera.transform.rotation.eulerAngles.y, 0) ;
                var _headAng = AvarageBuffer(HeadAngBuffer);
                // 頭の角度と 体とカメラの傾きの成分を打ち消す
                HeadAnchor.transform.localEulerAngles = HeadAngleOffset + _headAng + new Vector3(- TrackedCamera.transform.rotation.eulerAngles.x ,0,- TrackedCamera.transform.rotation.eulerAngles.z);
            }

            isActive = false;
        }

        private void fadeDetedtedEvent(object sender, FacelandmarkResultEventArgs facelandmarkResultEventArgs)
        {
            setDetectResult(facelandmarkResultEventArgs.Rect, facelandmarkResultEventArgs.LandmarkList);
            calcParams();
        }

        /// <summary>
        /// 体位置を取得
        /// </summary>
        /// <param name="rect">顔の矩形</param>
        /// <returns>体位置</returns>
        /// <remarks>
        /// https://software.intel.com/sites/landingpage/realsense/camera-sdk/v2016r3/documentation/html/index.html?doc_face_face_location_data.html
        /// </remarks>
        Vector3 GetBodyPos(Rect rect)
        {
            // 体位置に利用するため頭位置を取得
            float xMax = WebCameraScreenSize.x;
            float yMax = WebCameraScreenSize.y;
            float xPos = rect.x + (rect.width / 2);
            float yPos = rect.y + (rect.height / 2);
            float zPos = (yMax - rect.height) / yMax;

            // 末尾の除算で調整
            xPos = (xPos - (xMax / 2)) / (xMax / 2) / BodyPosX * zPos;
            yPos = -(yPos - (yMax / 2)) / (yMax / 2) / BodyPosY * zPos;
            zPos = zPos * BodyPosRatioZ + BodyPosOffsetZ;

            // 顔の大きさと中心から初期位置分ずらして体位置に利用
            return new Vector3(xPos, yPos, zPos);
        }

        /// <summary>
        /// 頭角度を取得
        /// </summary>
        /// <param name="points">顔の検出点</param>
        /// <returns>頭角度</returns>
        /// <remarks>
        /// https://software.intel.com/sites/landingpage/realsense/camera-sdk/v2016r3/documentation/html/index.html?doc_face_face_landmark_data.html
        /// </remarks>
        Vector3 GetHeadAng(List<Vector2> points)
        {
            // 頭向きに利用するため顔の中心と左右端、唇下、顎下を取得
            Vector2 center = points[30];
            Vector2 left = points[14];
            Vector2 right = points[2];
            Vector2 mouth = points[57];
            Vector2 chin = points[8];

            // 末尾で調整(0.2は顔幅に対する唇下から顎までの比 / 300はその値に対する倍率 / 10.416はUnityちゃん初期値)
            // エラの左右と顔のセンターの比で顔の左右の向きを判別
            float yAng = (Vector2.Distance(right, center) - Vector2.Distance(left, center)) / Vector2.Distance(left, right) * HeadRotateRatioY + HeadRotateOffsetY;
            // 2次元画像で口の下辺と顎の先の角度(z回転) 真上が基準なのでoffset 90度を引く
            float zAng = GetAngle(mouth, chin) - HeadRotateOffsetZ;
            // 顎の長さと顔の横幅の比
            float xAng = (Vector2.Distance(mouth, chin) / Vector2.Distance(left, right) - 0.2f) * HeadRotateRatioX + HeadRotateOffsetX;

            // 唇下と顎下の点から角度計算して頭向きに利用
            return new Vector3(xAng, yAng, zAng);
        }

        /// <summary>
        /// 3点の中間比を求める
        /// </summary>
        /// <param name="v1">端1</param>
        /// <param name="center">中点</param>
        /// <param name="v2">端2</param>
        /// <returns>中点比</returns>
        protected float GetCenterRatio(Vector2 v1, Vector2 center, Vector2 v2)
        {
            return (Vector2.Distance(v1, center) - Vector2.Distance(v2, center)) / Vector2.Distance(v1, v2);
        }

        /// <summary>
        /// 2点間の角度を求める
        /// http://qiita.com/2dgames_jp/items/60274efb7b90fa6f986a
        /// https://gist.github.com/mizutanikirin/e9a71ef994ebb5f0d912
        /// </summary>
        /// <param name="p1">点1</param>
        /// <param name="p2">点2</param>
        /// <returns>角度</returns>
        protected float GetAngle(Vector2 p1, Vector2 p2)
        {
            float dx = p2.x - p1.x;
            float dy = p2.y - p1.y;
            float rad = Mathf.Atan2(dy, dx);
            return rad * Mathf.Rad2Deg;
        }

        private void UnshiftBuffer( Vector3[] buf, Vector3 val)
        {
            if (buf.Length == 0) return;

            int i;
            for(i = buf.Length - 1; i > 0; i--)
            {
                buf[i] = buf[i - 1];
            }
            buf[0] = val;
        }

        private Vector3 AvarageBuffer(Vector3[] buf)
        {
            Vector3 ave = Vector3.zero;
            int i;
            int count = 0;
            for (i = 0; i < buf.Length; i++)
            {
                if (buf[i] != Vector3.zero)
                {
                    ave += buf[i];
                    count++;
                }
            }
            if (count != 0)
            {
                ave = ave / count;
            }
            return ave;
        }

        private float GetMouthOpen(List<Vector2> points)
        {
            Vector2 upperLipTop = points[51];
            Vector2 upperLipBottom = points[62];
            Vector2 lowerLipTop = points[66];
            Vector2 lowerLipBottom = points[57];
            return (lowerLipTop.y - upperLipBottom.y)/(lowerLipBottom.y - upperLipTop.y) * LipOpenRatio;
        }

        private float GetLeftEyeOpenRatio (List<Vector2> points)
        {
            float size = Mathf.Abs (points [44].y - points [46].y) / Mathf.Abs (points [27].y - points [30].y);
            return Mathf.InverseLerp (0.1f, 0.16f, size);
        }

        private float GetRightEyeOpenRatio (List<Vector2> points)
        {
            float size = Mathf.Abs (points [37].y - points [41].y) / Mathf.Abs (points [27].y - points [30].y);
            return Mathf.InverseLerp (0.1f, 0.16f, size);
        }

        BlendShapeKey GetFaceExpression(List<Vector2> points)
        {
            Vector2 noseTop = points[33];
            Vector2 mouthLeftMiddle = points[60];
            Vector2 mouthRightMiddle = points[64];

            var mouthSide = Vector2.Angle(mouthRightMiddle - mouthLeftMiddle , noseTop - mouthLeftMiddle);
            smileParam = mouthSide;// * Mathf.Abs(Mathf.Sin(HeadAng.y));
            if(smileParam < 40) return new BlendShapeKey(BlendShapePreset.Joy); 
            if(smileParam > 80) return new BlendShapeKey(BlendShapePreset.Sorrow); 
            
            return new BlendShapeKey(BlendShapePreset.Neutral);

        }
    }
}