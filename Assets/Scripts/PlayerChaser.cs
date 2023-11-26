using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChaser : MonoBehaviour
{
    private Transform player;
    public Vector3 positionFromPlayer;

    void Start()
    {
        player = this.transform.parent.Find("Player");
    }


    void Update()
    {
        this.transform.position = player.position + positionFromPlayer;
    }
}
