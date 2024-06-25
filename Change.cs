using System.Collections;
using OculusSampleFramework;
using UnityEngine;

public class Change : MonoBehaviour
{
    public GameObject[] m_penguin;
    public Vector3 m_juelmove;
    private bool is_grabyet = false;
    private DistanceGrabbable distanceGrabbable; // DistanceGrabbableのインスタンス

    void Start()
    {
        int j = Random.Range(3, 10);
        m_juelmove = new Vector3(0, j, 0);
        // 同じゲームオブジェクトにアタッチされたDistanceGrabbableを取得
        distanceGrabbable = GetComponent<DistanceGrabbable>();

        if (distanceGrabbable == null)
        {
            Debug.LogError("DistanceGrabbable component not found on this game object.");
        }
    }

    private IEnumerator DelayedFire()
    {
        yield return new WaitForSeconds(0.5f);
        Fire();
    }

    void Update()
    {
        if (distanceGrabbable != null && !distanceGrabbable.isGrabbed)
        {
            Debug.Log("Not Grabbed");
        }
        if (distanceGrabbable != null && distanceGrabbable.isGrabbed && !is_grabyet)
        {
            is_grabyet = true;
            Debug.Log("Grabbed");
        }
        else if (distanceGrabbable != null && is_grabyet && !distanceGrabbable.isGrabbed)
        {
            StartCoroutine(DelayedFire());
            is_grabyet = false;
        }
    }

    private void Fire()
    {
        if (m_penguin.Length == 0)
        {
            Debug.LogWarning("No penguin prefabs assigned.");
            return;
        }

        Rigidbody _rigidbody = GetComponent<Rigidbody>();
        Vector3 velocity = _rigidbody != null ? _rigidbody.velocity : Vector3.zero;
        int i = Random.Range(0, m_penguin.Length);
        GameObject pen = Instantiate(m_penguin[i], transform.position, transform.rotation);
        Rigidbody rb = pen.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.velocity = velocity;
            rb.AddForce(m_juelmove, ForceMode.Impulse);
        }
        else
        {
            Debug.LogWarning("Instantiated object does not have a Rigidbody component.");
        }

        Destroy(gameObject);
    }
}
