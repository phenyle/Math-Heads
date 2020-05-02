using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleEnvironmentController : MonoBehaviour
{

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayRotatedSoundFX(bool isPlay)
    {
        if(isPlay)
        {
            if(audioSource.clip == null)
            {
                audioSource.clip = GameRoot.instance.audioService.GetFXAudioClip(Constants.audioP03RotatedPuzzleEnvironment);
            }

            audioSource.loop = true;
            audioSource.Play();
        }
        else
        {
            audioSource.Stop();
        }
    }
}
