using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private GunData gunData;
    [SerializeField] private Transform gunMuzzle;

    private GameManager gm;

    public bool cooldownActive;

    private float timeSinceLastShot;

    // holy moly
    private bool CanShoot() => timeSinceLastShot > 1f / (gunData.FireRate / 60f);

    public float cooldownTimer;

    public bool shooting;

    private Animator gunAnim;

    private void Start()
    {
        gunAnim = GetComponent<Animator>();
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        // very unlike my coding (funky video tutorial moment)
        PlayerShoot.shootInput += Shoot;
    }

    public void Shoot()
    {
        if (gm.currentState != GameState.INTRO && gm.currentState != GameState.RESULTS && !cooldownActive && CanShoot())
        {
            shooting = true;
            if (Physics.Raycast(gunMuzzle.position, transform.forward, out RaycastHit hitInfo, gunData.MaxDistance))
            {
                // this isn't efficient
                if (hitInfo.transform.tag == "Enemy")
                {
                    // distance between 
                    var d1 = Vector3.Distance(hitInfo.point, transform.position);

                    hitInfo.transform.GetComponent<Enemy>().TakeDamage(gunData.Damage, d1);
                }
            }

            timeSinceLastShot = 0;
            OnGunShot();
        }
        
    }

    private void OnGunShot()
    {

    }

    private void Update()
    {
        if (shooting)
        {
            cooldownTimer += Time.deltaTime / 10;
        }

        if (!shooting)
        {
            cooldownTimer -= Time.deltaTime / 10;
        }

        // the
        if (cooldownTimer < 0)
        {
            cooldownTimer = 0;
            cooldownActive = false;
        }

        if (cooldownTimer > gunData.TimeUntilCooldown)
        {
            cooldownTimer = gunData.TimeUntilCooldown;
        }

        if (cooldownTimer >= gunData.TimeUntilCooldown)
        {
            shooting = false;
            cooldownActive = true;
        }

        gunAnim.SetBool("shooting", shooting);

        timeSinceLastShot += Time.deltaTime;
    }
}
