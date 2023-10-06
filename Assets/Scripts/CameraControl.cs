using UnityEngine;
using UnityEngine.InputSystem;

public class CameraControl : MonoBehaviour
{
    [SerializeField] private float sensitivity = 10f;
    [SerializeField] private float smoothing = 5f;
    
    private Vector2 _currentRotation = Vector2.zero;


    public void OnRotateCamera(InputAction.CallbackContext context)
    {
        Vector2 delta = context.ReadValue<Vector2>();
        
        float rotationX = -delta.y * sensitivity;
        float rotationY = delta.x * sensitivity;
        
        _currentRotation.x += rotationX;
        _currentRotation.y += rotationY;
        
        Quaternion targetRotation = Quaternion.Euler(_currentRotation.x, _currentRotation.y, 0f);
        
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * smoothing);
        
    }
    
    
    
}
