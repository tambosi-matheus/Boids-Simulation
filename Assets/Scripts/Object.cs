using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Object : MonoBehaviour
{
    public string group;

    [SerializeField] private LineRenderer speedLine, accelLine;
    private Rigidbody2D rb;
    [SerializeField] private Vector2 aceleration;
    public float sightRange;
    private List<Object> peersInRange;

    public float maxSpeed, maxAceleration, 
        separationStrength, alignmentStrength, cohesionStrength;

    public bool onScreen { get; private set; } = true;
    public bool showVel = false, showAccel = false;



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        transform.position = new Vector2(Random.Range(-50, 50), Random.Range(-50, 50));
        rb.velocity = Vector2.up * Random.Range(-30, 30) + Vector2.right * Random.Range(-30, 30);   
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        DrawLines();
        peersInRange = CheckRange();
        aceleration = 
            -(Vector2)transform.position.normalized +
            Alignment() * alignmentStrength + 
            Separation() * separationStrength + 
            Cohesion() * cohesionStrength;
        aceleration = aceleration.normalized * 50;
        
        if (aceleration.magnitude > maxAceleration)
            aceleration = aceleration.normalized * maxAceleration;  
        
        if (aceleration.magnitude > 0.1)                  
            rb.velocity += aceleration * Time.deltaTime;        
                
        if(rb.velocity.magnitude > maxSpeed)        
            rb.velocity = rb.velocity.normalized * maxSpeed;
        

        // Make the object face the velocity vector
        if (rb.velocity != Vector2.zero)
        {
            transform.right = rb.velocity;
        }
        //if (!rend.isVisible)
        //    rb.velocity = (destination.position - transform.position).normalized * maxSpeed;
    }

    private void DrawLines()
    {
        if (showVel && rb.velocity.magnitude > 0.1f)
        {
            speedLine.enabled = true;
            speedLine.SetPosition(0, transform.position);
            speedLine.SetPosition(1,
                (Vector2)transform.position + rb.velocity.normalized * rb.velocity.magnitude / 5);
        }
        else
            speedLine.enabled = false;
        if (showAccel && aceleration.magnitude > 0.1f)
        {
            accelLine.enabled = true;
            accelLine.SetPosition(0, transform.position);
            accelLine.SetPosition(1, (Vector2)transform.position + aceleration.normalized * 5);
        }
        else
            accelLine.enabled = false;
    }

    private List<Object> CheckRange()
    {
        var inRange = new List<Object>();

        foreach (Object obj in Pooler.Instance.pools[group])
        {
            if (obj == this) continue;
            if (onScreen && Vector2.Distance(transform.position, obj.transform.position) <= sightRange)            
                inRange.Add(obj);            
        }

        return inRange;
    }


    private Vector2 Separation()
    {
        Vector2 separation = new Vector2(0, 0);

        foreach(Object peer in peersInRange)
        {
            var vec = (Vector2)(transform.position - peer.transform.position);
            vec /= Vector2.Distance(transform.position, peer.transform.position);
            separation += vec;
        }

        separation = separation.normalized;
        
        return separation;
    }

    private Vector2 Alignment()
    {
        Vector2 alignment = new Vector2(0, 0);

        foreach (Object peer in peersInRange)
        {
            var vec = (Vector2)(peer.GetComponent<Rigidbody2D>().velocity);
            vec /= Vector2.Distance(transform.position, peer.transform.position);
            alignment += vec;
        }

        alignment = alignment.normalized;

        return alignment;
    }

    private Vector2 Cohesion()
    {
        var cohesion = new Vector2(0, 0);

        foreach (Object peer in peersInRange)
        {
            var vec = (Vector2)(peer.transform.position - transform.position);
            vec /= Vector2.Distance(transform.position, peer.transform.position);
            cohesion += vec;
        }

        cohesion = cohesion.normalized;

        return cohesion;
    }     

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.gameObject.tag.Equals("Boid"))
        {
            //rb.velocity = -rb.velocity;
            onScreen = false;
            transform.position = -transform.position;
        }
            
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.tag.Equals("Boid"))        
            onScreen = true;        
    }
}
