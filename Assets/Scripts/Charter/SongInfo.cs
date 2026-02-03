using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SongInfo : MonoBehaviour {

    public static SongInfo instance; 
    private static string songsDirectory;
    [SerializeField] private LoadChart loadChart;
    [SerializeField] private List<ChartCharacterPicker> characterPickers = new List<ChartCharacterPicker>();  

    public string songName;

    public CharacterObject BF;
    public CharacterObject enemy;
    public string stage;

    public SongObject songObject; 
    public float bpm;
    public float arrowSpeed;

    public List<ArrowGrid> grids = new List<ArrowGrid>();

    public List<FullChart> fullCharts = new List<FullChart>();
    public GridManager gridManager; 

    public SongManager songManager; 

    private void Awake()
    {
        instance = this; 

        if (!System.IO.Directory.Exists(System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/Songs/").ToString()))
        {
            songsDirectory = System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/Songs/").ToString();
        }
    }

    //erases everything on all grids
    public void ClearChart()
    {
        foreach(ArrowGrid grid in grids)
        {
            grid.ClearChart(); 
        }
    }

    private SongInfoSave CreateSongInfoObject()
    {
        SongInfoSave save = new SongInfoSave();

        save.stage = stage;
        save.bfName = BF.name;
        save.enemyName = enemy.name;
        save.bpm = bpm;
        save.arrowSpeed = arrowSpeed;

        foreach (ArrowGrid grid in grids)
        {
            string gridJSONInfo = grid.CreateJSON();
            save.gridJSONS.Add(gridJSONInfo);
        }

        return save;
    }

    //called from button
    public void Save()
    {
        StartCoroutine(CreateSongJSON());
    }

    private IEnumerator CreateSongJSON()
    {
        SongInfoSave save = CreateSongInfoObject();
        string json = JsonUtility.ToJson(save);

        //if the file doesn't exist make it exist >:(
        if (!System.IO.File.Exists(Application.persistentDataPath + "/Songs/" + songObject.name + ".json"))
        {
            System.IO.File.Create(Application.persistentDataPath + "/Songs/" + songObject.name + ".json");
            yield return new WaitForSeconds(3);
        }

        System.IO.File.WriteAllText(Application.persistentDataPath + "/Songs/" + songObject.name + ".json", json);
    }

    public IEnumerator LoadSongJSON()
    {
        bool newQuestionMark = false;

        grids.Clear(); 

        //creates a json for the song if it doesn't exist
        if(!System.IO.File.Exists(Application.persistentDataPath + "/Songs/" + songObject.name + ".json"))
        {
            ClearChart();
            loadChart.LoadGridRows(2);
            newQuestionMark = true;

            BF = Cache.characterCache["BoyfriendDotXML"];
            enemy = Cache.characterCache["GF"];
            stage = "Stage";
            bpm = 100;
            arrowSpeed = 10;
            yield return StartCoroutine(CreateSongJSON());
        }

        string jsonFile = System.IO.File.ReadAllText(Application.persistentDataPath + "/Songs/" + songObject.name + ".json");

        SongInfoSave save = JsonUtility.FromJson<SongInfoSave>(jsonFile);


        if (!newQuestionMark)
        {
            //set song values to JSON values
            BF = Cache.characterCache[save.bfName];
            enemy = Cache.characterCache[save.enemyName];

            stage = save.stage;
            bpm = save.bpm;
            arrowSpeed = save.arrowSpeed;

            loadChart.LoadGridRows(save.gridJSONS.Count);

            for (int i = 0; i < save.gridJSONS.Count; i++)
            {
                //loads information INTO each grid
                loadChart.gridObjects[i].LoadJSON(save.gridJSONS[i]);
            }

            grids[0].transform.localPosition = Vector3.zero;
            grids[0].name = "player";
            grids[1].name = "enemy";
        }

        grids[0].name = "player";
        grids[1].name = "enemy";
        characterPickers[0].SetDropDownValue(BF);
        characterPickers[1].SetDropDownValue(enemy);
    }

    public void StartGame()
    {
        DontDestroyOnLoad(gameObject);
        fullCharts = gridManager.fullCharts;
        Destroy(GetComponent<GridManager>());
        SceneManager.LoadScene(stage);
        StartCoroutine(YieldForLoadingScene());
    }
    private void LoadGameplayElements()
    {
        SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetSceneByName(stage));

        songManager = FindObjectOfType<SongManager>();
        songManager.bpm = bpm;
        songManager.music = songObject.music;
        songManager.voices = songObject.voices; 
        songManager.arrowSpeed = arrowSpeed;
        songManager.grids = grids;
        songManager.fullCharts = fullCharts; 

        foreach (GameObject character in GameObject.FindGameObjectsWithTag("Character Position"))
        {
            if (character.name == "BF position")
            {
                songManager.bf = character.GetComponent<RuntimeCharacter>(); 
                character.GetComponent<RuntimeCharacter>().characterObject = BF;
                character.GetComponent<RuntimeCharacter>().SetValues(); 
            }
            if (character.name == "Enemy position")
            {
                songManager.enemy = character.GetComponent<RuntimeCharacter>();
                character.GetComponent<RuntimeCharacter>().characterObject = enemy;
                character.GetComponent<RuntimeCharacter>().SetValues();
            }
        }
    }

    IEnumerator YieldForLoadingScene()
    {
        yield return CheckIfSceneIsLoaded(stage);
        LoadGameplayElements();
    }

    private bool CheckIfSceneIsLoaded(string scene)
    {
        if (SceneManager.GetSceneByName(scene) != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

[System.Serializable]
public class SongInfoSave {
    public string songName;

    public string bfName;
    public string enemyName;
    public string stage;

    public SongObject songObject;
    public float bpm;
    public float arrowSpeed;

    public List<string> gridJSONS = new List<string>();
}