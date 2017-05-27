using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]

public class DockGen : MonoBehaviour {

    public GameObject dock;

    private Vector3[] spot = new Vector3[4];
    private float[] heights = new float[4];
    private bool allsame = false;

    // Use this for initialization
    void Start () {

        while (transform.childCount != 0)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }

        spot[0] = new Vector3(transform.position.x + 10f, 70f, transform.position.z);
        spot[1] = new Vector3(transform.position.x - 10f, 70f, transform.position.z);
        spot[2] = new Vector3(transform.position.x, 70f, transform.position.z + 10f);
        spot[3] = new Vector3(transform.position.x, 70f, transform.position.z - 10f);

        for (int i=0; i<spot.Length; i++)
        {
            RaycastHit hit;
            Ray downRay = new Ray(spot[i], -Vector3.up);
            if (Physics.Raycast(downRay, out hit))
            {
                heights[i] = hit.distance;
            }
        }



        int furthest = 0;
        for (int i = 0; i < heights.Length; i++)
        {

            if (heights[i] > heights[furthest])
            {
                furthest = i;
            }
        }


        if (AllSame(heights))
        {
            return;
        }

        GameObject obj = Instantiate(dock, new Vector3(spot[furthest].x, 64.3f, spot[furthest].z), transform.rotation);
        obj.transform.SetParent(transform);
        obj.transform.LookAt(new Vector3(transform.position.x, 64.3f, transform.position.z));


    }
	
	// Update is called once per frame
	void Update () {
		
	}

    bool AllSame(float[] list)
    {
        bool first = true;
        float comparand = 0;
        for(int i = 0; i < list.Length; i++)
        {
            if (first) comparand = list[i];
            else if (list[i] != comparand) return false;
            first = false;
        }
        return true;
    }
}
