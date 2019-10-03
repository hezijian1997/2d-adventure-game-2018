using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinGet : MonoBehaviour
{
    [SerializeField] AudioClip CoinPickUpSFX;
    [SerializeField] int CoinScore = 100;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        FindObjectOfType<GameSession>().AddToScore(CoinScore);
        AudioSource.PlayClipAtPoint(CoinPickUpSFX, Camera.main.transform.position);
        Destroy(gameObject);
    }
}
