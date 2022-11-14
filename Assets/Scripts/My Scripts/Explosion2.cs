using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion2 : MonoBehaviour
{
    [SerializeField]
    private float cubeSize = 0.2f;
    [SerializeField]
    private int cubeInRow = 5;
    [SerializeField]
    private float explosionRadius = 4f;
    [SerializeField]
    private float explosionForce = 50f;
    [SerializeField]
    private float explosionUpward = 0.4f;

    float cubesPivotDistance;
    Vector3 cubesPivot;

    // Start is called before the first frame update
    public void Start()
    {
        //calculate pivot distance
        cubesPivotDistance = cubeSize * cubeInRow / 2;
        //use this value to create pivot vector
        cubesPivot = new Vector3(cubesPivotDistance, cubesPivotDistance, cubesPivotDistance);
    }

    // Update is called once per frame
    public void Update()
    {
        //empty
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Projectil"))
        {
            Debug.Log("Explode !");
            explode();
        }
    }

    /*private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Floor")
        {
            Debug.Log("Explode !");
            explode();
        }
    }*/

    public void explode()
    {
        //make object disappear
        gameObject.SetActive(false);

        //loop 3 times to create 5x5x5 pieces in x, y, z coordinates
        for (int x = 0; x < cubeInRow; x++)
        {
            for (int y = 0; y < cubeInRow; y++)
            {
                for (int z = 0; z < cubeInRow; z++)
                {
                    createPiece(x, y, z);
                }
            }
        }

        //get explosion position
        Vector3 explosionPosition = transform.position;

        //get colliders in that position and radius
        Collider[] colliders = Physics.OverlapSphere(explosionPosition, explosionRadius);
        //add explosion force to all colliders in that overlap sphere
        foreach (Collider hit in colliders)
        {
            //get rigidbody from collider object
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb != null)
            {
                //add explosion force to this body with given parameters
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius, explosionUpward);
            }
        }
    }

    public void createPiece(int x, int y, int z)
    {
        //create piece

        GameObject piece;
        piece = GameObject.CreatePrimitive(PrimitiveType.Cube);

        //set piece position and scale
        piece.transform.position = transform.position + new Vector3(cubeSize * x, cubeSize * y, cubeSize * z) - cubesPivot;
        piece.transform.localScale = new Vector3(cubeSize, cubeSize, cubeSize);

        //add rigidbody and set mass
        piece.AddComponent<Rigidbody>();
        piece.GetComponent<Rigidbody>().mass = cubeSize;

        //change color of the tiny cubes
        piece.transform.GetComponent<Renderer>().material = gameObject.GetComponent<Renderer>().material;
    }

    //Destroy(Gameobject , time);

}
