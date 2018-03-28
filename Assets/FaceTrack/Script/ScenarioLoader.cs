using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using System.Text;
using UnityEngine;

public class ScenarioLoader : MonoBehaviour {

    private string _scenario = "";

    void Start () {
        ReadFile();
    }

    void Update () {
    }

    void OnGUI()
    {
        GUI.TextArea(new Rect(5, 5, Screen.width, 50), _scenario);
    }

    void ReadFile()
    {
        // scenario.txtファイルを読み込む
        FileInfo fi = new FileInfo(Application.dataPath + "/" + "scenario.txt");
        try
        {
            // 一行毎読み込み
            using (StreamReader sr = new StreamReader(fi.OpenRead(), Encoding.UTF8))
            {
                _scenario = sr.ReadToEnd();
            }
        }
        catch (Exception e)
        {
            _scenario += SetDefaultText();
        }
    }

    string SetDefaultText()
    {
        string txt = Application.dataPath;
        return "シナリオがないから何も話せないよ～\n↓にシナリオを書いてね!\ndataPath:" + txt + "/scenario.txt";
    }
}
