using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AEGameStateChanger : MonoBehaviour
{
    void GameStateChange(DooRieGameState gameState)
    {
        TrPuzzleDooRie.xInstance._gameState = gameState;
        
    }
}
