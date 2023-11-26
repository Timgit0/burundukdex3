using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBehaviour : MonoBehaviour
{
    public int[] distances = new int[3] { 100, 100, 100};
    public Transform player;

    public bool isWall;

    void Start()
    {
        player = this.transform.parent.parent.parent.Find("Player");
    }

    public void ChangeColor(Color color)
    {
        color.a = 0.3f;
        this.gameObject.GetComponent<Renderer>().material.color = color;
    }

    void LateUpdate()
    {
        if (isWall && this.transform.parent.Find("anim").gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("NormalTile"))
        {
            this.transform.parent.Find("anim").gameObject.GetComponent<Animator>().Play("LavaTile");
        }

        if (!isWall && this.transform.parent.Find("anim").gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("LavaTile"))
        {
            this.transform.parent.Find("anim").gameObject.GetComponent<Animator>().Play("NormalTile");
        }
    }

    public void Turn()
    {
        distances = new int[3] { 100, 100, 100};
        if (isWall) return;
        Vector3 positionFromPlayer = this.transform.position - player.position;
        if (positionFromPlayer.magnitude < 0.86*1+0.1)
        {
            distances[0] = 0;
        }
        else if ((abs(positionFromPlayer.y) < 0.1f || (abs(abs(positionFromPlayer.x/positionFromPlayer.y) - 0.43f/0.75f) < 0.1f)) && positionFromPlayer.magnitude < 0.86*5+0.1)
        {
            distances[1] = 0;
            foreach (Transform tile in this.transform.parent.parent)
            {
                Vector3 tilePositionFromPlayer = tile.position - player.position;
                if ((abs(tilePositionFromPlayer.x/tilePositionFromPlayer.y - positionFromPlayer.x/positionFromPlayer.y) < 0.1f || abs(tilePositionFromPlayer.y/tilePositionFromPlayer.x - positionFromPlayer.y/positionFromPlayer.x) < 0.1f) && (tilePositionFromPlayer + positionFromPlayer).magnitude > positionFromPlayer.magnitude && tilePositionFromPlayer.magnitude < positionFromPlayer.magnitude)
                {
                    foreach (Transform enemyOnIt in this.transform.parent.parent.parent.Find("Enemies"))
                    {
                        if (enemyOnIt.position == tile.position)
                        {
                            distances[1] = 100;
                        }
                    }
                }
            }
        }
        if (positionFromPlayer.magnitude < 0.86*4+0.1)
        {
            distances[2] = 0;
        }


        for (int i = 0; i < 3; i++)
        {
            if (distances[i] != 0)
            {
                foreach (Transform tilep in this.transform.parent.parent)
                {
                    Transform tile = tilep.Find("tile");
                    if ((tile.position - this.transform.position).magnitude < 0.86*1+0.1)
                    {
                        if (tile.gameObject.GetComponent<TileBehaviour>().distances[i] < distances[i])
                        {
                            distances[i] = tile.gameObject.GetComponent<TileBehaviour>().distances[i] + 1;
                        }
                    }
                }
            }
        }
    }

    float abs(float x)
    {
        if (x < 0f) return -x;
        return x;
    }
}
