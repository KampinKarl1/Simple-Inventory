using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ThirdPersonMovement : MonoBehaviour
{
    #region Variables
    [SerializeField] Transform cameraBoom = null;
    [SerializeField] Transform body = null;
    Dictionary<Vector3, float> directions = new Dictionary<Vector3, float>
    {
        {Vector3.zero, 0f },
        {Vector3.forward, 0f },
        {new Vector3(1,0,1),45f }, //half right
        {Vector3.right, 90f },
        {new Vector3(1,0,-1),135f }, //half right going backward
        {Vector3.back, 180f },
        {new Vector3(-1,0,-1),225f }, //half left backward
        { Vector3.left, 270f },
        {new Vector3(-1,0,1),315f } //half left
    };

    [Header("Speeds")]
    private float currentSpeed = 0f;
    [SerializeField] private float speed = 3.0f;
    [SerializeField] private float sprintSpeed = 6.5f;
    [SerializeField] private float acceleration = 2.0f;

    [Space]
    [Header("Stamina?")]
    [SerializeField] private float maxEnergy = 45.0f;
    private float energy = 0f;
    [SerializeField] private float energyDegenerationRate = 3.0f;
    [SerializeField] private float regenRate = 2.0f;

    [Space]
    [Header("Input")]
    [Header("Directional Movement: Forward, Back, Left, Right")]
    [SerializeField] private KeyCode[] moveKeys = { KeyCode.W, KeyCode.S, KeyCode.A, KeyCode.D };
    [SerializeField] private KeyCode sprintKey = KeyCode.LeftShift;
    #endregion

    #region Builtin
    private void Start()
    {
        energy = maxEnergy;

        currentSpeed = speed;
    }

    void Update()
    {
        Vector3 move = Vector3.zero;

        if (Input.GetKey(moveKeys[0]))
            move += Vector3.forward;
        if (Input.GetKey(moveKeys[1]))
            move += Vector3.back;
        if (Input.GetKey(moveKeys[2]))
            move += Vector3.left;
        if (Input.GetKey(moveKeys[3]))
            move += Vector3.right;

        if (move != Vector3.zero)
        {
            Vector3 rot = Quaternion.Lerp(transform.rotation, cameraBoom.rotation, Time.deltaTime * 5.0f).eulerAngles;
            transform.rotation = Quaternion.Euler(0, rot.y, 0);

            Vector3 rot1 = Quaternion.LookRotation(transform.forward).eulerAngles;
            body.rotation = Quaternion.Euler(0, directions[move] + rot1.y, 0);
        }



        //--------------Sprinting----------------
        if (Input.GetKey(sprintKey))
            Sprint();

        if (currentSpeed > speed && !Input.GetKey(sprintKey))
        {
            currentSpeed = Mathf.Lerp(currentSpeed, speed, Time.deltaTime * acceleration);
        }

        transform.Translate(move * currentSpeed * Time.deltaTime);

        if (energy < maxEnergy && currentSpeed <= speed)
        {
            energy += Time.deltaTime * regenRate;
        }
    }
    #endregion

    private void Sprint()
    {
        if (energy <= 0)
        {
            currentSpeed = Mathf.Lerp(currentSpeed, speed, Time.deltaTime * acceleration);
            return;
        }

        currentSpeed = Mathf.Lerp(currentSpeed, sprintSpeed, Time.deltaTime * acceleration);

        energy -= Time.deltaTime * energyDegenerationRate;
    }
}
