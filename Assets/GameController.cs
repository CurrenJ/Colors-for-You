using Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject groundPlane;
    public GameObject cubePrefab;
    //camera viewport dimensions at cube distance
    public Vector2 frustumDims;
    //depth of cubes (z) - set in editor
    public float cubeDepth;
    //cube dimensions - calculated on start\
    public Vector3 cubeSize;
    //the number of cubes that fit on the screen, width-wise
    public int cubesAlongX;
    //the number of cubes that fit on the screen, height-wise
    public int cubesAlongY;
    //the distance of the front plane of the cubes from the camera
    public float distance;
    //how many cubes have been spawned in total at each index along the x-axis
    public int[] cubeCountY;
    //the time (in seconds) needed to wait after spawning a cube before spawning another cube at the same x, so that there are no collisions
    public float safeSpawnTime;
    //the time that the last cube was spawned at each index along the x-axis
    public float[] lastCubeSpawnTime;
    //the rate that the camera pans upwards (per second)
    public float panSpeed;
    //the time between each cube spawn
    public float cubeRate;
    //
    public float anyLastCubeSpawnTime;
    //
    public float initialDelay;
    //
    public float startTime;
    public float customNormalTilingScale;
    public Texture2D normalMap;

    private ColorManager colorManager;

    public bool spawnCube; //debug. tick to spawn a random cube.

    // Start is called before the first frame update
    void Start()
    {
        //For testing purposes, remove on build
        //Random.seed = 5318008;

        startTime = Time.time;
    }

    private void OnEnable() {
        if(startTime == -1)
            startTime = Time.time;    
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnCube)
        {
            spawnRandomCube();
            spawnCube = false;
        }

        if (Input.GetKeyDown(KeyCode.E))
            ClearByKnockdown();

        trySpawnCube();
        panCamera();
    }

    public void init()
    {
        distance = Mathf.Abs(Camera.main.transform.position.z) - (cubeDepth / 2);
        cubeCountY = new int[cubesAlongX];
        lastCubeSpawnTime = new float[cubesAlongX];
        frustumDims = getFrustumDims();
        cubeSize = calcCubeSize();
        cubeRate = (frustumDims.y / (panSpeed * (cubesAlongX * cubesAlongY)));
        initialDelay = cubeRate * (cubesAlongX * cubesAlongY / 2);
        colorManager = new ColorManager();
        initGroundPlane();
    }

    public void setColorRange(ColorRange cR)
    {
        colorManager = new ColorManager(cR.getColorFrom(), cR.getColorTo());
    }

    public void trySpawnCube()
    {
        if (Time.time - anyLastCubeSpawnTime >= cubeRate)
        {
            spawnRandomCube();
        }
    }

    public void panCamera()
    {

        Vector3 oldPosition = Camera.main.transform.position;
        Vector3 newPosition;
        if (Time.time - startTime > initialDelay)
        {
            newPosition = oldPosition;
            newPosition.y += panSpeed * Time.deltaTime;
            Camera.main.transform.position = newPosition;
        }
        else
        {
            newPosition = new Vector3(0, 0, oldPosition.z);
            Camera.main.transform.position = Vector3.LerpUnclamped(oldPosition, newPosition, Time.deltaTime);
        }
    }

    public void spawnRandomCube()
    {
        createCube(getRandomXIndex());
    }

    public int getRandomXIndex()
    {
        int minCount = -1;
        //identifies the least number of cubes in a column
        for (int i = 0; i < cubeCountY.Length; i++)
        {
            if (cubeCountY[i] < minCount || minCount < 0)
            {
                minCount = cubeCountY[i];
            }
        }

        //doesn't create any new cubes in a column if it has at least 5 more than the least populated column.
        //for evening and flow purposes
        //lol relatable
        int x = Random.Range(0, cubesAlongX);
        while (cubeCountY[x] - minCount >= cubesAlongY / 4 + 1)
            x = Random.Range(0, cubesAlongX);

        return x;
    }

    public Vector2 getFrustumDims()
    {
        var frustumHeight = 2.0f * distance * Mathf.Tan(Camera.main.fieldOfView * 0.5f * Mathf.Deg2Rad);
        return new Vector2(frustumHeight * Camera.main.aspect, frustumHeight);
    }

    public Vector3 calcCubeSize()
    {
        return new Vector3(frustumDims.x / cubesAlongX, frustumDims.y / cubesAlongY, cubeDepth);
    }

    //spawns a new cube given its index that corresponds to its x-axis position
    //returns whether or not the spawning was succesful
    public bool createCube(int x)
    {
        //if it is safe to spawn a cube at this index, do so
        //if a cube has never been spawned at that index yet, allow it
        if (cubeCountY[x] == 0 || Time.time - lastCubeSpawnTime[x] >= safeSpawnTime)
        {
            Vector3 startPosition = new Vector3(cubeSize.x * (-cubesAlongX / 2F + x + 1 / 2F), getSpawnHeight(), 0); //cube's position
            Vector3 endPosition = new Vector3(startPosition.x, groundPlane.transform.position.y + (cubeSize.y * (cubeCountY[x] + 1F / 2F)), 0);
            GameObject cube = Instantiate(cubePrefab, startPosition, new Quaternion()); //create gameobject and position it
            float tilingScale = 1 / ((float)cubesAlongX) / customNormalTilingScale;
            cube.GetComponent<Renderer>().material.shaderKeywords = new string[1] { "_NORMALMAP" };
            cube.GetComponent<Renderer>().material.SetTexture("_BumpMap", normalMap);
            cube.GetComponent<Renderer>().material.mainTextureScale = new Vector2(tilingScale, tilingScale);
            cube.GetComponent<Renderer>().material.mainTextureOffset = new Vector2((cubesAlongX - (x % cubesAlongX)) * tilingScale, cubeCountY[x] * -tilingScale);
            cube.transform.localScale = cubeSize; //scale the cube
            float lerpY = (cubeCountY[x] - colorManager.getColorBoundsStart(cubeCountY[x])) / ((float)colorManager.getColorBoundsLength(cubeCountY[x]));
            lerpY = positionFunction(lerpY);

            bool even = colorManager.isEvenColorFade(cubeCountY[x]);
            float lerpX;
            if (even)
                lerpX = (x / ((float)cubesAlongX));
            else lerpX = ((cubesAlongX - (x + 1)) / ((float)cubesAlongX));
            //float lerp = ((x + 1) + ((cubeCountY[x]) % colorManager.getColorBoundsLength(cubeCountY[x]))) / ((float)(cubesAlongX + colorManager.getColorBoundsLength(cubeCountY[x])));
            Color[] colorBounds = colorManager.getStartAndEndColors(cubeCountY[x]);

            float heightWeight = 0.8F;
            float totalLerp = (lerpY * heightWeight) + (lerpX * (1 - heightWeight));
            //Debug.Log(totalLerp);
            Color color = Color.Lerp(colorBounds[0], colorBounds[1], totalLerp);

            cube.GetComponent<Cube>().setCubeInfo(startPosition, endPosition, Random.Range(3F, 4F), color, frustumDims, new Vector2(x, cubeCountY[x]));
            GetComponent<CubeController>().addCube(cube);

            cubeCountY[x]++;
            lastCubeSpawnTime[x] = Time.time;
            anyLastCubeSpawnTime = Time.time;
            return true; //indicate that the spawning has succeeded
        }
        else return false; //indicate that the spawning has failed
    }

    public float positionFunction(float progress)
    {
        if (progress >= 1) //safeguard
            return 1;
        else if (progress <= 0) //safeguard
            return 0;
        else return -Mathf.Pow(progress - 1, 2) + 1; //position easing function
    }

    //returns the height at which new cubes should spawn. pans with the camera
    public float getSpawnHeight()
    {
        return Camera.main.transform.position.y + frustumDims.y / 2 + cubeSize.y;
    }

    //properly adjusts the dimensions of the groundplane so that all cubes fit
    public void initGroundPlane()
    {
        groundPlane.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y - frustumDims.y / 2F, 0);
        groundPlane.transform.localScale = new Vector3(frustumDims.x, 1, cubeDepth * 10);
    }

    public void ClearByKnockdown()
    {
        GetComponent<CubeController>().Knockdown();
        cubeCountY = new int[cubesAlongX];
        startTime = -1;
    }

    public void ResetCameraPosition(){
        Camera.main.transform.position = new Vector3(0, 0, Camera.main.transform.position.z);
    }
}
