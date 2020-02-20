using UnityEngine;

public class PuzzleSystem : SystemRoot
{
    [Header("Puzzle Windows")]
    public Puzzle01Window puzzle01Window;
    public Puzzle02Window puzzle02Window;
    public Puzzle03Window puzzle03Window;

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
            switch (puzzleName)
            {
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
            }            
        });
    }
}
