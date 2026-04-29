using System.Collections;
using UnityEngine;


public class GanController : MonoBehaviour, Yarma
{

    [SerializeField] GunData gunData;
    [SerializeField] GameObject user;
    [SerializeField] Transform barrel;


    int curAmo;
    bool canShoot = true;
    

    void Awake()
    {
        curAmo = gunData.maxAmo;
    }



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Shoot(EnemyController target) 
    {
        if (canShoot == false) return;
        Debug.Log("bang");

        RaycastHit hit;
        Vector3 origin = barrel.position;
        Vector3 direction = user.transform.forward;
        Vector3 endPoint; 

        if (Physics.Raycast(origin, direction, out hit, gunData.range))
        {
            endPoint = hit.point;

            if(hit.transform.GetComponent<EnemyController>())
            {
                hit.transform.GetComponent<EnemyController>().GetDamage(gunData.damage);
            }
        }
        else
        {
            endPoint = origin + direction * gunData.range;
        }

        --curAmo;

        Debug.Log("amo:" + curAmo);
        if (curAmo == 0) Reload();
        else StartCoroutine(WaitFireRate());

        
    }

    public void Reload() 
    {
    canShoot=false;
        StartCoroutine(Reloading());
    }
    public float GetRange() {  return 0f; }

    public void SwichWeapon() { } 

    IEnumerator WaitFireRate()
    {
        canShoot = false;
        yield return new WaitForSeconds(gunData.fireRate);
        canShoot = true;


    }
    IEnumerator Reloading()
    {
        Debug.Log("Reloading...");
        yield return new WaitForSeconds(gunData.reloadTime);
        canShoot=true;
        Debug.Log("reload!");
    }
}
