using UnityEngine;

public class MazeBrain : MonoBehaviour
{
    public DNA dna;
    public GameObject eyes;
    public float distanceTravelled;
    
    private int DNALength;
    private bool seeWall, alive;
    private Vector3 startPosition;

    public void Init()
    {
        distanceTravelled = 0;
        DNALength = 2;
        seeWall = true;
        alive = true;
        
        dna = new DNA(DNALength, 360);
        startPosition = this.transform.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("dead"))
        {
            distanceTravelled = 0;
            alive = false;
        }
    }

    private void Update()
    {
        if (!alive) return;

        seeWall = false;
        RaycastHit eyesHit;
        Debug.DrawRay(eyes.transform.position, eyes.transform.forward * 0.5f, Color.red);
        if(Physics.SphereCast(eyes.transform.position, 0.1f, eyes.transform.forward, out eyesHit, 0.5f))
        {
            if (eyesHit.collider.CompareTag("wall"))
            {
                seeWall = true;
            }
        }
    }

    private void FixedUpdate()
    {
        if (!alive) return;

        float h = 0;
        float v = dna.GetGene(0);

        if (seeWall)
        {
            h = dna.GetGene(1);
        }

        this.transform.Translate(0, 0, v * 0.001f);
        this.transform.Rotate(0, h, 0);
        distanceTravelled = Vector3.Distance(startPosition, this.transform.position);
    }

}
