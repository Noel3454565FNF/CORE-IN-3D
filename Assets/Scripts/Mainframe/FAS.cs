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
        StopCoroutine(VanishAnn(0));
        bg.gameObject.SetActive(false);
        auxBg.gameObject.SetActive(false);
        title.gameObject.SetActive(false);
        text.gameObject.SetActive(false);

        title.gameObject.GetComponent<TMPro.TextMeshPro>().text = tle;
        text.gameObject.GetComponent<TMPro.TextMeshPro>().text = txt;

        StartCoroutine(VanishAnn(vanishTime));
    }

    public void Clear()
    {
        title.gameObject.GetComponent<TMPro.TextMeshPro>().text = "";
        text.gameObject.GetComponent<TMPro.TextMeshPro>().text = "";
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
