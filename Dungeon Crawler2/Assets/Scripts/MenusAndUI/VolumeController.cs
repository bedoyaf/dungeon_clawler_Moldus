using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.UI;

/// <summary>
/// Handles game sounds settings
/// </summary>
public class VolumeController : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer; 
    //private Slider volumeSlider;  

    private void Start()
    {
        float initialVolume = 0.5f;



        Slider volumeSlider = GetComponent<Slider>();
        volumeSlider.value = initialVolume;
        SetVolume(initialVolume);
        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    public void SetVolume(float volume)
    {

        audioMixer.SetFloat("MasterVolume", Mathf.Log10(volume)*20);
        PlayerPrefs.SetFloat("MasterVolume", volume);
    }
}
