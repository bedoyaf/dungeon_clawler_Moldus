using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer; 
    //private Slider volumeSlider;  

    private void Start()
    {
        int initialVolume = 0;
        Slider volumeSlider = GetComponent<Slider>();
        volumeSlider.value = initialVolume;
        SetVolume(initialVolume);
        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("MasterVolume", volume);
        PlayerPrefs.SetFloat("MasterVolume", volume);
    }
}
