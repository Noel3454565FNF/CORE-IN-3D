using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TreeEditor;
using UnityEngine;

public class ShockwaveHandler : MonoBehaviour
{
    public Material DefaultMaterial;
    public GameObject DefaultGameObject;
    
    [HideInInspector]public float SWExpansion;



    [HideInInspector]public static ShockwaveHandler GSwH;

    private void Awake()
    {
        GSwH = this;
    }


    public IEnumerator ShockWaveBuilder(GameObject gameObject, Material material, Vector3 expansionT, float time, Vector3 Spawnpos)
    {
        print("ShockWave registered");
        var Tgame = GameObject.Instantiate(gameObject);
        //Tgame.gameObject.GetComponent<Material>().EnableKeyword("_Color");
        //Tgame.gameObject.GetComponent<Material>().SetColor("_Color", new Color(255f, 255f, 255f, 0.1f));
        Tgame.gameObject.transform.position = Spawnpos;
        Tgame.gameObject.GetComponent<Renderer>().material = material;
        Tgame.transform.LeanScale(expansionT, time);
        print("shockwave transform");
        var Tcolor = Tgame.gameObject.GetComponent<Renderer>().material.color;

        LeanTween.value(this.gameObject, Tcolor.a, 0, time)
            .setOnUpdate((float t) =>
            {
                Color currentC = Color.Lerp(Tcolor, new Color(Tcolor.r, Tcolor.g, Tcolor.b, 0f), t);
                Tgame.gameObject.GetComponent<MeshRenderer>().material.SetColor("_Color", currentC);
            })
            .setOnComplete(() =>
            {
                
            });
        yield return new WaitForSeconds(time + 1);
        GameObject.Destroy(Tgame);

        yield break;
    }


}
