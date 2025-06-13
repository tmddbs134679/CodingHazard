using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class IdleNode : ActionNode
{
    private float currentAngle = 0f;
    public float wanderRadius = 5f;
    public float minRotateSpeed = 1f;
    public float maxRotateSpeed = 10f;
    private float rotateSpeed;

    // 각도 변화량을 랜덤하게 살짝 흔들기 위한 변수
    private float angleNoise = 0f;
    private float noiseChangeSpeed = 1f;
    private bool goToEnd = true;
    public override BT.State Run(EnemyBase enemy)
    {
        //Debug.Log("여기 거치나");
        enemy.animator.SetBool(enemy.aniPara.RunParaHash, false);
        //이거 노드 나누는 게 나을 듯
        enemy.animator.SetBool(enemy.aniPara.walkParaHash, true);
        float distance = 0.2f;
  
        if (rotateSpeed == 0f)
            rotateSpeed = Random.Range(minRotateSpeed, maxRotateSpeed);

        // 노이즈를 천천히 변화시키면서 각도에 더해줌 (부드러운 랜덤성)
        angleNoise += (Random.Range(-1f, 1f)) * noiseChangeSpeed * Time.deltaTime;
        angleNoise = Mathf.Clamp(angleNoise, -20f, 20f);

        // 각도 증가 + 노이즈 반영
        currentAngle += (rotateSpeed + angleNoise) * Time.deltaTime;
        if (currentAngle >= 360f) currentAngle -= 360f;
        if (currentAngle < 0f) currentAngle += 360f;

        float rad = currentAngle * Mathf.Deg2Rad;
        Vector3 targetPos = enemy.startPos + new Vector3(Mathf.Cos(rad), 0, Mathf.Sin(rad)) * wanderRadius;

        Vector3 direction = (targetPos - enemy.transform.position).normalized;

        enemy.Controller.Look(direction);
        enemy.Controller.Move(direction);

        return BT.State.Running;
    }


}
