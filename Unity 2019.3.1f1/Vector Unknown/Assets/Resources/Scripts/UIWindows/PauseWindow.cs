using UnityEngine;
using UnityEngine.UI;

public class PauseWindow : WindowRoot
{
    public Transform panelPause;
    public Transform panelOption;
    public Slider sliderVolume;
    public Slider sliderSoundFX;

    protected override void InitWindow()
    {
        base.InitWindow();
        Debug.Log("Init Pause Window");
    }

    public void ClickResumeBtn()
    {
        audioService.PlayUIAudio(Constants.audioUIClickBtn);
        GameRoot.instance.Resume();
        Cursor.visible = false;
    }

    public void ClickSettingBtn()
    {
        audioService.PlayUIAudio(Constants.audioUIClickBtn);

        SetActive(panelPause, false);
        SetActive(panelOption, true);
    }

    public void ClickBackBtn()
    {
        audioService.PlayUIAudio(Constants.audioUIClickBtn);

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

    public void SetBgVolume()
    {
        audioService.SetBgVolume(sliderVolume.value);
    }

    public void SetSoundFXVolume()
    {
        audioService.SetSoundFXVolume(sliderSoundFX.value);
    }
}
