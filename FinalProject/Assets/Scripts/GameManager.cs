using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Linq;
using Cinemachine;
public class GameManager : MonoBehaviourPunCallbacks
{
    [Header("Stats")]
    public bool gameEnded = false;
    public float timeToWin;             // total time to hold the hat to win
    public float invincibleDuration;    // prevents players from instantly losing hat
    private float hatPickupTime;        // the time that hat was picked up

    [Header("Players")]
    public string playerPrefabLocation; // path in Resources folder
    public Transform[] spawnPoints;     // array of all available spawn points
    public PlayerMovement[] players;  // array of all the players
    private int playersInGame;          // number of players in the game
    [Header("Components")]
    public CinemachineTargetGroup targetgroup;
    // instance
    public static GameManager instance;

    private void Awake()
    {
        // lazy singleton - see NetworkManager for better implementation of this pattern
        // narrator justifies this based on the idea that we're not persisting this via DontDestroyOnLoad
        instance = this;
    }

    private void Start()
    {

        players = new PlayerMovement[PhotonNetwork.PlayerList.Length];
        //foreach(PlayerMovement player in players)
        //{
        //    targetgroup.AddMember(player.gameObject.transform, 1f, 4f);
        //}
        photonView.RPC("ImInGame", RpcTarget.AllBuffered);
    }

    // called everytime a player finishes its start function
    [PunRPC]
    void ImInGame()
    {
        playersInGame++;

        // when the last player announces they're in game, each player can spawn themselves
        if (playersInGame == PhotonNetwork.PlayerList.Length)
            SpawnPlayer();
    }

    void SpawnPlayer()
    {
        // instantiate the player across the network
        GameObject playerObj = PhotonNetwork.Instantiate(playerPrefabLocation, spawnPoints[Random.Range(0, spawnPoints.Length)].position, Quaternion.identity);

        // get the player script
        PlayerMovement playerScript = playerObj.GetComponent<PlayerMovement>();

        // initialize the player
        playerScript.photonView.RPC("Initialize", RpcTarget.All, PhotonNetwork.LocalPlayer);
    }

    public PlayerMovement GetPlayer(int playerId)
    {
        return players.First(x => x.id == playerId);
    }

    public PlayerMovement GetPlayer(GameObject playerObject)
    {
        return players.First(x => x.gameObject == playerObject);
    }

    // is the player able to take the hat at this current time?
    public bool CanGetHat()
    {
        if (Time.time > hatPickupTime + invincibleDuration)
            return true;
        else
            return false;
    }

    [PunRPC]
    void WinGame(int playerId)
    {
        gameEnded = true;
        PlayerMovement player = GetPlayer(playerId);

        // set the UI to show who's won
        //GameUI.instance.SetWinText(player.photonPlayer.NickName);

        Invoke("GoBackToMenu", 3.0f);
    }

    void GoBackToMenu()
    {
        PhotonNetwork.LeaveRoom();
        NetworkManager.instance.ChangeScene("Menu");
    }
}
