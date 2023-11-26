using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UpgradeLib;

public class ExitBehaviour : MonoBehaviour
{
    Transform player;
    public int enemies;
    List<GameObject> tiles = new List<GameObject>();
    public GameObject enemyPrefab;

    public GameObject powerUp;
    public List<Upgrade> upgrades = new List<Upgrade>();
    public List<ScriptedAltar> scriptedAltars = new List<ScriptedAltar>();

    int maxType = 3;

    void Start()
    {
        player = this.transform.parent.Find("Player");
        enemies = 0;
        foreach (Transform tile in this.transform.parent.Find("Tiles"))
        {
            tiles.Add(tile.Find("tile").gameObject);
        }
    }


    void Update()
    {
        if (player.position == this.transform.position)
        {
            int j = 0;
            print("Here we go");


            player.GetComponent<PlayerBehaviour>().StopAllCoroutines();
            player.GetComponent<PlayerBehaviour>().yourMove = true;


            player.transform.position = new Vector3(0f, -3f, 0f);
            print("Player done");



            foreach (Transform obj in this.transform.parent.Find("Objects"))
            {
                Destroy(obj.gameObject);
            }
            print("Objects done");


            enemies += 1;
            foreach (Transform enemy in this.transform.parent.Find("Enemies"))
            {
                Destroy(enemy.gameObject);
            }
            for (int i = 0; i < enemies; i++)
            {
                do
                {
                    j = Random.Range(0, tiles.Count);
                } while (tiles[j].transform.position == player.position);
                int type = Random.Range(0, maxType);
                GameObject chosen = null;
                foreach (Transform enemy in this.transform.parent.Find("Enemies"))
                {
                    if (enemy.position == tiles[j].transform.position) chosen = enemy.gameObject;
                }
                if (chosen == null)
                {
                    chosen = Instantiate(enemyPrefab, tiles[j].transform.position, Quaternion.identity, this.transform.parent.Find("Enemies"));
                }
                if (!chosen.GetComponent<EnemyBehaviour>().types.Contains(type))
                {
                    chosen.GetComponent<EnemyBehaviour>().types.Add(type);
                }
                chosen.GetComponent<EnemyBehaviour>().ClassChoosing();
            }
            print("Enemies done");

            foreach (GameObject tile in tiles)
            {
                tile.GetComponent<TileBehaviour>().isWall = false;
            }

            for (int i = 0; i < enemies/2; i++)
            {
                bool enemyOn = false;
                j = Random.Range(0, tiles.Count);                
                foreach (Transform enemy in this.transform.parent.Find("Enemies"))
                {
                    if (enemy.position == tiles[j].transform.position) enemyOn = true;
                }
                if (enemyOn || tiles[j].transform.position == player.position || tiles[j].transform.position == this.transform.position) continue;
                tiles[j].GetComponent<TileBehaviour>().isWall = true;
            }
            print("Walls done");


            int l;
            do
            {
                l = Random.Range(0, tiles.Count);
            } while (player.position == tiles[l].transform.position || this.transform.position == tiles[l].transform.position || tiles[l].GetComponent<TileBehaviour>().isWall);

            powerUp.transform.position = tiles[l].transform.position;

            PowerUpBehaviour altarScript = powerUp.transform.gameObject.GetComponent<PowerUpBehaviour>();
            bool altarIsScripted = false;

            altarScript.upgrades = new Upgrade[4];

            foreach (ScriptedAltar scriptedAltar in scriptedAltars)
            {
                if (scriptedAltar.appearsWhen == enemies)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        altarScript.upgrades[i] = scriptedAltar.upgrades[i];
                    }
                    scriptedAltars.Remove(scriptedAltar);
                    altarIsScripted = true;
                }
            }
            if (!altarIsScripted)
            {
                altarScript.upgrades[0] = Upgrade.HealthAndAmmo;
                for (int k = 1; k < altarScript.upgrades.Length; k++)
                {
                    if (upgrades.Count > 0)
                    {
                        altarScript.upgrades[k] = upgrades[Random.Range(0, upgrades.Count)];
                        upgrades.Remove(altarScript.upgrades[k]);
                    }
                    else altarScript.upgrades[k] = Upgrade.HealthAndAmmo;
                }
            }
            

            print("Altar done");
        }
    }
}
