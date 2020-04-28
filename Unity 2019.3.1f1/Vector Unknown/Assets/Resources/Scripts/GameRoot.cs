using UnityEngine;
using UnityEngine.SceneManagement;

public class GameRoot : MonoBehaviour
{
    public static GameRoot instance = null;
    public static bool isPause = false;
    public static bool isPuzzleLock = false;
    
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

    [HideInInspector]
    public int score = 0;
    //[HideInInspector]
    public bool[] puzzleCompleted = { false, false, false };
    public int exitPuzzle = 0;


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

        if (Input.GetKeyDown(KeyCode.R))
        {

        }
    }

    public void InitUI()
    {
        Transform canvas = transform.Find("Canvas");
        Transform uiWindows = canvas.Find("UIWindows");

        for (int i = 0; i < uiWindows.childCount; i++)
        {
            uiWindows.GetChild(i).gameObject.SetActive(false);
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

    public static void ShowTips(string tips, bool state, bool isReactive)
    {
        instance.dynamicWindow.ShowTips(tips, state, isReactive);
    }

    public void Pause()
    {
        IsLock(true);
        Time.timeScale = 0;
        pauseWindow.SetWindowState(true);
        isPause = true;
        audioService.PauseAllAudios();

        Debug.Log("Lock");
    }

    public void Resume()
    {
        //Debug.Log(DialogueManager.isInDialogue);
        //Debug.Log(isPuzzleLock);
        if (!DialogueManager.isInDialogue && !isPuzzleLock)
        {
            IsLock(false);

            Debug.Log("Unlock");
        }

        Time.timeScale = 1;
        pauseWindow.SetWindowState(false);
        isPause = false;
        audioService.ResumeAllAudios();
    }

    public void IsLock(bool state)
    {
        try
        {
            Camera.main.GetComponent<CameraController>().isLock = state;
            GameObject.FindGameObjectWithTag("Player").GetComponent<UnityStandardAssets.Characters.ThirdPerson.ThirdPersonUserControl>().isLock = state;
        }
        catch { }
    }

    public static void AddScore(int score)
    {
        instance.score += score;
    }
}
