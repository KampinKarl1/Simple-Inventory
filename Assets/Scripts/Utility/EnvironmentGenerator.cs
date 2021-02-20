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
        public float PercentOfAll => percent *.01f;
    }

    [SerializeField, Tooltip ("The layermask for objects that we don't want to put environment objects on.")] 
    LayerMask unwantedMask = 1 << 0;
    LayerMask groundMask = 1 << 0;

    [SerializeField] int groundLayer = 8;

    [SerializeField] int numObjectsToSpawn = 10;

    [SerializeField] private float spawnRadius = 10f;
    
    [SerializeField] List<EnvironmentalObject> spawnList = new List<EnvironmentalObject>();

    List<int> hashes = new List<int>();

    void Start()
    {
        InitializeLayerMasks();

        SpawnObjects();
    }

    private void InitializeLayerMasks() 
    {
        groundMask = 1 << LayerMask.NameToLayer("Ground"); 
    }

    private void SpawnObjects() 
    {
        for (int i = 0; i < spawnList.Count; i++)
        {
            float numToSpawn = spawnList[i].PercentOfAll * numObjectsToSpawn;
            print("Should spawn about " + numToSpawn.ToString() + " objects of type " + spawnList[i].Prefab.name) ;

            for (int j = 0; j < (int) numToSpawn; ++j)
            {
                Vector3 pos = GetRandomPos();

                if (OnGroundAndNotOverlapping(pos))
                {
                    GameObject o = Instantiate(spawnList[i].Prefab, pos, Quaternion.identity, transform);
                    o.transform.localScale = new Vector3(Random.Range(.8f, 1.2f), Random.Range(.6f, 1.2f), Random.Range(.8f, 1.2f));
                }
            }
        }
    }

    private Vector3 GetRandomPos()
    {
        float x = transform.position.x + Random.insideUnitSphere.x * spawnRadius;
        float z = transform.position.z + Random.insideUnitSphere.z * spawnRadius;

        Vector3 randpos = new Vector3(x, 90, z);

        RaycastHit hit;
        if (Physics.Raycast(randpos, Vector3.down, out hit, 100f))
            randpos.y = hit.point.y;

        int hash = randpos.GetHashCode();

        if (!hashes.Contains(hash))
            hashes.Add(hash);
        else
            return GetRandomPos();

        return randpos;
    }

    private bool OnGroundAndNotOverlapping(Vector3 pos)
    {
        if (Physics.CheckSphere(pos, 2.0f, unwantedMask))
            return false;
        if (Physics.Raycast(pos + Vector3.up, Vector3.down, 5.0f, groundMask))
            return true;
        return false;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;

        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
#endif
}
