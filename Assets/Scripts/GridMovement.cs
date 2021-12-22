using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMovement : MonoBehaviour
{
    List<Tile> selectableTiles = new List<Tile>();
    GameObject[] tiles;

    Stack<Tile> path = new Stack<Tile>();//the tile path, pushed on in reverse order than pulled off as we go
    Tile currentTile;//

    public bool moving = false;

    public int move = 5;
    public float jumpHeight = 2;
    public float moveSpeed = 2;

    //used in move()
    Vector3 velocity = new Vector3();
    Vector3 heading = new Vector3();

    float halfHeight = 0;//this is where the unit sits on the tile
    protected void Init()
    {
        tiles = GameObject.FindGameObjectsWithTag("tile");
        halfHeight = GetComponent<Collider>().bounds.extents.y;
    }

    public void GetCurrentTile()
    {
        currentTile = GetTargetTile(gameObject);//gets the tile underneath this object
        currentTile.current = true;
    }

    public Tile GetTargetTile(GameObject target)
    {
        RaycastHit hit;
        Tile tile = null;
        if(Physics.Raycast(target.transform.position, -Vector3.up, out hit, 1))
        {
            tile = hit.collider.GetComponent<Tile>();
        }

        return tile;
    }

    public void ComputeAdjacencyLists()
    {
        //if the map changes the number of tiles
        tiles = GameObject.FindGameObjectsWithTag("tile");

        foreach (GameObject tile in tiles)
        {
            Tile t = tile.GetComponent<Tile>();
            t.FindNeighbors(jumpHeight);
        }
    }

    public void FindSelectableTiles()
    {
        ComputeAdjacencyLists();
        GetCurrentTile();
        Queue<Tile> process = new Queue<Tile>();

        process.Enqueue(currentTile);
        currentTile.visited = true;
        //currentTile.parent = ?? leave as null
        while(process.Count > 0)
        {
            Tile t = process.Dequeue();

            selectableTiles.Add(t);
            t.selectable = true;

            //stop searching when you have spread out beyond your range
            if(t.distance+t.moveCost < move)
            {
                foreach(Tile tile in t.adjacencyList)
                {
                    if (!tile.visited)
                    {
                        tile.parent = t;//the adjacent tile's parent is the current proccessed tile
                        tile.visited = true;//don't revist tile
                        tile.distance = t.distance + t.moveCost;//as the search spreads out, each new ring has a greater distance from the player
                        process.Enqueue(tile);
                    }
                }
            }
        }
    }

    public void MoveToTile(Tile tile)
    {
        path.Clear();
        tile.target = true;
        moving = true;

        Tile next = tile;
        while(next != null)
        {
            path.Push(next);
            next = next.parent;//parent was set in find selectable, the parent chain leads back to the player's tile
        }
    }

    public void Move()
    {
        if(path.Count > 0)
        {
            Tile t = path.Peek();
            Vector3 target = t.transform.position;

            target.y += halfHeight + t.GetComponent<Collider>().bounds.extents.y;//target must sit on top of the tile

            if(Vector3.Distance(transform.position,target) >= 0.05f)
            {
                CalculateHeading(target);//sets the heading variable to a vector representing the direction to the target
                SetHorizontalVelocity();//get velocity vector in forward direction
                transform.forward = heading;//rotate to face direction
                transform.position += velocity * Time.deltaTime;
            }
            else
            {
                //tile center reached
                transform.position = target;
                path.Pop();//top tile on stack removed, next tile down becomes target
            }
        }
        else
        {
            RemoveSelectableTiles();
            moving = false;
        }
    }

    void CalculateHeading(Vector3 target)
    {
        //get different between 2 3d points and then normalize to values betweeen 0 and 1
        heading = target - transform.position;
        heading.Normalize();
    }

    void SetHorizontalVelocity()
    {
        //get velocity vector in forward direction
        velocity = heading * moveSpeed;
    }

    protected void RemoveSelectableTiles()
    {
        if(currentTile != null)
        {
            currentTile.current = false;
            currentTile = null;
        }

        foreach(Tile tile in selectableTiles)
        {
            tile.Reset();//set temp variables back to default
        }

        selectableTiles.Clear();
    }
}
