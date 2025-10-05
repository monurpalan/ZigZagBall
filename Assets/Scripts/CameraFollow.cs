using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float speed = 25f;

    private Vector3 offset;
    public bool isGameOver { get; set; } = false;

    private void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        if (player != null)
        {
            offset = transform.position - player.transform.position;
        }
    }

    private void FixedUpdate()
    {
        if (isGameOver || player == null)
            return;

        Vector3 targetPosition = player.transform.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetPosition, speed * Time.deltaTime);
    }
}