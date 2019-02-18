using System;
using System.Collections.Generic;
using System.Linq;
using LiveAvatar.UI;
using UnityEngine;
using UniRx;

namespace LiveAvatar.Speech
{
    public class SeikaCenterManager : MonoBehaviour
    {
        private string _baseUrl = "http://local:password@localhost:7180/";
        private string _castId = "2000";
        private List<Cast> _casts = new List<Cast>();
        private List<SettingRule> _settings = new List<SettingRule>(); 

        private List<UI_SliderField> _sliderFields;

        // Todo: キャラ一覧のドロップダウンUI
        // Todo: キャラ一覧のドロップダウン変更時パラメータ処理
        // ToDo: トークパラメータの生成・差し込み
        
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

        public void GetCastList()
        {
            ObservableWWW.Get(_baseUrl + "AVATOR").Subscribe(
                resultText => {_casts = readCast(resultText);},
                error => {Debug.LogError("www Error:" + error);}
            );
        }

        public void GetSettingList(int cid)
        {
            ObservableWWW.Get(_baseUrl + "AVATOR/" + cid.ToString() ).Subscribe(
                resultText => {_settings = readRule(resultText);},
                error => {Debug.LogError("www Error:" + error);}
            );
        }

    }
}