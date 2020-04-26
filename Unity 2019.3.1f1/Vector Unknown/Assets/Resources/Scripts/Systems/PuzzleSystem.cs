using UnityEngine;

public class PuzzleSystem : SystemRoot
{
    [Header("Title Window")]
    public TitleWindow titleWindow;

    [Header("Main Window")]
    public MainWindow mainWindow;

    [Header("Puzzle Windows")]
    public Puzzle01Window puzzle01Window;
    public Puzzle02Window puzzle02Window;
    public Puzzle03Window puzzle03Window;

    [Header("Credits Windows")]
    public CreditsWindow creditsWindow;

    [Header("Tutorial Windows")]
    public TutorialWindow tutorialWindow;

    public override void InitSystem()
    {
        Debug.Log("Init Puzzle System");
        base.InitSystem();
    }

    public void EnterPuzzle(string puzzleName)
    {
        resourceService.AsynLoadScene(puzzleName, () =>
        {
            CloseAllWindow();
            GameRoot.ShowTips("", false, false);
            switch (puzzleName)
            {
                case Constants.menuSceneName:
                    try
                    {
                        DialogueManager.instance.ResetAll();
                    }
                    catch { }

                    titleWindow.SetWindowState(true);
                    audioService.PlayBgMusic(Constants.audioBgMenu, true);
                    break;

                case Constants.mainSceneName:
                    mainWindow.SetWindowState(true);
                    audioService.PlayBgMusic(Constants.audioBgMain, true);
                    break;

                case Constants.puzzle01SceneName:
                    puzzle01Window.SetWindowState(true);
                    audioService.PlayBgMusic(Constants.audioBgPuzzle1, true);
                    break;

                case Constants.puzzle02SceneName:
                    puzzle02Window.SetWindowState(true);
                    audioService.PlayBgMusic(Constants.audioBgPuzzle2, true);
                    break;

                case Constants.puzzle03SceneName:
                    puzzle03Window.SetWindowState(true);
                    audioService.PlayBgMusic(Constants.audioBgPuzzle3, true);
                    break;

                case Constants.creditSceneName:
                    creditsWindow.SetWindowState(true);
                    audioService.PlayBgMusic(Constants.audioBgCredit, true);
                    break;

                case Constants.tutorialSceneName:
                    tutorialWindow.SetWindowState(true);
                    audioService.PlayBgMusic(Constants.audioBgTutorial, true);
                    break;
            }            
        });
    }
}
