using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField] private float fallDelay = 0.2f;
    [SerializeField] private float destroyDelay = 2f;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.UpdateScore();
            }
            Invoke(nameof(FallDown), fallDelay);
        }
    }

    private void FallDown()
    {
        rb.isKinematic = false;
        rb.useGravity = true;
        Destroy(gameObject, destroyDelay);
    }
}