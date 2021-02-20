using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Controller : MonoBehaviour
{
    Animator npcAnimator = null;
    Transform player = null;

    float waveFreq = 60f;
    float cooldown = 0;

    void Start()
    {
        npcAnimator = GetComponent<Animator>();

        player = FindObjectOfType<PointAndClickMover>().transform;
    }

    const float WAVE_DIST = 10f;
    private void Update()
    {
        if (Vector3.Distance(player.position, transform.position) < WAVE_DIST)
            Wave();

        transform.LookAt(player);

        if (cooldown > 0)
            cooldown -= Time.deltaTime;
    }

    private void Wave() 
    {
        if (cooldown > 0)
            return;

        cooldown = waveFreq;
        npcAnimator.SetTrigger("Wave");
    }
}
