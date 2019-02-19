using IST.RemoteTalk;
using UnityEngine;
using UnityEngine.UI;

namespace LiveAvatar.UI
{
    public class UI_SliderField : MonoBehaviour
    {

        public Text m_label;
        public Text m_val;
        public Slider m_slider;

        public void Initialize(string name, float min, float max, float val, bool wholeNumbers = false)
        {
            m_label.text = name;
            m_val.text = val.ToString();
            m_slider.maxValue = max;
            m_slider.minValue = min;
            m_slider.value = val;
            m_slider.wholeNumbers = wholeNumbers;
        }
        void Start () {
            m_slider.onValueChanged.AddListener(v => { m_val.text = v.ToString(); }); 
        }

        public TalkParam GetTalkParam()
        {
            var talkParam = new TalkParam();
            talkParam.name = m_label.text;
            talkParam.value = m_slider.value;
            talkParam.rangeMax = m_slider.maxValue;
            talkParam.rangeMin = m_slider.minValue;
            return talkParam;
        }

        public float GetValue()
        {
            return m_slider.value;
        }
    }
}
