using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Follow : MonoBehaviour
{
    [SerializeField] private Transform target = null;
    [SerializeField] private float rotateSpeed = 5.0f;

    [SerializeField] private float minAngleToRotate = 10.0f;

    float halfWidth = -1;

    private void Start()
    {
        halfWidth = Screen.width / 2f;
    }

    void Update()
    {
        transform.position = target.position;
        //----------------------------Look at Mouse------------------------------------
        if (PointerWithinBounds())
        {
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
                    float distX = Mathf.Abs(halfWidth - Input.mousePosition.x);
                    float prctg = distX / halfWidth;

                    Vector3 rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * (prctg * rotateSpeed)).eulerAngles;
                    transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);
                }
            }
        }
        //-----------------------------------------------------------------------------
    }

    private bool PointerWithinBounds()
    {
        if (EventSystem.current.IsPointerOverGameObject()) //This is annoying as fuck
            return false;

        Vector2 pointerPos = Input.mousePosition;

        if (pointerPos.x > Screen.width || pointerPos.x < 0)
            return false;
        if (pointerPos.y > Screen.height || pointerPos.y < 0)
            return false;

        return true;
    }
}
