using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*  game handles the menu item selection, which is used to control the currently selected unit
 * 
 */
public class Game : MonoBehaviour
{
    Cursor cursor;
    // Start is called before the first frame update
    void Start()
    {
        cursor = GetComponent<Cursor>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
