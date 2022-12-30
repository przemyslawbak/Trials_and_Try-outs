using UnityEngine;
public class SpawnPoint : MonoBehaviour
{
// 1
public GameObject prefabToSpawn;
// 2
public float repeatInterval;
public void Start()
{
// 3
if (repeatInterval > 0)
{
// 4
InvokeRepeating("SpawnObject", 0.0f, repeatInterval);
}
}
// 5
public GameObject SpawnObject()
{
// 6
if (prefabToSpawn != null)
{
// 7
return Instantiate(prefabToSpawn, transform.
position, Quaternion.identity);
}
// 8
return null;
}
}