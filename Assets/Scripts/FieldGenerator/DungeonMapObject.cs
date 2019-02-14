using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReviewGames
{
    public class DungeonMapObject : MonoBehaviour
    {
        [SerializeField] public CellStatus Status;
        [SerializeField] public DungeonMapIndex Index;
    }
}