//------------------------------------------------------------
// Zero Framework
// Copyright © 2025-2026 All rights reserved.
// Feedback: https://github.com/huozk0804/ZeroFramework
//------------------------------------------------------------

using UnityEngine;

namespace ZeroFramework
{
    public static partial class Utility
    {
        public static class Gizmos
        {
            public static void DrawFrustum(FrustumPlanes frustum)
            {
                if (frustum.zFar <= 0)
                    return;
                var p0 = new Vector3(frustum.left, frustum.top, frustum.zFar);
                var p1 = new Vector3(frustum.right, frustum.top, frustum.zFar);
                var p2 = new Vector3(frustum.right, frustum.bottom, frustum.zFar);
                var p3 = new Vector3(frustum.left, frustum.bottom, frustum.zFar);
                UnityEngine.Gizmos.DrawLine(p0, p1);
                UnityEngine.Gizmos.DrawLine(p1, p2);
				UnityEngine.Gizmos.DrawLine(p2, p3);
				UnityEngine.Gizmos.DrawLine(p3, p0);
                var f = frustum.zNear > 0 && frustum.zNear < frustum.zFar ? frustum.zNear / frustum.zFar : 0;
                f = 1 - f;
				UnityEngine.Gizmos.DrawRay(p0, -p0 * f);
				UnityEngine.Gizmos.DrawRay(p1, -p1 * f);
				UnityEngine.Gizmos.DrawRay(p2, -p2 * f);
				UnityEngine.Gizmos.DrawRay(p3, -p3 * f);
                f = 1 - f;
                if (f > 0)
                {
                    p0 *= f;
                    p1 *= f;
                    p2 *= f;
                    p3 *= f;
                    UnityEngine.Gizmos.DrawLine(p0, p1);
					UnityEngine.Gizmos.DrawLine(p1, p2);
					UnityEngine.Gizmos.DrawLine(p2, p3);
					UnityEngine.Gizmos.DrawLine(p3, p0);
                }
            }

            public static void DrawWiredCube(Vector3 center, Vector3 size, float bodyTransparency = 0.5f)
            {
                Color color = UnityEngine.Gizmos.color;
                float a = color.a;
                color.a = a * bodyTransparency;
				UnityEngine.Gizmos.color = color;
				UnityEngine.Gizmos.DrawCube(center, size);
                color.a = a;
				UnityEngine.Gizmos.color = color;
				UnityEngine.Gizmos.DrawWireCube(center, size);
            }

            public static float LodArc(Vector3 pos)
            {
#if UNITY_EDITOR
                if (UnityEditor.SceneView.currentDrawingSceneView != null)
                {
                    var cam = UnityEditor.SceneView.currentDrawingSceneView.camera;
                    var dis = Vector3.SqrMagnitude(cam.transform.position - pos);
                    return Mathf.Clamp(dis * 0.03f, 1, 3);
                }
#endif
                return 1;
            }

            public static void DrawArc(Vector3 center, Vector3 direction, Vector3 normal, float angle,
                float deltaAngle = 10f, bool drawSide = true)
            {
                if (angle < 1 || direction.sqrMagnitude <= 0.00001f)
                {
					UnityEngine.Gizmos.DrawRay(center, direction);
                    return;
                }

                float sides = Mathf.Ceil(angle / (LodArc(UnityEngine.Gizmos.matrix.MultiplyPoint(center)) * deltaAngle));
                deltaAngle = angle / sides;
                Quaternion rot = Quaternion.AngleAxis(-angle * 0.5f, normal);
                Vector3 p0 = rot * direction;
                if (drawSide)
					UnityEngine.Gizmos.DrawRay(center, p0);
                rot = Quaternion.AngleAxis(deltaAngle, normal);
                Vector3 p1 = p0;
                for (int i = 0; i < sides; i++)
                {
                    p1 = rot * p0;
					UnityEngine.Gizmos.DrawLine(center + p0, center + p1);
                    p0 = p1;
                }

                if (drawSide)
					UnityEngine.Gizmos.DrawRay(center, p1);
            }

