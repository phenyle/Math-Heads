using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectButtons : MonoBehaviour
{
    [Header("Title Cards")]
    public GameObject stageTitle;
    public GameObject stageDescrip;

    public SceneNameMod sceneName;
    public PuzzleCompleteMod puzzleComplete;

    private int level = 0;
    private bool submit = false;
    private string stageName;

    public int getLevel()
    {
        return level;
    }

    public void setLevel(int val)
    {
        level = val;
    }

    public void setSceneName(SceneNameMod val)
    {
        sceneName = val;
    }

    public void setPuzzleCompleteName(PuzzleCompleteMod val)
    {
        puzzleComplete = val;
    }

    public void submitButton()
    {
        


        //while the player is in the portal, if they select the "GO" button
        //Find which Puzzle/Scene this portal applies to
        switch (sceneName)
        {
            case SceneNameMod.Puzzle01SceneName:
                //Find which level the player selected and goto that puzzle
                //at that level
                switch (level)
                {
                    case 0:
                        GameRoot.ShowTips("Please select a level", true, false);
                        break;
                    case 1:
                        GameRoot.instance.puzzleSystem.EnterPuzzle(Constants.puzzle01SceneName);
                        break;
                    case 2:
                        GameRoot.ShowTips("That level doesn't exist right now\nCheck back later", true, false);
                        break;
                    case 3:
                        GameRoot.ShowTips("That level doesn't exist right now\nCheck back later", true, false);
                        break;
                    default:
                        GameRoot.ShowTips("Error: what?? how?", true, false);
                        break;

                }
                break;

            case SceneNameMod.Puzzle02SceneName:
                //Find which level the player selected and goto that puzzle
                //at that level
                switch (level)
                {
                    case 0:
                        GameRoot.ShowTips("Please select a level", true, false);
                        break;
                    case 1:
                        GameRoot.instance.puzzleSystem.EnterPuzzle(Constants.puzzle02SceneName);
                        break;
                    case 2:
                        GameRoot.instance.puzzleSystem.EnterPuzzle(Constants.puzzle02s2SceneName);
                        break;
                    case 3:
                        GameRoot.ShowTips("That level doesn't exist right now\nCheck back later", true, false);
                        break;
                    default:
                        GameRoot.ShowTips("Error: what?? how?", true, false);
                        break;

                }
                break;

            case SceneNameMod.Puzzle03SceneName:
                //Find which level the player selected and goto that puzzle
                //at that level
                switch (level)
                {
                    case 0:
                        GameRoot.ShowTips("Please select a level", true, false);
                        break;
                    case 1:
                        GameRoot.instance.puzzleSystem.EnterPuzzle(Constants.puzzle03SceneName);
                        break;
                    case 2:
                        GameRoot.ShowTips("That level doesn't exist right now\nCheck back later", true, false);
                        break;
                    case 3:
                        GameRoot.ShowTips("That level doesn't exist right now\nCheck back later", true, false);
                        break;
                    default:
                        GameRoot.ShowTips("Error: what?? how?", true, false);
                        break;

                }
                break;

            case SceneNameMod.Puzzle04SceneName:
                //Find which level the player selected and goto that puzzle
                //at that level
                switch (level)
                {
                    case 0:
                        GameRoot.ShowTips("Please select a level", true, false);
                        break;
                    case 1:
                        GameRoot.instance.puzzleSystem.EnterPuzzle(Constants.puzzle04s1SceneName);
                        break;
                    case 2:
                        GameRoot.instance.puzzleSystem.EnterPuzzle(Constants.puzzle04s2SceneName);
                        break;
                    case 3:
                        GameRoot.instance.puzzleSystem.EnterPuzzle(Constants.puzzle04s3SceneName);
                        break;
                    default:
                        GameRoot.ShowTips("Error: what?? how?", true, false);
                        break;

                }
                break;

            default:
                break;
        }

        this.gameObject.SetActive(false);

    }

    public void setSceneName(string sceneName)
    {
        stageName = sceneName;
    }

    public void setHeaderInfo(string title, string descript)
    {
        stageTitle.GetComponent<Text>().text = title;
        stageDescrip.GetComponent<Text>().text = descript;
    }

    public void setSubmit(bool val)
    {
        submit = val;
    }

    public bool getSubmit()
    {
        return submit;
    }
}
