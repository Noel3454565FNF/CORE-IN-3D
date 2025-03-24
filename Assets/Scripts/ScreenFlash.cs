using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;


public class ScreenFlash : MonoBehaviour
{
    public static ScreenFlash GSF;

    public Image image;
    
    // Start is called before the first frame update
    private void Awake()
    {
        GSF = this;
    }





    public IEnumerator ScreenFlashF(Color Cto, float Time, int waittime, float switchtofull, float switchtotrans)
    {
        var pa3 = Mathf.CeilToInt(Time / 2);

        LeanTween.value(this.gameObject, 0f, 1f, switchtofull)
        .setOnUpdate((float t) =>
        {
            Color currentColor = Color.Lerp(image.color, Cto, t);
            image.color = currentColor;
        });


        yield return new WaitForSeconds(waittime);


        LeanTween.value(this.gameObject, 0f, 1f, switchtotrans)
    .setOnUpdate((float t) =>
    {
        Color currentColor = Color.Lerp(image.color, new Color(Cto.r, Cto.g, Cto.b, 0f), t);
        image.color = currentColor;
    });

        yield break;
    }

    
}
