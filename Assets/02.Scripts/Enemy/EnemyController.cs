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
    public void Look(Vector3 dir)
    {
        dir.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 100f * Time.deltaTime);
        //transform.rotation = Quaternion.LookRotation(dir);
    }
    public void Move(Vector3 dir)
    {
        //이거 좀 변경할 필요 있음
      
        Vector3 velocity = dir * moveSpeed * Time.deltaTime;
        if (!controller.isGrounded)
            velocity.y = -1;
        controller.Move(velocity);
        Debug.Log(velocity);
        Look(dir);
    }
}
