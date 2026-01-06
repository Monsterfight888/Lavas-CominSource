using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Bullet : MonoBehaviourPunCallbacks, IPunObservable
{
    public Vector2 position;
    public float direction = 0;
    public float collectCountdown;
    public float startTime;
    bool first = true;
    bool first2 = true;
    void Start()
    {
        position = new Vector2(transform.position.x - 0.5f, transform.position.y - 0.5f);
        collectCountdown = startTime;
        Destroy(gameObject, 7f);
        
    }
    public void OnPhotonSerializeView(PhotonStream p_stream, PhotonMessageInfo p_messageinfo)
    {
        if (p_stream.IsWriting)
        {
            if (first)
            {
                p_stream.SendNext(direction);
                first = false;
            }
        }
        else if (p_stream.IsReading)
        {
            if (first)
            {
                direction = (float)p_stream.ReceiveNext();
                first = false;
                //position.y = (int)p_stream.ReceiveNext();
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        bool dfdsf = false;
        if (collectCountdown >= 0)
        {
            collectCountdown -= 1 * Time.deltaTime;
        }
        else
        {
            dfdsf = true;
            collectCountdown = startTime;
        }
        if (direction == 1 && dfdsf)
        {
            position.x += 1f;
        }
        else if (direction == 2 && dfdsf)
        {
            position.x -= 1f;
        }
        else if (direction == 3 && dfdsf)
        {
            position.y += 1f;
        }
        else if (direction == 4 && dfdsf)
        {
            position.y -= 1f;
        }
        for (int i = 0; i < World.theWorld.units.Count; i++)
        {
            if (World.theWorld.units[i].position == position)
            {
                transform.position = World.theWorld.units[i].gameObject.transform.position;
            }
        }
        List<unit> units = World.theWorld.units;
        unit BlockOn = World.theWorld.units[World.theWorld.FindUnitBasedOffPos(new Vector2(position.x, position.y))];
        if(BlockOn.isDestroyedByBullet == true)
        {
            Destroy(BlockOn.gameObject);
            units.Remove(BlockOn);
            GameObject refrence = Instantiate(player.instance.bridgePrefab, GameObject.Find("Grid/Tilemap").gameObject.transform);
            refrence.transform.position = new Vector2(transform.position.x, transform.position.y);
            refrence.GetComponent<unit>().SetPosition();
            units.Add(refrence.GetComponent<unit>());
            Destroy(gameObject);
        }
        if(player.instance.position == position)
        {
            if (first2)
            {
                gameObject.GetPhotonView().RPC("resetLevel", RpcTarget.All, 0, player.instance.playerInt);
                first2 = false;
            }
        }
    }
    [PunRPC]
    public void resetLevel(int level, int playerInt)
    {
        player.instance.StartCoroutine(player.instance.kill(level, playerInt));
    }
}
