using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OpenTK.Mathematics;
using DoubleOrSingle = System.Double;
using Point = OpenTK.Mathematics.Vector2d;
using Point3D = OpenTK.Mathematics.Vector3d;
using Vector3D = OpenTK.Mathematics.Vector3d;
using SMath = System.Math;

namespace Machine.ViewModels.GeometryExtensions.Builders
{
    public class MeshBuilder
    {
        private List<Point3D> positions = new List<Point3D>();
        private List<Vector3D> normals = new List<Vector3D>();
        private List<int> triangleIndices = new List<int>();

        /// <summary>
        /// The circle cache.
        /// </summary>
        private static readonly ThreadLocal<Dictionary<int, IList<Vector2d>>> CircleCache = new ThreadLocal<Dictionary<int, IList<Vector2d>>>(() => new Dictionary<int, IList<Vector2d>>());
        /// <summary>
        /// The closed circle cache.
        /// </summary>
        private static readonly ThreadLocal<Dictionary<int, IList<Vector2d>>> ClosedCircleCache = new ThreadLocal<Dictionary<int, IList<Vector2d>>>(() => new Dictionary<int, IList<Vector2d>>());


        /// <summary>
        /// Gets a circle section (cached).
        /// </summary>
        /// <param name="thetaDiv">
        /// The number of division.
        /// </param>
        /// <param name="closed">
        /// Is the circle closed?
        /// If true, the last point will not be at the same position than the first one.
        /// </param>
        /// <returns>
        /// A circle.
        /// </returns>
        public static IList<Point> GetCircle(int thetaDiv, bool closed = false)
        {
            IList<Point> circle = null;
            // If the circle can't be found in one of the two caches
            if ((!closed && !CircleCache.Value.TryGetValue(thetaDiv, out circle)) ||
                (closed && !ClosedCircleCache.Value.TryGetValue(thetaDiv, out circle)))
            {
                circle = new List<Point>(); //new PointCollection();
                // Add to the cache
                if (!closed)
                {
                    CircleCache.Value.Add(thetaDiv, circle);
                }
                else
                {
                    ClosedCircleCache.Value.Add(thetaDiv, circle);
                }
                // Determine the angle steps
                var num = closed ? thetaDiv : thetaDiv - 1;
                for (var i = 0; i < thetaDiv; i++)
                {
                    var theta = (DoubleOrSingle)SMath.PI * 2 * ((DoubleOrSingle)i / num);
                    circle.Add(new Point((DoubleOrSingle)SMath.Cos(theta), -(DoubleOrSingle)SMath.Sin(theta)));
                }
            }
            // Since Vector2Collection is not Freezable,
            // return new IList<Vector> to avoid manipulation of the Cached Values
            IList<Point> result = new List<Point>();
            foreach (var point in circle)
            {
                result.Add(new Point(point.X, point.Y));
            }
            return result;
        }

