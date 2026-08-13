using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DebugTextController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _debugUIText;
    [SerializeField] private Slider _stamBar;

    private PlayerBehaviors pb;
    private Rigidbody2D rb;
    private PlayerController pc;
    private PlayerResources pr;

    void Start()
    {
        pb = FindAnyObjectByType<PlayerBehaviors>();
        pc = FindAnyObjectByType<PlayerController>();
        rb = pb.GetComponent<Rigidbody2D>();
        pr = pb.GetComponent<PlayerResources>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateDebugText();
        UpdateStaminaBar();
    }

    public void UpdateDebugText()
    {
        string output = "velocity: " + rb.linearVelocity +
            "\ncurrent health: " + pb.CurrentHealth + "/" + pb.MaxHealth +
            "\ncurrent ammo: " + pb.CurrentAmmo + "/" + pb.MaxAmmo +
            "\ncurrent currency: " + pr.Currency +
            "\ncurrent stamina: " + pb.CurrentStamina + "/" + pb.MaxStamina;

        _debugUIText.text = output;
    }

    public void UpdateStaminaBar()
    {
        _stamBar.value = pb.CurrentStamina / pb.MaxStamina;
    }
}
