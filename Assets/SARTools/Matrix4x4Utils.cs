using UnityEngine;
using System.Collections;

public class Matrix4x4Utils
{

    // FROM http://forum.unity3d.com/threads/how-to-assign-matrix4x4-to-transform.121966/#post-1830992
    // And https://github.com/lordofduct/spacepuppy-unity-framework/blob/master/SpacepuppyBase/Utils/TransformUtil.cs

    /// <summary>
    /// Extract translation from transform matrix.
    /// </summary>
    /// <param name="matrix">Transform matrix. This parameter is passed by reference
    /// to improve performance; no changes will be made to it.</param>
    /// <returns>
    /// Translation offset.
    /// </returns>
    public static Vector3 ExtractTranslationFromMatrix(ref Matrix4x4 matrix)
    {
        Vector3 translate;
        translate.x = matrix.m03;
        translate.y = matrix.m13;
        translate.z = matrix.m23;
        return translate;
    }

    /// <summary>
    /// Extract rotation quaternion from transform matrix.
    /// </summary>
    /// <param name="matrix">Transform matrix. This parameter is passed by reference
    /// to improve performance; no changes will be made to it.</param>
    /// <returns>
    /// Quaternion representation of rotation transform.
    /// </returns>
    public static Quaternion ExtractRotationFromMatrix(ref Matrix4x4 matrix)
    {
        Vector3 forward;
        forward.x = matrix.m02;
        forward.y = matrix.m12;
        forward.z = matrix.m22;

        Vector3 upwards;
        upwards.x = matrix.m01;
        upwards.y = matrix.m11;
        upwards.z = matrix.m21;

        return Quaternion.LookRotation(forward, upwards);
    }

    /// <summary>
    /// Extract scale from transform matrix.
    /// </summary>
    /// <param name="matrix">Transform matrix. This parameter is passed by reference
    /// to improve performance; no changes will be made to it.</param>
    /// <returns>
    /// Scale vector.
    /// </returns>
    public static Vector3 ExtractScaleFromMatrix(ref Matrix4x4 matrix)
    {
        Vector3 scale;
        scale.x = new Vector4(matrix.m00, matrix.m10, matrix.m20, matrix.m30).magnitude;
        scale.y = new Vector4(matrix.m01, matrix.m11, matrix.m21, matrix.m31).magnitude;
        scale.z = new Vector4(matrix.m02, matrix.m12, matrix.m22, matrix.m32).magnitude;
        return scale;
    }

    /// <summary>
    /// Extract position, rotation and scale from TRS matrix.
    /// </summary>
    /// <param name="matrix">Transform matrix. This parameter is passed by reference
    /// to improve performance; no changes will be made to it.</param>
    /// <param name="localPosition">Output position.</param>
    /// <param name="localRotation">Output rotation.</param>
    /// <param name="localScale">Output scale.</param>
    public static void DecomposeMatrix(ref Matrix4x4 matrix, out Vector3 localPosition, out Quaternion localRotation, out Vector3 localScale)
    {
        localPosition = ExtractTranslationFromMatrix(ref matrix);
        localRotation = ExtractRotationFromMatrix(ref matrix);
        localScale = ExtractScaleFromMatrix(ref matrix);
    }

    /// <summary>
    /// Set transform component from TRS matrix.
    /// </summary>
    /// <param name="transform">Transform component.</param>
    /// <param name="matrix">Transform matrix. This parameter is passed by reference
    /// to improve performance; no changes will be made to it.</param>
    public static void SetTransformFromMatrix(Transform transform, ref Matrix4x4 matrix)
    {
        transform.localPosition = ExtractTranslationFromMatrix(ref matrix);
        transform.localRotation = ExtractRotationFromMatrix(ref matrix);
        transform.localScale = ExtractScaleFromMatrix(ref matrix);
    }


    // EXTRAS!

    /// <summary>
    /// Identity quaternion.
    /// </summary>
    /// <remarks>
    /// <para>It is faster to access this variation than <c>Quaternion.identity</c>.</para>
    /// </remarks>
    public static readonly Quaternion IdentityQuaternion = Quaternion.identity;
    /// <summary>
    /// Identity matrix.
    /// </summary>
    /// <remarks>
    /// <para>It is faster to access this variation than <c>Matrix4x4.identity</c>.</para>
    /// </remarks>
    public static readonly Matrix4x4 IdentityMatrix = Matrix4x4.identity;

    /// <summary>
    /// Get translation matrix.
    /// </summary>
    /// <param name="offset">Translation offset.</param>
    /// <returns>
    /// The translation transform matrix.
    /// </returns>
    public static Matrix4x4 TranslationMatrix(Vector3 offset)
    {
        Matrix4x4 matrix = IdentityMatrix;
        matrix.m03 = offset.x;
        matrix.m13 = offset.y;
        matrix.m23 = offset.z;
        return matrix;
    }
    public class DXToOGL
    {
        public static Matrix4x4 GetMatrix(Matrix4x4 matrixDX)
        {

            Matrix4x4 matrixOGL = new Matrix4x4();
            for (int c = 0; c < 4; c++)
                matrixOGL.SetColumn(c, matrixDX.GetRow(c));

            return matrixOGL;
        }

        public static Matrix4x4 CameraPosition(Matrix4x4 matrixDX)
        {

            Matrix4x4 matrixOGL = new Matrix4x4();
            for (int c = 0; c < 4; c++)
                matrixOGL.SetColumn(c, matrixDX.GetRow(c));

            matrixOGL = matrixOGL.inverse;

            return matrixOGL;
        }

        public static Matrix4x4 View(Matrix4x4 matrixDX)
        {
            return GetMatrix(matrixDX);
        }

        public static Matrix4x4 Projection(Matrix4x4 matrixDX)
        {
            Matrix4x4 matrixOGL = new Matrix4x4();

            for (int c = 0; c < 4; c++)
                matrixOGL.SetColumn(c, matrixDX.GetRow(c));
            matrixOGL.SetColumn(2, -matrixOGL.GetColumn(2));

            //TODO fix this hack
            matrixOGL[2, 2] = -1f;
            matrixOGL[2, 3] = -0.02f;

            return matrixOGL;
        }

    }

    public static float[] AsVector(Matrix4x4 matrix)
    {
        float[] result = new float[16];
        for (int y = 0; y < 4; ++y)
            for (int x = 0; x < 4; ++x)
                result[x * 4 + y] = matrix[x, y];
        return result;
    }
}