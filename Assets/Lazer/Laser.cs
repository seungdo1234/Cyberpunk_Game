using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private float defDistanceRay = 100f;
    public Transform laserFirePoint;
    public Transform laserEndPoint;
    public LineRenderer lineRenderer;
    Transform transform;

    // Start is called before the first frame update
    void Start()
    {
        transform = GetComponent<Transform>();    
    }

     void ShootLaser()
    {
        lineRenderer.SetPosition(0, laserFirePoint.position);
        lineRenderer.SetPosition(1, laserEndPoint.position);
    }
   
    void Draw2DRay(Vector2 startPos, Vector2 endPos)
    {
        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(1, endPos);
    }
    private void Update()
    {
        ShootLaser();
    }
}
