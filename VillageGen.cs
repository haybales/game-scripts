using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]


public class VillageGen : MonoBehaviour {
    public GameObject stall;
    public Transform center;
    public int stallCount;
    public Terrain terr;


    void Start() {

        while (transform.childCount != 0)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }

        
        Vector3 down = transform.TransformDirection(-Vector3.up);
        RaycastHit hit;

            if (Physics.Raycast(transform.position, down, out hit))
            {
                terr = hit.collider.gameObject.GetComponent<Terrain>();
            }

        

        
        Random.seed = Mathf.RoundToInt(center.transform.position.x * center.transform.position.y * center.transform.position.z);
        stallCount = Mathf.RoundToInt(Random.Range(3, 6));
        float rot = Random.rotationUniform.eulerAngles.x;
        for (int i = 0; i < stallCount; i++)
        {
            float x = center.transform.position.x + (6.5f * Mathf.Sin(rot * Mathf.Deg2Rad));
            float z = center.transform.position.z + (6.5f * Mathf.Cos(rot * Mathf.Deg2Rad));
            float y = terr.SampleHeight(new Vector3(x, 100f, z));
            Vector3 pos = new Vector3(x, y, z);

            
            var lookPos = pos-center.transform.position;
            lookPos.y = 0;
            Quaternion rotation = Quaternion.LookRotation(lookPos);

            RaycastHit hitt;

            if (Physics.Raycast(pos + (Vector3.up * 0.1f), -Vector3.up, out hitt, 3f))
            {
                
            }


            GameObject obj = Instantiate(stall, pos, rotation);
            obj.transform.SetParent(transform);
            obj.transform.rotation = Quaternion.FromToRotation(obj.transform.up, hitt.normal) * obj.transform.rotation;



            rot += Random.Range(40f, 360f/stallCount);
        }
    }


    // Update is called once per frame
    void Update () {
    }
}
