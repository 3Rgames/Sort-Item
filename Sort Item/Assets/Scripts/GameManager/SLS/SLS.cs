using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
[CustomEditor(typeof(SLS))]
public class SLSEditor: Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        SLS myScript = (SLS)target;
        if(GUILayout.Button("Clear PlayerPrefs"))
        {
            myScript.ClearPlayerPrefs();
        }
    }
}
#endif

public class SLS : MonoBehaviour
{
    private Save SV = new Save();

    #region Singleton

    public static SLS Instance;

    void Awake()
    {
        if (Instance == null)
            Instance = this; 
    }

    #endregion

    private void Start()
    {
        UploadData();
    }

    private void OnApplicationPause(bool pause)
    {
        if(pause)
            SaveData();
    }

    public void Save()
    {
        SaveData();
    }

    private void OnApplicationQuit()
    {
        SaveData();
    }

    public void ClearPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }

    private void SaveData()
    {
        SV.AudioEnabled = AudioController.Instance.AudioEnabled;
        SV.TapTicEnabled = TapTicController.Instance.TapTicEnabled;
        SV.CountOfLoops = LevelController.Instance.CountOfLoops;
        SV.Money = UIController.Instance.Money;
        PlayerPrefs.SetString("SV", JsonUtility.ToJson(SV));
    }

    private void UploadData()
    {
        if (PlayerPrefs.HasKey("SV"))
        {
            SV = JsonUtility.FromJson<Save>(PlayerPrefs.GetString("SV"));
            UIController.Instance.Money = SV.Money;
            LevelController.Instance.CountOfLoops = SV.CountOfLoops;
            TapTicController.Instance.TapTicEnabled = SV.TapTicEnabled;
            AudioController.Instance.AudioEnabled = SV.AudioEnabled;
        }
    }
}

[SerializeField]
public class Save
{
    public bool AudioEnabled;
    public bool TapTicEnabled;
    public int CountOfLoops;
    public int Money;
}

// To save other information you need:
// 1. add this information into the class "Save"
// 2. add this information into SaveData()
// 3. add this information into UploadData()
// SLS.Instance.Save(); - to sa
// Yours ever 3R