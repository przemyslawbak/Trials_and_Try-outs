using UnityEngine;

public class Player : Character
{
    // 1
void OnTriggerEnter2D(Collider2D collision)
{
// 2
if (collision.gameObject.CompareTag("CanBePickedUp"))
{
// 1
// Note: This should all be on a single line
Item hitObject = collision.gameObject.
GetComponent<Consumable>().item;
// 2
if (hitObject != null)
{
// 3
print("it: " + hitObject.objectName);
collision.gameObject.SetActive(false);
}
}
}
}