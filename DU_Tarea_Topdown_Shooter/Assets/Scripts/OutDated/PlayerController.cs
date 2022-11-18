using System.Collections;
using DefaultNamespace;
using UnityEngine;

public class PlayerController : MonoBehaviour, IKillable
{
    private Camera _cam;
    
    public Transform movementTrans;
    public Transform bulletSpawner;
    public GameObject bullet;
    
    public float playerSpeed;
    public float bulletSpeed;
    
    private Vector3 _movement;

    private const int layerMask = 1 << 9;

    private RaycastHit hit;
    private Ray ray;

    private void Awake()
    {
        Player = this;
        _cam = Camera.main;
    }

    private void Update()
    {
        _movement = Input.GetAxis("Horizontal") * transform.right.normalized +
                    Input.GetAxis("Vertical") * transform.up.normalized;
        _movement.y = 0.0f;

        movementTrans.Translate(_movement.normalized * (playerSpeed * Time.deltaTime));

        if (Input.GetButtonDown("Fire1")) StartCoroutine(Gun());
        if (Input.GetButtonUp("Fire1")) StopCoroutine(Gun());

        Aim();
    }

    private void Shoot()
    {
        var tempBullet = Instantiate(bullet, bulletSpawner.position, bulletSpawner.rotation) as GameObject;
        var rg = tempBullet.GetComponent<Rigidbody>();
        rg.AddForce(bulletSpawner.transform.forward * bulletSpeed);
        Destroy(tempBullet, 5.0f);
    }

    private IEnumerator Gun()
    {
        while (Input.GetButton("Fire1"))
        {
            Shoot();
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void Aim()
    {
        ray = _cam.ScreenPointToRay(Input.mousePosition);
        
        if(Physics.Raycast(ray, out hit, layerMask))
        {
            var lookDir = new Vector3(hit.point.x, transform.position.y, hit.point.z);
            transform.LookAt(lookDir);
            Debug.DrawLine(transform.position, hit.point, Color.red);
            Debug.DrawRay(ray.origin, ray.direction * 100f, Color.white);
        }
    }
    
    public static PlayerController Player { get; private set; }
    public void Die()
    {
        SceneLoader.ReloadScene();
    }
}
