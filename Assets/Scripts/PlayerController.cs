using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private float spd = 1;
    private Vector3 move = new Vector2();
    PhotonView view;
    private int playerNumber = 1;
    private GameObject score_obj;
    private int test_num = 0;
    void Start()
    {
        view = GetComponent<PhotonView>();
        playerNumber = PhotonNetwork.CurrentRoom.PlayerCount;
        if(playerNumber == 1)
        {
            score_obj = GameObject.Find("pl1_text");
        }
        if (playerNumber == 2)
        {
            score_obj = GameObject.Find("pl2_text");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (view.IsMine)
        {
            move.x = Input.GetAxis("Horizontal") * spd;
            move.z = Input.GetAxis("Vertical") * spd;
            transform.Translate(move);

            if (Input.GetKeyDown(KeyCode.X))
            {
                test_num++;
            }

            if(score_obj)
            {
                score_obj.GetComponent<Text>().text = $"Player {playerNumber}, test: {test_num}";
            }
        }
    }
}
