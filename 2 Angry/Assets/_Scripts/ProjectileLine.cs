using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLine : MonoBehaviour
{
    static public ProjectileLine S; // Singleton

    [Header("Set in Inspector")]
    public float minDist = 0.1f;

    private LineRenderer line; // trailer
    private GameObject _poi; // Object to be trail
    private List<Vector3> points; // List for saving points

    void Awake()
    {
        S = this;

        //Disable the LineRenderer until it's need.
        line = GetComponent<LineRenderer>();
        line.enabled = false;

        points = new List<Vector3>();

    }

    //Property
    public GameObject poi
    {
        get
        {
            return _poi;
        }

        set
        {
            _poi = value;
            if(_poi != null)
            {
                //set to something new
                line.enabled = false;
                points = new List<Vector3>();
                AddPoint();
            }
        }
    }

    //return the location of the most recently added point
    public Vector3 lastPoint
    {
        get
        {
            if (points == null)
            {
                return Vector3.zero;
            }
            return points[points.Count - 1];
        }

    }

    //this can be used to clear the line
    public void Clear()
    {
        _poi = null;
        line.enabled = false;
        points = new List<Vector3>();

    }

    public void AddPoint()
    {
        Vector3 pt = _poi.transform.position;

        //if the point isn't far enough from the last point, it returns
        if(points.Count > 0 && (pt - lastPoint).magnitude < minDist)
        {
            return;
        }
        if(points.Count == 0)
        {
            Vector3 launchPosDiff = pt - Slingshot.LAUNCH_POS;

            //it adds an extra bit of line to aid aiming later 
            points.Add(pt + launchPosDiff);
            points.Add(pt);
            line.positionCount = 2;

            //sets the first two points
            line.SetPosition(0, points[0]);
            line.SetPosition(1, points[1]);

            //enable LineRenderer
            line.enabled = true;

        }
        else
        {
            //normal adding of point
            points.Add(pt);
            line.positionCount = points.Count;
            line.SetPosition(points.Count - 1, lastPoint);
            line.enabled = true;
        }
    }




    void FixedUpdate()
    {
        if(poi == null)
        {
            //if there is no poi, then search
            if(FollowCam.POI != null)
            {
                if(FollowCam.POI.tag == "Projectile")
                {
                    poi = FollowCam.POI;
                }
                else
                {
                    //there is no poi
                    return;
                }
            }
            else
            {
                //there is no poi
                return;
            }
        }

        //if there is no poi, it's loc is added every fixedupdate
        AddPoint();
        if(FollowCam.POI == null)
        {
            //once followcam POi is null, make the local poi null too
            poi = null;
        }
    }



}
