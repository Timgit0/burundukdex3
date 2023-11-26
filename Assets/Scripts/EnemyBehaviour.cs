using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public List<int> types = new List<int>();
    public List<int> cooldowns = new List<int>();
    public List<int> currentCooldowns = new List<int>();

    public GameObject currentTile;
    public List<GameObject> tilesNear = new List<GameObject>();
    public Vector3 target;
    Transform player;
    int currentDistance;
    public GameObject arrow;
    public GameObject bomb;
    string myName;

    bool isDead;
    void Start()
    {
        player = this.transform.parent.parent.Find("Player");
        cooldowns = new List<int>() {0, 0, 3};
        currentCooldowns = new List<int>() {0, 0, 0};
    }

    public void ClassChoosing()
    {
        if (types.Contains(0)) myName = "Sword";
        if (types.Contains(1)) myName = "Archer";
        if (types.Contains(2)) myName = "Bomber";
        if (types.Contains(0) && types.Contains(1)) myName = "Hunter";
        if (types.Contains(1) && types.Contains(2)) myName = "Ranger";
        if (types.Contains(0) && types.Contains(2)) myName = "Barbarian";
        if (types.Contains(0) && types.Contains(1) && types.Contains(2)) myName = "Ninja";
        this.transform.Find("Anim").GetComponent<Animator>().Play(myName + " idle");
    }

    public void Die()
    {
        isDead = true;
        Destroy(this.gameObject);
    }

    public string Turn()
    {
        for (int i = 0; i < cooldowns.Count; i++)
        {
            if (currentCooldowns[i] > 0) currentCooldowns[i] -= 1;
        }

        string where;

        if (this.transform.position.y - player.position.y >= 0.5) where = " dr";
        else if (this.transform.position.y - player.position.y <= -0.5) where = " ur";
        else where = " r";

        string attackType = "move";
        if (player.transform.position.x < this.transform.position.x) this.transform.localScale = new Vector3(-1, 1, 1);
        else this.transform.localScale = new Vector3(1, 1, 1);
        if (isDead) return attackType;
        ChooseTarget();
        bool canGo = true;
        if (currentDistance == 0)
        {
            if (types.Contains(0) && currentTile.GetComponent<TileBehaviour>().distances[0] == 0)
            {
                player.gameObject.GetComponent<PlayerBehaviour>().hp -= 1;
                attackType = " sword";
                currentCooldowns[0] = cooldowns[0];
            }
            if (types.Contains(1) && currentTile.GetComponent<TileBehaviour>().distances[1] == 0)
            {
                player.gameObject.GetComponent<PlayerBehaviour>().hp -= 1;
                attackType = " bow";
                currentCooldowns[1] = cooldowns[1];

                //GameObject tmpArrow = Instantiate(arrow, this.transform.position, Quaternion.LookRotation(player.position - this.transform.position), this.transform.parent.parent.Find("Objects"));
                GameObject tmpArrow = Instantiate(arrow, this.transform.position, Quaternion.identity, this.transform.parent.parent.Find("Objects"));
                float angle = 0f;
                if ((player.position.y - this.transform.position.y) < -0.1f) angle -= 60;
                if ((player.position.y - this.transform.position.y) > 0.1f) angle += 60;
                if ((player.position.x - this.transform.position.x) < 0f) angle = 180f - angle;
                tmpArrow.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                tmpArrow.transform.GetComponent<ArrowBehaviour>().velocity = (player.position - this.transform.position)*3f;
                tmpArrow.transform.Find("Anim").GetComponent<Animator>().Play(myName + " arrow");
                tmpArrow.transform.GetComponent<ArrowBehaviour>().Stop();
            }
            
            if (attackType == "move")
            {
                if (currentTile.GetComponent<TileBehaviour>().distances[2] == 0 && types.Contains(2))
                {
                    List<Transform> possibilities = new List<Transform>();
                    foreach (Transform tileNearP in this.transform.parent.parent.Find("Tiles"))
                    {
                        if ((tileNearP.position - player.position).magnitude < 0.86f+0.1f && !(tileNearP.position == player.position) && !tileNearP.Find("tile").GetComponent<TileBehaviour>().isWall)
                        {
                            bool canBomb = true;
                            foreach (Transform enemy in this.transform.parent)
                            {
                                if ((enemy.position - tileNearP.position).magnitude < 0.1) canBomb = false;
                            }
                            foreach (Transform obj in this.transform.parent.parent.Find("Objects"))
                            {
                                if ((obj.position - tileNearP.position).magnitude < 0.1) canBomb = false;
                            }
                            if ((tileNearP.position - this.transform.position).magnitude > 0.86*3+0.1 || (tileNearP.position - this.transform.position).magnitude < 0.86*1+0.1) canBomb = false;
                            if (canBomb) possibilities.Add(tileNearP);
                        }
                    }
                    if (possibilities.Count != 0)
                    {
                        Transform positionToBomb = possibilities[Random.Range(0, possibilities.Count)];
                        
                        GameObject tmpBomb = Instantiate(bomb, this.transform.position, Quaternion.identity, this.transform.parent.parent.Find("Objects"));
                        tmpBomb.transform.GetComponent<BombBehaviour>().myName = myName + " bomb";
                        tmpBomb.transform.GetComponent<BombBehaviour>().player = player;
                        tmpBomb.transform.GetComponent<BombBehaviour>().explodeRange = 1f;
                        tmpBomb.transform.Find("Anim").GetComponent<Animator>().Play(tmpBomb.GetComponent<BombBehaviour>().myName + " fly");
                        tmpBomb.transform.GetComponent<BombBehaviour>().velocity = (positionToBomb.position - this.transform.position)*3f;
                        tmpBomb.transform.GetComponent<BombBehaviour>().Stop();

                        if (this.transform.position.y - positionToBomb.position.y >= 1) where = " dr";
                        else if (this.transform.position.y - positionToBomb.position.y <= -1) where = " ur";
                        else where = " r";
                        attackType = " bomb";
                        currentCooldowns[2] = cooldowns[2];
                    }
                }
                
            }
            canGo = false;
            if (types.Count == 1) attackType = "";
            this.transform.Find("Anim").GetComponent<Animator>().Play(myName + attackType + where);
            StartCoroutine(turnIdleAfter(myName));
        }
        foreach (Transform enemy in this.transform.parent)
        {
            if (enemy.position == target) canGo = false;
        }
        foreach (Transform obj in this.transform.parent.parent.Find("Objects"))
        {
            if ((obj.position - target).magnitude < 0.1) canGo = false;
        }
        if (player.position == target) canGo = false;
        if (canGo) this.transform.position = target;
        return attackType;
    }

    void ChooseTarget()
    {
        tilesNear = new List<GameObject>();
        foreach (Transform tile in this.transform.parent.parent.Find("Tiles"))
        {
            if ((tile.position - this.transform.position).magnitude < 0.1f)
            {
                currentTile = tile.Find("tile").gameObject;
            }
            else if ((tile.position - this.transform.position).magnitude < 0.86f*1+0.1f)
            {
                tilesNear.Add(tile.Find("tile").gameObject);
            }
        }
        currentDistance = 100;
        foreach (int type in types)
        {
            if ((currentTile.GetComponent<TileBehaviour>().distances[type] < currentDistance) && (currentCooldowns[type] == 0))
            {
                currentDistance = currentTile.GetComponent<TileBehaviour>().distances[type];
            }
        }
        foreach (GameObject tile in tilesNear)
        {
            foreach (int type in types)
            {
                if ((tile.GetComponent<TileBehaviour>().distances[type] < currentDistance) && (currentCooldowns[type] == 0))
                {
                    target = tile.transform.position;
                    return;
                }
            }
        }
        foreach (GameObject tile in tilesNear)
        {
            if ((tile.GetComponent<TileBehaviour>().distances[1] < currentDistance))
            {
                target = tile.transform.position;
                return;
            }
        }
        target = this.transform.position;
    }

    IEnumerator turnIdleAfter(string animName)
    {
        animName += " idle";
        yield return new WaitForSeconds(1f/3f);
        this.transform.Find("Anim").GetComponent<Animator>().Play(animName);
    }
}
