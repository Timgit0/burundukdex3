using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UpgradeLib;

public class PowerUpBehaviour : MonoBehaviour
{
    private PlayerBehaviour player;
    private ExitBehaviour exit;
    public Upgrade[] upgrades;
    public TMP_Text[] buttonsTexts;

    void Start()
    {
        player = this.transform.parent.Find("Player").gameObject.GetComponent<PlayerBehaviour>();
        exit = this.transform.parent.Find("Exit").GetComponent<ExitBehaviour>();
        buttonsTexts = new TMP_Text[4];
        for (int i = 0; i < 4; i++)
        {
            buttonsTexts[i] = this.transform.parent.parent.Find("Canvas").Find("Actions").Find("Upgrade" + i.ToString()).Find("Text (TMP)").gameObject.GetComponent<TMP_Text>();
        }
    }


    void Update()
    {
        if (this.transform.position == player.transform.position)
        {
            for (int i = 0; i < 4; i++)
            {
                buttonsTexts[i].text = UpgradeManager.UpgradeToString(upgrades[i]);
            }
        }
        else
        {
            foreach (TMP_Text text in buttonsTexts)
            {
                text.text = "";
            }
        }
    }

    public void DoUpgrade(int number)
    {
        foreach (TMP_Text text in buttonsTexts)
        {
            text.text = "";
        }
        if (this.transform.position == player.transform.position)
        {
            if (UpgradeManager.FollowingUpgrade(upgrades[number]) != Upgrade.nullUpgrade)
            {
                exit.upgrades.Add(UpgradeManager.FollowingUpgrade(upgrades[number]));
            }
            switch (upgrades[number])
            {
                case Upgrade.HealthAndAmmo:
                {
                    player.hp += 5; player.rounds += 5; player.grenades += 2;
                    break;
                }
                case Upgrade.FireRange:
                {
                    player.fireRange += 1;
                    break;
                }
                case Upgrade.FireRange1:
                {
                    player.fireRange += 1;
                    break;
                }
                case Upgrade.FireRange2:
                {
                    player.fireRange += 1;
                    break;
                }
                case Upgrade.FireRange3:
                {
                    player.fireRange += 1;
                    break;
                }

                case Upgrade.GrenadeThrowRange:
                {
                    player.maxGrenadeRange += 1;
                    break;
                }
                case Upgrade.GrenadeThrowRange1:
                {
                    player.maxGrenadeRange += 1;
                    break;
                }
                case Upgrade.GrenadeThrowRange2:
                {
                    player.maxGrenadeRange += 1;
                    break;
                }

                case Upgrade.GrenadeExplodeRange:
                {
                    player.explodeRange += 1;
                    break;
                }

                default: break;
            }
            player.GetComponent<PlayerBehaviour>().upgrades.Add(upgrades[number]);
            this.transform.position += new Vector3(0f, 0f,-1f);
            StartCoroutine(Burn());
        }
    }

    IEnumerator Burn()
    {
        this.transform.Find("Anim").GetComponent<Animator>().Play("Altar using");
        yield return new WaitForSeconds(1f/3f);
        this.transform.position = new Vector3(100f, 100f, 0f);
        this.transform.Find("Anim").GetComponent<Animator>().Play("Altar idle");
    }
}
