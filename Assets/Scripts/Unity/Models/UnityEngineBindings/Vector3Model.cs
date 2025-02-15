using UnityEngine;
using ProtoBuf;

/// <summary>
/// UnityEngine's Vector3 Bindings
/// Vector3 is a struct that represents a point in 3D space.
/// 
/// Why this exists?
/// Unity's Vector3 is not serializable by protobuf-net.
/// 
/// Is this has methods like Unity's Vector3?
/// No, this is just a data transfer object.
/// You must cast this to Vector3 to use it as a Vector3.
/// 
/// <example>
/// Vector3Model vector3Model = new Vector3Model(1, 2, 3);
/// Vector3 vector3 = (Vector3)vector3Model;
/// </example>
/// </summary>
[System.Serializable]
[ProtoContract]
public class Vector3Model
{
    [ProtoMember(1)]
    public float x;
    [ProtoMember(2)]
    public float y;
    [ProtoMember(3)]
    public float z;

    public Vector3Model(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public Vector3Model(Vector3 vector3)
    {
        this.x = vector3.x;
        this.y = vector3.y;
        this.z = vector3.z;
    }

    public Vector3 ToVector3()
    {
        return new Vector3(x, y, z);
    }

    public static implicit operator Vector3(Vector3Model vector3Model)
    {
        return vector3Model.ToVector3();
    }

    public static implicit operator Vector3Model(Vector3 vector3)
    {
        return new Vector3Model(vector3);
    }
}
