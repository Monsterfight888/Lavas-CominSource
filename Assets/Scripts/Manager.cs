using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Manager : MonoBehaviour
{
    public string player_prefab;
    public player[] otherPlayers;
    void Start()
    {
        
        PhotonNetwork.Instantiate(player_prefab, transform.position, transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
