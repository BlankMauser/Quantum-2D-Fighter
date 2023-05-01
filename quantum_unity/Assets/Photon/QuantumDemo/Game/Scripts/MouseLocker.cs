using UnityEngine;

public class MouseLocker : MonoBehaviour
{
    public bool Locked;

    private void Update()
    {
        Cursor.lockState = Locked ? CursorLockMode.Locked : CursorLockMode.None;
        
        if(Input.GetKeyDown(KeyCode.Escape))
            Toggle();
    }

    public void Toggle()
    {
        Locked = !Locked;

        GetComponent<CameraFollow>().enabled = Locked;
    }
}