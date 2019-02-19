using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LiveAvatar.UI;
using UnityEngine;
using UniRx;
using UnityEngine.UI;

namespace LiveAvatar.Speech
{
    public class SeikaCenterManager : MonoBehaviour
    {
        private string _baseUrl = "http://local:password@localhost:7180/";
        private int _castId = 2000;
        private List<Cast> _casts = new List<Cast>();
        private List<SettingRule> _settings = new List<SettingRule>();

        [SerializeField]
        private InputField BaseUrl;
        [SerializeField]
        private Dropdown _castSelector;

        [SerializeField]
        private GameObject Sliders;
        [SerializeField]
        private GameObject SliderPrefab;

        private List<UI_SliderField> _sliderFields;

        [Serializable]
        public class Cast
        {
            public int cid;
            public string name;
        }
        public enum SeikaSettingCategory
        {
            effect,
            emotion
        }
        public enum deptRule
        {
            value,
            max,
            min,
            step
        }
        public struct SettingRule
        {
            public SeikaSettingCategory category;
            public string key;
            public float max;
            public float min;
            public float step;
            public float value;
        }

        [Serializable]
        public class SeikaPramJson
        {
            public string parameter;
            public float value;
        }

        [Serializable]
        public class SeikaResponseJson
        {
            public SeikaPramJson[] data;
        }

        [Serializable]
        public class CastsJson
        {
            public Cast[] data;
        }

        [Serializable]
        public class SpeechJson
        {
            public string talktext;
            public float speed;
            public float volume;
            public float pitch;
            public float intonation;
            public Emotion[] emotions;
        }

        [Serializable]
        public class Emotion
        {
            public string Key;
            public float Value;
        }

        // Talk送信するときのヘッダ
        private Hashtable HttpHeaders {
            get { 
                var headers = new Hashtable ();
                headers ["Content-Type"] = "application/json";
                return headers;
            }
        }

        private List<SettingRule> readRule(string jsonString)
        {
            if (jsonString[0] == '[')
            {
                jsonString = "{\"data\":" + jsonString + "}";
            }
            var jsonParams = JsonUtility.FromJson<SeikaResponseJson>(jsonString);

            var settings = new Dictionary<string,SettingRule>();
            foreach (var param in jsonParams.data)
            {
                deptRule dept;
                var _pd = param.parameter.Split('_');
                var category = (SeikaSettingCategory)Enum.Parse(typeof(SeikaSettingCategory) , _pd[0]);
                var key = _pd[1];
                if (_pd.Length == 3)
                {
                    dept = deptRule.value;
                }
                else
                {
                    dept = (deptRule)Enum.Parse(typeof(deptRule) ,_pd[3]);
                }
                SettingRule _s;
                if (settings.ContainsKey(key))
                {
                    _s = settings[key];
                    settings.Remove(key);
                }
                else
                {
                    _s = new SettingRule();
                    _s.key = key;
                    _s.category = category;
                }
                switch (dept)
                {
                    case deptRule.value:
                        _s.value =  param.value;
                        break;
                    case deptRule.max:
                        _s.max =  param.value;
                        break;
                    case deptRule.min:
                        _s.min =  param.value;
                        break;
                    case deptRule.step:
                        _s.step =  param.value;
                        break;
                }
                settings.Add(key,_s);
            }
            return settings.Values.ToList();
        }

        private List<Cast> readCast(string jsonString)
        {
            if (jsonString[0] == '[')
            {
                jsonString = "{\"data\":" + jsonString + "}";
            }
            var jsonCasts = JsonUtility.FromJson<CastsJson>(jsonString);
            return jsonCasts.data.ToList();
        }

        private void SetCastList()
        {
            // Castリストをドロップダウンに更新する
            _castSelector.ClearOptions();
            _castSelector.AddOptions(_casts.Select(a => a.name).ToList());
        }
        private void SetSettingList()
        {
            // 音声パラメータを更新する
            if(_settings == null || _settings.Count == 0) return;

            foreach (Transform t in Sliders.transform)
            {
                Destroy(t.gameObject);
            }

            for (int i = 0; i < _settings.Count; i++)
            {
                var slider = Instantiate(SliderPrefab, Sliders.transform) as GameObject;
                var sliderComponent = slider.GetComponent<UI_SliderField>();
                var wholeNumbers = !(Math.Abs(_settings[i].step - Mathf.Floor(_settings[i].step)) > 0);
                sliderComponent.Initialize(_settings[i].key,_settings[i].min,_settings[i].max,_settings[i].value, wholeNumbers);
            }
            _sliderFields = Sliders.GetComponentsInChildren<UI_SliderField>().ToList();
        }

        public void OnhangeCast(Dropdown change)
        {
            if (_casts.Count > 0)
            {
                _castId = _casts[change.value].cid;
                GetSettingList(_castId);
            }
        }

        public void GetCastList()
        {
            ObservableWWW.Get(_baseUrl + "AVATOR").Subscribe(
                resultText =>
                {
                    _casts = readCast(resultText);
                    SetCastList();
                    OnhangeCast(_castSelector);
                },
                error => {Debug.LogError("GetCast www Error:" + error);}
            );
        }

        private void GetSettingList(int cid)
        {
            // 音声パラメータを取得・更新する
            ObservableWWW.Get(_baseUrl + "AVATOR/" + cid.ToString() + "/current").Subscribe(
                resultText =>
                {
                    _settings = readRule(resultText);
                    SetSettingList();
                },
                error => {Debug.LogError("GetSetting www Error:" + error);}
            );
        }

        private void Awake()
        {
            _castSelector.onValueChanged.AddListener(_ => { OnhangeCast(_castSelector); });
            BaseUrl.onValueChanged.AddListener(OnChangeBaseUrl);
        }


        private string makeSpeechData(string text)
        {
            var data = new SpeechJson();
            data.talktext = text;
            var emotions = new List<Emotion>();
            for (int i = 0; i < _settings.Count; i++)
            {
                if (_settings[i].category == SeikaSettingCategory.effect)
                {
                    switch (_settings[i].key)
                    {
                        case "volume":
                            data.volume = _sliderFields[i].GetValue();
                            break;
                        case "speed":
                            data.speed = _sliderFields[i].GetValue();
                            break;
                        case "pitch":
                            data.pitch = _sliderFields[i].GetValue();
                            break;
                        case "intonation":
                            data.intonation = _sliderFields[i].GetValue();
                            break;
                    }
                }
                else if (_settings[i].category == SeikaSettingCategory.emotion)
                {
                    emotions.Add(new Emotion(){Key = _settings[i].key, Value = _sliderFields[i].GetValue()});
                }

                if (emotions.Count > 0)
                {
                    data.emotions = emotions.ToArray();
                }
            }
            return JsonUtility.ToJson(data);
        }
        public void Speech(string text)
        {
            var jsonParams = makeSpeechData(text);
            byte[] postData = System.Text.Encoding.UTF8.GetBytes (jsonParams);
            ObservableWWW.Post(_baseUrl + "PLAY/" + _castId.ToString(), postData).Subscribe(_ => { });
        }
        private void OnChangeBaseUrl(string url)
        {
            _baseUrl = url;
        }

    }
}