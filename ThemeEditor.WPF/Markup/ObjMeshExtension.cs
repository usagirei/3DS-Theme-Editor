using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Markup;
using System.Windows.Media.Media3D;

namespace ThemeEditor.WPF.Markup
{
    internal class ObjMeshExtension : MarkupExtension
    {
        
        public Uri Source { get; set; }

        public ObjMeshExtension(string source)
        {
            Source = new Uri(source, UriKind.Absolute);
        }

        private struct Vertex3D
        {
            public readonly Point3D Position;
            public readonly Vector3D Normal;
            public readonly Vector3D TexCoord;

            public Vertex3D(Point3D p, Vector3D n, Vector3D u)
            {
                Position = p;
                Normal = n;
                TexCoord = u;
            }
        }

        private struct Face3D
        {
            public Vertex3D A;
            public Vertex3D B;
            public Vertex3D C;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var s = System.Windows.Application.GetResourceStream(Source);
            if (s == null)
                throw new ArgumentException("Object not Found", "Source");

            using (s.Stream)
            using (var sr = new StreamReader(s.Stream, Encoding.ASCII))
            {
                MeshGeometry3D geom = new MeshGeometry3D();

                List<Point3D> verts = new List<Point3D>();
                List<Vector3D> normals = new List<Vector3D>();
                List<Face3D> faces = new List<Face3D>();

                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (line.Length == 0 || line[0] == '#')
                        continue;

                    var ls = line.Split(' ');
                    switch (ls[0])
                    {
                        case "v":
                            {
                                float x, y, z;
                                float.TryParse(ls[1], NumberStyles.Any, CultureInfo.InvariantCulture, out x);
                                float.TryParse(ls[2], NumberStyles.Any, CultureInfo.InvariantCulture, out y);
                                float.TryParse(ls[3], NumberStyles.Any, CultureInfo.InvariantCulture, out z);
                                Point3D p = new Point3D(x, y, z);
                                verts.Add(p);
                                break;
                            }
                        case "vn":
                            {
                                float x, y, z;
                                float.TryParse(ls[1], NumberStyles.Any, CultureInfo.InvariantCulture, out x);
                                float.TryParse(ls[2], NumberStyles.Any, CultureInfo.InvariantCulture, out y);
                                float.TryParse(ls[3], NumberStyles.Any, CultureInfo.InvariantCulture, out z);
                                Vector3D p = new Vector3D(x, y, z);
                                normals.Add(p);
                                break;
                            }
                        case "f":
                            {
                                var v0 = ls[1].Split('/');
                                var v1 = ls[2].Split('/');
                                var v2 = ls[3].Split('/');
                                Face3D f = new Face3D
                                {
                                    A = new Vertex3D(verts[int.Parse(v0[0]) - 1], normals[int.Parse(v0[2]) - 1], new Vector3D()),
                                    B = new Vertex3D(verts[int.Parse(v1[0]) - 1], normals[int.Parse(v1[2]) - 1], new Vector3D()),
                                    C = new Vertex3D(verts[int.Parse(v2[0]) - 1], normals[int.Parse(v2[2]) - 1], new Vector3D()),
                                };
                                faces.Add(f);
                                break;
                            }
                    }
                }

                int i = 0;
                foreach (var face3D in faces)
                {
                    geom.Positions.Add(face3D.A.Position);
                    geom.Positions.Add(face3D.B.Position);
                    geom.Positions.Add(face3D.C.Position);

                    geom.Normals.Add(face3D.A.Normal);
                    geom.Normals.Add(face3D.B.Normal);
                    geom.Normals.Add(face3D.C.Normal);

                    geom.TriangleIndices.Add(i++);
                    geom.TriangleIndices.Add(i++);
                    geom.TriangleIndices.Add(i++);
                }

                geom.Freeze();

                return geom;
            }
        }
    }
}
