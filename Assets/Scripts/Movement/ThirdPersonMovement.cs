using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ThirdPersonMovement : MonoBehaviour
{
    #region Variables
    [Header("Speeds")]
    private float currentSpeed = 0f;
    [SerializeField] private float speed = 3.0f;
    [SerializeField] private float sprintSpeed = 6.5f;
    [SerializeField] private float rotateSpeed = 5.0f;

    [Space]
    [SerializeField] private float minAngleToRotate = 10.0f;

    [Header ("Input")]
    [Header ("Directional Movement: Forward, Back, Left, Right")]
    [SerializeField] private KeyCode[] moveKeys = { KeyCode.W, KeyCode.S, KeyCode.A, KeyCode.D };
    [SerializeField] private KeyCode sprintKey = KeyCode.LeftShift;
    #endregion

    #region Builtin
    private void Start()
    {
        currentSpeed = speed;
    }

    void Update()
    {
        

        Vector3 move = Vector3.zero;
       
        if (Input.GetKey(moveKeys[0]))
            move += Vector3.forward;
        if (Input.GetKey(moveKeys[1]))
            move += Vector3.back;

        if (Input.GetKey(sprintKey))
            Sprint();
        if (Input.GetKeyUp(sprintKey))
            currentSpeed = speed;

        //Eliminated left and right because it sucks.

        transform.Translate(move * currentSpeed * Time.deltaTime);

        if (currentSpeed < sprintSpeed)
        {
            energy += Time.deltaTime * regenRate;
        }

        if (EventSystem.current.IsPointerOverGameObject()) //This is annoying as fuck
            return;

        //----------------------------Look at Mouse------------------------------------
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100f))
        {
            Vector3 mousePos = hit.point;

            Vector3 dir = mousePos - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(dir);

            float angle = Quaternion.Angle(lookRotation, transform.rotation);//Check the angle so the user can choose whether to make very fine rotations.

            if (angle >= minAngleToRotate)
            {
                Vector3 rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * rotateSpeed).eulerAngles;
                transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);
            }
        }
        //-----------------------------------------------------------------------------

        
    }
    #endregion

    [SerializeField] private float energy = 1.0f;
    [SerializeField] private float energyDegenerationRate = 3.0f;
    [SerializeField] private float regenRate = 2.0f;

    private void Sprint() 
    {
        if (energy <= 0)
        {
            currentSpeed = speed;
            return;
        }

        currentSpeed = sprintSpeed;

        energy -= Time.deltaTime * energyDegenerationRate;
    }

}
