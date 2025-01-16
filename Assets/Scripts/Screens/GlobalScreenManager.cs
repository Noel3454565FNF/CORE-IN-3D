using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GlobalScreenManager : MonoBehaviour
{

    public static GlobalScreenManager Instance;

    [SerializeField]
    [Header("Memes images on screens")]
    public List<Texture> spr;
    public RawImage Memes;

    [Header("Global screen components")]
    public List<GameObject> GscGameobjects;
    // Start is called before the first frame update
    void Start()
    {
        if (GscGameobjects != null)
        {
            foreach(var gameObject in GscGameobjects)
            {
                gameObject.SetActive(false);
            }
        }
        int lmao = UnityEngine.Random.Range(0, spr.Count);
        Memes.texture = spr[lmao];
    }

    private void Awake()
    {
        Instance = this;   
    }

    public void MakeMemeGoAway()
    {
        Memes.gameObject.SetActive(false);
        if (GscGameobjects != null)
        {
            foreach (var gameObject in GscGameobjects)
            {
                gameObject.SetActive(true);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