        /// <summary>
        /// Adds a surface of revolution.
        /// </summary>
        /// <param name="points">The points (x coordinates are distance from the origin along the axis of revolution, y coordinates are radius, )</param>
        /// <param name="textureValues">The v texture coordinates, one for each point in the <paramref name="points" /> list.</param>
        /// <param name="origin">The origin of the revolution axis.</param>
        /// <param name="direction">The direction of the revolution axis.</param>
        /// <param name="thetaDiv">The number of divisions around the mesh.</param>
        /// <remarks>
        /// See http://en.wikipedia.org/wiki/Surface_of_revolution.
        /// </remarks>
        public void AddRevolvedGeometry(IList<Point> points, Point3D origin, Vector3D direction, int thetaDiv)
        {
            direction.Normalize();

            // Find two unit vectors orthogonal to the specified direction
            var u = direction.FindAnyPerpendicular();
            var v = Vector3D.Cross(direction, u);
            u.Normalize();
            v.Normalize();

            var circle = GetCircle(thetaDiv);

            var index0 = this.positions.Count;
            var n = points.Count;

            var totalNodes = (points.Count - 1) * 2 * thetaDiv;
            var rowNodes = (points.Count - 1) * 2;

            for (var i = 0; i < thetaDiv; i++)
            {
                var w = (v * circle[i].X) + (u * circle[i].Y);

                for (var j = 0; j + 1 < n; j++)
                {
                    // Add segment
                    var q1 = origin + (direction * points[j].X) + (w * points[j].Y);
                    var q2 = origin + (direction * points[j + 1].X) + (w * points[j + 1].Y);

                    // TODO: should not add segment if q1==q2 (corner point)
                    // const double eps = 1e-6;
                    // if (Point3D.Subtract(q1, q2).LengthSquared < eps)
                    // continue;
                    this.positions.Add(q1);
                    this.positions.Add(q2);

                    if (this.normals != null)
                    {
                        var tx = points[j + 1].X - points[j].X;
                        var ty = points[j + 1].Y - points[j].Y;
                        var normal = (-direction * ty) + (w * tx);
                        normal.Normalize();
                        this.normals.Add(normal);
                        this.normals.Add(normal);
                    }

                    var i0 = index0 + (i * rowNodes) + (j * 2);
                    var i1 = i0 + 1;
                    var i2 = index0 + ((((i + 1) * rowNodes) + (j * 2)) % totalNodes);
                    var i3 = i2 + 1;

                    this.triangleIndices.Add(i1);
                    this.triangleIndices.Add(i0);
                    this.triangleIndices.Add(i2);

                    this.triangleIndices.Add(i1);
                    this.triangleIndices.Add(i2);
                    this.triangleIndices.Add(i3);
                }
            }
        }

        /// <summary>
        /// Adds a (possibly truncated) cone.
        /// </summary>
        /// <param name="origin">
        /// The origin.
        /// </param>
        /// <param name="direction">
        /// The direction (normalization not required).
        /// </param>
        /// <param name="baseRadius">
        /// The base radius.
        /// </param>
        /// <param name="topRadius">
        /// The top radius.
        /// </param>
        /// <param name="height">
        /// The height.
        /// </param>
        /// <param name="baseCap">
        /// Include a base cap if set to <c>true</c> .
        /// </param>
        /// <param name="topCap">
        /// Include the top cap if set to <c>true</c> .
        /// </param>
        /// <param name="thetaDiv">
        /// The number of divisions around the cone.
        /// </param>
        /// <remarks>
        /// See http://en.wikipedia.org/wiki/Cone_(geometry).
        /// </remarks>
        public void AddCone(Point3D origin, Vector3D direction,
            double baseRadius, double topRadius, double height,
            bool baseCap, bool topCap, int thetaDiv)
        {
            var pc = new List<Point>();
            var tc = new List<double>();
            if (baseCap)
            {
                pc.Add(new Point(0, 0));
                tc.Add(0);
            }

            pc.Add(new Point(0, (DoubleOrSingle)baseRadius));
            tc.Add(1);
            pc.Add(new Point((DoubleOrSingle)height, (DoubleOrSingle)topRadius));
            tc.Add(0);
            if (topCap)
            {
                pc.Add(new Point((DoubleOrSingle)height, 0));
                tc.Add(1);
            }

            this.AddRevolvedGeometry(pc, origin, direction, thetaDiv);
        }

        /// <summary>
        /// Adds a cone.
        /// </summary>
        /// <param name="origin">The origin point.</param>
        /// <param name="apex">The apex point.</param>
        /// <param name="baseRadius">The base radius.</param>
        /// <param name="baseCap">
        /// Include a base cap if set to <c>true</c> .
        /// </param>
        /// <param name="thetaDiv">The theta div.</param>
        public void AddCone(Point3D origin, Point3D apex, double baseRadius, bool baseCap, int thetaDiv)
        {
            var dir = apex - origin;
            this.AddCone(origin, dir, baseRadius, 0, dir.Length, baseCap, false, thetaDiv);
        }

