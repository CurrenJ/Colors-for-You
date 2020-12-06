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
        foreach(Cube c in cubes) {
            if (c.infoSet)
            {
                c.transform.localPosition = c.getPosition();

                if (c.transform.position.y + c.transform.lossyScale.y / 2 < Camera.main.transform.position.y - c.frustumDims.y / 2)
                {
                    removeCube(c);
                    Destroy(c.gameObject);
                }

                if (c.depthBounce)
                {
                    c.doDepthBounce();
                }
            }
        }

        foreach (Cube c in toRemove)
            cubes.Remove(c);

        toRemove.Clear();
    }

    public void addCube(GameObject cube) {
        cubes.Add(cube.GetComponent<Cube>());
        Debug.Log(cubes.Count);
    }

    public void removeCube(Cube cube) {
        toRemove.Add(cube);
    }
}
