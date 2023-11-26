using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tutorialer1 : MonoBehaviour
{
    int tutorialTextNum;
    public List<TMP_Text> tutorialTexts = new List<TMP_Text>();

    public Transform player;
    public GameObject exit;
    public Transform enemies;
    public Transform altar;

    Vector3 startPosition;

    void Start()
    {
        startPosition = player.position;
        tutorialTextNum = 0;
    }

    void Update()
    {
        switch (tutorialTextNum)
        {
            case 0:
            {
                if (player.position != startPosition)
                {
                    tutorialTextNum++;
                }
                break;
            }

            case 1:
            {
                if (exit.GetComponent<ExitBehaviour>().enemies > 0)
                {
                    tutorialTextNum++;
                }
                break;
            }

            case 2:
            {
                if (enemies.childCount == 0)
                {
                    tutorialTextNum++;
                }
                break;
            }

            case 3:
            {
                if (altar.position == player.position)
                {
                    tutorialTextNum++;
                }
                break;
            }

            case 4:
            {
                if ((altar.position - player.position).magnitude > 30f)
                {
                    tutorialTextNum++;
                }
                break;
            }

            case 5:
            {
                if (exit.GetComponent<ExitBehaviour>().enemies > 1)
                {
                    tutorialTextNum++;
                }
                break;
            }

            case 6:
            {
                if (altar.position == player.position)
                {
                    tutorialTextNum++;
                }
                break;
            }

            case 7:
            {
                if ((altar.position - player.position).magnitude > 30f)
                {
                    tutorialTextNum++;
                }
                break;
            }

            case 8:
            {
                if (exit.GetComponent<ExitBehaviour>().enemies > 2)
                {
                    tutorialTextNum++;
                }
                break;
            }

            default: break;;
        }

        for (int i = 0; i < tutorialTexts.Count; i++)
        {
            tutorialTexts[i].gameObject.SetActive(false);
        }
        tutorialTexts[tutorialTextNum].gameObject.SetActive(true);
    }
}
