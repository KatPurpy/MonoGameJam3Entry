using System.Numerics;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

public static class NumericToXNA
{
    public static Microsoft.Xna.Framework.Vector2 ConvertNumericToXNA(System.Numerics.Vector2 vector) => new(vector.X, vector.Y);

    public static Microsoft.Xna.Framework.Vector3 ConvertNumericToXNA(System.Numerics.Vector3 vector) => new (vector.X, vector.Y, vector.Z);
        
    public static Microsoft.Xna.Framework.Vector4 ConvertNumericToXNA(System.Numerics.Vector4 vector) => new (vector.X, vector.Y, vector.Z, vector.W);
        
    public static Microsoft.Xna.Framework.Quaternion ConvertNumericToXNA(System.Numerics.Quaternion quaternion) => new (quaternion.X, quaternion.Y, quaternion.Z, quaternion.W);
        
    public static Matrix ConvertNumericToXNA(System.Numerics.Matrix4x4 matrix) => new Matrix(matrix.M11, matrix.M12, matrix.M13, matrix.M14, matrix.M21, matrix.M22, matrix.M23, matrix.M24, matrix.M31, matrix.M32, matrix.M33, matrix.M34, matrix.M41, matrix.M42, matrix.M43, matrix.M44);

    public static System.Numerics.Vector2 ConvertXNAToNumeric(Microsoft.Xna.Framework.Vector2 vector) => new(vector.X, vector.Y);

    public static System.Numerics.Vector3 ConvertXNAToNumeric(Microsoft.Xna.Framework.Vector3 vector) => new (vector.X, vector.Y, vector.Z);

    public static System.Numerics.Vector4 ConvertXNAToNumeric(Microsoft.Xna.Framework.Vector4 vector) => new (vector.X, vector.Y, vector.Z, vector.W);

    public static System.Numerics.Quaternion ConvertXNAToNumeric(Microsoft.Xna.Framework.Quaternion quaternion) => new (quaternion.X, quaternion.Y, quaternion.Z, quaternion.W);



/*public static IEnumerable<Microsoft.Xna.Framework.Vector3> ConvertNumericToXNA(IEnumerable<System.Numerics.Vector3> v) => v.Select(v => v.toXNA());
        
public static IEnumerable<Microsoft.Xna.Framework.Vector4> ConvertNumericToXNA(IEnumerable<System.Numerics.Vector4> v) => v.Select(v => v.toXNA());
        
public static IEnumerable<Microsoft.Xna.Framework.Quaternion> ConvertNumericToXNA(IEnumerable<System.Numerics.Quaternion> v) => v.Select(v => v.toXNA());
        
public static IEnumerable<Microsoft.Xna.Framework.Matrix> ConvertNumericToXNA(IEnumerable<Matrix4x4> v) => v.Select(v => v.toXNA());
*/
}

