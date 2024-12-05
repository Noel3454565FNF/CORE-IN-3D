using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFlash : MonoBehaviour
{
    public static ScreenFlash GSF;

    public Image image;
    
    // Start is called before the first frame update
    private void Awake()
    {
        GSF = this;
    }

    public async Task ScreenFlashF(Color Cto, float Time, int waittime)
    {
        var pa1 = 1f;
        var pa2 = 1f;
        
        

        var pa3 = Mathf.CeilToInt(Time / 2);


        var tempI = 0.700;
        float Tempa = 0;
        //while (image.color.a < tempI)
        //{
        //    image.color = new Color(Cto.r, Cto.g, Cto.b, Tempa);
        //    Tempa = Tempa + 0.010f;
        //    await Task.Delay(0001);
        //}



        LeanTween.value(this.gameObject, 0f, 1f, 2f)
        .setOnUpdate((float t) =>
        {
            Color currentColor = Color.Lerp(image.color, Cto, t);
            image.color = currentColor;
        });


            await Task.Delay(waittime);


            LeanTween.value(this.gameObject, 0f, 1f, 2f)
        .setOnUpdate((float t) =>
        {
            Color currentColor = Color.Lerp(image.color, new Color(Cto.r, Cto.g, Cto.b, 0f), t);
            image.color = currentColor;
        });
    }


}
