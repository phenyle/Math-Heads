using UnityEngine;

public class AudioService : MonoBehaviour
{
    public AudioSource audioBg;
    public AudioSource audioUI;

    public void InitService()
    {
        Debug.Log("Init Audio Service");
    }

    public void PlayBgMusic(string audioName, bool isLoop)
    {
        //Get the audio clip by the resource service
        AudioClip audioClip = GameRoot.instance.resourceService.LoadAudio("Audios/Musics/" + audioName, true);

        //Check if the background audio clip whether is null or next audio clip name matches the current audio clip
        if (audioBg.clip == null || audioBg.clip.name != audioClip.name)
        {
            audioBg.clip = audioClip;
            audioBg.loop = isLoop;
            audioBg.Play();
        }
    }

    public void PlayUIAudio(string audioName)
    {
        //Get the audio clip by the resource service
        AudioClip audioClip = GameRoot.instance.resourceService.LoadAudio("Audios/Sound FX/" + audioName, true);
        audioUI.clip = audioClip;
        audioUI.Play();
    }

    public void SetBgVolume(float volume)
    {
        audioBg.volume = volume;
    }
}
