using UnityEngine;
using System.Collections.Generic;

public class GUIMainMenu : MonoBehaviour {

    //MainScene
    public GameObject MainScene;

    //Connect panel
    public GameObject ConnectPanel;
    public GameObject MainMenuConnectButton;
    public GameObject PopupListPlatoon;
    public GameObject PopuListName;

    //Main menu panel
    public GameObject MainMenuPanel;
    public GameObject TrainingModeButton;

    //Settings panel
    public GameObject SettingsPanel;

    public GameObject StudentsPopupListPrefab;

    void Awake()
    {
        UIEventListener.Get(MainMenuConnectButton).onClick += MainMenuConnectButtonClick;
        UIEventListener.Get(TrainingModeButton).onClick += TrainingModeButtonClick;
        GUIMainMenuState.Init();
    }

    void MainMenuConnectButtonClick(GameObject gameObject)
    {
        Debug.Log("MainMenuConnectButton clicked");
        MainMenuPanel.active = true;
        ConnectPanel.active = false;
    }

    void TrainingModeButtonClick(GameObject gameObject)
    {
        Debug.Log("TrainingModeButtonClick clicked");
        MainMenuPanel.active = false;
        MainScene.active = true;
    }
}

public static class GUIMainMenuState
{
    static List<string> platoons;
    static Dictionary<string, List<string>> students;
    static string currentPlatoon = "123";

    public static List<string> Platoons
    {
        get
        {
            return platoons;
        }
        set
        {
            platoons = value;
        }
    }
    public static Dictionary<string, List<string>> Students
    {
        get
        { 
            return students;
        }
        set
        { 
           students = value;
        }
    }

    public static string CurrentPlatoon
    {
        get
        {
            return currentPlatoon;
        }
        set
        {
            currentPlatoon = value;
        }
    }

    public static void Init()
    {
        Platoons = new List<string>()
        {
            "123",
            "124"
        };
        Students = new Dictionary<string, List<string>>();
        Students.Add("123", new List<string>(new string[] { "Летягин", "Прибауткин" }));
        Students.Add("124", new List<string>(new string[] { "Васин", "Алёшин" }));
    }
}
