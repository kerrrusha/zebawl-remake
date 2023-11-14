using System.Collections;
using UnityEngine;

public class TileTemporaryController : MonoBehaviour
{
    public int waitBeforeFall = 2;
    public int respawnAfter = 3;

    private Rigidbody rb;
    private Vector3 initialPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        initialPosition = transform.position;

        MakeNotMovable();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(FallAfterDelay());
        }
    }

    IEnumerator FallAfterDelay()
    {
        yield return new WaitForSeconds(waitBeforeFall);
        MakeMovable();
        StartCoroutine(RespawnAfterFalling());
    }

    private void MakeMovable()
    {
        rb.useGravity = true;
        rb.isKinematic = false;
    }

    private void MakeNotMovable()
    {
        rb.useGravity = false;
        rb.isKinematic = true;
    }

    IEnumerator RespawnAfterFalling()
    {
        yield return new WaitForSeconds(respawnAfter);
        MakeNotMovable();
        transform.position = initialPosition;
    }
}
