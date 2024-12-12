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
    bool grounded;
    public GameObject ground;


    void Awake()
    {

       jugglingGuy = GetComponent<Rigidbody>();
        jugglingGuy.isKinematic = true;
    }
    void Update()
    {
        if (!isPlaying)
            return;

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

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == ground)
        {
            // grounded = true;
            Debug.Log("Grounded");
            End();

            
        }
    }

    void End()
    {
        Debug.Log("StartedEnd");
        timeTaken = Time.time - startTime;
        isPlaying = false;
        playButton.SetActive(true);
        leaderboard.SetActive(true);
        Leaderboard.instance.SetLeaderboardEntry(-Mathf.RoundToInt(timeTaken * 1000.0f));
    }
}
