using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class Menu : MonoBehaviourPunCallbacks, ILobbyCallbacks
{
    [Header("Screens")]
    public GameObject startScreen;
    public GameObject mainScreen;
    public GameObject createRoomScreen;
    public GameObject lobbyScreen;
    public GameObject lobbyBrowserScreen;

    [Header("Main Screen")]
    public Button createRoomButton;
    public Button findRoomButton;

    [Header("Lobby")]
    public TextMeshProUGUI playerListText;
    public TextMeshProUGUI roomInfoText;
    public Button startGameButton;

    [Header("Lobby Browser")]
    public RectTransform roomListContainer;
    public GameObject roomButtonPrefab;
    private List<GameObject> roomButtons = new List<GameObject>();
    private List<RoomInfo> roomList = new List<RoomInfo>();
   
   
   [Header("Single or Multi Player")]
   public GameObject singlePlayerScreen;
   public GameObject multiPlayerScreen;
    
  public void OnSinglePlayerButton()
  {
      startScreen.SetActive(false);
      SetScreen(singlePlayerScreen);
  }

 public void OnMultiPlayerButton()
  {
        startScreen.SetActive(false);
        SetScreen(multiPlayerScreen);
        SetScreen(mainScreen);
       
  }

    public void OnBackToStartButton()
    {
        SetScreen(startScreen);
        singlePlayerScreen.SetActive(false);
    }

  void Start()
  {
      createRoomButton.interactable = false;
      findRoomButton.interactable = false;

      Cursor.lockState = CursorLockMode.None;

      if (PhotonNetwork.InRoom)
      {
          PhotonNetwork.CurrentRoom.IsVisible = true;
          PhotonNetwork.CurrentRoom.IsOpen = true;
      }

  }

  void SetScreen(GameObject screen)
  {

      mainScreen.SetActive(false);
      createRoomScreen.SetActive(false);
      lobbyScreen.SetActive(false);
      lobbyBrowserScreen.SetActive(false);

      screen.SetActive(true);

      if (screen == lobbyBrowserScreen)
          UpdateLobbyBrowserUI();

  }

  public void OnPlayerNameValueChanged(TMP_InputField playerNameInput)
  {
      PhotonNetwork.NickName = playerNameInput.text;
  }

  public override void OnConnectedToMaster()
  {
      createRoomButton.interactable = true;
      findRoomButton.interactable = true;
  }

  public void OnCreateRoomButton()
  {
        
      SetScreen(createRoomScreen);
        mainScreen.SetActive(false);

    }

  public void OnFindRoomButton()
  {
        
        SetScreen(lobbyBrowserScreen);
        mainScreen.SetActive(false);
    }

  public void OnBackButton()
  {
      SetScreen(mainScreen);
  }

  public void OnCreateButton(TMP_InputField roomNameInput)
  {
      NetworkManager.instance.CreateRoom(roomNameInput.text);
  }

  public override void OnJoinedRoom()
  {
      SetScreen(lobbyScreen);
      photonView.RPC("UpdateLobbyUI", RpcTarget.All);
  }

  [PunRPC]
  void UpdateLobbyUI()
  {

      startGameButton.interactable = PhotonNetwork.IsMasterClient;

      playerListText.text = "";
      foreach (Player player in PhotonNetwork.PlayerList)
          playerListText.text += player.NickName + "\n";

      roomInfoText.text = "<b>Room Name</b>\n" + PhotonNetwork.CurrentRoom.Name;
  }

  public override void OnPlayerLeftRoom(Player otherPlayer)
  {
      UpdateLobbyUI();
  }

  public void OnStartGameButton()
  {

      PhotonNetwork.CurrentRoom.IsOpen = false;
      PhotonNetwork.CurrentRoom.IsVisible = false;

      NetworkManager.instance.photonView.RPC("ChangeScene", RpcTarget.All, "MultiPlayer");
  }

  public void OnLeaveLobbyButton()
  {
      PhotonNetwork.LeaveRoom();
      SetScreen(mainScreen);
  }
  void UpdateLobbyBrowserUI()
  {

      foreach (GameObject button in roomButtons)
          button.SetActive(false);

      for (int x = 0; x < roomList.Count; ++x)
      {

          GameObject button = x >= roomButtons.Count ? CreateRoomButton() : roomButtons[x];
          button.SetActive(true);

          button.transform.Find("RoomNameText").GetComponent<TextMeshProUGUI>().text =
         roomList[x].Name;
          button.transform.Find("PlayerCountText").GetComponent<TextMeshProUGUI>().text
          = roomList[x].PlayerCount + " / " + roomList[x].MaxPlayers;


          Button buttonComp = button.GetComponent<Button>();
          string roomName = roomList[x].Name;
          buttonComp.onClick.RemoveAllListeners();
          buttonComp.onClick.AddListener(() => { OnJoinRoomButton(roomName); });
      }
  }

  GameObject CreateRoomButton()
  {
      GameObject buttonObj = Instantiate(roomButtonPrefab, roomListContainer.transform)
     ;
      roomButtons.Add(buttonObj);
      return buttonObj;
  }

  public void OnJoinRoomButton(string roomName)
  {
      NetworkManager.instance.JoinRoom(roomName);
  }

  public void OnRefreshButton()
  {
      UpdateLobbyBrowserUI();
  }

  public override void OnRoomListUpdate(List<RoomInfo> allRooms)
  {
      roomList = allRooms;
  }
   
}

