using UnityEngine;

public class GameRoot : MonoBehaviour
{
    public static GameRoot instance = null;

    [Header("Common API")]
    public LoadingWindow loadingWindow;
    public DynamicWindow dynamicWindow;

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
    }

    private void Start()
    {
        Debug.Log("Game Start...");
        InitUI();
        InitGameRoot();
    }

    private void InitUI()
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
        menuSystem.EnterMenu();
    }

    public static void AddTips(string tips)
    {
        instance.dynamicWindow.AddTips(tips);
    }
}
