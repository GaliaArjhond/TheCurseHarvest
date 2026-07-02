using UnityEngine;

[CreateAssetMenu(fileName = "New Halimaw", menuName = "Curse Harvest/Halimaw")]
public class HalimawEntry : ScriptableObject
{
    public string monsterName;

    [TextArea(4,8)]
    public string lore;

    [TextArea(2,4)]
    public string weakness;

    public Sprite icon;
}