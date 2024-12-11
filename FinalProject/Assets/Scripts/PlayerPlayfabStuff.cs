using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerPlayfabStuff : MonoBehaviour
{

    private float startTime;
    private float timeTaken;

    private bool isPlaying;

    public GameObject leaderboard;

    public GameObject playButton;
    public TextMeshProUGUI curTimeText;
    private double curTimeNum;

    void Update()
    {
        curTimeText.text = (Time.time - startTime).ToString("F2");

    }
    public void Begin()
    {
        startTime = Time.time;
        isPlaying = true;
        playButton.SetActive(false);
        leaderboard.SetActive(false);
    }

    void End()
    {
        timeTaken = Time.time - startTime;
        isPlaying = false;
        playButton.SetActive(true);
        leaderboard.SetActive(true);
        Leaderboard.instance.SetLeaderboardEntry(-Mathf.RoundToInt(timeTaken * 1000.0f));
    }
}
