using UnityEngine;

public class HitBoss : MonoBehaviour
{
    public int damage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Player>().HP_Min -= damage;
        }
    }
}
