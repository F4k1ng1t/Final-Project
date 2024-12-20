using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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

    public Rigidbody2D jugglingGuy;
    public GameObject jugglee;
    bool grounded;
    public GameObject ground;


    void Awake()
    {

       jugglingGuy = GetComponent<Rigidbody2D>();
        jugglingGuy.isKinematic = true;
    }
    void Update()
    {
        if (!isPlaying)
            return;

        curTimeText.text = (Time.time - startTime).ToString("F2");

    }
    public async void Begin()
    {
        playButton.SetActive(false);
        leaderboard.SetActive(false);

        await Task.Delay(2000);

        startTime = Time.time;
        isPlaying = true;
        
        jugglingGuy.isKinematic = false;
    }

    void OnCollisionEnter2D(Collision2D collision)
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
