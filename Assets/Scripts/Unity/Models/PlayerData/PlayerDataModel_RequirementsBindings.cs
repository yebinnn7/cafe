using UnityEngine;
using ProtoBuf;

/// <summary>
/// Temporary migrated class for `Data` class
/// 
/// ACTION REQUIRED: Remodeling this unclear class is required.
///  1. Clarify the purpose of the `Data` class.
///  2. Make new class that represents the purpose of the `Data` class.
///  3. Migrate and remove the `Data` class and this.
/// </summary>
[System.Serializable]
[ProtoContract]
public class DataMigrated
{
    [ProtoMember(1)]
    public int id;
    [ProtoMember(2)]
    public int level;
    [ProtoMember(3)]
    public float exp;
    [ProtoMember(4)]
    public Vector3Model pos;
    [ProtoMember(5)]
    public int[] machine_level;

    public DataMigrated(Vector3 pos, int id, int level, float exp)
    {
        this.pos = pos;
        this.id = id;
        this.level = level;
        this.exp = exp;
        this.machine_level = machine_level ?? new int[5];
    }

    public static implicit operator DataMigrated(Data data)
    {
        return new DataMigrated(data.pos, data.id, data.level, data.exp);
    }

    public static implicit operator Data(DataMigrated data)
    {
        return new Data(data.pos, data.id, data.level, data.exp);
    }
}
