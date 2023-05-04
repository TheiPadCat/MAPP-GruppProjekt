using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental;
using UnityEngine;

public class JeolTentacle : MonoBehaviour
{
    [SerializeField] Transform end;
    [SerializeField] Transform start;
    [SerializeField] Transform controll1;
    [SerializeField] Transform controll2;

    private SpringJoint2D startJoint;
    private SpringJoint2D controll1Joint;
    private SpringJoint2D controll2Joint;

    private List<SpringJoint2D> joints = new List<SpringJoint2D>();


    [SerializeField] float tentacleLength;
    [SerializeField] int AmountOfPoints;

    private Vector3[] bezierPoints;
    public LineRenderer lineRenderer;

  
    // public Gradient gradient;



    Vector3 final;

    // Start is called before the first frame update
    void Start()
    {

        startJoint = start.GetComponent<SpringJoint2D>();
        controll1Joint = controll1.GetComponent<SpringJoint2D>();
        controll2Joint = controll2.GetComponent<SpringJoint2D>();
        joints.Add(startJoint);
        joints.Add(controll1Joint);
        joints.Add(controll2Joint);


        start.GetComponent<SpringJoint2D>().distance = tentacleLength / 3;
        controll1.GetComponent<SpringJoint2D>().distance = tentacleLength / 3;
        controll2.GetComponent<SpringJoint2D>().distance = tentacleLength / 3;
        //DENNA CHILD COUNT MÅSTE SKRIVAR OM DEN SUGER
        /* for (int i = 0; i < transform.childCount - 2; i++)
         {
             transform.GetChild(i).GetComponent<SpringJoint2D>().distance = tentacleLength / 3;
         }
        */

        bezierPoints = new Vector3[AmountOfPoints];
        // lineRenderer = transform.GetComponentInChildren<LineRenderer>();
        if (lineRenderer == null)
        {
            Debug.Log("bruuuuuh");
        }

    }

    // Update is called once per frame
    void Update()
    {

   
        calculateBezier();
        drawLine();
    }



    private void calculateBezier()
    {
        float t = 0;

        for (int i = 0; i < AmountOfPoints; i++)
        {
            t += (tentacleLength / AmountOfPoints) / tentacleLength;

            //skriv om med riktiga bezier formel
            Vector3 point1 = Vector3.Lerp(start.position, controll1.position, t);
            Vector3 point2 = Vector3.Lerp(controll1.position, controll2.position, t);
            Vector3 point3 = Vector3.Lerp(controll2.position, end.position, t);
            Vector3 lerp1 = Vector3.Lerp(point1, point2, t);
            Vector3 lerp2 = Vector3.Lerp(point2, point3, t);
      
            final = Vector3.Lerp(lerp1, lerp2, t);
            final.z = 0;
            bezierPoints[i] = final;

        }
    }


    private void drawLine()
    {
        //    bezierPoints[0] = start.position;
        lineRenderer.positionCount = bezierPoints.Length;
        //lineRenderer.SetPosition(0, start.position);
        lineRenderer.SetPositions(bezierPoints);
    }

    private void OnDrawGizmos()
    {

        // Gizmos.color = Color.cyan;
        // Gizmos.DrawWireSphere(final, 2);
    }

    public void setSprings(float distance, float damping, float frequency)
    {
        startJoint.distance = distance;

        foreach (SpringJoint2D s in joints)
        {
            s.distance = distance;
            s.dampingRatio = damping;
            s.frequency = frequency;
        }

    }
   
}
