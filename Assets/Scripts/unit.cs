using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class unit : MonoBehaviour
{
    public Vector2 position;
    public bool isMoveable = true;
    public bool isDestroyedByBullet;
    public enum Type
    {
        Ground,
        Lava,
        Generator,
        Shop,
        Bridge,
        Wall,
        TNT,
        unbreakable
    }
    public Type unitType;
    void Awake()
    {
        position = new Vector2(transform.position.x - 0.5f, transform.position.y - 0.5f);
        //position = new Vector2(position.x - 1, position.y - 1);
    }
    public void SetPosition()
    {
        position = new Vector2(transform.position.x - 0.5f, transform.position.y - 0.5f);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
