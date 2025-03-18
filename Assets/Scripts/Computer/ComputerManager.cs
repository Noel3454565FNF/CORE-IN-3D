using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ComputerManager : MonoBehaviour
{

    public LineClone LC;

    [Header("GUI")]
    public TMPro.TMP_InputField CmdInput;
    public Image bg;


    [Header("Stuff")]
    private string dummy;
    public string prefixe;
    public bool systemBusy = false;
   enum ComputerType
    {
        MaintenanceSL1,
        MaintenanceSL2,
        ReactorSysMaintenance
    };

    [SerializeField]
    ComputerType CT = new ComputerType();


    // Start is called before the first frame update
    void Start()
    {
        TMPro.TMP_InputField.SubmitEvent bleh = new TMPro.TMP_InputField.SubmitEvent();
        bleh.AddListener(CommandExecuter);
        CmdInput.onSubmit = bleh;

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void CommandExecuter(string fullCommand)
    {
        CmdInput.text = prefixe + "";
        string alpha = fullCommand.ToLower();
        if (systemBusy == false)
            {
            systemBusy = true;
                if (CT == ComputerType.MaintenanceSL1)
                {
                    if (alpha == prefixe + "test")
                    {
                        StartCoroutine(TEST());
                    }

                    else if (alpha == prefixe + "youandme" || alpha == prefixe + "myfriend" || alpha == prefixe + "my friend")
                    {
                        StartCoroutine(YOUANDME());
                    }

                else
                {
                    systemBusy = false;
                    LC.EntryPoint("ERROR: Unknown command!", Color.red);
                }

            }
        }


        IEnumerator TEST()
        {
            LC.EntryPoint("TESTING...", Color.white);
            yield return new WaitForSeconds(3f);
            LC.EntryPoint("TEST SUCCESS!", Color.green);
            systemBusy = false;
        }


        IEnumerator YOUANDME()
        {
            LC.EntryPoint("I promised you... remember?", Color.white);
            yield return new WaitForSeconds(1.5f);
            LC.EntryPoint("I'll never leave without you.", Color.white);
            yield return new WaitForSeconds(2f);
            LC.EntryPoint("Even if that mean i have to die here...", Color.white);
            yield return new WaitForSeconds(1.5f);
            LC.EntryPoint("Dying with you isn't that bad silly!", Color.white);
            yield return new WaitForSeconds(1.5f);
            LC.EntryPoint("So please... stop crying...", Color.white);
            yield return new WaitForSeconds(1.5f);
            LC.EntryPoint("And smile with me :>", Color.white);
            yield return new WaitForSeconds(1.5f);
            LC.EntryPoint("Smile with me one last time...", Color.white);
            systemBusy = false;
        }


    }

    public void GUIAppear()
    {

    }
}
