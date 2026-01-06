using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class player : MonoBehaviourPunCallbacks, IPunObservable
{
    public static player instance;
    public Vector2 position;
    public float x;
    public unit whatIAmStandingOn;
    public int whatIAmStandingOnIndex;
    public GameObject myGenerator;
    public List<GameObject> allGeneratorInstances;
    public float collectCountdown;
    public float startTime;
    public int moneys;
    public GameObject shopText;
    public bool usingShop;
    public Transform beak;
    private Vector2 beakDirection = new Vector2(0, 1);
    public GameObject bridgePrefab;
    public Text bridgesText;
    public int bridgesLeft;
    public int wallsLeft;
    public Text moneysText;
    public Text collectText;
    public int currentItem;
    public GameObject WallPrefab;
    public GameObject bulletPrefab;
    public int bulletsLeft;
    public Text bulletText;
    private PhotonView view;
    public GameObject MainCamera;
    public int playerInt = 0;
    public bool gameStarted = false;
    public bool kjfsdl = true;
    public GameObject TNTPrefab;
    public Vector2 TNTArea;
    public GameObject lavaPref;
    public int GroundLeft;
    public int TNTLeft;
    public GameObject GroundPref;
    public int DrillsLeft;
    private void Awake()
    {
        view = GetComponent<PhotonView>();
        if (view.IsMine)
        {
            instance = this;
        }
    }
    public void OnPhotonSerializeView(PhotonStream p_stream, PhotonMessageInfo p_messageinfo)
    {
        if (p_stream.IsWriting)
        {
            p_stream.SendNext(position);
            p_stream.SendNext(beakDirection);
            p_stream.SendNext(playerInt);
        }
        else if(p_stream.IsReading)
        {
            position = (Vector2)p_stream.ReceiveNext();
            beakDirection = (Vector2)p_stream.ReceiveNext();
            playerInt = (int)p_stream.ReceiveNext();
            //position.y = (int)p_stream.ReceiveNext();
        }
    }
    void Start()
    {
        TNTArea = new Vector2(0.32f, 0.32f);
        if (view.IsMine)
        {
            
            playerInt = 1;
            if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
            {
                playerInt = 2;
                GetComponent<SpriteRenderer>().color = Color.green;
                beak.GetComponent<SpriteRenderer>().color = Color.green;
                position = new Vector2(0, 31);
            }
        }
        Button startButton = GameObject.Find("Canvas/Lobby Text/Start").GetComponent<Button>();
        startButton.onClick.AddListener(startKey);
        if (!view.IsMine) return;
        MainCamera.SetActive(true);
        bridgesText = GameObject.Find("Canvas/BridgesLeft").GetComponent<Text>();
        moneysText = GameObject.Find("Canvas/MoneysLeft").GetComponent<Text>();
        collectText = GameObject.Find("Canvas/CollectTimeLeft").GetComponent<Text>();
        bulletText = GameObject.Find("Canvas/BulletsLeft").GetComponent<Text>();
        shopText = GameObject.Find("Canvas/Shop Text");
        Button buyBridger = GameObject.Find("Canvas/Shop Text/Bridger").GetComponent<Button>();
        buyBridger.onClick.AddListener(buyBridge);
        Button buyBulletb = GameObject.Find("Canvas/Shop Text/Bullet").GetComponent<Button>();
        buyBulletb.onClick.AddListener(buyBullet);
        Button buyButton = GameObject.Find("Canvas/Shop Text/Wall").GetComponent<Button>();
        buyButton.onClick.AddListener(buyWall);
        Button buyExplosivee = GameObject.Find("Canvas/Shop Text/Explosive").GetComponent<Button>();
        buyExplosivee.onClick.AddListener(buyTNT);
        Button buyGrounde = GameObject.Find("Canvas/Shop Text/Grounds").GetComponent<Button>();
        buyGrounde.onClick.AddListener(buyGround);
        Button buyFromGrunnings = GameObject.Find("Canvas/Shop Text/Drills").GetComponent<Button>();
        buyFromGrunnings.onClick.AddListener(buyDrill);
        GameObject LobbyText = GameObject.Find("Canvas/Lobby Text");
        shopText.SetActive(false);
        LobbyText.SetActive(true);

        List<unit> units = World.theWorld.units;
        for (int i = 0; i < World.theWorld.units.Count; i++)
        {
            if (units[i].position == position)
            {
                transform.position = units[i].gameObject.transform.position;
                whatIAmStandingOn = units[i];
                whatIAmStandingOnIndex = i;

            }
        }
        collectCountdown = startTime; 
    }
    public void startKey()
    {
        gameObject.GetPhotonView().RPC("startKeyPun", RpcTarget.All);
    }
    [PunRPC]
    private void startKeyPun()
    {
        gameStarted = true;
        PhotonNetwork.CurrentRoom.IsOpen = false;
    }
    public IEnumerator kill(int level, int player)
    {
        player[] players = FindObjectsOfType<player>();
        for (int i = 0; i < players.Length; i++)
        {
            if (player == players[i].playerInt)
            {
                players[i].GetComponent<SpriteRenderer>().color = Color.red;
                players[i].beak.GetComponent<SpriteRenderer>().color = Color.red;
            }
        }
        //gameStarted = false;
        if(playerInt == 1 && photonView.IsMine)
        {
            yield return new WaitForSeconds(2f);
        }
        else
        {
            yield return new WaitForSeconds(1f);
        }
        
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel(0);

    }
    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine && playerInt == 2)
        {
            GameObject startButtonGame = GameObject.Find("Canvas/Lobby Text/Start");
            Destroy(startButtonGame);
        }
        if (gameStarted == true)
        {
            if (!view.IsMine)
            {
                if (playerInt == 2)
                {
                    GetComponent<SpriteRenderer>().color = Color.green;
                    beak.GetComponent<SpriteRenderer>().color = Color.green;
                }
                List<unit> units = World.theWorld.units;
                for (int i = 0; i < units.Count; i++)
                {
                    transform.position = new Vector2(position.x + 0.5f, position.y + 0.5f);
                    if (units[i].position == position)
                    {
                        //transform.position = units[i].gameObject.transform.position;
                        //whatIAmStandingOn = units[i];
                        //whatIAmStandingOnIndex = i;

                    }
                }
                if (beakDirection == new Vector2(1, 0))
                {
                    beak.rotation = Quaternion.Euler(0, 0, -90);
                    beak.localPosition = new Vector2(0.9f, 0);
                }
                else if (beakDirection == new Vector2(-1, 0))
                {
                    beak.rotation = Quaternion.Euler(0, 0, 90);
                    beak.localPosition = new Vector2(-0.9f, 0);
                    //beakDirection = new Vector2(-1, 0);
                }
                else if (beakDirection == new Vector2(0, 1))
                {
                    beak.rotation = Quaternion.Euler(0, 0, 0);
                    beak.localPosition = new Vector2(0.02f, 0.9f);
                    //beakDirection = new Vector2(0, 1)
                }
                else if (beakDirection == new Vector2(0, -1))
                {
                    beak.rotation = Quaternion.Euler(0, 0, 180);
                    beak.localPosition = new Vector2(0.02f, -0.9f);
                    //beakDirection = new Vector2(0, -1);
                    //beakDirection.x = 0;
                    return;
                }
                /*if (whatIAmStandingOn.unitType == unit.Type.Ground)
                {
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        replaceUnit(instance.whatIAmStandingOnIndex, 0, instance.position.x, instance.position.y, true);
                    }
                }*/
                return;
            }
            if (kjfsdl)
            {
                GameObject LobbyText = GameObject.Find("Canvas/Lobby Text").gameObject;
                LobbyText.SetActive(false);
                kjfsdl = false;
            }
            if (currentItem == 0)
            {
                bridgesText.text = "Bridges: " + bridgesLeft;
            }
            else if (currentItem == 1)
            {
                bridgesText.text = "Walls: " + wallsLeft;
            }
            else if(currentItem == 2)
            {
                bridgesText.text = "TNT: " + TNTLeft;
            }
            else if (currentItem == 3)
            {
                bridgesText.text = "Ground: " + GroundLeft;
            }
            else if (currentItem == 4)
            {
                bridgesText.text = "Drills: " + DrillsLeft;
            }

            bulletText.text = "Bullets: " + bulletsLeft;
            moneysText.text = "Moneys: " + moneys;

            if (collectCountdown >= 0)
            {
                collectCountdown -= 1 * Time.deltaTime;
            }
            else
            {
                if (photonView.IsMine)
                {
                    moneys += allGeneratorInstances.Count;
                    collectCountdown = startTime;
                }
            }
            collectText.text = "Collect Time: " + Mathf.RoundToInt(collectCountdown);
            if (!usingShop)
            {

                if (Input.GetKey(KeyCode.F))
                {
                    if (Input.GetKeyDown(KeyCode.D))
                    {
                        beak.rotation = Quaternion.Euler(0, 0, -90);
                        beak.localPosition = new Vector2(0.9f, 0);
                        beakDirection = new Vector2(1, 0);
                        return;
                    }
                    else if (Input.GetKeyDown(KeyCode.A))
                    {
                        beak.rotation = Quaternion.Euler(0, 0, 90);
                        beak.localPosition = new Vector2(-0.9f, 0);
                        beakDirection = new Vector2(-1, 0);
                        return;
                    }
                    else if (Input.GetKeyDown(KeyCode.W))
                    {
                        beak.rotation = Quaternion.Euler(0, 0, 0);
                        beak.localPosition = new Vector2(0.02f, 0.9f);
                        beakDirection = new Vector2(0, 1);
                        return;
                    }
                    else if (Input.GetKeyDown(KeyCode.S))
                    {
                        beak.rotation = Quaternion.Euler(0, 0, 180);
                        beak.localPosition = new Vector2(0.02f, -0.9f);
                        beakDirection = new Vector2(0, -1);
                        //beakDirection.x = 0;
                        return;
                    }
                }
                int direction = 0;
                List<unit> units = World.theWorld.units;
                if (Input.GetKeyDown(KeyCode.D))
                {
                    position.x += 1f;
                    direction = 2;
                }
                else if (Input.GetKeyDown(KeyCode.A))
                {
                    position.x -= 1f;
                    direction = 1;
                }
                else if (Input.GetKeyDown(KeyCode.W))
                {
                    position.y += 1f;
                    direction = -2;
                }
                else if (Input.GetKeyDown(KeyCode.S))
                {
                    position.y -= 1f;
                    direction = -1;
                }

                for (int i = 0; i < World.theWorld.units.Count; i++)
                {
                    if (!units[i].isMoveable && units[i].position == position)
                    {
                        if (direction == 1)
                        {
                            position.x += 1f;
                        }
                        else if (direction == 2)
                        {
                            position.x -= 1f;
                        }
                        else if (direction == -1)
                        {
                            position.y += 1f;
                        }
                        else if (direction == -2)
                        {
                            position.y -= 1f;
                        }
                        else if (direction == 0)
                        {
                            //you are standing on an immovable block
                            Debug.LogError("Somethins wrong mr");
                        }
                    }
                    else if (units[i].position == position)
                    {
                        /*if(units[i].transform.position != transform.position)
                        {*/
                        //gameObject.GetPhotonView().RPC("makeBlockImmovablePun", RpcTarget.All, position.x, position.y, true);
                        //}
                        player[] players = FindObjectsOfType<player>();

                        for (int a = 0; a < players.Length; a++)
                        {
                            if (position == players[a].position && playerInt != players[a].playerInt)
                            {
                                if (direction == 1)
                                {
                                    position.x += 1f;
                                }
                                else if (direction == 2)
                                {
                                    position.x -= 1f;
                                }
                                else if (direction == -1)
                                {
                                    position.y += 1f;
                                }
                                else if (direction == -2)
                                {
                                    position.y -= 1f;
                                }
                                /*else if (direction == 0)
                                {
                                    //you are standing on an immovable block
                                    Debug.LogError("Somethins wrong mr");
                                }*/
                            }
                        }
                        transform.position = units[i].gameObject.transform.position;
                        whatIAmStandingOn = units[i];
                        whatIAmStandingOnIndex = i;

                        //gameObject.GetPhotonView().RPC("makeBlockImmovablePun", RpcTarget.All, position.x, position.y, false);
                    }
                }

                if (whatIAmStandingOn.unitType == unit.Type.Ground|| whatIAmStandingOn.unitType == unit.Type.Generator)
                {
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        /*Destroy(whatIAmStandingOn.gameObject);
                        units.RemoveAt(whatIAmStandingOnIndex);
                        GameObject refrence = Instantiate(myGenerator, GameObject.Find("Grid/Tilemap").gameObject.transform);
                        refrence.transform.position = new Vector2(position.x + 0.5f, position.y + 0.5f);
                        refrence.GetComponent<unit>().SetPosition();
                        units.Add(refrence.GetComponent<unit>());
                        allGeneratorInstances.Add(refrence);
                        //whatIAmStandingOn = refrence.GetComponent<unit>();*/
                        gameObject.GetPhotonView().RPC("replaceUnit", RpcTarget.All, 0, position.x, position.y, true, playerInt);
                        for (int i = 0; i < World.theWorld.units.Count; i++)
                        {
                            if (units[i].position == position)
                            {
                                //transform.position = units[i].gameObject.transform.position;
                                whatIAmStandingOn = units[i];
                                whatIAmStandingOnIndex = i;

                            }
                        }
                    }
                }
                if (Input.GetKeyDown(KeyCode.R))
                {
                    unit targetUnit = World.theWorld.units[World.theWorld.FindUnitBasedOffPos(new Vector2(position.x + beakDirection.x, position.y + beakDirection.y))];
                    if (targetUnit.unitType == unit.Type.Lava && bridgesLeft != 0 && currentItem == 0)
                    {
                        //World.theWorld.gameObject.GetPhotonView().RPC("TakeDamage", RpcTarget.All, whatIAmStandingOn, whatIAmStandingOnIndex, bridgePrefab, position, false);
                        //unit refrence = World.theWorld.replaceUnit(whatIAmStandingOn, whatIAmStandingOnIndex, myGenerator, position);
                        /*Destroy(targetUnit.gameObject);
                        units.Remove(targetUnit);
                        GameObject refrence = Instantiate(bridgePrefab, GameObject.Find("Grid/Tilemap").gameObject.transform);
                        refrence.transform.position = new Vector2(transform.position.x + beakDirection.x, transform.position.y + beakDirection.y);
                        refrence.GetComponent<unit>().SetPosition();
                        units.Add(refrence.GetComponent<unit>());*/
                        gameObject.GetPhotonView().RPC("replaceUnit", RpcTarget.All, 1, position.x + beakDirection.x, position.y + beakDirection.y, false, playerInt);
                        bridgesLeft--;
                        //whatIAmStandingOn = refrence.GetComponent<unit>();
                    }
                    //finish
                    else if (targetUnit.unitType == unit.Type.Bridge && wallsLeft != 0 && currentItem == 1)
                    {
                        /*Destroy(targetUnit.gameObject);
                        units.Remove(targetUnit);
                        GameObject refrence = Instantiate(WallPrefab, GameObject.Find("Grid/Tilemap").gameObject.transform);
                        refrence.transform.position = new Vector2(transform.position.x + beakDirection.x, transform.position.y + beakDirection.y);
                        refrence.GetComponent<unit>().SetPosition();
                        units.Add(refrence.GetComponent<unit>());*/
                        gameObject.GetPhotonView().RPC("replaceUnit", RpcTarget.All, 2, position.x + beakDirection.x, position.y + beakDirection.y, false, playerInt);
                        wallsLeft--;
                        //whatIAmStandingOn = refrence.GetComponent<unit>();
                    }
                    else if(targetUnit.unitType == unit.Type.Bridge && currentItem == 2 && TNTArea == new Vector2(0.32f, 0.32f) && TNTLeft != 0)
                    {
                        gameObject.GetPhotonView().RPC("replaceUnit", RpcTarget.All, 3, position.x + beakDirection.x, position.y + beakDirection.y, false, playerInt);
                        TNTLeft--;
                        TNTArea = new Vector2(position.x + beakDirection.x, position.y + beakDirection.y);
                    }
                    else if(currentItem == 2 && TNTArea != new Vector2(0.32f, 0.32f))
                    {
                        unit targetUnit2 = World.theWorld.units[World.theWorld.FindUnitBasedOffPos(TNTArea)];

                        replaceUnitTNT(targetUnit2, TNTArea);
                        replaceUnitTNT(World.theWorld.units[World.theWorld.FindUnitBasedOffPos(new Vector2(TNTArea.x + 1, TNTArea.y))],
                            new Vector2(TNTArea.x + 1, TNTArea.y));
                        replaceUnitTNT(World.theWorld.units[World.theWorld.FindUnitBasedOffPos(new Vector2(TNTArea.x - 1, TNTArea.y))],
                            new Vector2(TNTArea.x - 1, TNTArea.y));
                        replaceUnitTNT(World.theWorld.units[World.theWorld.FindUnitBasedOffPos(new Vector2(TNTArea.x, TNTArea.y + 1))],//
                            new Vector2(TNTArea.x, TNTArea.y + 1));
                        replaceUnitTNT(World.theWorld.units[World.theWorld.FindUnitBasedOffPos(new Vector2(TNTArea.x, TNTArea.y - 1))],//
                            new Vector2(TNTArea.x, TNTArea.y - 1));
                        replaceUnitTNT(World.theWorld.units[World.theWorld.FindUnitBasedOffPos(new Vector2(TNTArea.x - 1, TNTArea.y - 1))],
                            new Vector2(TNTArea.x - 1, TNTArea.y - 1));
                        replaceUnitTNT(World.theWorld.units[World.theWorld.FindUnitBasedOffPos(new Vector2(TNTArea.x + 1, TNTArea.y - 1))],//
                            new Vector2(TNTArea.x + 1, TNTArea.y - 1));
                        replaceUnitTNT(World.theWorld.units[World.theWorld.FindUnitBasedOffPos(new Vector2(TNTArea.x - 1, TNTArea.y + 1))],//
                            new Vector2(TNTArea.x - 1, TNTArea.y + 1));
                        replaceUnitTNT(World.theWorld.units[World.theWorld.FindUnitBasedOffPos(new Vector2(TNTArea.x + 1, TNTArea.y + 1))],//
                            new Vector2(TNTArea.x + 1, TNTArea.y + 1));
                        replaceUnitTNT(World.theWorld.units[World.theWorld.FindUnitBasedOffPos(new Vector2(TNTArea.x + 2, TNTArea.y))],//
                            new Vector2(TNTArea.x + 2, TNTArea.y));
                        replaceUnitTNT(World.theWorld.units[World.theWorld.FindUnitBasedOffPos(new Vector2(TNTArea.x - 2, TNTArea.y))],//
                            new Vector2(TNTArea.x - 2, TNTArea.y));
                        replaceUnitTNT(World.theWorld.units[World.theWorld.FindUnitBasedOffPos(new Vector2(TNTArea.x, TNTArea.y - 2))],//
                            new Vector2(TNTArea.x, TNTArea.y - 2));
                        replaceUnitTNT(World.theWorld.units[World.theWorld.FindUnitBasedOffPos(new Vector2(TNTArea.x, TNTArea.y + 2))],//
                            new Vector2(TNTArea.x, TNTArea.y + 2));
                        player[] players = FindObjectsOfType<player>();
                        for (int i = 0; i < players.Length; i++)
                        {
                            if (players[i].position == new Vector2(TNTArea.x, TNTArea.y + 2) ||
                                players[i].position == new Vector2(TNTArea.x, TNTArea.y - 2) ||
                                players[i].position == new Vector2(TNTArea.x - 2, TNTArea.y) ||
                                players[i].position == new Vector2(TNTArea.x + 2, TNTArea.y) ||
                                players[i].position == new Vector2(TNTArea.x + 1, TNTArea.y + 1) ||
                                players[i].position == new Vector2(TNTArea.x - 1, TNTArea.y + 1) ||
                                players[i].position == new Vector2(TNTArea.x + 1, TNTArea.y - 1) ||
                                players[i].position == new Vector2(TNTArea.x - 1, TNTArea.y - 1) ||
                                players[i].position == new Vector2(TNTArea.x, TNTArea.y + 1) ||
                                players[i].position == new Vector2(TNTArea.x - 1, TNTArea.y) ||
                                players[i].position == new Vector2(TNTArea.x + 1, TNTArea.y))
                                {
                                    gameObject.GetPhotonView().RPC("resetLevel", RpcTarget.All, 0, players[i].playerInt);
                                }
                        }
                        
                        TNTArea = new Vector2(0.32f, 0.32f);
                    }
                    else if (targetUnit.unitType == unit.Type.Bridge && currentItem == 3 && GroundLeft != 0)
                    {
                        gameObject.GetPhotonView().RPC("replaceUnit", RpcTarget.All, 5, position.x + beakDirection.x, position.y + beakDirection.y, false, playerInt);
                        GroundLeft--;
                    }
                    else if (targetUnit.unitType == unit.Type.Bridge && currentItem == 4 && DrillsLeft != 0)
                    {
                        gameObject.GetPhotonView().RPC("replaceUnit", RpcTarget.All, 4, position.x + beakDirection.x, position.y + beakDirection.y, false, playerInt);
                        DrillsLeft--;
                    }
                }
                if (Input.GetKeyDown(KeyCode.E) && bulletsLeft != 0)
                {
                    unit targetUnit = World.theWorld.units[World.theWorld.FindUnitBasedOffPos(new Vector2(position.x + beakDirection.x, position.y + beakDirection.y))];
                    GameObject refrence = PhotonNetwork.Instantiate("Bullet", targetUnit.transform.position, targetUnit.transform.rotation);
                    if (beakDirection.x == 1)
                    {
                        refrence.GetComponent<Bullet>().direction = 1;
                    }
                    else if (beakDirection.x == -1)
                    {
                        refrence.GetComponent<Bullet>().direction = 2;
                    }
                    else if (beakDirection.y == 1)
                    {
                        refrence.GetComponent<Bullet>().direction = 3;
                    }
                    else if (beakDirection.y == -1)
                    {
                        refrence.GetComponent<Bullet>().direction = 4;
                    }
                    bulletsLeft--;

                }

            }
            bool justdid = false;
            if (World.theWorld.units[World.theWorld.FindUnitBasedOffPos(new Vector2(position.x + 1, position.y))].unitType == unit.Type.Shop && usingShop == false)
            {
                if (Input.GetKeyDown(KeyCode.C))
                {
                    //Debug.Log("You are talking to the shop");
                    shopText.SetActive(true);
                    usingShop = true;
                    justdid = true;
                }
            }

            if (usingShop == true && Input.GetKeyDown(KeyCode.C) && justdid == false)
            {
                usingShop = false;
                shopText.SetActive(false);
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                currentItem++;
                if (currentItem == 5)
                {
                    currentItem = 0;
                }
            }
        }
        else
        {
            if (photonView.IsMine)
            {
                Text playersCount = GameObject.Find("Canvas/Lobby Text/Player Count/Text").GetComponent<Text>();
                playersCount.text = "Players: " + PhotonNetwork.CurrentRoom.PlayerCount.ToString();
            }
        }
    }
    [PunRPC]
    private void makeBlockImmovablePun(float positionX, float positionY, bool which)
    {
        World.theWorld.makeBlockImmovable(new Vector2(positionX, positionY), which);
    }
    [PunRPC]
    public void resetLevel(int level, int playerInt)
    {
        player.instance.StartCoroutine(player.instance.kill(level, playerInt));
    }
    public void buyBridge()
    {
        if(moneys >= 5)
        {
            moneys -= 5;
            bridgesLeft += 5;
        }
    }
    public void buyDrill()
    {
        if (moneys >= 1)
        {
            moneys -= 1;
            DrillsLeft += 1;
        }
    }
    public void replaceUnitTNT(unit t_unit, Vector2 position)
    {
        if (t_unit.unitType == unit.Type.Bridge || t_unit.unitType == unit.Type.TNT)
        {
            gameObject.GetPhotonView().RPC("replaceUnit", RpcTarget.All, 4, position.x, position.y, false, playerInt);
        }
        else if(t_unit.unitType == unit.Type.Generator|| t_unit.unitType == unit.Type.Ground|| t_unit.unitType == unit.Type.Wall)
        {
            gameObject.GetPhotonView().RPC("replaceUnit", RpcTarget.All, 1, position.x, position.y, false, playerInt);
        }
    }
    public void buyWall()
    {
        if (moneys >= 2)
        {
            moneys -= 2;
            wallsLeft++;
        }
    }
    public void buyBullet()
    {
        if (moneys >= 15)
        {
            moneys -= 15;
            bulletsLeft++;
        }
    }
    public void buyTNT()
    {
        if (moneys >= 35)
        {
            moneys -= 35;
            TNTLeft++;
        }
    }
    public void buyGround()
    {
        if (moneys >= 50)
        {
            moneys -= 50;
            GroundLeft++;
        }
    }
    [PunRPC]
    private void setDirectionOfBullet(GameObject refrence)
    {
    }
    [PunRPC]
    private void replaceUnit(int replaceUnit, float positionX, float positionY, bool generator, int playerInt)
    {
        World.theWorld.replaceUnit(replaceUnit, new Vector2(positionX, positionY), generator, playerInt);
    }
}