            public static void DrawArc(Vector3 center, Vector3 start, Vector3 end, Vector3 normal,
                float deltaAngle = 10f, bool drawSide = true)
            {
                Vector3 dir1 = start - center;
                Vector3 dir2 = end - center;
                if (drawSide)
                {
					UnityEngine.Gizmos.DrawLine(center, start);
					UnityEngine.Gizmos.DrawLine(center, end);
                }

                float degree = Vector3.Angle(dir1, dir2);
                if (degree < 1)
                    return;
                float len1 = dir1.magnitude;
                dir1 = dir1.normalized;
                float len2 = dir2.magnitude;
                dir2 = dir2.normalized;
                float sides = Mathf.Ceil(degree / (LodArc(UnityEngine.Gizmos.matrix.MultiplyPoint(center)) * deltaAngle));
                deltaAngle = degree / sides;
                Quaternion rot;
                Vector3 p0 = start;
                Vector3 p1 = p0;
                Vector3 dir;
                for (int i = 0; i < sides; i++)
                {
                    rot = Quaternion.AngleAxis(i * deltaAngle, normal);
                    dir = rot * dir1;
                    dir *= Mathf.Lerp(len1, len2, i * deltaAngle / degree);
                    p1 = center + dir;
					UnityEngine.Gizmos.DrawLine(p0, p1);
                    p0 = p1;
                }
            }

            public static void DrawCapsuleCollider(CapsuleCollider cap, float scale = 1)
            {
                float cylen = cap.height - cap.radius * 2;
                float radius = cap.radius;
                cylen *= scale;
                radius *= scale;
                Vector3 p0, p1, p2, p3;
                switch (cap.direction)
                {
                    case 0:
                        p0 = cap.center - Vector3.right * cylen * 0.5f + Vector3.up * radius;
                        p1 = p0 + cylen * Vector3.right;
                        p2 = p0 + Vector3.down * radius * 2;
                        p3 = p2 + cylen * Vector3.right;
						UnityEngine.Gizmos.DrawLine(p0, p1);
						UnityEngine.Gizmos.DrawLine(p2, p3);
                        DrawArc(p0 + Vector3.down * radius, p0, p2, Vector3.forward, 5, false);
                        DrawArc(p1 + Vector3.down * radius, p1, p3, Vector3.back, 5, false);

                        p0 = cap.center - Vector3.right * cylen * 0.5f + Vector3.forward * radius;
                        p1 = p0 + cylen * Vector3.right;
                        p2 = p0 + Vector3.back * radius * 2;
                        p3 = p2 + cylen * Vector3.right;
						UnityEngine.Gizmos.DrawLine(p0, p1);
						UnityEngine.Gizmos.DrawLine(p2, p3);
                        DrawArc(p0 + Vector3.back * radius, p0, p2, Vector3.down, 5, false);
                        DrawArc(p1 + Vector3.back * radius, p1, p3, Vector3.up, 5, false);
                        break;
                    case 1:
                        p0 = cap.center + Vector3.down * cylen * 0.5f + Vector3.left * radius;
                        p1 = p0 + cylen * Vector3.up;

                        p2 = p0 + Vector3.right * radius * 2;
                        p3 = p2 + cylen * Vector3.up;
						UnityEngine.Gizmos.DrawLine(p0, p1);
						UnityEngine.Gizmos.DrawLine(p2, p3);
                        DrawArc(p0 + Vector3.right * radius, p0, p2, Vector3.forward, 5, false);
                        DrawArc(p1 + Vector3.right * radius, p1, p3, Vector3.back, 5, false);

                        p0 = cap.center + Vector3.down * cylen * 0.5f + Vector3.back * radius;
                        p1 = p0 + cylen * Vector3.up;

                        p2 = p0 + Vector3.forward * radius * 2;
                        p3 = p2 + cylen * Vector3.up;
						UnityEngine.Gizmos.DrawLine(p0, p1);
						UnityEngine.Gizmos.DrawLine(p2, p3);
                        DrawArc(p0 + Vector3.forward * radius, p0, p2, Vector3.left, 5, false);
                        DrawArc(p1 + Vector3.forward * radius, p1, p3, Vector3.right, 5, false);
                        break;
                    case 2:
                        p0 = cap.center + Vector3.back * cylen * 0.5f + Vector3.left * radius;
                        p1 = p0 + cylen * Vector3.forward;

                        p2 = p0 + Vector3.right * radius * 2;
                        p3 = p2 + cylen * Vector3.forward;
						UnityEngine.Gizmos.DrawLine(p0, p1);
						UnityEngine.Gizmos.DrawLine(p2, p3);
                        DrawArc(p0 + Vector3.right * radius, p0, p2, Vector3.down, 5, false);
                        DrawArc(p1 + Vector3.right * radius, p1, p3, Vector3.up, 5, false);

                        p0 = cap.center + Vector3.back * cylen * 0.5f + Vector3.down * radius;
                        p1 = p0 + cylen * Vector3.forward;

                        p2 = p0 + Vector3.up * radius * 2;
                        p3 = p2 + cylen * Vector3.forward;
						UnityEngine.Gizmos.DrawLine(p0, p1);
						UnityEngine.Gizmos.DrawLine(p2, p3);
                        DrawArc(p0 + Vector3.up * radius, p0, p2, Vector3.right, 5, false);
                        DrawArc(p1 + Vector3.up * radius, p1, p3, Vector3.left, 5, false);
                        break;
                    default:
                        break;
                }
            }