        /// <summary>
        /// Adds a cylinder to the mesh.
        /// </summary>
        /// <param name="p1">
        /// The first point.
        /// </param>
        /// <param name="p2">
        /// The second point.
        /// </param>
        /// <param name="diameter">
        /// The diameters.
        /// </param>
        /// <param name="thetaDiv">
        /// The number of divisions around the cylinder.
        /// </param>
        /// <remarks>
        /// See http://en.wikipedia.org/wiki/Cylinder_(geometry).
        /// </remarks>
        public void AddCylinder(Point3D p1, Point3D p2, double diameter, int thetaDiv)
        {
            var n = p2 - p1;
            var l = n.Length;
            n.Normalize();
            this.AddCone(p1, n, diameter / 2, diameter / 2, l, false, false, thetaDiv);
        }
        /// <summary>
        /// Adds a cylinder to the mesh.
        /// </summary>
        /// <param name="p1">
        /// The first point.
        /// </param>
        /// <param name="p2">
        /// The second point.
        /// </param>
        /// <param name="radius">
        /// The diameters.
        /// </param>
        /// <param name="thetaDiv">
        /// The number of divisions around the cylinder.
        /// </param>
        /// <param name="cap1">
        /// The first Cap.
        /// </param>
        /// <param name="cap2">
        /// The second Cap.
        /// </param>
        /// <remarks>
        /// See http://en.wikipedia.org/wiki/Cylinder_(geometry).
        /// </remarks>
        public void AddCylinder(Point3D p1, Point3D p2, double radius = 1, int thetaDiv = 32, bool cap1 = true, bool cap2 = true)
        {
            var n = p2 - p1;
            var l = n.Length;
            n.Normalize();
            this.AddCone(p1, n, radius, radius, l, cap1, cap2, thetaDiv);
        }

        /// <summary>
        /// Adds a cube face.
        /// </summary>
        /// <param name="center">
        /// The center of the cube.
        /// </param>
        /// <param name="normal">
        /// The normal vector for the face.
        /// </param>
        /// <param name="up">
        /// The up vector for the face.
        /// </param>
        /// <param name="dist">
        /// The distance from the center of the cube to the face.
        /// </param>
        /// <param name="width">
        /// The width of the face.
        /// </param>
        /// <param name="height">
        /// The height of the face.
        /// </param>
        public void AddCubeFace(Point3D center, Vector3D normal, Vector3D up, double dist, double width, double height)
        {
            var right = Vector3D.Cross(normal, up);
            var n = normal * (DoubleOrSingle)dist / 2;
            up *= (DoubleOrSingle)height / 2;
            right *= (DoubleOrSingle)width / 2;
            var p1 = center + n - up - right;
            var p2 = center + n - up + right;
            var p3 = center + n + up + right;
            var p4 = center + n + up - right;

            var i0 = this.positions.Count;
            this.positions.Add(p1);
            this.positions.Add(p2);
            this.positions.Add(p3);
            this.positions.Add(p4);
            if (this.normals != null)
            {
                this.normals.Add(normal);
                this.normals.Add(normal);
                this.normals.Add(normal);
                this.normals.Add(normal);
            }

            this.triangleIndices.Add(i0 + 2);
            this.triangleIndices.Add(i0 + 1);
            this.triangleIndices.Add(i0 + 0);
            this.triangleIndices.Add(i0 + 0);
            this.triangleIndices.Add(i0 + 3);
            this.triangleIndices.Add(i0 + 2);
        }

