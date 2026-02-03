using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GridManager : MonoBehaviour
{
    public SongInfo songInfo;
    public float scrollScale;
    public GameObject chartGameObject;
    public static GridManager instance; 

    public int currentSectionNum;
    public List<ChartSection> currentSections = new List<ChartSection>();
    public List<RuntimeChartSection> runtimeSections = new List<RuntimeChartSection>(); 
    public List<FullChart> fullCharts = new List<FullChart>();

    [SerializeField] private GameObject gridPrefab;

    public AudioSource hitNoise; 
    public AudioSource voices;
    public AudioSource music;
    public float songPos;
    public float timer;
    

    public bool moving = false;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        GameObject grid = Instantiate(gridPrefab);
        grid.transform.parent = chartGameObject.transform;
        grid.transform.localPosition = Vector2.zero;

        runtimeSections.Add(grid.GetComponent<RuntimeChartSection>());

        grid = Instantiate(gridPrefab);
        grid.transform.parent = chartGameObject.transform;
        grid.transform.localPosition = new Vector2(6, 0);

        runtimeSections.Add(grid.GetComponent<RuntimeChartSection>());

        fullCharts.Add(new FullChart());
        fullCharts.Add(new FullChart());

        currentSectionNum = 0; 
        songInfo = GetComponent<SongInfo>();

        //decides how many sections there needs to be
        int sectionCount = Mathf.RoundToInt(CustomMath.RoundToNearestFactorOf(CustomMath.WorldSpaceToSongPos(SongInfo.instance.songObject.timeSigTop * 
            SongInfo.instance.songObject.timeSigBottom, 
            SongInfo.instance.songObject.timeSigTop),
            SongInfo.instance.songObject.music.length));

        int currentChart = 0; 
        foreach(FullChart fullChart in fullCharts)
        {
            currentSectionNum = 0;

            for (int i = 0; i < sectionCount; i++)
            {
                ChartSection section = new ChartSection();
                section.section = i;
                fullChart.sections.Add(section);
            }
            foreach (ChartSection chart in fullChart.sections)
            {
                chart.columns.Add(new ArrowArray());
                chart.columns.Add(new ArrowArray());
                chart.columns.Add(new ArrowArray());
                chart.columns.Add(new ArrowArray());
            }

            currentSections.Add(fullChart.sections[0]);
            runtimeSections[currentChart].chartClass = currentSections[currentChart];

            currentChart++; 
        }
    }

    public void Update()
    {
        if (chartGameObject != null && !moving)
        {
            chartGameObject.transform.position = new Vector2(chartGameObject.transform.position.x, chartGameObject.transform.position.y + (Input.mouseScrollDelta.y * -scrollScale));
        }    

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!moving)
            {
                moving = true;
                StartCoroutine(MoveWithMusic());
            }
            else
            {
                voices.Stop();
                music.Stop(); 
                moving = false;
                chartGameObject.transform.position = new Vector3(chartGameObject.transform.position.x,
                    CustomMath.RoundToNearestFactorOf(.5f, chartGameObject.transform.position.y), 0);
            }
        }

        //switches out chart sections while scrolling
        if (chartGameObject.transform.position.y > 16)
        {
            int chartLoop = 0; 
            chartGameObject.transform.position = new Vector2(-12, 0);
            currentSectionNum++;

            foreach (RuntimeChartSection chartSection in runtimeSections)
            {
                chartSection.ClearSection();
                currentSections[chartLoop] = fullCharts[chartLoop].sections[currentSectionNum];
                chartSection.NewSection(fullCharts[chartLoop].sections[currentSectionNum]);
                chartLoop++;
            }
        }
        if (chartGameObject.transform.position.y < 0)
        {
            int chartLoop = 0;
            chartGameObject.transform.position = new Vector2(-12, 16);
            currentSectionNum--;

            foreach (RuntimeChartSection chartSection in runtimeSections)
            {
                chartSection.ClearSection();
                currentSections[chartLoop] = fullCharts[chartLoop].sections[currentSectionNum];
                chartSection.NewSection(fullCharts[chartLoop].sections[currentSectionNum]);
                chartLoop++;
            }
        }

        songPos = CustomMath.WorldSpaceToSongPos(chartGameObject.transform.position.y, currentSections[0].timeSigTop) +
            CustomMath.WorldSpaceToSongPos(currentSections[0].timeSigTop * currentSections[0].timeSigBottom * currentSectionNum, currentSections[0].timeSigTop);
    }

    private IEnumerator MoveWithMusic()
    {
        voices.time = songPos;
        music.time = songPos; 

        voices.Play(); 
        music.Play(); 

        //move 4 units per beat
        while (moving)
        {
            yield return null;
            chartGameObject.transform.Translate((Vector3.up * (4 * (songInfo.bpm / 60))) * Time.deltaTime);
        }
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
