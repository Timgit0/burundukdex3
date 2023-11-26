using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombBehaviour : MonoBehaviour
{
    public Vector3 velocity;
    public string myName;
    public float explodeRange;
    public Transform player;

    void Update()
    {
        this.transform.position += velocity * Time.deltaTime;
    }

    public void Stop()
    {
        StartCoroutine(Stopping());
    }

    public void Boom()
    {
        StartCoroutine(Kaboom());
    }

    IEnumerator Stopping()
    {
        yield return new WaitForSeconds(1f/3f);
        velocity = new Vector3(0f, 0f, 0f);
    }

    IEnumerator Kaboom()
    {
        this.transform.Find("Anim").GetComponent<Animator>().Play(myName + " boom");
        yield return new WaitForSeconds(1f/3f);
        foreach (Transform enemy in this.transform.parent.parent.Find("Enemies"))
        {
            if ((enemy.position - this.transform.position).magnitude < 0.86f*explodeRange+0.1f) enemy.gameObject.GetComponent<EnemyBehaviour>().Die();
        }
        
        {
            if ((player.position - this.transform.position).magnitude < 0.86f*explodeRange+0.1f) player.gameObject.GetComponent<PlayerBehaviour>().hp -= 1;
        }
        Destroy(this.transform.gameObject);
    }
}
