using UnityEngine;

public class BreakdownPoint : MonoBehaviour
{
    public bool IsBroken { get; private set; }

    [Header("Изображение")]
    [SerializeField] private GameObject exclamationMark;

    private void Awake()
    {
        if (exclamationMark != null)
        {
            exclamationMark.SetActive(false);
        }

        IsBroken = false;
    }

    public void Break()
    {
        IsBroken = true;
        if (exclamationMark != null)
        {
            exclamationMark.SetActive(true);
        }
        Debug.Log($"Консоль {gameObject.name} сломана!");
    }

    public void Fix()
    {
        IsBroken = false;
        if (exclamationMark != null)
        {
            exclamationMark.SetActive(false);
        }
        Debug.Log($"Консоль {gameObject.name} починена!");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!IsBroken || !other.CompareTag("Player"))
        {
            return;
        }
        Debug.Log($"Игрок начал чинить консоль {gameObject.name}");

        if (SkillCheckManager.Instance != null)
        {
            SkillCheckManager.Instance.StartCheck(this);
        }
        else
        {
            Debug.LogError("SkillCheckManager.Instance необнаружен");
        }
    }
}