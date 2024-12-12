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

    public Rigidbody jugglingGuy;
    public GameObject jugglee;
    bool IsGrounded;


    void Start()
    {
        jugglingGuy = GetComponent<Rigidbody>();
    }
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
        jugglingGuy.isKinematic = false;
    }

    void OnTriggerStay(Collider jugglee)
    {
        if (jugglee.transform.tag == "Ground")
        {
            IsGrounded = true;
            Debug.Log("Grounded");
            End();
        }
        else
        {
            IsGrounded = false;
            Debug.Log("Not Grounded!");
        }
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
