using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmSpawner : MonoBehaviour
{
    public GameObject ArmSpawnPoint;
    public GameObject NecroArm;

    // Start is called before the first frame update
    void Start()
    {
        GameObject newArm = Instantiate(
            NecroArm,
            ArmSpawnPoint.transform.position,
            Quaternion.identity,
            this.transform // Set SoulGolem as parent
        );

        newArm.transform.localScale = Vector3.one; // keeps original scale
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
