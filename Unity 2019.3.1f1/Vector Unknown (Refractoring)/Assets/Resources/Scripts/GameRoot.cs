using UnityEngine;
using UnityEngine.SceneManagement;

public class GameRoot : MonoBehaviour
{
    public static GameRoot instance = null;
    public static bool isPause = false;

    [Header("Common API")]
    public LoadingWindow loadingWindow;
    public DynamicWindow dynamicWindow;
    public PauseWindow pauseWindow;

    //Services
    [HideInInspector]
    public ResourceService resourceService;
    [HideInInspector]
    public AudioService audioService;

    //Systems
    [HideInInspector]
    public MenuSystem menuSystem;
    [HideInInspector]
    public PuzzleSystem puzzleSystem;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        instance = this;
        
        if(SceneManager.GetActiveScene().name == Constants.gameRootSceneName)
        {
            InitUI();
        }
        InitGameRoot();
    }

    private void Start()
    {
        Debug.Log("Game Start...");
        //InitUI();
        //InitGameRoot();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(!isPause 
                && SceneManager.GetActiveScene().name != Constants.gameRootSceneName
                && SceneManager.GetActiveScene().name != Constants.menuSceneName)
            {
                Pause();
            }
            else
            {
                Resume();
            }
        }
    }

    public void InitUI()
    {
        Transform canvas = transform.Find("Canvas");

        for (int i = 0; i < canvas.childCount; i++)
        {
            canvas.GetChild(i).gameObject.SetActive(false);
        }

        dynamicWindow.SetWindowState(true);
    }

    private void InitGameRoot()
    {
        //1. Init all services
        //a. Resource service
        resourceService = GetComponent<ResourceService>();
        resourceService.InitService();

        //b. Audio service
        audioService = GetComponent<AudioService>();
        audioService.InitService();

        //2. Init all systems
        //a. Menu System
        menuSystem = GetComponent<MenuSystem>();
        menuSystem.InitSystem();

        //b. Puzzle System
        puzzleSystem = GetComponent<PuzzleSystem>();
        puzzleSystem.InitSystem();

        //3.Enter Menu Scene
        if (SceneManager.GetActiveScene().name == Constants.gameRootSceneName)
        {
            menuSystem.EnterMenu();
        }
    }

    public static void AddTips(string tips)
    {
        instance.dynamicWindow.AddTips(tips);
    }

    public void Pause()
    {
        Time.timeScale = 0;
        pauseWindow.SetWindowState(true);
        isPause = true;
        audioService.PauseAllAudios();
    }

    public void Resume()
    {
        Time.timeScale = 1;
        pauseWindow.SetWindowState(false);
        isPause = false;
        audioService.ResumeAllAudios();
    }
}
