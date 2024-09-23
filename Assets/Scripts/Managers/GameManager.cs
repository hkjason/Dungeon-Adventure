using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameState gameState;
    public SaveData saveData;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.Log("two singleton failure");
            Destroy(gameObject);
        }

        if (System.IO.File.Exists(Application.persistentDataPath + "/savedata.txt"))
        {
            LoadData();
        }
        else
        {
            saveData.Initialize();
            SaveData();
        }
    }

    private void Start()
    {
        LoadData();
        if(saveData.lastPlayedSlot == 99)
        {
            StartUIManager.instance.continueButtonUI.interactable = false;
        }
    }

    public void SaveData()
    {
        SaveManager.WriteToBinaryFile(Application.persistentDataPath + "/savedata.txt", saveData);
    }
    public void LoadData()
    {
        saveData = SaveManager.ReadFromBinaryFile<SaveData>(Application.persistentDataPath + "/savedata.txt");
    }

    public void Continue()
    {
        LoadData();
        saveData.loadType = LoadType.LoadGame;
        SaveData();
        LoadGame(saveData.lastPlayedSlot);
    }
    public void LoadGame(int i)
    {
        StartUIManager.instance.startCanvas.SetActive(false);

        LoadData();
        saveData.lastPlayedSlot = i;
        SceneController.instance.SceneChange(1);
        SaveData();
    }

    public void LoadNewGame(int idx)
    {
        LoadData();
        saveData.lastPlayedSlot = idx;
        saveData.Desave();
        LoadGame(idx);
    }
}
public enum GameState
{
    MainMenu,
    Loading,
    Pause,
    InGame,
    LevelFailed,
    LevelPassed
}

public enum LoadType
{
    NewGame,
    LoadGame
}