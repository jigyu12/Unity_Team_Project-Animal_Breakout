using UnityEngine;

public abstract class CollidableMapObject : MonoBehaviour
{
    // Temporary Code //
    public GameObject player;
    public void Awake()
    {
        player = GameObject.FindGameObjectWithTag(Utils.PlayerTag);
    }

    private void Start()
    {
        enabled = false;
        var relayRunManager = GameObject.FindObjectOfType<RelayRunManager>();

        relayRunManager.onLoadPlayer += (playerStatus) => player = playerStatus.gameObject;
        relayRunManager.onDiePlayer += (playerStatus) => player = null;

    }

    public void Update()
    {
        if (player != null && transform.position.z < player.transform.position.z - 2f)
        {
            Destroy(gameObject);
        }
    }
    // Temporary Code //

    public abstract ObjectType ObjectType { get; protected set; }
    protected abstract ICollisionBehaviour CollisionBehaviour { get; set; }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Utils.PlayerTag))
        {
            CollisionBehaviour.OnCollision(gameObject, other);
        }
    }
}