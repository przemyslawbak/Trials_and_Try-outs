using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class RPGGameManager : MonoBehaviour
{
// 1
public static RPGGameManager sharedInstance = null;
void Awake()
{// 2
if (sharedInstance != null && sharedInstance != this)
{
// 3
Destroy(gameObject);
}
else
{
// 4
sharedInstance = this;
}
}
void Start()
{
// 5
SetupScene();
}
// 6
public void SetupScene()
{
// empty, for now
}
}