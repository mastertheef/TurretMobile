﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;
using UnityEngine;

public class LevelManager : MonoBehaviour {

    [SerializeField] private List<LevelInfo> levels;
    public List<LevelInfo> Levels { get { return levels; } }
    private readonly string levelInfoFile = Application.persistentDataPath + "/levelInfo.dat";

    public static LevelManager Instance;

    private void Awake()
    {
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

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
