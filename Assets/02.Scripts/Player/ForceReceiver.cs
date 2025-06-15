using UnityEngine;

public class ForceReceiver : MonoBehaviour
{
    [SerializeField] private CharacterController controller;
    private float verticalVelocity;

    public Vector3 Movement => Vector3.up * verticalVelocity;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (controller.isGrounded && verticalVelocity < 0f)
        {
            verticalVelocity = -2f;  // 약간 음수로 고정해 접지 유지
        }
        else
        {
            verticalVelocity += -20f * Time.deltaTime;
        }
    }

    public void Jump(float jumpForce)
    {
        verticalVelocity = jumpForce;
    }
}