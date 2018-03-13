using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LiveAvatar
{
    public class FacelandmarkResultEventArgs : EventArgs
    {
        private readonly Rect _rect;
        private readonly List<Vector2> _landmarkList;
        public Rect Rect { get { return _rect; } }
        public List<Vector2> LandmarkList { get { return _landmarkList; } }

        public FacelandmarkResultEventArgs(Rect rect, List<Vector2> landmarkList)
        {
            _rect = rect;
            _landmarkList = landmarkList;
        }
    }
}
