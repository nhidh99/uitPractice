﻿using UnityEngine;

public class Egg : MonoBehaviour
{
    private float spawnX;
    private float spawnY;
    private Rigidbody2D rb2d;

    public bool isRespawning = true;
    private float spawnRate;
    private float timeSinceLastSpawned = 0f;

    public static float gravityScaleMin = 0.3f;
    public static float gravityScaleMax = 1.2f;
    public static float spawnRateMin = 0.5f;
    public static float spawnRateMax = 2.0f;

    public AudioSource audio;
    public AudioClip scoreSound;
    public AudioClip diedSound;

    void Start()
    {
        spawnX = transform.position.x;
        spawnY = transform.position.y;
        spawnRate = Random.Range(spawnRateMin, spawnRateMax);
        rb2d = GetComponent<Rigidbody2D>();
        audio = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (GameControl.instance.gameOver) return;
        if (isRespawning)
        {
            timeSinceLastSpawned += Time.deltaTime;
            if (timeSinceLastSpawned >= spawnRate)
            {
                GameControl.instance.BirdCount();
                isRespawning = false;
                timeSinceLastSpawned = 0;
                rb2d.gravityScale = Random.Range(gravityScaleMin, gravityScaleMax);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        isRespawning = true;
        spawnRate = Random.Range(spawnRateMin, spawnRateMax);
        rb2d.velocity = Vector2.zero;
        rb2d.gravityScale = 0f;
        transform.position = new Vector2(spawnX, spawnY);

        int spriteIndex = Random.Range(0, GameControl.instance.sprites.Length);
        Sprite sprite = GameControl.instance.sprites[spriteIndex];
        GetComponent<SpriteRenderer>().sprite = sprite;


        if (other.gameObject.tag == "Basket")
        {
            GameControl.instance.BirdScored();
            audio.clip = scoreSound;
            audio.Play();
        }
        else
        {
            audio.clip = diedSound;
            audio.Play();
        }
    }
}