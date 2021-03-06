﻿using Assets.Scripts.HarvestResources;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class ResourceManager : CrossSceneSingleton<ResourceManager> {

    [SerializeField] private List<Requrements> Requirements;

    Dictionary<ResourceEnum, int> Resources;
    private string resourceFile;

    private void Awake()
    {
        resourceFile = Application.persistentDataPath + "/resources.dat";
        if (File.Exists(resourceFile))
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (var file = File.Open(resourceFile, FileMode.Open))
            {
                Resources = bf.Deserialize(file) as Dictionary<ResourceEnum, int>;
                file.Close();
            }
        }
        Resources = new Dictionary<ResourceEnum, int>();
    }


    public void AddResource(ResourceEnum resource, int amount)
    {
        Resources.Add(resource, amount);
    }

    public void SaveResources()
    {
        BinaryFormatter bf = new BinaryFormatter();
        using (var file = File.Open(resourceFile, FileMode.OpenOrCreate))
        {
            bf.Serialize(file, Resources);
            file.Close();
        }
    }

    public bool ProduceResource(ResourceEnum resource, int ammountNeeded)
    {
        var reqirements = Requirements.FirstOrDefault(x => x.Resource == resource);
        foreach (var req in reqirements.Requirement)
        {
            if (Resources[req.resource] < req.ammount * ammountNeeded)
            {
                return false;
            }
        }

        reqirements.Requirement.ForEach(x => Resources[x.resource] -= x.ammount * ammountNeeded);
        Resources[resource] += ammountNeeded;
        SaveResources();
        return true;
    }
}
