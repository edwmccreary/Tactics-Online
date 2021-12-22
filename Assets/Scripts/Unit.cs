using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    int UserId { get; set; }
    //public User UnitOwner { get; set; }
    public int UnitId { get; set; }
    public string Name { get; set; }
    int HitPoints { get; set; } = 100;
    int Strength = 10;
    int Defense = 5;
    int Damage = 0;

    public int AttackRange = 1;
    public int Itemrange = 1;
    public int MoveRange = 4;
    public float VerticalRange = 2;

    GameObject[] tiles;
    Tile currentTile;
    List<Tile> selectableTiles = new List<Tile>();
    PlayerMovement playerMovement;

    public enum UnitStates
    {
        UnSelected,
        Selected,
        Moving,
        Targeting,
        Attacking,
        Used
    }
    public UnitStates UnitState = UnitStates.UnSelected;

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    public void Attack(Unit other)
    {
        Damage = Strength;//calculate damage here
        other.TakeDamage(Damage);
    }

    public void TakeDamage(int dmg)
    {
        dmg -= Defense;//adjust damage based on stats here
        HitPoints -= dmg;
    }
    // Start is called before the first frame update
    

    // Update is called once per frame
    void Update()
    {
        switch (UnitState)
        {
            case UnitStates.UnSelected:
                
                break;
            case UnitStates.Selected:
                playerMovement.CheckMouse();
                break;
            case UnitStates.Moving:
                if (playerMovement.moving)
                {
                    playerMovement.Move();
                }
                else
                {
                    ChangeState(UnitStates.Selected);
                }
                break;
            case UnitStates.Targeting:
                
                break;
            case UnitStates.Used:
                
                break;
        }
    }


    public void FindAttackTiles()
    {
        ComputeAdjacencyLists();
        GetCurrentTile();
        Queue<Tile> process = new Queue<Tile>();

        process.Enqueue(currentTile);
        currentTile.visited = true;
        //currentTile.parent = ?? leave as null
        while (process.Count > 0)
        {
            Tile t = process.Dequeue();

            selectableTiles.Add(t);
            t.attacking = true;

            //stop searching when you have spread out beyond your range
            if (t.distance < AttackRange)
            {
                foreach (Tile tile in t.adjacencyList)
                {
                    if (!tile.visited)
                    {
                        tile.parent = t;//the adjacent tile's parent is the current proccessed tile
                        tile.visited = true;//don't revist tile
                        tile.distance = t.distance + 1;//as the search spreads out, each new ring has a greater distance from the player
                        process.Enqueue(tile);
                    }
                }
            }
        }
    }
    public void GetCurrentTile()
    {
        currentTile = GetTargetTile(gameObject);//gets the tile underneath this object
        currentTile.current = true;
    }
    public void ComputeAdjacencyLists()
    {
        //if the map changes the number of tiles
        tiles = GameObject.FindGameObjectsWithTag("tile");

        foreach (GameObject tile in tiles)
        {
            Tile t = tile.GetComponent<Tile>();
            t.FindNeighbors(VerticalRange);
        }
    }

    public Tile GetTargetTile(GameObject target)
    {
        RaycastHit hit;
        Tile tile = null;
        if (Physics.Raycast(target.transform.position, -Vector3.up, out hit, 1))
        {
            tile = hit.collider.GetComponent<Tile>();
        }

        return tile;
    }

    public void ChangeState(UnitStates state)
    {
        UnitState = state;
        switch (state)
        {
            case UnitStates.UnSelected :
                Highlight(false);
                break;
            case UnitStates.Selected:
                Highlight(true);
                FindAttackTiles();
                playerMovement.FindSelectableTiles();
                break;
            case UnitStates.Moving:
                Highlight(false);
                break;
            case UnitStates.Targeting:
                Highlight(false);
                break;
            case UnitStates.Used:
                Highlight(false);
                break;
        }
    }

    public void Highlight(bool apply)
    {
        if (apply)
        {
            GetComponent<Renderer>().material.color = Color.blue;
        }
        else
        {
            GetComponent<Renderer>().material.color = Color.white;
        }
    }
}
