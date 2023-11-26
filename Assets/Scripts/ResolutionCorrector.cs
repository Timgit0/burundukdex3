/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResolutionCorrector : MonoBehaviour
{
    void Start()
    {
        int H = Screen.height;
        int W = Screen.width;
        print(H);
        print(W);
        print("AAA");

        foreach (Transform button in this.transform)
        {
            print(button.gameObject.GetComponent<RectTransform>().position);
            button.gameObject.GetComponent<RectTransform>().position = new Vector3 (button.gameObject.GetComponent<RectTransform>().position.x*W/1482f, button.gameObject.GetComponent<RectTransform>().position.y*H/747f, 0f);
            print(button.gameObject.GetComponent<RectTransform>().position);
            print("A");
        }

        this.transform.parent.Find("RoundsGrenades").gameObject.GetComponent<RectTransform>().position = new Vector3 (this.transform.parent.Find("RoundsGrenades").gameObject.GetComponent<RectTransform>().position.x*W/1482f, this.transform.parent.Find("RoundsGrenades").gameObject.GetComponent<RectTransform>().position.y*H/741f, 0f);
        this.transform.parent.Find("HP").gameObject.GetComponent<RectTransform>().position = new Vector3 (this.transform.parent.Find("HP").gameObject.GetComponent<RectTransform>().position.x*W/1482f, this.transform.parent.Find("HP").gameObject.GetComponent<RectTransform>().position.y*H/741f, 0f);
    }
}
*/