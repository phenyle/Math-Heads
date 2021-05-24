using System.Collections.Generic;
using System.Security.Cryptography;
using System;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using GoogleSheetsForUnity;


public class TitleWindow : WindowRoot
{
    public Transform panelMain;
    public Transform panelOptions;
    public Slider sliderVolume;
    public Slider sliderSoundFX;
    public Dropdown ddResolution;

    public Transform panelLogin;
    public InputField lg_username;
    public InputField lg_pass;

    public Transform panelNewAcct;
    public InputField na_first;
    public InputField na_last;
    public InputField na_pass1;
    public InputField na_pass2;
    public Dropdown na_education;

    public Transform panelError;
    public Text errorText;

    private SendToGoogle GSFU;
    private bool loadingPlayer = false;
    private int attemptNum = 0;

    //private Resolution[] resolutions;

    protected override void InitWindow()
    {
        base.InitWindow();

        // SetBgVolume();
        sliderVolume.value = audioService.bgVolume;
        audioService.PlayBgMusic(Constants.audioBgMenu, true);

        // SetSoundFXVolume();
        sliderSoundFX.value = audioService.UIFXVolume;

        // show mouse
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        GSFU = GameObject.Find("GameRoot").GetComponent<SendToGoogle>();
        loadingPlayer = false;
        attemptNum = 0;


        //Initialize Resolution Dropdown
        /*
        resolutions = Screen.resolutions;
        ddResolution.ClearOptions();

        List<string> options = new List<string>();
        int currentResolutionIndex = 0;

        for(int i = 0; i < resolutions.Length; i++)
        {
            if((double)resolutions[i].width/resolutions[i].height == 16.0/9.0)
            {
                string option = resolutions[i].width + " x " + resolutions[i].height;
                options.Add(option);

                if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
                {
                    currentResolutionIndex = i;
                }
            }
        }

        ddResolution.AddOptions(options);
        ddResolution.value = currentResolutionIndex;
        ddResolution.RefreshShownValue();*/
        //*************************************
    }

    public void LateUpdate()
    {
        if(loadingPlayer)
        {
            try
            {
                string salt = GSFU.player.users.salt;
                string passattempt = ComputeHash(Encoding.UTF8.GetBytes(lg_pass.text), Encoding.UTF8.GetBytes(salt));
                if (passattempt.CompareTo(GSFU.player.users.hash) == 0)
                {
                    loadingPlayer = false;
                    Debug.Log("LOGIN SUCCESFUL");

                    //load in other player settings
                    for(int i = 1; i < GSFU.player.sheetNames.Length; i++)
                    {
                        GSFU.RetrievePlayerData(GSFU.player.sheetNames[i], "name", lg_username.text);
                    }

                    attemptNum = 0;
                    PlayGame();
                }
                else
                {
                    loadingPlayer = false;
                    Debug.Log("LOGIN ERROR: wrong user/pass");
                    attemptNum = 0;
                    errorText.text = "Username or password Incorrect or Not Found";
                    SetActive(panelError, true);
                }

            }
            catch
            {
                attemptNum++;
                if (attemptNum > 1000)
                {
                    //login timeout
                    loadingPlayer = false;
                    Debug.Log("LOGIN ERROR: timeout");
                    attemptNum = 0;
                    errorText.text = "Username or password Incorrect or Not Found";
                    SetActive(panelError, true);
                }
                else
                    Debug.Log("data not yet received");
            }

        }
    }

