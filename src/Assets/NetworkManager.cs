using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {

	private const string gameTypeName = "Keel2";
	private const string gameName = "TestRoom";

	private HostData[] hostList;
	
	public GameObject playerPrefab;
	
	private TrackingBehavior trackingBehavior;


	private void RefreshHostList()
	{
		MasterServer.RequestHostList (gameTypeName);
	}

	void OnMasterServerEvent(MasterServerEvent msEvent)
	{
		if (msEvent == MasterServerEvent.HostListReceived)
		{
			hostList = MasterServer.PollHostList();
		}
	}

	private void StartServer()
	{
		Network.InitializeServer (4, 25000, !Network.HavePublicAddress ());
		MasterServer.RegisterHost (gameTypeName, gameName);
	}

	void OnServerInitialized()
	{
		GameObject player;
		
		Debug.Log ("Server Initialized");
		
		player = SpawnPlayer();
		TrackPlayerLocal(player);
	}

	private void JoinServer(HostData hostData)
	{
		Network.Connect (hostData);
	}
	
	void OnConnectedToServer()
	{
		GameObject player;
		
		Debug.Log("Connected to Server");
		
		player = SpawnPlayer();
		TrackPlayerLocal(player);
	}

	private void TrackPlayerLocal(GameObject player)
	{
		trackingBehavior = Camera.main.GetComponent<TrackingBehavior>();
		trackingBehavior.target = player;
	}

	private GameObject SpawnPlayer()
	{
		GameObject player;
		
		player = (GameObject)Network.Instantiate(
			playerPrefab, new Vector3(75.0f, 10.0f, 25.0f) , Quaternion.identity, 0);
		
		return player;	
	}

	void OnGUI()
	{
		if (!Network.isClient && !Network.isServer) {
			if (GUI.Button(new Rect(100, 100, 250, 100), "Start Server"))
			{
				StartServer();
			}
			
			if (GUI.Button(new Rect(100, 250, 250, 100), "Refresh Hosts"))
			{
				RefreshHostList();
			}
			
			if (hostList != null)
			{
				for (int i = 0; i < hostList.Length; i++)
				{
					if (GUI.Button(new Rect(400, 100 + (110 * i), 300, 100),
						hostList[i].gameName))
					{
						JoinServer(hostList[i]);
					}
				}
			}
		}
	}
	
}
