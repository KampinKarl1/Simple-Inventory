using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentGenerator : MonoBehaviour
{    
    [System.Serializable]
    struct EnvironmentalObject 
    {
        [SerializeField] private GameObject prefab;
        [SerializeField] private float percent;
        public GameObject Prefab => prefab;
        public float PercentOfAll => percent;
    }

    [SerializeField] LayerMask unwantedMask = 1 << 0;

    [SerializeField] private float spawnRadius = 10f;

    [SerializeField] int numObjectsToSpawn = 10;

    [SerializeField] List<EnvironmentalObject> spawnList = new List<EnvironmentalObject>();

    List<int> hashes = new List<int>();

    void Start()
    {
        for (int i = 0; i < spawnList.Count; i++)
        {
            for (int j = 0; j < spawnList[i].PercentOfAll * numObjectsToSpawn; ++j)
            {
                Vector3 pos = GetRandomPos();

                if (OnGroundAndNotOverlapping(pos))
                    Instantiate(spawnList[i].Prefab, pos, Quaternion.identity, transform);                
            }    
        }
    }
    private Vector3 GetRandomPos()
    {
        float x = transform.position.x + Random.insideUnitSphere.x * spawnRadius;
        float z = transform.position.z + Random.insideUnitSphere.z * spawnRadius;

        Vector3 randpos = new Vector3(x, 0, z);

        int hash = randpos.GetHashCode();

        if (!hashes.Contains(hash))
            hashes.Add(hash);
        else
            return GetRandomPos();

        return randpos;
    }

    private bool OnGroundAndNotOverlapping(Vector3 pos) 
    =>
        (!Physics.CheckSphere(pos, 2.0f, unwantedMask)
                       &&
        Physics.Raycast(pos + Vector3.up, Vector3.down, 5.0f, 1 << 8));    
}
