using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStage : MonoBehaviour
{
    [Header("Boss Settings")]
    [SerializeField] private GameObject warningText;
    [SerializeField] private float warningDuration = 3f;
    [SerializeField] private AudioClip bossMusic;
    [SerializeField] private float bossStartDelay = 2f;
    
    private AudioSource audioSource;
    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        
        StartCoroutine(BossWarning());
    }
    
    IEnumerator BossWarning()
    {
        if (warningText != null)
        {
            warningText.SetActive(true);
            yield return new WaitForSeconds(warningDuration);
            warningText.SetActive(false);
        }
        
        yield return new WaitForSeconds(bossStartDelay);
        
        if (audioSource != null && bossMusic != null)
        {
            audioSource.clip = bossMusic;
            audioSource.Play();
        }
    }
}