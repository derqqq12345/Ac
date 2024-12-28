using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int score = 0;
    public int currentStage = 1;
    public int maxStages = 3;
    
    public void GameOver()
    {
        Time.timeScale = 0;
    }
    
    public void AddScore(int points)
    {
        score += points;
    }
    
    public void NextStage()
    {
        if (currentStage < maxStages)
        {
            currentStage++;
            LoadStage(currentStage);
        }
        else
        {
            GameComplete();
        }
    }
    
    void LoadStage(int stageNumber)
    {
    }
    
    public void GameComplete()
    {
    }
}