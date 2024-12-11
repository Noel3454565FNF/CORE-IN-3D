using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class FAS : MonoBehaviour
{

    public GameObject bg, auxBg, title, text;

    public static FAS GFAS;

    private void Awake()
    {
        GFAS = this;
        
    }


    public void WriteAnAnnouncement(string tle, string txt, int vanishTime)
    {
        print("FAS called");
        StopCoroutine(VanishAnn(0));
        print("FAS called");
        bg.gameObject.SetActive(true);
        auxBg.gameObject.SetActive(true);
        title.gameObject.SetActive(true);
        text.gameObject.SetActive(true);
        print("FAS called");


        title.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = tle;
        text.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = txt;

        print("FAS called");

        StartCoroutine(VanishAnn(vanishTime));
        print("FAS called");
    }

    public void Clear()
    {
        title.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "";
        text.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "";
    }

    IEnumerator VanishAnn(int waitbeforevanishtime)
    {
        yield return new WaitForSeconds(waitbeforevanishtime);
        Clear();
        bg.gameObject.SetActive(false);
        auxBg.gameObject.SetActive(false);
        title.gameObject.SetActive(false);
        text.gameObject.SetActive(false);
    }
}
