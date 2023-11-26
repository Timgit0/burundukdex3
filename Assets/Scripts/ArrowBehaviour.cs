using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowBehaviour : MonoBehaviour
{
    public Vector3 velocity;

    void Update()
    {
        this.transform.position += velocity * Time.deltaTime;
    }

    public void Stop()
    {
        StartCoroutine(Stopping());
    }

    IEnumerator Stopping()
    {
        yield return new WaitForSeconds(1f/3f);
        Destroy(this.gameObject);
    }
}
