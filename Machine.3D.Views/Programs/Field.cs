using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machine._3D.Views.Programs
{
    internal abstract class Field
    {
        public int Location { get; set; }
        public string Name { get; set; }
    }

    internal class Field<T> : Field
    {
        private Action<int, T> setter;

        public Field() : base()
        {
            setter = FieldSetter.Get<T>();
        }

        private T _value;
        public T Value
        {
            get => _value;
            set
            {
                _value = value;
                setter.Invoke(Location, _value);
            }
        }
    }

    internal abstract class FieldSetter
    {
        protected abstract Type MappedType { get; }

        private class MappedFieldSetter<T> : FieldSetter
        {
            protected override Type MappedType => typeof(T);

            public Action<int, T> Setter { get; private set; }

            public MappedFieldSetter(Action<int, T> setter)
            {
                Setter = setter;
            }
        }

        private static readonly List<FieldSetter> Setters = new List<FieldSetter>();

        private FieldSetter()
        {
        }

        static FieldSetter()
        {
            Setters = new List<FieldSetter>
            {
                new MappedFieldSetter<bool>((_,value) => GL.Uniform1(_, value ? 1 : 0)),
                new MappedFieldSetter<int>(GL.Uniform1),
                //new MappedFieldSetter<uint>(GL.Uniform1),
                new MappedFieldSetter<float>(GL.Uniform1),
                //new MappedFieldSetter<double>(GL.Uniform1),
                //new MappedFieldSetter<Half>((_, half) => GL.Uniform1(_, half)),
                //new MappedFieldSetter<Color>((_, color) => GL.Uniform4(_, color)),
                new MappedFieldSetter<Vector2>(GL.Uniform2),
                new MappedFieldSetter<Vector3>(GL.Uniform3),
                new MappedFieldSetter<Vector4>(GL.Uniform4),
                //new MappedFieldSetter<Vector2d>((_, vector) => GL.Uniform2(_, vector.X, vector.Y)),
                new MappedFieldSetter<Vector2h>((_, vector) => GL.Uniform2(_, vector.X, vector.Y)),
                //new MappedFieldSetter<Vector3d>((_, vector) => GL.Uniform3(_, vector.X, vector.Y, vector.Z)),
                new MappedFieldSetter<Vector3h>((_, vector) => GL.Uniform3(_, vector.X, vector.Y, vector.Z)),
                //new MappedFieldSetter<Vector4d>((_, vector) => GL.Uniform4(_, vector.X, vector.Y, vector.Z, vector.W)),
                new MappedFieldSetter<Vector4h>((_, vector) => GL.Uniform4(_, vector.X, vector.Y, vector.Z, vector.W)),
                new MappedFieldSetter<Matrix2>((_, matrix) => GL.UniformMatrix2(_, false, ref matrix)),
                new MappedFieldSetter<Matrix3>((_, matrix) => GL.UniformMatrix3(_, false, ref matrix)),
                new MappedFieldSetter<Matrix4>((_, matrix) => GL.UniformMatrix4(_, false, ref matrix)),
                //new MappedFieldSetter<Matrix2x3>((_, matrix) => GL.UniformMatrix2x3(_, false, ref matrix)),
                //new MappedFieldSetter<Matrix2x4>((_, matrix) => GL.UniformMatrix2x4(_, false, ref matrix)),
                //new MappedFieldSetter<Matrix3x2>((_, matrix) => GL.UniformMatrix3x2(_, false, ref matrix)),
                //new MappedFieldSetter<Matrix3x4>((_, matrix) => GL.UniformMatrix3x4(_, false, ref matrix)),
                //new MappedFieldSetter<Matrix4x2>((_, matrix) => GL.UniformMatrix4x2(_, false, ref matrix)),
                //new MappedFieldSetter<Matrix4x3>((_, matrix) => GL.UniformMatrix4x3(_, false, ref matrix))
            };
        }

        public static Action<int, T> Get<T>()
        {
            var setter = Setters.FirstOrDefault(_ => _.MappedType == typeof(T));
            if (setter == null) throw new NotImplementedException($"Setter not implemented for type {typeof(T).FullName}");
            return ((MappedFieldSetter<T>)setter).Setter;
        }

        public static FieldSetter GetSetter<T>()
        {
            var setter = Setters.FirstOrDefault(_ => _.MappedType == typeof(T));
            if (setter == null) throw new NotImplementedException($"Setter not implemented for type {typeof(T).FullName}");

            return setter;
        }

        public static FieldSetter GetSetter(Type type)
        {
            var setter = Setters.FirstOrDefault(_ => _.MappedType == type);
            if (setter == null) throw new NotImplementedException($"Setter not implemented for type {type.FullName}");

            return setter;
        }
    }

}
