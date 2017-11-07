using UnityEngine;
using System.Collections;

public class Fractal : MonoBehaviour {

    public float maxTwist;
    public Mesh[] meshes;
    public Material material;

    public int maxDepth;

    public float childScale;

    private int depth;

    private bool ok=false;

    public float spawnProbability;

    public float maxRotationSpeed;

    private float rotationSpeed;

    private static Vector3[] childDirections = {
        Vector3.up,
        Vector3.right,
        Vector3.left,
        Vector3.forward,
        Vector3.back
    };

    private static Quaternion[] childOrientations = {
        Quaternion.identity,
        Quaternion.Euler(0f, 0f, -90f),
        Quaternion.Euler(0f, 0f, 90f),
        Quaternion.Euler(90f, 0f, 0f),
        Quaternion.Euler(-90f, 0f, 0f)
    };

    private Material[,] materials;
    private void InitializeMaterials()
    {
        materials = new Material[maxDepth + 1, 2];
        for (int i = 0; i <= maxDepth; i++)
        {
            float t = i / (maxDepth - 1f);
            t *= t;
            materials[i, 0] = new Material(material);
            materials[i, 0].color = Color.Lerp(Color.white, Color.yellow, t);
            materials[i, 1] = new Material(material);
            materials[i, 1].color = Color.Lerp(Color.white, Color.cyan, t);
        }
        materials[maxDepth, 0].color = Color.magenta;
        materials[maxDepth, 1].color = Color.red;
    }


    private void Start()
    {
        rotationSpeed = Random.Range(-maxRotationSpeed, maxRotationSpeed);
        transform.Rotate(Random.Range(-maxTwist, maxTwist), 0f, 0f);
        if (materials == null)
        {
            InitializeMaterials();
        }
        gameObject.AddComponent<MeshFilter>().mesh = meshes[Random.Range(0, meshes.Length)];
        gameObject.AddComponent<MeshRenderer>().material = materials[depth, Random.Range(0, 2)];

        //GetComponent<MeshRenderer>().material.color = Color.Lerp(Color.white, Color.yellow, (float)depth / maxDepth);

        if (depth < maxDepth)
        {
            StartCoroutine(CreateChildren());
        }
    }

    private void Update()
    {
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
    }

    private void Initialize(Fractal parent, int childIndex,bool d)
    {
        spawnProbability = parent.spawnProbability;
        meshes = parent.meshes;
        //material = parent.material;
        maxTwist = parent.maxTwist;
        materials = parent.materials;
        maxDepth = parent.maxDepth;
        depth = parent.depth + 1;
        childScale = parent.childScale;
        transform.parent = parent.transform;
        transform.localScale = Vector3.one * childScale;
        maxRotationSpeed = parent.maxRotationSpeed;
        if (d&&childIndex==-1)
        {
            MeshFilter mf = parent.GetComponent<MeshFilter>();
            Mesh m = mf.mesh;

            if (m.name == "Cylinder Instance")
            {
                Debug.Log("loloooool");
                transform.localPosition = Vector3.down * (1.1f + 0.5f * childScale);
                transform.localRotation = Quaternion.Euler(0f, 0f, -180f);

            }
            else
            {
                transform.localPosition = Vector3.down * (0.5f + 0.5f * childScale);
                transform.localRotation = Quaternion.Euler(0f, 0f, -180f);

            }

            ok = false;
        }

        else
        {
            MeshFilter mf = parent.GetComponent<MeshFilter>();
            Mesh m = mf.mesh;
        
            Debug.Log(m.name);
            if (m.name == "Cylinder Instance" && childIndex == 0)
            {
                transform.localPosition = childDirections[childIndex] * (1.1f + 0.5f * childScale);
                transform.localRotation = childOrientations[childIndex];
            }
            else
            {
                transform.localPosition = childDirections[childIndex] * (0.5f + 0.5f * childScale);
                transform.localRotation = childOrientations[childIndex];


            }


        }


    }

    private IEnumerator CreateChildren()
    {
        if (depth == 0)
        {
            ok = true;
            yield return new WaitForSeconds(Random.Range(0.1f, 0.5f));
            new GameObject("Fractal Child").AddComponent<Fractal>().
                Initialize(this, -1, ok);
        }
        for (int i = 0; i < childDirections.Length; i++)
        {
            if (Random.value < spawnProbability)
            {
                yield return new WaitForSeconds(Random.Range(0.1f, 0.5f));
                new GameObject("Fractal Child").AddComponent<Fractal>().
                Initialize(this, i, ok);

            }
        }
    }


}