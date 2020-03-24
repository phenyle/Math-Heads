using UnityEngine;
using UnityEngine.UI;

public class TitleWindow : WindowRoot
{
    public Transform panelMain;
    public Transform panelOptions;
    public Slider sliderVolume;

    protected override void InitWindow()
    {
        base.InitWindow();

        SetBgVolume();
    }

    public void ClickPlayBtn()
    {
        audioService.PlayUIAudio(Constants.audioUIStartBtn);
        GameRoot.instance.puzzleSystem.EnterPuzzle(Constants.puzzle01SceneName);
    }

    public void ClickTutorialBtn()
    {
        audioService.PlayUIAudio(Constants.audioUIClickBtn);
        //TODO
    }

    public void ClickOptionsBtn()
    {
        SetActive(panelMain, false);
        SetActive(panelOptions, true);
        audioService.PlayUIAudio(Constants.audioUIClickBtn);
    }

    public void ClickQuitBtn()
    {
        audioService.PlayUIAudio(Constants.audioUIClickBtn);
        Application.Quit();
    }

    public void SetBgVolume()
    {
        audioService.SetBgVolume(sliderVolume.value);
    }

    public void ClickBackBtn()
    {
        SetActive(panelOptions, false);
        SetActive(panelMain, true);
        audioService.PlayUIAudio(Constants.audioUIClickBtn);
    }
}
