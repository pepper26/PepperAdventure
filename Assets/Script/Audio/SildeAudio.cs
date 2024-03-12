using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SildeAudio : MonoBehaviour
{
    public Slider musicP;
    public Slider sfxP;
    public Slider musicB;
    public Slider sfxB;
    // Update is called once per frame
    void Update()
    {
        musicB.value = musicP.value;
        sfxB.value = sfxP.value;

    }
}
