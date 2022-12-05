using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D target)
    {
        if (target.tag == "DooRie" || target.tag == "Fruit")
            Destroy(target.gameObject);
    }
}
