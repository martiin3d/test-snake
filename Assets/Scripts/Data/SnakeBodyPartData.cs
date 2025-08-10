using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SnakeBodyPart", menuName = "Martin Luquet/Snake Body Parts", order = 2)]
public class SnakeBodyPartData : ScriptableObject
{
    public Sprite HeadSprite;
    public Sprite BodySprite;
}
