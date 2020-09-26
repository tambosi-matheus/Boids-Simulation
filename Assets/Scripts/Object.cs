using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Object : MonoBehaviour
{
    public string group;

    Transform destination;

    Rigidbody2D rb;
    Renderer rend;
    private Vector2 aceleration;
    public float sightRange;
    private List<GameObject> peersInRange;

    public float maxSpeed;
    public float maxAceleration;
    public float destinationSpeed;
    public float separationStrength;
    public float alignmentStrength;
    public float cohesionStrength;



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rend = GetComponent<Renderer>();

        transform.position = new Vector2(Random.Range(-50, 50), Random.Range(-50, 50));
        rb.velocity = Vector2.up * Random.Range(-30, 30) + Vector2.right * Random.Range(-30, 30);
        destination = GameObject.FindGameObjectWithTag("Destination").transform;        
    }

    // Update is called once per frame
    void Update()
    {
        peersInRange = CheckRange();
        aceleration = Alignment() * alignmentStrength + Separation() * separationStrength + Cohesion() * cohesionStrength ;
        aceleration += (Vector2)((destination.position - transform.position).normalized * destinationSpeed) / 8;

        if (aceleration.magnitude > maxAceleration)        
            aceleration = aceleration.normalized * maxAceleration;  
        
        if (aceleration.magnitude > 0.1)                  
            rb.velocity += aceleration;        
        
        
        if(rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }

        // make the object face the velocity vector
        if (rb.velocity != Vector2.zero)
        {
            transform.up = rb.velocity;
        }
        if (!rend.isVisible)
            rb.velocity = -rb.velocity;
    }

    private List<GameObject> CheckRange()
    {
        List<GameObject> inRange = new List<GameObject>();

        foreach (GameObject obj in Pooler.Instance.pools[group])
        {
            if (obj != gameObject && Vector2.Distance(transform.position, obj.transform.position) <= sightRange)
            {

                inRange.Add(obj);
            }
        }

        return inRange;
    }


    private Vector2 Separation()
    {
        Vector2 separation = new Vector2(0, 0);

        foreach(GameObject peer in peersInRange)
        {
            Vector2 vec = (Vector2)(transform.position - peer.transform.position);
            vec /= Vector2.Distance(transform.position, peer.transform.position);
            separation += vec;
        }
        
        separation /= (peersInRange.Count);
        
        return separation;
    }

    private Vector2 Alignment()
    {
        Vector2 alignment = new Vector2(0, 0);

        foreach (GameObject peer in peersInRange)
        {
            Vector2 vec;
            vec = (Vector2)(peer.GetComponent<Rigidbody2D>().velocity);
            vec /= Vector2.Distance(transform.position, peer.transform.position);
            alignment += vec;
        }

        alignment /= peersInRange.Count;

        return alignment;
    }

    private Vector2 Cohesion()
    {
        Vector2 cohesion = new Vector2(0, 0);

        foreach (GameObject peer in peersInRange)
        {
            Vector2 vec;
            vec = (Vector2)(peer.transform.position - transform.position);
            vec /= Vector2.Distance(transform.position, peer.transform.position);
            cohesion += vec;
        }

        cohesion /= peersInRange.Count;

        return cohesion;
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
