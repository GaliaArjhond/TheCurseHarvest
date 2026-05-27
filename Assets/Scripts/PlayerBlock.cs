using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBlock : MonoBehaviour
{
    [Header("Block Settings")]
    [SerializeField] private float blockDamageReduction = 0.5f;
    [SerializeField] private float blockMoveSlow = 0.5f;

    [Header("Parry")]
    [SerializeField] private float parryWindow = 0.2f;

    private bool isBlocking = false;
    private bool parryActive = false;

    public bool IsBlocking => isBlocking;
    public bool IsParrying => parryActive;

    public float DamageReduction => blockDamageReduction;
    public float MoveSlow => blockMoveSlow;

    void Update()
    {
        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            StartBlock();
        }

        if (Mouse.current.rightButton.isPressed)
        {
            HoldBlock();
        }

        if (Mouse.current.rightButton.wasReleasedThisFrame)
        {
            StopBlock();
        }
    }

    void StartBlock()
    {
        HotbarControler hotbar =
            FindFirstObjectByType<HotbarControler>();

        if (hotbar == null)
            return;

        Item item = hotbar.GetSelectedItem();

        if (item == null)
            return;

        if (item.itemType != Item.ItemType.Weapon)
            return;

        isBlocking = true;

        StopAllCoroutines();
        StartCoroutine(ParryWindowRoutine());
    }

    void HoldBlock()
    {
        if (!isBlocking)
            return;
    }

    void StopBlock()
    {
        isBlocking = false;
        parryActive = false;
    }

    System.Collections.IEnumerator ParryWindowRoutine()
    {
        parryActive = true;

        Debug.Log("PARRY WINDOW ACTIVE");

        yield return new WaitForSeconds(parryWindow);

        parryActive = false;
    }
}