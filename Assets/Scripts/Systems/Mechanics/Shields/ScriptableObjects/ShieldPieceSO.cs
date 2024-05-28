using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "NewShieldPieceSO", menuName = "ScriptableObjects/ShieldPiece")]
public class ShieldPieceSO : ScriptableObject
{
    public int id;
    public Dialect dialect;
    public string pieceName;
    public Sprite pieceImage;
}
