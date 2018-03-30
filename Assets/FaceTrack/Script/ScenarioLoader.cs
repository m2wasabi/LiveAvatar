using System.IO;
using System;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScenarioLoader : MonoBehaviour
{
    [SerializeField]
    public GameObject UIprefab;

    public GameObject[] SelectionArray;
    public int currentSelected = 0;

    public GameObject ContentBox;
    public Text Telop;
    public ToggleGroup ToggleGroup;

    private string _scenario = "";

    void Start () {
        ReadFile();
        CreateTelopToggles();
        var selectionTransforms = ContentBox.GetComponentsInChildren<Toggle>();
        SelectionArray = new GameObject[selectionTransforms.Length];
        for (int i = 0; i < selectionTransforms.Length; i++)
        {
            SelectionArray[i] = selectionTransforms[i].gameObject;
        }

        EventSystem.current.SetSelectedGameObject(SelectionArray[currentSelected], null);
    }

    void Update () {
        //Debug.Log("CurrentSelect " + currentSelected + "Array is " + SelectionArray[currentSelected]);

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            MoveUp();
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            MoveDOwn();
        }
    }

    public void MoveUp()
    {
        currentSelected--;

        if (currentSelected < 0)
        {
            currentSelected = 0;
        }
        EventSystem.current.SetSelectedGameObject(SelectionArray[currentSelected], null);
    }

    public void MoveDOwn()
    {
        currentSelected++;

        if (currentSelected >= SelectionArray.Length - 1)
        {
            currentSelected = SelectionArray.Length - 1;
        }
        EventSystem.current.SetSelectedGameObject(SelectionArray[currentSelected], null);
    }

    public void ToggleAction()
    {
        EventSystem.current.currentSelectedGameObject.GetComponent<Toggle>().isOn =
            !EventSystem.current.currentSelectedGameObject.GetComponent<Toggle>().isOn;
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
