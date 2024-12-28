using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class StageManager : MonoBehaviour
{
    [Header("Stage Settings")]
    [SerializeField] private GameObject[] stagePrefabs;
    [SerializeField] private float stageTransitionDelay = 2f;
    [SerializeField] private UnityEvent onStageComplete;
    [SerializeField] private UnityEvent onGameComplete;
    
    [Header("Debug")]
    [SerializeField] private bool showDebugInfo = true;
    
    private int currentStageIndex = 0;
    private GameObject currentStage;
    
    void Start()
    {
        if (stagePrefabs == null || stagePrefabs.Length == 0)
        {
            Debug.LogError("No stage prefabs assigned!");
            return;
        }
        
        LoadStage(0);
    }
    
    public void LoadStage(int stageIndex)
    {
        if (currentStage != null)
        {
            Destroy(currentStage);
        }
        
        if (stageIndex < stagePrefabs.Length && stagePrefabs[stageIndex] != null)
        {
            currentStage = Instantiate(stagePrefabs[stageIndex]);
            currentStageIndex = stageIndex;
            
            if (showDebugInfo)
            {
                Debug.Log($"Loaded stage {stageIndex + 1}");
            }
        }
    }
    
    public void OnStageComplete(int stageNumber)
    {
        onStageComplete?.Invoke();
        StartCoroutine(StageTransition());
    }
    
    IEnumerator StageTransition()
    {
        yield return new WaitForSeconds(stageTransitionDelay);
        
        if (currentStageIndex + 1 < stagePrefabs.Length)
        {
            LoadStage(currentStageIndex + 1);
        }
        else
        {
            if (showDebugInfo)
            {
                Debug.Log("Game Complete!");
            }
            onGameComplete?.Invoke();
        }
    }
}