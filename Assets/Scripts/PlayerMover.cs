using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMover : MonoBehaviour
{
    // [SerializeField] private float maxSpeed = 10f;
    
    [SerializeField] private float speed = 0f;
    private Vector2 _input;
    
    public void OnMove(InputAction.CallbackContext context)
    {
        _input = context.ReadValue<Vector2>();
    }

    private void Update()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        Vector3 movement = new Vector3(_input.x, 0f, _input.y);
        movement = Camera.main.transform.TransformDirection(movement);
        
        movement *= speed;
        
        transform.Translate(movement*Time.deltaTime, Space.World);
    }
}