        public void AddBackground(Point3D viewPosition, Vector3D look, Vector3D up, float fov, float depthFar, float aspectRatio, Vector3D upColor, Vector3D downColor)
        {
            var lefth = Vector3D.Cross(look, up);
            var center = viewPosition + look * (depthFar / 1.01);
            var h = MathHelper.Tan(fov * 0.5) * depthFar;
            var w = h * aspectRatio;
            var p1 = center - lefth * w - up * h;
            var p2 = center + lefth * w - up * h;
            var p3 = center + lefth * w + up * h;
            var p4 = center - lefth * w + up * h;
            var n = -look;

            var i0 = this.positions.Count;
            this.positions.Add(p1);
            this.positions.Add(p2);
            this.positions.Add(p3);
            this.positions.Add(p4);
            if (this.normals != null)
            {
                this.normals.Add(downColor);
                this.normals.Add(downColor);
                this.normals.Add(upColor);
                this.normals.Add(upColor);
            }

            this.triangleIndices.Add(i0 + 0);
            this.triangleIndices.Add(i0 + 1);
            this.triangleIndices.Add(i0 + 3);
            this.triangleIndices.Add(i0 + 1);
            this.triangleIndices.Add(i0 + 2);
            this.triangleIndices.Add(i0 + 3);
        }

        /// <summary>
        /// Adds a box with the specified faces, aligned with the specified axes.
        /// </summary>
        /// <param name="center">The center point of the box.</param>
        /// <param name="x">The x axis.</param>
        /// <param name="y">The y axis.</param>
        /// <param name="xlength">The length of the box along the X axis.</param>
        /// <param name="ylength">The length of the box along the Y axis.</param>
        /// <param name="zlength">The length of the box along the Z axis.</param>
        /// <param name="faces">The faces to include.</param>
        public void AddBox(Point3D center, Vector3D x, Vector3D y, double xlength, double ylength, double zlength)
        {
            var z = Vector3D.Cross(x, y);

            this.AddCubeFace(center, x, z, xlength, ylength, zlength);
            this.AddCubeFace(center, -x, z, xlength, ylength, zlength);
            this.AddCubeFace(center, -y, z, ylength, xlength, zlength);
            this.AddCubeFace(center, y, z, ylength, xlength, zlength);
            this.AddCubeFace(center, z, y, zlength, xlength, ylength);
            this.AddCubeFace(center, -z, y, zlength, xlength, ylength);
        }

        /// <summary>
        /// Adds a box with the specified faces, aligned with the X, Y and Z axes.
        /// </summary>
        /// <param name="center">
        /// The center point of the box.
        /// </param>
        /// <param name="xlength">
        /// The length of the box along the X axis.
        /// </param>
        /// <param name="ylength">
        /// The length of the box along the Y axis.
        /// </param>
        /// <param name="zlength">
        /// The length of the box along the Z axis.
        /// </param>
        /// <param name="faces">
        /// The faces to include.
        /// </param>
        public void AddBox(Point3D center, double xlength, double ylength, double zlength)
        {
            this.AddBox(center, new Vector3D(1, 0, 0), new Vector3D(0, 1, 0), xlength, ylength, zlength);
        }

        public void ToMesh(out Vector3[] vertexes, out uint[] indexes, out Vector3[] normals)
        {
            if (this.normals != null && this.positions.Count != this.normals.Count)
            {
                throw new InvalidOperationException();
            }

            vertexes = this.positions.Select(v => new Vector3((float)v.X, (float)v.Y, (float)v.Z)).ToArray();
            normals = this.normals.Select(v => new Vector3((float)v.X, (float)v.Y, (float)v.Z)).ToArray();
            indexes = this.triangleIndices.Select(i => (uint)i).ToArray();
        }
    }

    public static class Vector3DExtensions
    {
        /// <summary>
        /// Find a <see cref="Vector3D"/> that is perpendicular to the given <see cref="Vector3D"/>.
        /// </summary>
        /// <param name="n">
        /// The input vector.
        /// </param>
        /// <returns>
        /// A perpendicular vector.
        /// </returns>
        public static Vector3D FindAnyPerpendicular(this Vector3D n)
        {
            n.Normalize();
            Vector3D u = Vector3D.Cross(new Vector3D(0, 1, 0), n);
            if (u.LengthSquared < 1e-3)
            {
                u = Vector3D.Cross(new Vector3D(1, 0, 0), n);
            }

            return u;
        }
    }

}
