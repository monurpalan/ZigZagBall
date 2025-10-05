using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Falling Settings")]
    [SerializeField] private float fallSpeed = 25f;
    [SerializeField] private float groundCheckDistance = 2f;
    [SerializeField] private float destroyDelay = 5f;

    [Header("Dependencies")]
    [SerializeField] private CameraFollow cameraFollow;

    private bool isGameOver = false;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (cameraFollow == null)
        {
            cameraFollow = Camera.main.GetComponent<CameraFollow>();
        }
    }

    private void Update()
    {
        if (isGameOver) return;

        // Oyuncunun altını kontrol etmek için bir ışın (Raycast) gönderir.
        // Eğer belirli bir mesafede zemin yoksa, oyuncu düşmeye başlar ve oyun biter.
        if (!Physics.Raycast(transform.position, Vector3.down, groundCheckDistance)) // Eğer oyuncunun altında zemin yoksa düşmeye başla
        {
            TriggerGameOver();
        }
    }

    private void TriggerGameOver()
    {
        isGameOver = true;
        rb.velocity = Vector3.down * fallSpeed;
        rb.constraints = RigidbodyConstraints.None;
        if (cameraFollow != null) cameraFollow.isGameOver = true;
        GameManager.Instance.GameOver();
        Destroy(gameObject, destroyDelay);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isGameOver && other.CompareTag("Diamond"))
        {
            GameManager.Instance.UpdateDiamonds();
            Destroy(other.gameObject);
        }
    }
}