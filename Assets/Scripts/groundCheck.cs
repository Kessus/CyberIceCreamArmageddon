using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class groundCheck : MonoBehaviour
{
    private Player player;

    private void Start()
    {
        player = gameObject.GetComponentInParent<Player>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        player.grounded = true;
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        player.grounded = false;
    }
}
