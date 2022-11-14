using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pick : MonoBehaviour
{

    [SerializeField] GameObject player;
    [SerializeField] float interactRange = 0.5f;

    [SerializeField]
    private GameObject GetSphereToWin;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Distance entre le joueur et l'objet. Permet de savoir si le joueur est assez prêt pour activer l'objet
        Vector3 distanceToPlayer = player.transform.position - transform.position;

        //Si le joueur est proche
        if (distanceToPlayer.magnitude <= interactRange && Input.GetKeyDown(KeyCode.E))
        {
            //Action (joue un son, fait une animation, ...)
            Debug.Log("Player picked the object !");
            GetSphereToWin.gameObject.SetActive(false);
            player.GetComponent<PlayerMovement>().nbSpheres++;
        }
    }

    /*void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Destroy"))
        {
            Destroy(other.gameObject);
        }
    }*/

}
