﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject world;
    public GameObject[] blocks;
    public Text gameTimer;

    public int oneStartTime;
    public int twoStarTime;
    public int threeStarTime;

    [Space]
    public GameObject StartScreen;
    public GameObject startText;
    public Text startTimer;
    [Space]
    public GameObject endScreen;
    public Text finalTime;
    public GameObject nextLevelButton;

    int startingBlockCount;
    int blockCount;
    int timeCompleted;
    int gameTime;
    int timeBeforeTimer;

    bool startGameTime;
    [HideInInspector]
    public bool ended;

    GameObject star1;
    GameObject star2;
    GameObject star3;

    private void Start()
    {
        star1 = GameObject.Find("Star1");
        star2 = GameObject.Find("Star2");
        star3 = GameObject.Find("Star3");

        star1.SetActive(false);
        star2.SetActive(false);
        star3.SetActive(false);
        nextLevelButton.SetActive(false);
     

        PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "Unlocked", 1);

        gameTime = 0;
        ended = false;
        Health.cantDied = false;
        startingBlockCount = blocks.Length;
        blockCount = startingBlockCount;
        StartCoroutine(StartingScreen());
    }

    IEnumerator StartingScreen()
    {
        StartScreen.SetActive(true);
        yield return new WaitForSeconds(3);
        startText.SetActive(false);

        startTimer.gameObject.SetActive(true);
        StartCoroutine(StartCountdown(3));

    }

    IEnumerator StartCountdown(int length)
    {
        for(int i = length; i > 0; i--)
        {
            startTimer.text = "Starting in " + i + "...";
            yield return new WaitForSeconds(1);
        }
        StartScreen.SetActive(false);
        timeBeforeTimer = (int)Time.timeSinceLevelLoad;
        startGameTime = true;
        world.SetActive(true);
        Movement.canMove = true;
        gameTimer.gameObject.SetActive(true);
    }

    private void Update()
    {
        if(startGameTime)
        {
            gameTime = (int)Time.timeSinceLevelLoad - timeBeforeTimer;
            gameTimer.text =  gameTime.ToString();
        }

        if(blockCount <= 0 && !ended)
        {
            ended = true;
            GameEnded();
        }
    }

    void GameEnded()
    {
        print("Ads Removed = " + IAPManager.adsRemoved + " -- AdsRemoved Pref = " + PlayerPrefs.GetInt("AdsRemoved"));
        if (IAPManager.adsRemoved == false && PlayerPrefs.GetInt("AdsRemoved") == 0)
        {
            if (Advertisement.IsReady())
            {
                if (SceneManager.GetActiveScene().buildIndex % 2 == 0)
                    Advertisement.Show();
            }
        }

        Movement.canMove = false;
        gameTimer.gameObject.SetActive(false);
        endScreen.SetActive(true);
        timeCompleted = gameTime;
        finalTime.text = gameTime.ToString() + " Seconds!";
        nextLevelButton.SetActive(true);
        Health.cantDied = true;
        CheckStars();
    }

    void CheckStars()
    {
        if(timeCompleted <= oneStartTime)
        {
            if (PlayerPrefs.GetInt(SceneManager.GetActiveScene().name) < 1)
                PlayerPrefs.SetInt(SceneManager.GetActiveScene().name, 1);

            star1.SetActive(true);

            if(timeCompleted <= twoStarTime)
            {
                if (PlayerPrefs.GetInt(SceneManager.GetActiveScene().name) < 2)
                    PlayerPrefs.SetInt(SceneManager.GetActiveScene().name, 2);

                star2.SetActive(true);

                if (timeCompleted <= threeStarTime)
                {
                    if (PlayerPrefs.GetInt(   SceneManager.GetActiveScene().name) < 3)
                        PlayerPrefs.SetInt(   SceneManager.GetActiveScene().name, 3);

                    star3.SetActive(true);
                }
            }
        }
        else if(timeCompleted > oneStartTime)
        {
            PlayerPrefs.SetInt(   SceneManager.GetActiveScene().name, 4);
        }

    }

    public void BlockDestroyed()
    {
        blockCount--;
    }

}