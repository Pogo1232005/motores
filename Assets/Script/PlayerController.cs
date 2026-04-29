
using System.Collections;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;

public class PlayerController : MonoBehaviour
{
     public enum TargetType
    {
        None, Floor, Enemy
    }

    public struct Target
    {
        public Target(TargetType type, RaycastHit hit)
        {
            Type = type;
            Hit = hit;
        }
       public TargetType Type;
       public RaycastHit Hit;
    
    }

    NavMeshAgent m_agent;
    DiabloInput m_input;

    InputAction m_moveAction;

    InputAction[] m_switchWeaponAction = new InputAction[3];

    [SerializeField] GameObject[] Weapons = new GameObject[3];

    Yarma currentWeapon;

    Vector3 mousePos = new Vector3();
    [SerializeField] LayerMask mask; // string layer
    Target m_target = new Target( TargetType.None, new RaycastHit());
    [SerializeField] float attack_range = 10f;
    [SerializeField] float cooldown = 1f;
    bool can_shoot = true;


    void Awake()
    {
        m_agent = GetComponent<NavMeshAgent>();
        m_input = new DiabloInput();
        m_input.Main.Enable();

        m_moveAction = m_input.Main.Move;

        m_switchWeaponAction = new InputAction[3] { m_input.Main.weapon1, m_input.Main.weapon2, m_input.Main.weapon3};
    }

    void Start()
    {
        for (int i = 0; i < Weapons.Length; i++)
        {
            if (i == 0) currentWeapon = Weapons[i].GetComponent<Yarma>();
            else
            {
                Weapons[i].SetActive(false);
            }
        }

        


    }


    void Update()
    {
        mousePos = Mouse.current.position.value;

        if(m_moveAction.WasPressedThisFrame())
        {
            MoveTo();
        }

        switch (m_target.Type)
        {
            case TargetType.Enemy:
                m_agent.destination = m_target.Hit.transform.position;
                if(Vector3.Distance(transform.position, m_agent.destination) <= currentWeapon.GetRange())
                {
                   
                    m_agent.isStopped = true;
                    transform.LookAt(m_agent.destination);
                    currentWeapon.Shoot(m_target.Hit.transform.GetComponent<EnemyController>());
                }

                break;
            case TargetType.Floor:
                break;
            case TargetType.None:
            default:
                break;

        }
        for (int i = 0; i <m_switchWeaponAction.Length; i++)
        {
            if (m_switchWeaponAction[i].WasPressedThisFrame())
            {
                if (Weapons[i] & (currentWeapon!= Weapons[i].GetComponent<Yarma>()))
                {
                    currentWeapon = Weapons[i].GetComponent<Yarma>();
                    currentWeapon.SwichWeapon();
                    break;
                }
            }
        }
        m_agent.destination = m_target.Hit.point;
        m_agent.isStopped=false;

       /* else
        {
            switch (m_target.type)
            {
                case TargetType.Enemy:
                    m_agent.destination = m_target.body.transform.position;
                    break;
                case TargetType.floor:
                default:
                    break;
            }
        }
       */
    }
    private void OnDrawGizmos()
    {
        if(m_target.Type != TargetType.None)
        {
            Gizmos.color = new Color (1f, 1f, 0f, 1f);
            Gizmos.DrawWireSphere(m_agent.destination, 0.5f);
        }
        Gizmos.color=new Color (1f,0f,0f,1f);
        if (currentWeapon != null) Gizmos.DrawWireSphere(transform.position, currentWeapon.GetRange());
    }
    void MoveTo()
    {
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(mousePos), out hit, 100,mask))
            {
                string layerName = LayerMask.LayerToName(hit.transform.gameObject.layer);
                switch (layerName)
                {
                    case "n":
                        m_target = new Target(TargetType.Enemy, hit);
                        break;
                    case "floor":
                        m_target = new Target(TargetType.None, hit);
                        break;
                        default:
                        break;


                }
                m_agent.destination = m_target.Hit.point;
                m_agent.isStopped = false;

                Debug.Log("CLICK detectado en: " + m_target.Type + hit.point);
                m_agent.destination = hit.point;
            }
            else
            {
                Debug.Log("NO detecta nada");
            }
        }
    }
   /* IEnumerator ShootCooldown()
    {
        yield return new WaitForSeconds(2f);
        can_shoot = true;
    }*/
}
/*void Shoot()
{
    can_shoot = false;
    Debug.Log("BANG!!");
    Debug.DrawLine(transform.position,m_target.Hit.transform.position,Color.yellow,0.1f);
    StartCoroutine(ShootCooldown());
*/


