using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private CharacterController controller;
    private float moveSpeed;
    public CharacterController Controller {  get { return controller; } }
    private void Awake()
    {
        controller = GetComponent<CharacterController>();

    }
    
    public void Init(float _moveSpeed)
    {
        moveSpeed = _moveSpeed;
    }
    public void Move(Vector3 dir)
    {

        Vector3 velocity = dir * moveSpeed * Time.deltaTime;
        controller.Move(velocity);
        Debug.Log(velocity);
    }
}
