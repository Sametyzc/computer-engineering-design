using UnityEngine;

public class FlockManager : MonoBehaviour
{
    const int threadGroupSize = 124;

    public ComputeShader compute;
    public FlockSettings settings;
    public bool showCollisionSphere;
    Boid[] boids;

    void Start()
    {
        //boids = FindObjectsOfType<Boid>();
        int childCounter = 0;
        boids = new Boid[transform.childCount];
        foreach (Transform child in transform)
        {
            boids[childCounter] = child.GetComponent<Boid>().Initialize(settings, showCollisionSphere);
            childCounter++;
        }
    }

    void Update()
    {
        if (boids != null)
        {
            int numBoids = boids.Length;
            var boidData = new BoidData[numBoids];

            for (int i = 0; i < boids.Length; i++)
            {
                boidData[i].position = boids[i].position;
                boidData[i].direction = boids[i].forward;
                boids[i].showCollisionSphere = showCollisionSphere;
            }

            var boidBuffer = new ComputeBuffer(numBoids, BoidData.Size);
            boidBuffer.SetData(boidData);

            compute.SetBuffer(0, "boids", boidBuffer);
            compute.SetInt("numBoids", boids.Length);
            compute.SetFloat("viewRadius", settings.perceptionRadius);
            compute.SetFloat("avoidRadius", settings.avoidanceRadius);

            int threadGroups = Mathf.CeilToInt(numBoids / (float)threadGroupSize);
            compute.Dispatch(0, threadGroups, 1, 1);

            boidBuffer.GetData(boidData);

            for (int i = 0; i < boids.Length; i++)
            {
                boids[i].avgFlockHeading = boidData[i].flockHeading;
                boids[i].centreOfFlockmates = boidData[i].flockCentre;
                boids[i].avgAvoidanceHeading = boidData[i].avoidanceHeading;
                boids[i].numPerceivedFlockmates = boidData[i].numFlockmates;

                boids[i].UpdateBoid();
            }

            boidBuffer.Release();
        }
    }
}

public struct BoidData
{
    public Vector3 position;
    public Vector3 direction;

    public Vector3 flockHeading;
    public Vector3 flockCentre;
    public Vector3 avoidanceHeading;
    public int numFlockmates;

    public static int Size
    {
        get
        {
            return sizeof(float) * 3 * 5 + sizeof(int);
        }
    }
}