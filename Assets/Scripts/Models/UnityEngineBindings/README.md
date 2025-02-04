# Bound Classes for defined in UnityEngine

Cause Unity Engine-defined classes cannot add attributes, there are some classes defined seperately to add attributes like `[ProtoContract]` or `[ProtoMember]` to them.

Basically, these classes are not completely same as the original classes (not defined methods), but support the type casting between the original classes and the binding classes easily.

```cs
Vector3Model vector3Model = new Vector3Model(1, 2, 3);
Vector3 vector3 = vector3Model;
Vector3Model newVector3Model = vector3;
```
