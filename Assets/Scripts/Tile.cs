using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    public bool current = false;
    public bool target = false;
    public bool selectable = false;
    public bool walkable = true;//strictly for tiles where movement is impossible
    public bool blocked = false;
    public bool attacking = false;
    public List<Tile> adjacencyList = new List<Tile>();
    public Vector3 groundPos;

    public int moveCost = 1;

    //BFS (breadth first search)
    public bool visited = false;//if this tile has been added to selectable tiles list
    public Tile parent = null;//where the adjacentcy search began
    public int distance = 0;//number of tiles traversed
    public Unit unit;//the unit occupying this tile

    Text myText;
    Renderer tile_renderer;
    // Start is called before the first frame update
    void Start()
    {
        groundPos = GetComponent<Transform>().position + Vector3.up * (GetComponent<Collider>().bounds.extents.y + 0.1f);
        tile_renderer = GetComponent<Renderer>();
        //text = transform.Find("text").GetComponent<TextMesh>();
        myText = transform.Find("Tile_grid/Canvas/Text").GetComponent<Text>();

        myText.text = $"{distance}";
    }

    // Update is called once per frame
    void Update()
    {
        if (current)
        {
            tile_renderer.material.color = Color.magenta;
        }
        else if (target)
        {
            tile_renderer.material.color = Color.green;
        }
        else if (selectable)
        {
            tile_renderer.material.color = Color.blue;
        }
        else if (attacking)
        {
            tile_renderer.material.color = Color.red;
        }
        else
        {
            tile_renderer.material.color = Color.white;
        }
        myText.text = $"{distance}";
    }

    public void Reset()
    {
        adjacencyList.Clear();

        current = false;
        target = false;
        selectable = false;
        
        visited = false;
        parent = null;
        distance = 0;

        attacking = false;
    }

    public void FindNeighbors(float jumpHeight)
    {
        Reset();
        CheckTile(Vector3.forward,jumpHeight);
        CheckTile(-Vector3.forward, jumpHeight);
        CheckTile(Vector3.right, jumpHeight);
        CheckTile(-Vector3.right, jumpHeight);
    }

    public void CheckTile(Vector3 direction, float jumpHeight)
    {
        
        Vector3 halfExtents = new Vector3(0.25f,(0.25f+jumpHeight)/2.0f,0.25f);
        Collider[] colliders = Physics.OverlapBox(transform.position + direction, halfExtents);
        foreach (Collider item in colliders)
        {
            Tile tile = item.GetComponent<Tile>();
            if(tile != null && tile.walkable && blocked == false)
            {
                adjacencyList.Add(tile);
            }
        }
    }

    public Unit GetUnit()
    {
        RaycastHit hit;
        Unit foundUnit = null;
        if (Physics.Raycast(transform.position, Vector3.up, out hit, 1))
        {
            foundUnit = hit.collider.GetComponent<Unit>();
        }
        Debug.Log($"Check Unit: {foundUnit}");
        return foundUnit;
    }

    
}
