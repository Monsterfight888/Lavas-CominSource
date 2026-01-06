using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class World : MonoBehaviour
{
    public List<unit> units;
    public static World theWorld;
    private List<unit> orgWorld;
    private void Awake()
    {
        theWorld = this;
        units = FindObjectsOfType<unit>().ToList();
        orgWorld = units;
    }
    public int FindUnitBasedOffPos(Vector2 position)
    {
        for (int i = 0; i < units.Count; i++)
        {
            if (units[i].position == position)
            {
                return i;
            }
        }
        return 0;
    }
    public void makeBlockImmovable(Vector2 position, bool which)
    {
        units[FindUnitBasedOffPos(position)].isMoveable = which;
    }
    public void replaceUnit(int replaceUnit, Vector2 position, bool generator, int playerThatCallsIt)
    {
        GameObject refrence = null;
        if(units[FindUnitBasedOffPos(position)].unitType == unit.Type.unbreakable)
        {
            return;
        }
            player[] players = FindObjectsOfType<player>();
            for (int i = 0; i < players.Length; i++)
            {
                //if (players[i].playerInt == playerThatCallsIt)
                //{
                if (players[i].allGeneratorInstances.Contains(units[FindUnitBasedOffPos(position)].gameObject))
                {
                    players[i].allGeneratorInstances.Remove(units[FindUnitBasedOffPos(position)].gameObject);
                }
                    
                //}
            }
        Destroy(units[FindUnitBasedOffPos(position)].gameObject);
        units.RemoveAt(FindUnitBasedOffPos(position));
        if(replaceUnit == 0)
        {
            refrence = Instantiate(player.instance.myGenerator, GameObject.Find("Grid/Tilemap").gameObject.transform);
        }
        else if(replaceUnit == 1)
        {
            refrence = Instantiate(player.instance.bridgePrefab, GameObject.Find("Grid/Tilemap").gameObject.transform);
        }
        else if(replaceUnit == 2)
        {
            refrence = Instantiate(player.instance.WallPrefab, GameObject.Find("Grid/Tilemap").gameObject.transform);
        }
        else if(replaceUnit == 3)
        {
            refrence = Instantiate(player.instance.TNTPrefab, GameObject.Find("Grid/Tilemap").gameObject.transform);
        }
        else if(replaceUnit == 4)
        {
            refrence = Instantiate(player.instance.lavaPref, GameObject.Find("Grid/Tilemap").gameObject.transform);
        }
        else if (replaceUnit == 5)
        {
            refrence = Instantiate(player.instance.GroundPref, GameObject.Find("Grid/Tilemap").gameObject.transform);
        }

        refrence.transform.position = new Vector2(position.x + 0.5f, position.y + 0.5f);
        refrence.GetComponent<unit>().SetPosition();
        unit refrenceUnit = refrence.GetComponent<unit>();
        units.Add(refrenceUnit);
        //refrenceUnit.isMoveable = false;
        //refrenceUnit.position = new Vector2(0,0);
        if (generator)
        {
            if (playerThatCallsIt == 2)
            {
                refrence.GetComponent<SpriteRenderer>().color = new Color32(29, 94, 36, 255);
            }
            //player[] players = FindObjectsOfType<player>();
            for (int i = 0; i < players.Length; i++)
            {
                if(players[i].playerInt == playerThatCallsIt)
                {
                        players[i].allGeneratorInstances.Add(refrence);
                    
                    //int count = ;
                    List<GameObject> t_allGeneratorInstances = new List<GameObject>();
                    for (int l = 0; l < players[i].allGeneratorInstances.Count; l++)
                    {
                        t_allGeneratorInstances.Add(players[i].allGeneratorInstances[l]);
                    }
                    
                    players[i].allGeneratorInstances.Clear();
                    for (int a = 0; a < t_allGeneratorInstances.Count; a++)
                    {
                        if (t_allGeneratorInstances[a].gameObject != null)
                        {
                            players[i].allGeneratorInstances.Add(t_allGeneratorInstances[a].gameObject);
                            //a--;
                        }
                    }
                }
            }

            //players[playerThatCallsIt - 1].
        }
    }
    void Start()
    {
        Application.targetFrameRate = 60;
        /*unit[] stuff;
        stuff.ToList<>*/
        
        /*float[] x = {0};
        float[] y = {0};
        for (int i = 0; i < units.Count; i++)
        {
            x[i] = units[i].position.x;
            y[i] = units[i].position.y;
        }
        for (int i = 0; i < units.Count; i++)
        {
            for (int a = 0; a < units.Count; a++)
            {
                if (x[i] == x[a] && y[i] == x[a] && x[i] == y[a] && y[i] == x[a])
                {
                    if(units.ElementAt(i).unitType == unit.type.Ground && units.ElementAt(a).unitType == unit.type.Lava)
                    {
                        Destroy(units.ElementAt(a).gameObject);
                        units.RemoveAt(a);
                    }
                }
            }
            
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
