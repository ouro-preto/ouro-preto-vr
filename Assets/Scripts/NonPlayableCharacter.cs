using UnityEngine;

public class NonPlayableCharacter : MonoBehaviour
{
    [SerializeField]
    private GameObject[] m_Prefabs;

    // Start is called before the first frame update
    void Start()
    {
        var i = Random.Range(0, m_Prefabs.Length);
        var gameObject = Instantiate(m_Prefabs[i], transform.position, Quaternion.identity, transform);
        gameObject.transform.LookAt(Vector3.zero);
    }

    private void OnTriggerEnter(Collider other)
    {
        print("dfuasdfasd");
    }
}
