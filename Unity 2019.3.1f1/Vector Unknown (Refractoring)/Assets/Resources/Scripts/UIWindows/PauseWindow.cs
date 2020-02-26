using UnityEngine;

public class PauseWindow : WindowRoot
{
    protected override void InitWindow()
    {
        base.InitWindow();
        Debug.Log("Init Pause Window");
    }

    public void ClickResumeBtn()
    {
        audioService.PlayUIAudio(Constants.audioUIClickBtn);
        GameRoot.instance.Resume();
    }

    public void ClickMenuBtn()
    {
        audioService.PlayUIAudio(Constants.audioUIClickBtn);
        GameRoot.instance.InitUI();
        GameRoot.instance.menuSystem.EnterMenu();
        GameRoot.instance.Resume();
    }
}
