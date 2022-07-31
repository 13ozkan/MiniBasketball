using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketballController : MonoBehaviour
{
    public float moveSpeed = 10;
    public Transform ball;
    public Transform arms;
    public Transform posOverHead;
    public Transform posDribble;
    public Transform target;
    private bool isBallInHands = true;
    private bool isBallFlying = false;
    private float T = 0;

    void Update()
    {
        Vector3 direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        transform.position += direction * moveSpeed * Time.deltaTime;
        transform.LookAt(transform.position + direction);

        if (isBallInHands)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                ball.position = posOverHead.position;
                arms.localEulerAngles = Vector3.right * 180;

                transform.LookAt(target.parent.position);
            }
            else
            {
                ball.position = posDribble.position + Vector3.up * Mathf.Abs(Mathf.Sin(Time.time * 5));
                arms.localEulerAngles = Vector3.right * 0;
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                isBallInHands = false;
                isBallFlying = true;
                T = 0;
            }
        }


        if (isBallFlying)
        {
            T += Time.deltaTime;
            float duration = 0.5f;
            float t01 = T / duration;

            Vector3 A = posOverHead.position;
            Vector3 B = target.position;
            Vector3 pos = Vector3.Lerp(A, B, t01);

            Vector3 ars = Vector3.up * 5 * Mathf.Sin(t01 * 3.14f);

            ball.position = pos;

            if (t01 >= 1)
            {
                isBallFlying = false;
                ball.GetComponent<Rigidbody>().isKinematic = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isBallInHands && !isBallFlying)
        {
            isBallInHands = true;
            ball.GetComponent<Rigidbody>().isKinematic = true;
        }
    }
}
