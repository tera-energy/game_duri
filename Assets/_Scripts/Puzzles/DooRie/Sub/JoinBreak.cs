using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoinBreak : MonoBehaviour
{
    private void OnJointBreak2D(Joint2D joint)
    {
        TrPuzzleDooRie.xInstance._fellOffFruitCount++;
        transform.SetParent(null);
    }
}
