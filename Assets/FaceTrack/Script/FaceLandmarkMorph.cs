using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LiveAvatar
{
    public class FaceLandmarkMorph : MonoBehaviour
    {
        public GameObject BodyAnchor;
        public GameObject HeadAnchor;
        public GameObject TargetModel;

        private Rect _rect;
        private List<Vector2> _landmarkList;
        private bool isActive = false;

        private Vector2Int WebCameraScreenSize = new Vector2Int(320,240);

        private Vector3 BodyPos;
        private Vector3 HeadAng;

        private Vector3[] BodyPosBuffer = new Vector3[5];
        private Vector3[] HeadAngBuffer = new Vector3[5];

        // キャラクター制御パラメーターの調整値
        protected float BodyPosX = 3;
        protected float BodyPosY = 3;
        protected float BodyPosZ = 2;
        protected float BodyPosYOffset;
        protected Vector3 HeadAngleOffset;
        protected float HeadAngX = 70;
        protected float HeadAngY = 90;
        protected float HeadAngZ = 300;
        protected float LipOpenRatio = 200;

        private MouthController mouthController;

        // Use this for initialization
        void Start()
        {
            WebCamManager.Instance.OnFacelandmarkUpdated += fadeDetedtedEvent;
            if (HeadAnchor)
            {
                HeadAngleOffset = HeadAnchor.transform.eulerAngles;
            }
            else
            {
                HeadAngleOffset = Vector3.zero;
            }
            if (BodyAnchor)
            {
                BodyPosYOffset = BodyAnchor.transform.position.y;
            }
            if (TargetModel)
            {
                mouthController = TargetModel.GetComponentInChildren<MouthController>();
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
            if(!isActive) return;
            BodyPos = GetBodyPos(_rect);
            HeadAng = GetHeadAng(_landmarkList);
            UnshiftBuffer(BodyPosBuffer, BodyPos);
            UnshiftBuffer(HeadAngBuffer, HeadAng);

            if (mouthController)
            {
                mouthController.MouthOpen = GetMouthOpen(_landmarkList);
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (isActive)
            {
                var _bodyPos = AvarageBuffer(BodyPosBuffer);
                BodyAnchor.transform.position = _bodyPos;
                //            HeadAnchor.transform.localEulerAngles = new Vector3(HeadAng.x, HeadAng.y, HeadAng.z );
                var _headAng = AvarageBuffer(HeadAngBuffer);
                HeadAnchor.transform.eulerAngles = HeadAngleOffset + new Vector3(_headAng.y, -_headAng.x, _headAng.z);
//                HeadAnchor.transform.eulerAngles = HeadAngleOffset + new Vector3(HeadAng.y, HeadAng.x, HeadAng.z + 10);
            }
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
            zPos = zPos / BodyPosZ;

            // 初期位置のオフセットを適用
            yPos += BodyPosYOffset;

            // 顔の大きさと中心から初期位置分ずらして体位置に利用
            return new Vector3(-xPos, yPos, zPos);
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
            float xAng = (Vector2.Distance(left, center) - Vector2.Distance(right, center)) / Vector2.Distance(left, right) * HeadAngX;
            float yAng = GetAngle(mouth, chin) - HeadAngY;
            float zAng = (Vector2.Distance(mouth, chin) / Vector2.Distance(left, right) - 0.2f) * HeadAngZ;

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
        /// 2点感の角度を求める
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

        float GetMouthOpen(List<Vector2> points)
        {
            Vector2 upperLipTop = points[51];
            Vector2 upperLipBottom = points[62];
            Vector2 lowerLipTop = points[66];
            Vector2 lowerLipBottom = points[57];
            return (lowerLipTop.y - upperLipBottom.y)/(lowerLipBottom.y - upperLipTop.y) * LipOpenRatio;
        }
    }
}