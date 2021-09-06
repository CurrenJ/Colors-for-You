using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeController : MonoBehaviour
{
    private List<Cube> cubes;
    private List<Cube> toRemove;
    // Start is called before the first frame update
    void Start()
    {
        cubes = new List<Cube>();
        toRemove = new List<Cube>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCubes();
    }

    private void UpdateCubes()
    {
        foreach (Cube c in cubes)
        {
            if (c.infoSet)
            {
                c.UpdateTransform();

                if (((c.transform.position.y + c.transform.lossyScale.y / 2 < Camera.main.transform.position.y - c.frustumDims.y / 2) && !c.selfDestruct)
                    || c.toDestroy)
                {
                    removeCube(c);
                    Destroy(c.gameObject);
                }
            }
        }

        foreach (Cube c in toRemove)
            cubes.Remove(c);

        toRemove.Clear();
    }

    public void addCube(GameObject cube)
    {
        cubes.Add(cube.GetComponent<Cube>());
        Debug.Log(cubes.Count);
    }

    public void Knockdown()
    {
        Vector3 pointOfForce = Camera.main.transform.position;
        foreach (Cube c in cubes)
        {
            c.SelfDestruct(pointOfForce, 3, 2);
        }
    }

    public void removeCube(Cube cube)
    {
        toRemove.Add(cube);
    }
}