            public static void MarkTransform(Transform trans, float size)
            {
				UnityEngine.Gizmos.matrix = Matrix4x4.identity;
				UnityEngine.Gizmos.color = Color.red;
				UnityEngine.Gizmos.DrawRay(trans.position, trans.right * size);
				UnityEngine.Gizmos.color = Color.blue;
				UnityEngine.Gizmos.DrawRay(trans.position, trans.forward * size);
				UnityEngine.Gizmos.color = Color.green;
				UnityEngine.Gizmos.DrawRay(trans.position, trans.up * size);
            }

            public static void MarkLine(Vector3 worldPos, float length, float rad)
            {
#if UNITY_EDITOR
                UnityEditor.SceneView scene = UnityEditor.SceneView.currentDrawingSceneView;
                if (!scene || !scene.camera)
                    return;
                Matrix4x4 defM = UnityEngine.Gizmos.matrix;
				UnityEngine.Gizmos.matrix = scene.camera.cameraToWorldMatrix;

                Vector3 p = scene.camera.worldToCameraMatrix.MultiplyPoint(worldPos);
                Vector3 sp = scene.camera.WorldToScreenPoint(worldPos);
                Vector3 sp2 = scene.camera.WorldToScreenPoint(UnityEngine.Gizmos.matrix.MultiplyPoint(p + Vector3.right));
                float pixScale = 1f / Vector3.Distance(sp, sp2);

                Vector3 p1;
                Vector2 off = new Vector2(Mathf.Sin(rad), Mathf.Cos(rad));
                off *= length * pixScale;
                p1.x = p.x + off.x;
                p1.y = p.y + off.y;
                p1.z = p.z;
				UnityEngine.Gizmos.DrawLine(p, p1);
				UnityEngine.Gizmos.matrix = defM;
#endif
            }

            public static void DebugInScene(Vector3 worldPos, float size = 2, float duration = 1)
            {
                Debug.DrawRay(worldPos - Vector3.right * size * 0.5f, Vector3.right * size, Color.red, duration);
                Debug.DrawRay(worldPos - Vector3.up * size * 0.5f, Vector3.up * size, Color.green, duration);
                Debug.DrawRay(worldPos - Vector3.forward * size * 0.5f, Vector3.forward * size, Color.blue, duration);
            }

