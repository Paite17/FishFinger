using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private GunData gunData_;
    [SerializeField] private Transform gunMuzzle;

    private GameManager gm;

    public bool cooldownActive;

    private float timeSinceLastShot;

    // holy moly
    private bool CanShoot() => timeSinceLastShot > 1f / (gunData_.FireRate / 60f);

    public float shootingTimer;

    public float cooldownTimer;

    public bool shooting;

    private Animator gunAnim;

    public GunData GunData_
    {
        get { return gunData_; }
    }

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
            Debug.Log("Shooting");
            shooting = true;
            if (Physics.Raycast(gunMuzzle.position, transform.forward, out RaycastHit hitInfo, gunData_.MaxDistance))
            {
                // this isn't efficient
                if (hitInfo.transform.tag == "Enemy")
                {
                    // distance between 
                    var d1 = Vector3.Distance(hitInfo.point, transform.position);

                    hitInfo.transform.GetComponent<Enemy>().TakeDamage(gunData_.Damage, d1);
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
            shootingTimer += Time.deltaTime / 10;
        }

        if (!shooting)
        {
            shootingTimer -= Time.deltaTime / 10;
        }

        if (cooldownActive)
        {
            cooldownTimer -= Time.deltaTime / 10;
        }

        // the
        if (shootingTimer <= 0)
        {
            shootingTimer = 0;
        }

        if (cooldownTimer <= 0 && cooldownActive)
        {
            cooldownActive = false;
        }

        if (shootingTimer > gunData_.TimeUntilCooldown)
        {
            shootingTimer = gunData_.TimeUntilCooldown;
        }

        if (shootingTimer >= gunData_.TimeUntilCooldown && !cooldownActive)
        {
            shooting = false;
            cooldownActive = true;
            cooldownTimer = gunData_.CooldownLength;
            shootingTimer = 0;
        }


        if (gunAnim != null)
        {
            gunAnim.SetBool("shooting", shooting);
        }
        

        timeSinceLastShot += Time.deltaTime;
    }
}
