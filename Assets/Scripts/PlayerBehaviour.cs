using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UpgradeLib;

public class PlayerBehaviour : MonoBehaviour
{
    private Vector3 speed;
    public GameObject target;
    private string _mode;
    public string Mode 
    {
        get => _mode;
        set
        {
            if (value == "move" || value == "fire" || value == "strike" || value == "throw") _mode = value;
        }
    }

    public float explodeRange = 1f;
    public float minGrenadeRange = 1f;
    public float maxGrenadeRange = 2f;
    public float fireRange = 2f;

    public int rounds = 10;
    public int grenades = 3;

    public GameObject bomb;

    public TMP_Text roundsText;

    public int hp;
    public TMP_Text hpText;

    public bool yourMove;

    public List<Upgrade> upgrades = new List<Upgrade>();

    public BombBehaviour rethrownGrenade;

    void Start()
    {
        rethrownGrenade = null;
        hp = 5;
        Mode = "move";
        yourMove = true;
    }

    void Update()
    {
        foreach (Transform tile in this.transform.parent.Find("Tiles"))
        {
            if (Check(tile.Find("tile").gameObject)) 
            {
                tile.Find("tile").gameObject.GetComponent<TileBehaviour>().ChangeColor(Color.green);
                foreach (Transform enemy in this.transform.parent.Find("Enemies"))
                {
                    if (enemy.position == tile.transform.position) tile.Find("tile").gameObject.GetComponent<TileBehaviour>().ChangeColor((4*Color.green+Color.red)/5);
                }
            }
            else tile.Find("tile").gameObject.GetComponent<TileBehaviour>().ChangeColor((Color.gray+Color.red)/2);
        }
    }

