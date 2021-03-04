using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;


public class MenuController : MonoBehaviour
{
    public bool isMenuOpen;
    private bool _isClosing;
    public Canvas mainCanvas;

    //This is VR Camera
    public XRRig Rig;

    public UnityEvent menuOpen;

    public UnityEvent menuClosed;

    [Header("Toggle Group")]
    [SerializeField] Toggle _teleportToggle;
    [SerializeField] Toggle _contMovementToggle;
    [SerializeField] Toggle _snapTurnToggle;
    private LocomotionSystem _locomotion;

    // Start is called before the first frame update

    void Start()
    {

        DisableMenu();
        setToggles();
    }

    private void setToggles()
    {
        _teleportToggle.SetIsOnWithoutNotify(Rig.GetComponent<TeleportationController>().EnableTeleport);
        _contMovementToggle.SetIsOnWithoutNotify(Rig.GetComponent<ContinuousMovement>().enableContinuousMovement);
        _snapTurnToggle.SetIsOnWithoutNotify(Rig.GetComponent<SnapTurnController>().SnapTurnIsOn);
    }
    public void toggleMenu()
    {
        if (isMenuOpen)
        {
            //Menu has to be enabled in editor, this closes the menu when scene starts
            DisableMenu();
        }
        else
        {
            EnableMenu();
        }
    }

    void OnEnable()
    {
        EnableMenu();
    }
    public void EnableMenu()
    {
        isMenuOpen = true;
        mainCanvas.gameObject.SetActive(isMenuOpen);

        mainCanvas.GetComponent<Animator>().Play(Animator.StringToHash("Base Layer" + "MenuAnim"));
        transform.position = Rig.cameraGameObject.transform.position;
        transform.rotation = Quaternion.Euler(0, Rig.cameraGameObject.transform.eulerAngles.y, 0);
        menuOpen.Invoke();
    }

    public void DisableMenu()
    {
        isMenuOpen = false;
        if (_isClosing) return;
        StartCoroutine(PlayAndWaitForAnim(mainCanvas.GetComponent<Animator>(), "MenuAnimClose"));
    }


    public IEnumerator PlayAndWaitForAnim(Animator targetAnim, string stateName)
    {
        _isClosing = true;
        const string animBaseLayer = "Base Layer";
        int MenuAnimCloseHash = Animator.StringToHash(animBaseLayer + "MenuAnimClose");
        //Get hash of animation
        int animHash = 0;
        if (stateName == "MenuAnimClose")
            animHash = MenuAnimCloseHash;

        //targetAnim.Play(MenuAnimCloseHash);
        targetAnim.CrossFadeInFixedTime(stateName, .2f);

        //Wait until we enter the current state
        /* while (targetAnim.GetCurrentAnimatorStateInfo(0).fullPathHash != animHash)
         {
             Debug.Log("enter Stattae");
             yield return null;
         }*/

        float counter = 0;
        float waitTime = targetAnim.GetCurrentAnimatorStateInfo(0).length;

        //Now, Wait until the current state is done playing
        while (counter < (waitTime))
        {

            counter += Time.deltaTime;
            yield return null;
        }
        isMenuOpen = false;
        _isClosing = false;
        mainCanvas.gameObject.SetActive(isMenuOpen);
        menuClosed.Invoke();
    }
}
