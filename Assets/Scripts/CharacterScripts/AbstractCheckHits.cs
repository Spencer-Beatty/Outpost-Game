using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractCheckHits : MonoBehaviour
{
    public abstract IEnumerator Attack();
    public abstract void GetHit();
}
