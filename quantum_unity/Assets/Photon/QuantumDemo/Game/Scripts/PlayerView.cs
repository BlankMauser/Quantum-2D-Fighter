using Quantum;
using UnityEngine;

[RequireComponent(typeof(EntityView))]
public class PlayerView : MonoBehaviour
{
    private EntityView _view;

    private void Start()
    {
        _view = GetComponent<EntityView>();

        Frame f = QuantumRunner.Default.Game.Frames.Verified;

        PlayerLink playerLink = f.Get<PlayerLink>(_view.EntityRef);

        if (QuantumRunner.Default.Session.IsLocalPlayer(playerLink.Player))
        {
            Camera.main!.GetComponent<CameraFollow>().SetTarget(transform);
        }
    }
}