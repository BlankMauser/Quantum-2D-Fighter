using Photon.Deterministic;
using Quantum;
using UnityEngine;
using Rewired;

public class LocalTestInput : MonoBehaviour
{
    public Player localPlayer;
    // Subscribe to Quantum Input
    private void Start() {
        localPlayer = Rewired.ReInput.players.GetPlayer(0);
        QuantumCallback.Subscribe(this, (CallbackPollInput callback) => PollInput(callback));
        }

    public void PollInput(CallbackPollInput callback) {
        Quantum.Input input = new Quantum.Input();

        // Note: Use GetButton not GetButtonDown/Up Quantum calculates up/down itself.
        input.Jump = localPlayer.GetButton("Jump1");
        input.Attack = localPlayer.GetButton("Attack");

        var x = localPlayer.GetAxis("Left X");
        var y = localPlayer.GetAxis("Left Y");

        var dir = ProcessInput(x, y);

        // Input that is passed into the simulation needs to be deterministic that's why it's converted to FPVector2.
        input.Direction = dir.ToFPVector2();

        callback.SetInput(input, DeterministicInputFlags.Repeatable);
        }

    private static Vector3 ProcessInput(float x, float y) {
        // convert the orbit camera vector into a usable directional vector
        Camera cam = Camera.main;

        Vector3 forward = Vector3.Normalize(Vector3.ProjectOnPlane(cam.transform.forward, Vector3.up));
        Vector3 right = Vector3.Normalize(Vector3.ProjectOnPlane(cam.transform.right, Vector3.up));

        var dir = Vector3.Normalize((x * right) + (y * forward));

        return dir;
        }
    }
