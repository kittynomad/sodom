using UnityEngine;
using TMPro;

public class DebugTextController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _debugUIText;

    private PlayerBehaviors pb;
    private Rigidbody2D rb;
    private PlayerController pc;

    void Start()
    {
        pb = FindAnyObjectByType<PlayerBehaviors>();
        pc = FindAnyObjectByType<PlayerController>();
        rb = pb.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateDebugText();
    }

    public void UpdateDebugText()
    {
        string output = "velocity: " + rb.linearVelocity + 
            "\ncurrent health: " + pb.CurrentHealth + "/" + pb.MaxHealth;

        _debugUIText.text = output;
    }
}
