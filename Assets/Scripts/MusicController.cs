using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    AudioClip[] clips;
    AudioSource audioSource;
    int clipNo = 0;
    void Start()
    {
        clips = Resources.LoadAll<AudioClip>("Sounds/music");
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(PlayNextClip());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator PlayNextClip()
    {
        audioSource.clip = clips[clipNo];
        audioSource.Play();
        clipNo += 1;
        if (clipNo > clips.Length){
            clipNo = 0;
        }
        yield return new WaitForSeconds(audioSource.clip.length);
        PlayNextClip();
    }
}
