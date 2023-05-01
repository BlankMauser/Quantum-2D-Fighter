using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private Transform _target;
    
    [SerializeField]
    private float _distance = 10.0f;

    [SerializeField]
    private float _verticalSpeed = 100f;
    
    [SerializeField]
    private float _horizontalSpeed = 220f;

    [SerializeField]
    private float _yMin = -10;
    
    [SerializeField]
    private float _yMax = 100;

    private float _x;
    private float _y;
    
    private float _previousDistance;

    public void SetTarget(Transform t) => _target = t;

    private void Start()
    {
        Vector3 angle = transform.eulerAngles;
        
        _x = angle.y;
        _y = angle.x;
    }
    
    private void LateUpdate()
    {
        if (_target == null)
            return;
        
        _x += Input.GetAxis("Mouse X") * _horizontalSpeed * 0.02f;
        _y -= Input.GetAxis("Mouse Y") * _verticalSpeed * 0.02f;

        _y = Mathf.Clamp(_y, _yMin, _yMax);
        
        Quaternion rotation = Quaternion.Euler(_y, _x, 0);
        Vector3 position = rotation * new Vector3(0.0f, 0.0f, -_distance) + _target.transform.position;

        Transform t = transform;
        
        t.rotation = rotation;
        t.position = position;

        if (Mathf.Abs(_previousDistance - _distance) > Mathf.Epsilon)
        {
            _previousDistance = _distance;
            
            Quaternion rot = Quaternion.Euler(_y, _x, 0);
            Vector3 pos  = rot * new Vector3(0.0f, 0.0f, -_distance) + _target.transform.position;
            
            t.rotation = rot;
            t.position = pos;
        }
    }
    
}