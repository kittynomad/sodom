using UnityEngine;

public class CorpseController : MonoBehaviour
{
    [SerializeField] private float _healthValue;
    [SerializeField] private int _ammoValue;
    [SerializeField] private float _damageValue;

    public float HealthValue { get => _healthValue; set => _healthValue = value; }
    public int AmmoValue { get => _ammoValue; set => _ammoValue = value; }
    public float DamageValue { get => _damageValue; set => _damageValue = value; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
