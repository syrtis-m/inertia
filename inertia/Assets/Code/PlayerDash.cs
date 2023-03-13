using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerDash : MonoBehaviour
{
    PlayerMovement moveScript;

    public float dashSpeed;
    public float dashTime;
    public float dashCooldown;

    float lastDash;

    public Vector3 moveDirection;
    public Transform cam;

    public TMP_Text dashChargeText;

    // Start is called before the first frame update
    void Start()
    {
        moveScript = GetComponent<PlayerMovement>();
        lastDash = Time.time - dashCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        }

        float nextDash = lastDash + dashCooldown;
        if (Time.time > nextDash)
        {
            dashChargeText.text = "Dash Charges: 1";
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) && Time.time > nextDash)
        {
            StartCoroutine(Dash());
            lastDash = Time.time;
            dashChargeText.text = "Dash Charges: 0";
        }
    }

    IEnumerator Dash()
    {
        float startTime = Time.time;
        while(Time.time < startTime + dashTime)
        {
            moveScript.controller.Move(moveDirection.normalized * dashSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
