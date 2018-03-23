using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;
using UnityEngine;

public class LevelManager : MonoBehaviour {

    [SerializeField] private List<LevelInfo> levels;
    [SerializeField] private RectTransform LevelContainer;
    [SerializeField] private LevelButton levelButtonPrefab;
    
    private string levelInfoFile;

    public static LevelManager Instance;

    private void Awake()
    {
        levelInfoFile = Application.persistentDataPath + "/levelInfo.dat";

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        using (var file = File.Open(levelInfoFile, FileMode.OpenOrCreate))
        {
            bf.Serialize(file, levels);
            file.Close();
        }
    }

    public void Load()
    {
        if (File.Exists(levelInfoFile))
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (var file = File.Open(levelInfoFile, FileMode.Open))
            {
                levels = bf.Deserialize(file) as List<LevelInfo>;
                file.Close();
            }
        }
    }

    public void UnlockNext()
    {
        levels.First(x => x.Unlocked == false).Unlocked = true;
        Save();
    }

    public List<LevelInfo> GetLevels()
    {
        return levels;
    }

    public void UpdateLevels()
    {
        var buttons = LevelContainer.GetComponentsInChildren<LevelButton>();
        foreach (var button in buttons)
        {
            Destroy(button.gameObject);
        }

        foreach (var level in levels)
        {
            var newButton = Instantiate(levelButtonPrefab, LevelContainer);
            newButton.ConfigureButton(level);
        }
    }

    // Use this for initialization
    void Start () {
		if (File.Exists(levelInfoFile))
        {
            Load();
        }

        UpdateLevels();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
