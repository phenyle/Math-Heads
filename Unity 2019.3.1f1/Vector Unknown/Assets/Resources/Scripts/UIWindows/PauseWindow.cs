using UnityEngine;
using UnityEngine.UI;

public class PauseWindow : WindowRoot
{
    public Transform panelPause;
    public Transform panelOption;
    public Transform puzzleControls;
    public Slider sliderVolume;
    public Slider sliderSoundFX;
    public Text controlsText;
    public Text puzzleText;

    protected override void InitWindow()
    {
        base.InitWindow();
        Debug.Log("Init Pause Window");
        sliderVolume.value = audioService.bgVolume;
        sliderSoundFX.value = audioService.UIFXVolume;
        defaultBasicControlsText();
    }

    public void ClickResumeBtn()
    {
        audioService.PlayUIAudio(Constants.audioUIClickBtn);
        GameRoot.instance.Resume();
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
        GameObject.Find("DialogueManager").GetComponent<DialogueManager>().EndDialogue();
        //Resume can unlock the lock
        GameRoot.isPuzzleLock = false;

        //Dialogue manager can unlock the lock;
        DialogueManager.isPuzzleLock = false;
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

    public void defaultBasicControlsText()
    {
        controlsText.text = "Move:\n" +
                            "W - Move Forward\n" +
                            "A - Move Left\n" +
                            "S - Move Backward\n" +
                            "D - Move Right\n" +
                            "Left Shift - Run\n" +
                            "\n" +
                            "I - Rotate Camera Up\n" +
                            "J - Rotate Camera Left\n" +
                            "K - Rotate Camera Down\n" +
                            "L - Rotate Camera Right\n" +
                            "\n" +
                            "Mouse - Move Camera\n" +
                            "Z - Swich To Top-Down View\n" +
                            "Tab - Show / Hide Mouse\n" +
                            "Space - Advance Dialogue\n" +
                            "Escape - Pause Game";
    }

    public void setBasicControlsText(string newControls)
    {
        controlsText.text = newControls;
    }

    public void showPuzzleControls(bool val)
    {
        SetActive(puzzleControls, val);
    }

    public void setPuzzleControlsText(string newControls)
    {
        puzzleText.text = newControls;
    }

    /// <summary>
    /// Resizes the Puzzle Controls UI window Element
    /// For Reference the default size of the Puzzle Controls UI Window is
    /// width: 500
    /// height: 600
    /// the UI element is also automatically moved to offset the size changes
    /// </summary>
    /// <param name="width">new input width size</param>
    /// <param name="height">new input height size</param>
    public void resizePuzzleControls(float width, float height)
    {
        float xDelta = width / puzzleControls.GetComponent<RectTransform>().rect.width;
        float yDelta = height / puzzleControls.GetComponent<RectTransform>().rect.height;
        float xDifference = width - puzzleControls.GetComponent<RectTransform>().rect.width;

        float yOffset = puzzleControls.Find("Title").GetComponent<RectTransform>().rect.y * (yDelta - 1);

        puzzleControls.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
        puzzleControls.localPosition += new Vector3(xDifference / 2, 0, 0);
        puzzleControls.Find("Text").GetComponent<RectTransform>().sizeDelta *= new Vector2(xDelta, yDelta);
        puzzleControls.Find("Title").transform.localPosition += new Vector3(0, yOffset, 0);
    }
}
