using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour
{
    Vector3 pos = new Vector3();
    Transform tile;
    Tile tile_class;
    float scale_amt = 0;
    float sin_value = 0;
    Vector3 scale_vector;
    float original_scale;

    public Unit SelectedUnit;
    public Unit TargetedUnit;//the unit that is targeted by the selected unit
    // Start is called before the first frame update
    void Start()
    {
        scale_vector = transform.localScale;
        original_scale = scale_vector.y;
    }

    // Update is called once per frame
    void Update()
    {
        sin_value += 0.02f;
        if(sin_value > Mathf.PI*2)
        {
            sin_value = 0;
        }
        scale_amt = 0.02f+(Mathf.Sin(sin_value)*0.02f);
        transform.localScale = new Vector3(original_scale+scale_amt, original_scale + scale_amt, original_scale + scale_amt);
        
        if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
        {
            tile = GetTileAtMouse();
            if (tile)
            {
                tile_class = tile.GetComponent<Tile>();
                pos = tile_class.groundPos;
                transform.position = pos;
            }
        }

        if (Input.GetButtonDown("Fire1"))
        {
            Unit getUnit = tile_class.GetUnit();//check the currently selected tile class instance for an occupying unit
            if (getUnit)
            {
                if(getUnit == SelectedUnit)
                {
                    SelectedUnit.ChangeState(Unit.UnitStates.UnSelected);
                    Debug.Log("Found Unit Un-Selecting");
                    SelectedUnit = null;
                }
                else
                {
                    SelectedUnit = getUnit;
                    SelectedUnit.ChangeState(Unit.UnitStates.Selected);
                    Debug.Log("Found Unit Selecting");
                }
                
            }
            else
            {
                if (SelectedUnit)
                {
                    SelectedUnit.ChangeState(Unit.UnitStates.UnSelected);
                    Debug.Log("Found Unit Un-Selecting");
                    SelectedUnit = null;
                }
            }
        }
    } 

    public Transform GetTileAtMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.tag == "tile")
            {
                return hit.transform;
            }
        }
        return null;
    }
    
}
