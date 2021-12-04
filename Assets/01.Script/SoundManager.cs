using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoSingleton<SoundManager>
{
    [System.Serializable]
    private struct SoundClip
    {
        public string keys;
        public AudioClip clips;
    }

    [SerializeField] private List<SoundClip> unitsSoundList = new List<SoundClip>();
    [SerializeField] private List<SoundClip> uiSoundList = new List<SoundClip>();
    [SerializeField] private AudioMixerGroup mainMixer = null;

    private bool soundOn = true;

    public void SetSoundClip(AudioSource audio, string type, string key)
    {
        audio.clip = FindClip(type, key);
    }

    public AudioClip FindClip(string type,string key)
    {
        switch (type)
        {
            case "Units":
                foreach(var au in unitsSoundList)
                {
                    if (au.keys.Equals(key))
                        return au.clips;
                }
                break;

            case "UI":
                foreach (var au in uiSoundList)
                {
                    if (au.keys.Equals(key))
                        return au.clips;
                }
                break;
        }

        return null;
    }

    public void ToggleSound()
    {
        soundOn = !soundOn;
        if (soundOn)
        {
            mainMixer.audioMixer.SetFloat("Master", 0);
            UIManager.Instance.SoundMuteMuttonImageChange(false);
        }
        else
        {
            mainMixer.audioMixer.SetFloat("Master", -80);
            UIManager.Instance.SoundMuteMuttonImageChange(true);
        }
    }
}
