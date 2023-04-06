using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//Adopted from https://www.youtube.com/watch?v=vTNWUbGkZ58
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
    public GameObject speedlines;
    private GameObject speedlines_instance;

    // Start is called before the first frame update
    void Start()
    {
        moveScript = GetComponent<PlayerMovement>();
        lastDash = Time.time - dashCooldown;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
        float targetAngle = 0;
        if (direction.magnitude >= 0.1f)
        {
            targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        }

        float nextDash = lastDash + dashCooldown;
        if (Time.time > nextDash)
        {
            dashChargeText.text = "Dash Ready";
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) && Time.time > nextDash)
        {
            speedlines_instance = Instantiate(speedlines, gameObject.transform, false);
            speedlines_instance.transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);//this doesn't seem to work that well.
            StartCoroutine(Dash());
            lastDash = Time.time;
            dashChargeText.text = "";
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
        Destroy(speedlines_instance);
    }
}
