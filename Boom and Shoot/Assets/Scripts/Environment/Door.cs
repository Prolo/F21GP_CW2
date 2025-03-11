using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

/*
 Script for handling the different types of doors and similar objects within the world
 */

// Types of doors
public enum DoorType
{
    normal,
    key,
    bigKey,
    button,
    enemy
}
public class Door : Interactable
{
    [Header("Door Type")]
    [SerializeField] private DoorType type;
    //[SerializeField] private enemy trigger;

    // The doors state, opened or not
    [Header("Door State")]
    [SerializeField] private bool isOpen;
    [SerializeField] private BoolValue opened;

    // Some doors display dialogue when you attempt to open them
    [Header("Dialogue")]
    [SerializeField] private GameObject dialog;
    [SerializeField] private TMP_Text dialogText;
    [SerializeField] private string display;

    private void Start()
    {
        isOpen = opened.runtimeValue;
        if (isOpen)
            this.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            OpenDoor();
        }
    }

    public void OpenDoor()
    {  
            if (active)
            {
                if (type == DoorType.normal)
                {
                    Open();
                }
            }           
    }           
 

    public void Open()
    {
        isOpen = true;
        opened.runtimeValue = isOpen;
        on.Raise();
        this.gameObject.SetActive(false);
    }

    IEnumerator closeDialog()
    {
        yield return new WaitForSeconds(2);
        dialog.SetActive(false);
        
    }

}
