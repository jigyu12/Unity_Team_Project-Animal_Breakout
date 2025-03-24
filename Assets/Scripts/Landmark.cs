using UnityEngine;

public class Landmark : MonoBehaviour
{
    [SerializeField] private GameObject sideStructure;
    [SerializeField] private GameObject wall;
    [SerializeField] private GameObject swipeArea;

    [SerializeField] private GameObject leftLaneStartPos;
    [SerializeField] private GameObject rightLaneStartPos;

    [SerializeField] private int sideStructureCount;

    private void Start()
    {
        SetLaneStartPositions();
        CreateRandomLandmark();
    }

    private void SetLaneStartPositions()
    {
        float halfPlaneLength = GetParentPlaneLength() * 0.5f;

        Vector3 startPoint = transform.position - transform.forward * halfPlaneLength;

        leftLaneStartPos.transform.position = new Vector3(
            leftLaneStartPos.transform.position.x,
            leftLaneStartPos.transform.position.y,
            startPoint.z
        );

        rightLaneStartPos.transform.position = new Vector3(
            rightLaneStartPos.transform.position.x,
            rightLaneStartPos.transform.position.y,
            startPoint.z
        );
    }

    private void CreateRandomLandmark()
    {
        WayType wayType = (WayType)Random.Range(0, (int)WayType.Count);

        int sideStructureCountSub = sideStructureCount / 3;

        float totalDistance = GetParentPlaneLength();

        float gapBetweenStructures = totalDistance / sideStructureCount;

        Vector3 midPoint = (leftLaneStartPos.transform.position + rightLaneStartPos.transform.position) * 0.5f;

        switch (wayType)
        {
            case WayType.Straight:
                {
                    CreateLaneStructures(leftLaneStartPos.transform, gapBetweenStructures);
                    CreateLaneStructures(rightLaneStartPos.transform, gapBetweenStructures);
                    break;
                }
            case WayType.Left:
                {
                    CreateLaneStructures(leftLaneStartPos.transform, gapBetweenStructures, sideStructureCountSub);
                    CreateLaneStructures(rightLaneStartPos.transform, gapBetweenStructures);

                    Vector3 wallPos = midPoint + (transform.forward * totalDistance);
                    Instantiate(wall, wallPos, Quaternion.identity, transform);

                    Vector3 swipeAreaPos = wallPos - transform.forward * (gapBetweenStructures * 0.5f);
                    GameObject swipe = Instantiate(swipeArea, swipeAreaPos, Quaternion.identity, transform);

                    SwipeTurnTrigger trigger = swipe.GetComponent<SwipeTurnTrigger>();
                    if (trigger != null)
                        trigger.allowedDirection = TurnDirection.Left;

                    break;
                }
            case WayType.Right:
                {
                    CreateLaneStructures(leftLaneStartPos.transform, gapBetweenStructures);
                    CreateLaneStructures(rightLaneStartPos.transform, gapBetweenStructures, sideStructureCountSub);

                    Vector3 wallPos = midPoint + (transform.forward * totalDistance);
                    Instantiate(wall, wallPos, Quaternion.identity, transform);

                    Vector3 swipeAreaPos = wallPos - transform.forward * (gapBetweenStructures * 0.5f);
                    GameObject swipe = Instantiate(swipeArea, swipeAreaPos, Quaternion.identity, transform);

                    SwipeTurnTrigger trigger = swipe.GetComponent<SwipeTurnTrigger>();
                    if (trigger != null)
                        trigger.allowedDirection = TurnDirection.Right;

                    break;
                }
            case WayType.UnavoidableWall:
                {
                    CreateLaneStructures(leftLaneStartPos.transform, gapBetweenStructures);
                    CreateLaneStructures(rightLaneStartPos.transform, gapBetweenStructures);

                    Vector3 wallPos = midPoint + (transform.forward * totalDistance);
                    Instantiate(wall, wallPos, Quaternion.identity, transform);
                    break;
                }
        }
    }

    private void CreateLaneStructures(Transform laneStartTransform, float gapBetweenStructures, int sideStructureCountSub = 0)
    {
        for (int i = 0; i < sideStructureCount - sideStructureCountSub; i++)
        {
            Vector3 position = laneStartTransform.position + transform.forward * (gapBetweenStructures * i);
            Instantiate(sideStructure, position, Quaternion.identity, laneStartTransform);
        }
    }

    private float GetParentPlaneLength()
    {
        return transform.parent.localScale.z * 5f; // 10f is original plane size
    }
}
