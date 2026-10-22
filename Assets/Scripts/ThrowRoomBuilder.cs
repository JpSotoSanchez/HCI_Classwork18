using UnityEngine;

/// <summary>
/// Builds a large closed box (floor, ceiling, 4 walls) with a bouncy physics material
/// and spawns one throwable cube inside it. Put it on an empty GameObject at (0, 0, 0).
/// The XR Origin should start inside the room, near the center.
/// </summary>
public class ThrowRoomBuilder : MonoBehaviour
{
    [Header("Room")]
    [SerializeField] private Vector3 roomSize = new Vector3(8f, 5f, 8f);
    [SerializeField] private float wallThickness = 0.5f;

    [Header("Cube")]
    [SerializeField] private float cubeSize = 0.2f;
    [SerializeField] private Vector3 cubeStartPosition = new Vector3(0f, 1.1f, 0.5f);

    private void Awake()
    {
        PhysicsMaterial bouncy = new PhysicsMaterial("RoomBouncy")
        {
            bounciness = 0.6f,
            dynamicFriction = 0.3f,
            staticFriction = 0.3f,
            bounceCombine = PhysicsMaterialCombine.Maximum
        };

        BuildRoom(bouncy);
        SpawnCube(bouncy);
    }

    private void BuildRoom(PhysicsMaterial mat)
    {
        float sx = roomSize.x;
        float sy = roomSize.y;
        float sz = roomSize.z;
        float t = wallThickness;

        CreateWall("Floor", new Vector3(0f, -t / 2f, 0f), new Vector3(sx + 2f * t, t, sz + 2f * t), mat);
        CreateWall("Ceiling", new Vector3(0f, sy + t / 2f, 0f), new Vector3(sx + 2f * t, t, sz + 2f * t), mat);

        CreateWall("Wall_Left", new Vector3(-(sx / 2f + t / 2f), sy / 2f, 0f), new Vector3(t, sy, sz + 2f * t), mat);
        CreateWall("Wall_Right", new Vector3(sx / 2f + t / 2f, sy / 2f, 0f), new Vector3(t, sy, sz + 2f * t), mat);

        CreateWall("Wall_Back", new Vector3(0f, sy / 2f, -(sz / 2f + t / 2f)), new Vector3(sx, sy, t), mat);
        CreateWall("Wall_Front", new Vector3(0f, sy / 2f, sz / 2f + t / 2f), new Vector3(sx, sy, t), mat);
    }

    private void CreateWall(string wallName, Vector3 center, Vector3 size, PhysicsMaterial mat)
    {
        GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        wall.name = wallName;
        wall.isStatic = true;
        wall.transform.SetParent(transform, false);
        wall.transform.localPosition = center;
        wall.transform.localScale = size;
        wall.GetComponent<Collider>().sharedMaterial = mat;
    }

    private void SpawnCube(PhysicsMaterial mat)
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.name = "ThrowableCube";
        cube.transform.position = cubeStartPosition;
        cube.transform.localScale = Vector3.one * cubeSize;
        cube.GetComponent<Collider>().sharedMaterial = mat;

        // Adds Rigidbody and XRGrabInteractable automatically and configures them.
        cube.AddComponent<ThrowableCube>();
    }
}
