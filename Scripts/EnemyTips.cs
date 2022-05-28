using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTips : MonoBehaviour
{
    [SerializeField]
    string enemyName;
    [SerializeField]
    string enemyToolTip;
    [SerializeField]
    Sprite enemySprite;

    public string EnemyName => enemyName;
    public string EnemyToolTip => enemyToolTip;
    public Sprite EnemySprite => enemySprite;
}
