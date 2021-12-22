using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestObject : MonoBehaviour
{
    public float turn_dampening;
    Vector3 mouseDir = new Vector3();
    int halfwidth = Screen.width / 2;
    int halfheight = Screen.height / 2;
    public Transform target;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        transform.LookAt(target);
        //transform.position += transform.forward*Input.GetAxis("Vertical");
        //mouseDir.x = (Input.mousePosition.x - halfwidth) * (1 / halfwidth);
        //mouseDir.y = (Input.mousePosition.y - halfheight) * (1 / halfheight);

        




    }
}
