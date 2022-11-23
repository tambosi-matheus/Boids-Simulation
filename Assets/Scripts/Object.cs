using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static InGameManager;

public class Object : MonoBehaviour
{
    public string group;

    [SerializeField] private LineRenderer speedLine, accelLine;
    private Rigidbody2D rb;
    private Vector2 aceleration;
    
    private List<Object> peersInRange;


    public float maxSpeed, maxAceleration, separationStrength, alignmentStrength, cohesionStrength;
    public float sightRange;


    private float m_sightAngle;
    public float sightAngle
    {
        get { return m_sightAngle; }
        set 
        {
            m_sightAngle = value;
            sightImage.fillAmount = sightAngle / 360;
            sightImage.rectTransform.rotation = Quaternion.Euler(0, 0, 90 + m_sightAngle / 2) * transform.rotation;
        }
    }
    public bool showVel = false, showAccel = false, showSight = false;

    [SerializeField] private Image sightImage;
    public RectTransform sightImageSize;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sightImageSize = sightImage.GetComponent<RectTransform>();
    }

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector2(Random.Range(-boidBounds.x, boidBounds.x), Random.Range(-boidBounds.y, boidBounds.y));
        rb.velocity = Vector2.up * Random.Range(-30, 30) + Vector2.right * Random.Range(-30, 30);
                
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        DrawLines();
        var pastAcel = aceleration;
        peersInRange = CheckRange();
        aceleration =            
            Alignment() * alignmentStrength +
            Separation() * separationStrength +
            Cohesion() * cohesionStrength;
        aceleration = aceleration.normalized * maxAceleration;
        aceleration += InBounds();


        if (aceleration.magnitude < 0.5f)
            aceleration = Quaternion.Euler(0, 0, Random.Range(-1, 1)) * pastAcel;
        rb.velocity += aceleration * Time.deltaTime;

        if(rb.velocity.magnitude > maxSpeed)        
            rb.velocity = rb.velocity.normalized * maxSpeed;
        

        // Make the object face the velocity vector
        if (rb.velocity != Vector2.zero)        
            transform.right = rb.velocity;
        
    }

    private void DrawLines()
    {
        if (showVel && rb.velocity.magnitude > 0.1f)
        {
            speedLine.enabled = true;
            speedLine.SetPosition(0, transform.position);
            speedLine.SetPosition(1, (Vector2)transform.position + rb.velocity / 5);
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
        if (showSight)
            sightImage.enabled = true;
        else
            sightImage.enabled = false;
        
    }

    private List<Object> CheckRange()
    {
        var inRange = new List<Object>();

        foreach (Object obj in Pooler.Instance.pools[group])
        {
            if (obj == this) continue;
            if (Vector2.Distance(transform.position, obj.transform.position) <= sightRange)
            {
                var angle = Vector2.Angle(transform.right, obj.transform.position - transform.position);                
                if(angle < 30)
                    inRange.Add(obj);            
            }
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
    
    private Vector2 InBounds()
    {
        Vector2 dist = Vector2.zero;
        if(Mathf.Abs(transform.position.x) > boidBounds.x)
            dist.x = transform.position.x > 0 ? Mathf.Pow(boidBounds.x - transform.position.x, 3) : Mathf.Pow(-boidBounds.x - transform.position.x, 3);
        if(Mathf.Abs(transform.position.y) > boidBounds.y)                    
            dist.y = transform.position.y > 0 ? Mathf.Pow(boidBounds.y - transform.position.y, 3) : Mathf.Pow(-boidBounds.y - transform.position.y, 3);        
        return dist;
    }
}
