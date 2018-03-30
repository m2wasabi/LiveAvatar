using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class ScenarioLoader : MonoBehaviour
{
    [SerializeField]
    public GameObject UIprefab;

    public GameObject ContentBox;
    public Text Telop;
    public ToggleGroup ToggleGroup;

    private string _scenario = "";
    private int UIOffset = 0;

    void Start () {
        ReadFile();
        CreateTelopToggles();
    }

    void Update () {
    }

    void OnGUI()
    {
//        GUI.TextArea(new Rect(5, 5, Screen.width, 50), _scenario);
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

    void CreateTelopToggles()
    {
        StringReader rs = new StringReader(_scenario);
        while (rs.Peek() > -1)
        {
            GameObject toggle = Instantiate(UIprefab, ContentBox.transform) as GameObject;
            toggle.GetComponent<Toggle>().group = ToggleGroup;
            toggle.GetComponentInChildren<Text>().text = rs.ReadLine();
            toggle.GetComponent<TelopSwitch>().telopTarget = Telop;
        }
    }
}