    void LateUpdate()
    {
        roundsText.text = "rounds: " + rounds.ToString() + '\n' + "frag grenades: " + grenades.ToString();
        hpText.text = "HP: " + hp.ToString();

        this.transform.position += speed;
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition)) != null)
            {
                target = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition)).gameObject;
                if (Check(target)) Do(target);
            }
        }
        EnemyIntends();
    }

    bool Check(GameObject tile)
    {
        if (!yourMove) return false;
        float distance = (this.transform.position - tile.transform.position).magnitude;

        switch (Mode)
        {
            case "move":
            {
                if (tile.GetComponent<TileBehaviour>().isWall) return false;
                foreach (Transform obj in this.transform.parent.Find("Enemies"))
                {
                    if (obj.position == tile.transform.position) return false;
                }
                foreach (Transform obj in this.transform.parent.Find("Objects"))
                {
                    if ((obj.position - tile.transform.position).magnitude < 0.1) return false;
                }
                if (abs(distance - 0.86f*1) > 0.1f) return false;
                return true;
            }
            case "fire":
            {
                if (distance > 0.86f*fireRange - 0.1f || distance < 0.1f) return false;
                return true;
            }
            case "strike":
            {
                if (abs(distance - 0.86f*1) > 0.1f) return false;
                foreach (Transform enemy in this.transform.parent.Find("Enemies"))
                {
                    if (((enemy.position - tile.transform.position).magnitude < 0.1f) || 
                    (upgrades.Contains(Upgrade.StrikeSwing) && (enemy.position - tile.transform.position).magnitude < 0.86f + 0.1f && (enemy.position - this.transform.position).magnitude < 0.86f + 0.1f) ||
                    (upgrades.Contains(Upgrade.StrikeSwing1) && (enemy.position - tile.transform.position).magnitude < 0.86f*2f - 0.1f && (enemy.position - this.transform.position).magnitude < 0.86f + 0.1f) ||
                    (upgrades.Contains(Upgrade.StrikeBack) && (enemy.position + tile.transform.position - 2*this.transform.position).magnitude < 0.1f) ||
                    (upgrades.Contains(Upgrade.StrikeLunge) && (enemy.position + this.transform.position - 2*tile.transform.position).magnitude < 0.1f))
                    return true;
                }
                return false;
            }
            case "throw":
            {
                if (upgrades.Contains(Upgrade.GrenadeRethrow) && (distance < 0.86 + 0.1) && rethrownGrenade == null)
                {
                    foreach (Transform obj in this.transform.parent.Find("Objects"))
                    {
                        if ((obj.position - tile.transform.position).magnitude < 0.1 && obj.GetComponent<BombBehaviour>() != null)
                        {
                            rethrownGrenade = obj.GetComponent<BombBehaviour>();
                            return true;
                        }
                    }
                }
                if (distance > 0.86f*maxGrenadeRange - 0.1f || distance < 0.86f*minGrenadeRange + 0.1f) return false;
                return true;
            }

            default: return false;
        }
    }

    void Do(GameObject target)
    {
        string prefix;
        if (target.transform.position.y - this.transform.position.y >= 0.5)
        {
            prefix = "hero ur";
        }
        else if (target.transform.position.y - this.transform.position.y <= -0.5)
        {
            prefix = "hero dr";
        }
        else
        {
            prefix = "hero r";
        }
        if (target.transform.position.x - this.transform.position.x > 0)
        {
            this.transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            this.transform.localScale = new Vector3(-1, 1, 1);
        }

        switch (Mode)
        {
            case "move":
            {
                //StartCoroutine(Move);
                this.transform.position = target.transform.position;

                break;
            }
            case "fire":
            {
                if (rounds > 0)
                {
                    rounds -= 1;
                    
                    this.transform.GetComponent<Animator>().Play(prefix);
                    StartCoroutine(turnIdleAfter());
                }
                else return;
                foreach (Transform enemy in this.transform.parent.Find("Enemies"))
                {
                    if (enemy.position == target.transform.position) 
                    enemy.gameObject.GetComponent<EnemyBehaviour>().Die();
                }

                break;
            }
            case "strike":
            {
                prefix += " spade";
                this.transform.GetComponent<Animator>().Play(prefix);
                StartCoroutine(turnIdleAfter());
                foreach (Transform enemy in this.transform.parent.Find("Enemies"))
                {
                    if (((enemy.position - target.transform.position).magnitude < 0.1f) || 
                    (upgrades.Contains(Upgrade.StrikeSwing) && (enemy.position - target.transform.position).magnitude < 0.86f + 0.1f && (enemy.position - this.transform.position).magnitude < 0.86f + 0.1f) ||
                    (upgrades.Contains(Upgrade.StrikeSwing1) && (enemy.position - target.transform.position).magnitude < 0.86f*2f - 0.1f && (enemy.position - this.transform.position).magnitude < 0.86f + 0.1f) ||
                    (upgrades.Contains(Upgrade.StrikeBack) && (enemy.position + target.transform.position - 2*this.transform.position).magnitude < 0.1f) ||
                    (upgrades.Contains(Upgrade.StrikeLunge) && (enemy.position + this.transform.position - 2*target.transform.position).magnitude < 0.1f))
                    enemy.gameObject.GetComponent<EnemyBehaviour>().Die();
                }

                break;
            }
            case "throw":
            {
                if (rethrownGrenade != null && rethrownGrenade.transform.position == target.transform.position)
                {
                    rethrownGrenade.transform.position = this.transform.position;
                    return;
                }
                if (grenades > 0 && rethrownGrenade == null) grenades -= 1;
                else if(rethrownGrenade != null)
                {
                    rethrownGrenade.transform.position = this.transform.position;
                    rethrownGrenade.transform.Find("Anim").GetComponent<Animator>().Play(rethrownGrenade.myName + " fly");
                    rethrownGrenade.velocity = (target.transform.position - this.transform.position)*3f;
                    rethrownGrenade.Stop();
                    rethrownGrenade = null;
                    break;
                }
                else return;
                prefix += " bomb";
                this.transform.GetComponent<Animator>().Play(prefix);
                StartCoroutine(turnIdleAfter());

                GameObject tmpBomb = Instantiate(bomb, this.transform.position, Quaternion.identity, this.transform.parent.Find("Objects"));
                tmpBomb.transform.GetComponent<BombBehaviour>().myName = "Hero bomb";
                tmpBomb.transform.GetComponent<BombBehaviour>().player = this.transform;
                tmpBomb.transform.GetComponent<BombBehaviour>().explodeRange = explodeRange;
                tmpBomb.transform.Find("Anim").GetComponent<Animator>().Play(tmpBomb.GetComponent<BombBehaviour>().myName + " fly");
                tmpBomb.transform.GetComponent<BombBehaviour>().velocity = (target.transform.position - this.transform.position)*3f;
                tmpBomb.transform.GetComponent<BombBehaviour>().Stop();

                break;
            }
            default: break;
        }
        Mode = "move";
        foreach (Transform tile in this.transform.parent.Find("Tiles"))
        {
            tile.Find("tile").GetComponent<TileBehaviour>().Turn();
        }
        StartCoroutine(EnemiesMakeTurns());
    }

    void EnemyIntends()
    {
        if (Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition)) == null) return;
        GameObject tile = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition)).gameObject;
        GameObject enemyOn = null;
        GameObject objOn = null;

        if (Mode == "fire" && Check(tile))
        {
            tile.GetComponent<TileBehaviour>().ChangeColor(Color.red);
        }

        if (Mode == "strike" && Check(tile))
        {
            foreach (Transform tmpTile in this.transform.parent.Find("Tiles"))
            {
                if (tmpTile.position == this.transform.position) continue;
                if (((tmpTile.position - tile.transform.position).magnitude < 0.1f) || 
                    (upgrades.Contains(Upgrade.StrikeSwing) && (tmpTile.position - tile.transform.position).magnitude < 0.86f + 0.1f && (tmpTile.position - this.transform.position).magnitude < 0.86f + 0.1f) ||
                    (upgrades.Contains(Upgrade.StrikeSwing1) && (tmpTile.position - tile.transform.position).magnitude < 0.86f*2f - 0.1f && (tmpTile.position - this.transform.position).magnitude < 0.86f + 0.1f) ||
                    (upgrades.Contains(Upgrade.StrikeBack) && (tmpTile.position + tile.transform.position - 2*this.transform.position).magnitude < 0.1f) ||
                    (upgrades.Contains(Upgrade.StrikeLunge) && (tmpTile.position + this.transform.position - 2*tile.transform.position).magnitude < 0.1f))
                tmpTile.Find("tile").GetComponent<TileBehaviour>().ChangeColor(Color.red);
            }
        }

        if (Mode == "throw" && Check(tile))
        {
            foreach (Transform tilep in this.transform.parent.Find("Tiles"))
            {
                if ((tilep.position - tile.transform.position).magnitude < 0.86*explodeRange + 0.1f)
                {
                    tilep.Find("tile").gameObject.GetComponent<TileBehaviour>().ChangeColor(Color.red);
                }
            }
        }
        foreach (Transform enemy in this.transform.parent.Find("Enemies"))
        {
            if (enemy.position == tile.transform.position)
            {
                enemyOn = enemy.gameObject;
            }
        }
        if (enemyOn == null) 
        {
            foreach (Transform obj in this.transform.parent.Find("Objects"))
            {
                if ((obj.position - tile.transform.position).magnitude < 0.1f)
                {
                    objOn = obj.gameObject;
                }
            }
            if (objOn == null) return;
            if (objOn.GetComponent<BombBehaviour>() != null)
            {
                foreach (Transform tilep in this.transform.parent.Find("Tiles"))
                {
                    GameObject tilet = tilep.Find("tile").gameObject;
                    Vector3 positionFrom = tilep.position - tile.transform.position;
                    if (positionFrom.magnitude < 0.86*objOn.GetComponent<BombBehaviour>().explodeRange + 0.1)
                    {
                        tilet.GetComponent<TileBehaviour>().ChangeColor((Color.red + Color.blue)/2);
                    }
                }
            }
            return;
        }
        foreach (Transform tilep in this.transform.parent.Find("Tiles"))
        {
            GameObject tilet = tilep.Find("tile").gameObject;
            Vector3 positionFrom = tilep.position - tile.transform.position;
            if (positionFrom.magnitude < 0.86*1+0.1 && enemyOn.GetComponent<EnemyBehaviour>().types.Contains(0))
            {
                tilet.GetComponent<TileBehaviour>().ChangeColor((Color.red + Color.blue)/2);
            }
            if ((abs(positionFrom.y) < 0.1f || (abs(abs(positionFrom.x/positionFrom.y) - 0.43f/0.75f) < 0.1f)) && positionFrom.magnitude < 0.86*5+0.1 && positionFrom.magnitude > 0.86*1+0.1 && enemyOn.GetComponent<EnemyBehaviour>().types.Contains(1))
            {
                tilet.GetComponent<TileBehaviour>().ChangeColor((Color.red + Color.blue)/2);
            }
            if (positionFrom.magnitude < 0.86*3+0.1 && 0.86*1+0.1 < positionFrom.magnitude && enemyOn.GetComponent<EnemyBehaviour>().types.Contains(2))
            {
                tilet.GetComponent<TileBehaviour>().ChangeColor((Color.red + Color.blue)/2);
            }
        }
    }

    float abs(float x)
    {
        if (x < 0f) return -x;
        return x;
    }
    
    IEnumerator turnIdleAfter()
    {
        yield return new WaitForSeconds(1f/3f);
        this.transform.GetComponent<Animator>().Play("hero idle");
    }

    IEnumerator EnemiesMakeTurns()
    {
        yourMove = false;

        foreach (Transform obj in this.transform.parent.Find("Objects"))
        {
            if (obj.GetComponent<BombBehaviour>() != null) 
            {
                obj.GetComponent<BombBehaviour>().Boom();
            }
        }
        if (this.transform.parent.Find("Objects").Find("Bomb(Clone)") != null) yield return new WaitForSeconds(1f/3f);

        foreach (Transform enemy in this.transform.parent.Find("Enemies"))
        {
            if (enemy.GetComponent<EnemyBehaviour>().Turn() != "move") yield return new WaitForSeconds(1f/3f);
        }

        yourMove = true;
    }
    /*IEnumerator Move(Vector3 way)
    {
        speed = way;
        yield return new WaitForSeconds(1f/3f);
        speed = new Vector3(0f, 0f, 0f);
    }*/
}
