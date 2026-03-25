using UnityEngine;
using UnityEngine.AI; // Required for NavMeshAgent

/// <summary>
/// Controls AI navigation using Unity's NavMesh system.
/// Makes an AI enemy follow the player by finding a path and moving towards them.
/// Requires a NavMeshAgent component on the same GameObject as this script.
/// </summary>
public class Navmesh : MonoBehaviour
{
    /// <summary>Cached reference to the NavMeshAgent component on this GameObject.</summary>
    private NavMeshAgent navMeshAgent;
    
    /// <summary>Cached reference to the player GameObject to avoid searching every frame.</summary>
    private GameObject player;

    /// <summary>
    /// Called when the game starts.
    /// Sets up the NavMeshAgent and finds the Player GameObject.
    /// Validates that all required components exist.
    /// </summary>
    void Start()
    {
        // Get the NavMeshAgent component on this AI's GameObject
        navMeshAgent = GetComponent<NavMeshAgent>();
        
        // Validate that NavMeshAgent exists (required for navigation)
        if (navMeshAgent == null)
        {
            Debug.LogError("Navmesh: NavMeshAgent component not found! Add a NavMeshAgent to this GameObject.", this);
            enabled = false; // Disable this script if NavMeshAgent is missing
            return;
        }
        
        // Find the Player GameObject in the scene
        player = GameObject.Find("Player");
        
        // Validate that Player was found
        if (player == null)
        {
            Debug.LogError("Navmesh: Player GameObject not found! Ensure there's a GameObject named 'Player' in the scene.", this);
            enabled = false; // Disable this script if Player is missing
            return;
        }
    }

    /// <summary>
    /// Called once per frame.
    /// Updates the AI's navigation destination to the player's current position.
    /// The NavMeshAgent automatically calculates and follows the path.
    /// </summary>
    void Update()
    {
        // Safety check: ensure player still exists (in case it was destroyed)
        if (player == null)
        {
            return; // Exit early if player is gone
        }
        
        // Safety check: ensure NavMeshAgent still exists
        if (navMeshAgent == null)
        {
            return; // Exit early if NavMeshAgent is gone
        }
        
        // Set the destination for the NavMeshAgent (it will handle pathfinding automatically)
        // The agent will follow the NavMesh and avoid obstacles en route
        navMeshAgent.SetDestination(player.transform.position);
    }
}