    public void PlayGame()
    {
        //Reset puzzle complete status
        for (int i = 0; i < GameRoot.instance.puzzleCompleted.Length; i++)
        {
            GameRoot.instance.puzzleCompleted[i] = false;
        }

        DialogueManager.instance.ResetAll();

        audioService.PlayUIAudio(Constants.audioUIStartBtn);
        //GameRoot.instance.puzzleSystem.EnterPuzzle(Constants.puzzle01SceneName);// original 

        GameRoot.instance.puzzleSystem.EnterPuzzle(Constants.mainSceneName);//added by LaQuez Brown 1-26-21

        // hides mouse when loading game
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void ClickPlayBtn()
    {
        SetActive(panelMain, false);
        SetActive(panelLogin, true);

        /**
        //Reset puzzle complete status
        for (int i = 0; i < GameRoot.instance.puzzleCompleted.Length; i++)
        {
            GameRoot.instance.puzzleCompleted[i] = false;
        }

        DialogueManager.instance.ResetAll();

        audioService.PlayUIAudio(Constants.audioUIStartBtn);
        //GameRoot.instance.puzzleSystem.EnterPuzzle(Constants.puzzle01SceneName);// original 

        GameRoot.instance.puzzleSystem.EnterPuzzle(Constants.mainSceneName);//added by LaQuez Brown 1-26-21

        // hides mouse when loading game
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        **/

    }

    public void ClickTutorialBtn()
    {
        audioService.PlayUIAudio(Constants.audioUIClickBtn);
        GameRoot.instance.puzzleSystem.EnterPuzzle(Constants.tutorialSceneName);
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

    //------------------------------------
    // OPTIONS Functions
    //------------------------------------
    public void SetBgVolume()
    {
        audioService.SetBgVolume(sliderVolume.value);
    }

    public void SetSoundFXVolume()
    {
        audioService.SetSoundFXVolume(sliderSoundFX.value);
    }

    public void ClickBackBtn()
    {
        SetActive(panelOptions, false);
        SetActive(panelMain, true);
        audioService.PlayUIAudio(Constants.audioUIClickBtn);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex + 1);
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;

    }

    public void SetResolution(int index)
    {
        //Resolution resolution = resolutions[index];
        //Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        //Screen.SetResolution(1920, 1080, false);
    }

    //------------------------------------
    // LOGIN Functions
    //------------------------------------
    public void ClickLoginBackBtn()
    {
        lg_username.text = "";
        lg_pass.text = "";

        SetActive(panelLogin, false);
        SetActive(panelMain, true);
    }

    public void ClickNewAccount()
    {
        lg_username.text = "";
        lg_pass.text = "";

        SetActive(panelLogin, false);
        SetActive(panelNewAcct, true);
    }

    public void ClickLoginBtn()
    {
        if (GSFU.player == null)
            GSFU.CreatePlayer();

        GSFU.RetrievePlayerData("users", "name", lg_username.text);

        loadingPlayer = true;
    }


    //------------------------------------
    // CREATE ACCOUNT Functions
    //------------------------------------
    public void ClickNwAcctBackBtn()
    {
        na_first.text = "";
        na_last.text = "";
        na_pass1.text = "";
        na_pass2.text = "";
 //       na_education.options = na_education.options[0];

        SetActive(panelNewAcct, false);
        SetActive(panelLogin, true);
    }

    public void ClickNAloginBtn()
    {
        if(GSFU.player == null)
            GSFU.CreatePlayer();

        GSFU.player.users.education = na_education.captionText.text;

        if (na_first.text.Length == 0 && na_last.text.Length == 0)
        {
            errorText.text = "You must enter in both a First and Last name.";
            SetActive(panelError, true);
        }
        else
        {
            GSFU.player.SetName(na_first.text + "." + na_last.text);

            if(na_pass1.text.CompareTo(na_pass2.text) != 0)
            {
                errorText.text = "The passwords do not match";
                SetActive(panelError, true);
            }
            else
            {
                if(na_pass1.text.Length < 8 || na_pass2.text.Length < 8)
                {
                    errorText.text = "Password must be at least 8 characters long";
                    SetActive(panelError, true);
                }
                else
                {
                    string salt = GenerateSalt();
                    string hashedpass = ComputeHash(Encoding.UTF8.GetBytes(na_pass1.text), Encoding.UTF8.GetBytes(salt));
                    GSFU.player.users.hash = "\'" + hashedpass;
                    GSFU.player.users.salt = "\'" + salt;

                    GSFU.player.users.tot_time = 0.0f;
                    
                    GSFU.SaveNewPlayer();

                    //Reset puzzle complete status
                    for (int i = 0; i < GameRoot.instance.puzzleCompleted.Length; i++)
                    {
                        GameRoot.instance.puzzleCompleted[i] = false;
                    }

                    DialogueManager.instance.ResetAll();

                    audioService.PlayUIAudio(Constants.audioUIStartBtn);
                    //GameRoot.instance.puzzleSystem.EnterPuzzle(Constants.puzzle01SceneName);// original 

                    GameRoot.instance.puzzleSystem.EnterPuzzle(Constants.mainSceneName);//added by LaQuez Brown 1-26-21

                    // hides mouse when loading game
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;
                }
            }
        }
    }

    private string ComputeHash(byte[] bytesToHash, byte[] salt)
    {
        var byteResult = new Rfc2898DeriveBytes(bytesToHash, salt, 10000);
        return Convert.ToBase64String(byteResult.GetBytes(24));
    }

    private string GenerateSalt()
    {
        var bytes = new byte[128 / 8];
        var rng = new RNGCryptoServiceProvider();
        rng.GetBytes(bytes);
        return Convert.ToBase64String(bytes);
    }

    //--------------------------------
    // MISC
    //--------------------------------

    public void ErrorOKButton()
    {
        SetActive(panelError, false);
    }

}
