using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossSceneSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance = null;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>();
            }
            else if (instance != FindObjectOfType<T>())
            {
                Destroy(instance);
            }

            DontDestroyOnLoad(FindObjectOfType<T>());
            return instance;
        }
    }
}