            public static void MarkInScene(Vector3 worldPos, float pixel = 10, float rad = 0)
            {
#if UNITY_EDITOR
                UnityEditor.SceneView scene = UnityEditor.SceneView.currentDrawingSceneView;
                if (!scene || !scene.camera)
                    return;
                Matrix4x4 defM = UnityEngine.Gizmos.matrix;
				UnityEngine.Gizmos.matrix = scene.camera.cameraToWorldMatrix;

                Vector3 p = scene.camera.worldToCameraMatrix.MultiplyPoint(worldPos);
                Vector3 sp = scene.camera.WorldToScreenPoint(worldPos);
                Vector3 sp2 = scene.camera.WorldToScreenPoint(UnityEngine.Gizmos.matrix.MultiplyPoint(p + Vector3.right));
                float pixScale = 1f / Vector3.Distance(sp, sp2);

                Vector3 p1;
                Vector2 off = new Vector2(Mathf.Sin(rad), Mathf.Cos(rad));
                off *= pixel * pixScale;
                p1.x = p.x + off.x;
                p1.y = p.y + off.y;
                p1.z = p.z;
                Vector3 p2 = p1;
                p2.x = p.x * 2f - p1.x;
                p2.y = p.y * 2f - p1.y;
				UnityEngine.Gizmos.DrawLine(p1, p2);
                p1.x = p.x + off.y;
                p1.y = p.y - off.x;
                p2.x = p.x * 2f - p1.x;
                p2.y = p.y * 2f - p1.y;
				UnityEngine.Gizmos.DrawLine(p1, p2);
				UnityEngine.Gizmos.matrix = defM;
#endif
            }

            public static void MarkAxisSystem(float size)
            {
				UnityEngine.Gizmos.color = Color.red;
				UnityEngine.Gizmos.DrawRay(Vector3.zero, Vector3.right * size);
				UnityEngine.Gizmos.color = Color.green;
				UnityEngine.Gizmos.DrawRay(Vector3.zero, Vector3.up * size);
				UnityEngine.Gizmos.color = Color.blue;
				UnityEngine.Gizmos.DrawRay(Vector3.zero, Vector3.forward * size);
            }

            //屏幕像素大小与本地单位大小比值
            public static float FactorToPixel(Camera camera, Transform trans, Vector3 localPos = new Vector3())
            {
                Vector3 lp1 = trans ? trans.localToWorldMatrix.MultiplyPoint(localPos) : localPos;

                Vector3 sp1 = camera.WorldToScreenPoint(lp1);
                Vector3 sp2 = camera.ScreenToWorldPoint(sp1 + Vector3.right);
                Vector3 lp2 = trans ? trans.worldToLocalMatrix.MultiplyPoint(sp2) : sp2;
                float dis = Vector3.Distance(lp2, localPos);
                return dis > 0f ? 1f / dis : 0f;
            }

            //本地单位大小与屏幕像素大小比值
            public static float FactorFromPixel(Camera camera, Transform trans, Vector3 localPos = new Vector3())
            {
                Vector3 lp1 = trans ? trans.localToWorldMatrix.MultiplyPoint(localPos) : localPos;

                Vector3 sp1 = camera.WorldToScreenPoint(lp1);
                Vector3 sp2 = camera.ScreenToWorldPoint(sp1 + Vector3.right);
                Vector3 lp2 = trans ? trans.worldToLocalMatrix.MultiplyPoint(sp2) : sp2;
                float dis = Vector3.Distance(lp2, localPos);
                return dis;
            }

            public static float FactorToSceneViewPixel(Transform trans, Vector3 localPos = new Vector3())
            {
#if UNITY_EDITOR
                UnityEditor.SceneView scene = UnityEditor.SceneView.currentDrawingSceneView;
                if (scene)
                    return FactorToPixel(scene.camera, trans, localPos);
                else
                    return 0f;
#else
        return 0f;
#endif
            }

            public static Camera GetSceneViewCamera()
            {
#if UNITY_EDITOR
                UnityEditor.SceneView scene = UnityEditor.SceneView.currentDrawingSceneView;
                if (scene)
                    return scene.camera;
                else
                    return null;
#else
            return null;
#endif
            }

            public static float FactorFromSceneViewPixel(Transform trans, Vector3 localPos = new Vector3())
            {
#if UNITY_EDITOR
                UnityEditor.SceneView scene = UnityEditor.SceneView.currentDrawingSceneView;
                if (scene)
                    return FactorFromPixel(scene.camera, trans, localPos);
                else
                    return 0f;
#else
        return 0f;
#endif
            }
        }
    }
}