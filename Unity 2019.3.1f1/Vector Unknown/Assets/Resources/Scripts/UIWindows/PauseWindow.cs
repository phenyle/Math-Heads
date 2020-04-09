using UnityEngine;

public class PauseWindow : WindowRoot
{
    public Transform panelPause;
    public Transform panelOption;

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

    public void ClickSettingBtn()
    {
        SetActive(panelPause, false);
        SetActive(panelOption, true);
    }

    public void ClickBackBtn()
    {
        SetActive(panelOption, false);
        SetActive(panelPause, true);
    }

    public void ClickMenuBtn()
    {
        audioService.PlayUIAudio(Constants.audioUIClickBtn);
        GameRoot.instance.InitUI();
        GameRoot.instance.menuSystem.EnterMenu();
        GameRoot.instance.Resume();
    }
}
