using UnityEngine;

public class Player : Character
{
    // 1
void OnTriggerEnter2D(Collider2D collision)
{
// 2
if (collision.gameObject.CompareTag("CanBePickedUp"))
{
// 3
collision.gameObject.SetActive(false);
}
}
}