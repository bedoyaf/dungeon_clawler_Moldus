using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer; 
    //private Slider volumeSlider;  

    private void Start()
    {
        Slider volumeSlider = GetComponent<Slider>();
        SetVolume(volumeSlider.value);
        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    public void SetVolume(float volume)
    {
        Debug.Log("skibidi");
        audioMixer.SetFloat("MasterVolume", volume);
        PlayerPrefs.SetFloat("MasterVolume", volume);
    }
}
