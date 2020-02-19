using UnityEngine;

public class Puzzle01Window : WindowRoot
{
    public Transform panelStart;

    private GameControllerPuzzle01 GCP01;

    protected override void InitWindow()
    {
        Debug.Log("Init Puzzle01 window");
        base.InitWindow();

        GCP01 = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControllerPuzzle01>();
        GCP01.InitGameController();
    }

    public void ClickSubmit()
    {
        audioService.PlayUIAudio(Constants.audioUIClickBtn);
        SetActive(panelStart, false);
    }
}
