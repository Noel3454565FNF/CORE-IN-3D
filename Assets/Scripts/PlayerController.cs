using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    [Header("Limbo.")]
    private string l = "nothing";
    public static PlayerController me;
    public enum LimboPlaces
    {
        sdv1,
        stallSD,
        purgeSD
    }

    [Header("Player Movement Related")]
    public Rigidbody rgb;
    public float moveSpeed = 5f;       // Adjusted for practical movement
    public bool canMove = true;

    public bool inUI = false;
    public bool isTyping = false;

    // Start is called before the first frame update
    void Start()
    {
        if (gameObject.name == "PlayerObj")
        {
            rgb = GetComponent<Rigidbody>();
        }
    }

    private void Awake()
    {
        me = this;
    }

    // Update is called once per frame
    void Update()
    {
        // Place additional logic here if needed
    }

    private void FixedUpdate()
    {
        if (canMove && !inUI && !isTyping)
        {
            // Get input for movement
            float horizontal = 0f;
            float vertical = 0f;
            float jump = 0f;

            if (Input.GetKey(KeyCode.RightArrow) | Input.GetKey(KeyCode.D))
            {
                horizontal = 1f;
            }
            if (Input.GetKey(KeyCode.LeftArrow) | Input.GetKey(KeyCode.A))
            {
                horizontal = -1f;
            }
            if (Input.GetKey(KeyCode.UpArrow) | Input.GetKey(KeyCode.W))
            {
                vertical = 1f;
            }
            if (Input.GetKey(KeyCode.DownArrow) | Input.GetKey(KeyCode.S))
            {
                vertical = -1f;
            }
            if (Input.GetKey(KeyCode.Space))
            {
                jump = 1f;
            }

            // Calculate movement direction relative to player's orientation
            Vector3 moveDirection = transform.right * horizontal + transform.forward * vertical;

            // Normalize movement direction and apply speed
            moveDirection = moveDirection.normalized * moveSpeed;

            // Preserve existing vertical velocity and add jump force
            moveDirection.y = rgb.velocity.y + (jump > 0 ? 5f : 0f); // Adjust jump strength as needed

            // Apply movement to the rigidbody
            rgb.velocity = moveDirection;
        }
    }


    

    //public void LIMBOTELEPORT(LimboPlaces where)
    //{
        
    //    SceneManager.UnloadSceneAsync("main");
    //    SceneManager.LoadScene("Limbo");
    //}

    public void OSTPLAYER(AudioClip ost, float volume)
    {
        if (ost != null)
        {
            GameObject newsss = new GameObject(ost.name + " ID " + Random.Range(0, 999999));
            AudioSource newsssAUD = newsss.AddComponent<AudioSource>();
            newsssAUD.clip = ost;
            newsssAUD.volume = volume;
            GameObject last = GameObject.Instantiate(newsss);
            last.GetComponent<AudioSource>().Play();
            StartCoroutine(ScheduleForRemoval(ost, last));
        }
    }

    private IEnumerator ScheduleForRemoval(AudioClip clip, GameObject todestroy)
    {
        yield return new WaitForSeconds(clip.length + 1);
        GameObject.Destroy(todestroy);
    }

    /// <summary>
    /// call a coroutine at the end of the ost
    /// </summary>
    /// <param name="ost"></param>
    /// <param name="volume"></param>
    /// <param name="callback"></param>
    /// <returns></returns>
    public IEnumerator OSTPLAYER(AudioClip ost, float volume, IEnumerator callback)
    {
        if (ost != null)
        {
            GameObject newsss = new GameObject(ost.name + " ID " + Random.Range(0, 999999));
            AudioSource newsssAUD = newsss.AddComponent<AudioSource>();
            newsssAUD.clip = ost;
            newsssAUD.volume = volume;
            GameObject last = GameObject.Instantiate(newsss);
            last.GetComponent<AudioSource>().Play();
            
            yield return new WaitForSeconds(ost.length);
            StartCoroutine(callback);
        }
    }

    /// <summary>
    /// return the audiosource for better audio control
    /// </summary>
    /// <param name="ost"></param>
    /// <param name="volume"></param>
    /// <param name="useless"></param>
    /// <returns></returns>
    public AudioSource OSTPLAYER(AudioClip ost, float volume, string useless = "placeholder do not use me!")
    {
        if (ost != null)
        {
            GameObject newsss = new GameObject(ost.name + " ID " + Random.Range(0, 999999));
            AudioSource newsssAUD = newsss.AddComponent<AudioSource>();
            newsssAUD.clip = ost;
            newsssAUD.volume = volume;
            GameObject last = GameObject.Instantiate(newsss);
            last.GetComponent<AudioSource>().Play();
            return last.GetComponent<AudioSource>();
        }
        return null;
    }
}
