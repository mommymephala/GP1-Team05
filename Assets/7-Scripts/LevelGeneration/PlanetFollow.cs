using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetFollow : MonoBehaviour
{
    private Rigidbody _rb;
    private PlayerMovement1 _player;
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _player = FindObjectOfType<PlayerMovement1>();
    }

    // Update is called once per frame
    void Update()
    {
        _rb.velocity = new Vector3(0, 0, _player.currentVelocity);
    }
}
