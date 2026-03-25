using UnityEngine;

public class NavMesh : MonoBehaviour
{
    public GameObject player;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPosition = player.transform.position; 
        GetComponent<UnityEngine.AI.NavMeshAgent>().SetDestination(targetPosition);
    }
}
