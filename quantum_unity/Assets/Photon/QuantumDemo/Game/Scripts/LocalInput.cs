using Photon.Deterministic;
using Quantum;
using UnityEngine;

public class LocalInput : MonoBehaviour
{
    private void Start()
    {
        QuantumCallback.Subscribe(this, (CallbackPollInput callback) => PollInput(callback));
    }

    public void PollInput(CallbackPollInput callback)
    {
        Quantum.Input input = new Quantum.Input();

        // Note: Use GetButton not GetButtonDown/Up Quantum calculates up/down itself.
        input.Jump = UnityEngine.Input.GetButton("Jump");
        
        var x = UnityEngine.Input.GetAxis("Horizontal");
        var y = UnityEngine.Input.GetAxis("Vertical");
        
        var dir = ProcessInput(x, y);

        // Input that is passed into the simulation needs to be deterministic that's why it's converted to FPVector2.
        input.Direction = dir.ToFPVector2();

        callback.SetInput(input, DeterministicInputFlags.Repeatable);
    }

    private static Vector3 ProcessInput(float x, float y)
    {
        // convert the orbit camera vector into a usable directional vector
        Camera cam = Camera.main;

        Vector3 forward = Vector3.Normalize(Vector3.ProjectOnPlane(cam.transform.forward, Vector3.up));
        Vector3 right   = Vector3.Normalize(Vector3.ProjectOnPlane(cam.transform.right, Vector3.up));

        var dir = Vector3.Normalize((x * right) + (y * forward));
        
        return dir;
    }
}