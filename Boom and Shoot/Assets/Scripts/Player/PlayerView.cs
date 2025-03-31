using UnityEngine;

public class PlayerView : MonoBehaviour
{

    [SerializeField] private float minViewDistance = 25f, mouseSensitivity = 100f, xRotation = 0f, yRotation = 0f;
    [SerializeField] private Transform playerBody;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        if (Input.GetKey(KeyCode.V))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        yRotation += mouseX;
        xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation, -90f, minViewDistance);
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0f);   
        // playerBody.Rotate(Vector3.up * mouseX);
    }
}
