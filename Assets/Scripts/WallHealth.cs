using UnityEngine;

public class WallHealth : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy")) return;

        //Enemy가 닿으면 체력 1 감소
        if (SystemController.instance != null)
        {
            SystemController.instance.TakeDamage(1);
        }

        //Enemy 제거
        Destroy(collision.gameObject);
    }
}
