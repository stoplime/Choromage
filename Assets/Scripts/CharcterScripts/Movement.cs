using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Movement : MonoBehaviour{
    // coefficient of friction
    private float mu;
    private float normalForce;

    private float speed;
    private Vector2 pos;
    private Quaternion facing;

    private bool targetInRange = false;
    private int targetSightIndex = -1;

    private Vector3 velocity;

    private bool? strafe;

    private Rigidbody rb;

    private GameObject eyes;

    public float Speed
    {
        get
        {
            return speed;
        }

        set
        {
            speed = value;
        }
    }

    public Vector2 Pos
    {
        get
        {
            return pos;
        }

        set
        {
            pos = value;
        }
    }

    public Quaternion Facing
    {
        get
        {
            return facing;
        }

        set
        {
            facing = value;
        }
    }

    public bool TargetInRange
    {
        get
        {
            return targetInRange;
        }

        set
        {
            targetInRange = value;
        }
    }

    public int TargetSightIndex
    {
        get
        {
            return targetSightIndex;
        }

        set
        {
            targetSightIndex = value;
        }
    }

    private void Start() {
        rb = GetComponent<Rigidbody>();
    }

    public void SpotTarget(GameObject target, float distance)
    {
        //RaycastHit[] hits = Physics.RaycastAll(transform.position, target.transform.position - transform.position, distance, 1 << 8).OrderBy(h=>h.distance).ToArray();
        //print(eyes);
        if (eyes != null)
        {
            //print("eyes");
            RaycastHit[] hits = Physics.RaycastAll(eyes.transform.position, target.transform.position - eyes.transform.position, distance).OrderBy(h => h.distance).ToArray();
            //print();
            List<RaycastHit> hitters = hits.ToList();
            //GameObject.Find("Debugger").GetComponent<Text>().text = Help.print(gameObject.name, hitters.Count, hitters);
            //Help.print(gameObject.name, hitters.Count, hitters);
            TargetInRange = false;
            TargetSightIndex = -1;
            for (int i = 0; i < hits.Length; i++)
            {
                //print(hits[i].transform.gameObject);
                if (hits[i].transform.gameObject == target)
                {
                    TargetInRange = true;
                    TargetSightIndex = i;
                    // GameObject.Find("Debugger").GetComponent<Text>().text = Help.print(gameObject.name, hitters.Count, hitters, string.Format("\n[{0}]",TargetSightIndex));
                    break;
                }
            }
        }
        else
        { 
            RaycastHit[] hits = Physics.RaycastAll(transform.position, target.transform.position - transform.position, distance).OrderBy(h => h.distance).ToArray();
            //print();
            List<RaycastHit> hitters = hits.ToList();
            //GameObject.Find("Debugger").GetComponent<Text>().text = Help.print(gameObject.name, hitters.Count, hitters);
            Help.print(gameObject.name, hitters.Count, hitters);
            TargetInRange = false;
            TargetSightIndex = -1;
            for (int i = 0; i < hits.Length; i++)
            {
                //print(hits[i].transform.gameObject);
                if (hits[i].transform.gameObject == target)
                {
                    TargetInRange = true;
                    TargetSightIndex = i;
                    // GameObject.Find("Debugger").GetComponent<Text>().text = Help.print(gameObject.name, hitters.Count, hitters, string.Format("\n[{0}]",TargetSightIndex));
                    break;
                }
            }
        }
    }

    private void Update()
    {
        UpdateVelocity();
        //print(gameObject.name);
        //print(velocity);
        //transform.position += velocity;
        //rb.AddForce(transform.position + velocity * Time.deltaTime);
        rb.velocity += velocity;
        if (gameObject.name == "wolf")
        {
            //Help.LiveDebugText(rb.velocity, velocity);
            //Help.print(rb.velocity, velocity);
        }
        
    }

    public void CreateMovement(float mu, GameObject eye)
    {
        this.mu = mu;

        normalForce = Physics.gravity.magnitude;
        velocity = Vector3.zero;
        strafe = null;

        eyes = eye;
    }

    private void UpdateVelocity()
    {
        // Calculate change in velocity
        velocity += (velocity - (facing * Vector3.forward * speed)) * Time.deltaTime;

        // Set to zero when too low
        if (velocity.sqrMagnitude < 0.001)
        {
            velocity = Vector3.zero;
        }
        else
        {
            // Friction
            velocity += (-velocity).normalized * mu * normalForce * Time.deltaTime;
        }
    }

    public void Strafe(bool left)
    {
        if (left)
        {
            velocity += (velocity - (facing * Vector3.left * speed)) * Time.deltaTime;
        }
        else
        {
            velocity += (velocity - (facing * Vector3.right * speed)) * Time.deltaTime;
        }
    }
}
