using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public Text scoreToDisplay;
    public static int score = 0;
    public ScoreStorageSO storage;
    private bool died;
    
    private void OnEnable()
    {
        Hero.died += HeroOndied;
    }

    private void HeroOndied(int obj)
    {
        died = true;
        if (storage.totalScore < score)
        {
            storage.totalScore = score + 1;
        }
    }

    public static void AddScore()
    {
        score++;
    }

    private void Update()
    {
        if (!died)
        {
            scoreToDisplay.text = $"Score: {score}";
        }
        else
        {
            score = 0;
            scoreToDisplay.text = $"Score: {score}";
        }
    }
}
