using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class MenuBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (!Directory.Exists(Application.persistentDataPath + "/Characters/"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/Characters/");
        }
        if (!Directory.Exists(Application.persistentDataPath + "/Images/Characters/"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/Images/Characters");
        }
        if (!Directory.Exists(Application.persistentDataPath + "/Songs/"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/Songs/");
        }

        print(Application.persistentDataPath);
        
        if (!Cache.loaded)
        {
            Cache.LoadAll();
        }

        Cache.loaded = true; 
    }

    public void LoadCreator()
    {
        SceneManager.LoadScene(2);
    }
    public void LoadEditor()
    {
        SceneManager.LoadScene(3);
    }
    public void LoadChart()
    {
        SceneManager.LoadScene(4);
    }
}
