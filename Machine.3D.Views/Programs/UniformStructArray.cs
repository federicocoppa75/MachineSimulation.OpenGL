using ObjectTK.Shaders.Variables;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Machine._3D.Views.Programs
{
    internal class UniformStructArray<T> : Uniform<T> where T : struct, IFieldValueProvider
    {

        private UniformStructArrayItem<T>[] _array;

        public UniformStructArray() : base((i, t) => { })
        {
        }

        public new void Set(T[] values) 
        {
            if (values == null) throw new ArgumentNullException();
            if (values.Length != _array.Length) throw new ArgumentException($"The size of the array passed is wrong! ({values.Length} != {_array.Length})");

            for (int i = 0; i < _array.Length; i++)
            {
                _array[i].Set(values[i]);
            }
        }

        public void Initialize(int size)
        {
            _array = new UniformStructArrayItem<T>[size];

            for (int i = 0; i < size; i++)
            {
                var us = new UniformStructArrayItem<T>();
                
                us.Initialize();
                _array[i] = us;
            }
        }

        public void Link(string istanceName)
        {
            for (int i = 0; i < _array.Length; i++)
            {
                _array[i].Link(ProgramHandle, $"{istanceName}[{i}]");
            }
        }
    }

    internal class UniformStructArrayItem<T> : IFieldValueSetter where T : struct, IFieldValueProvider
    {
        private Dictionary<string, Field> _fields = new Dictionary<string, Field>();

        public new void Set(T value) => value.SetFieldsValues(this);

        public void Initialize()
        {
            const BindingFlags flags = BindingFlags.Instance | BindingFlags.Public;
            var type = typeof(T);
            foreach (var property in type.GetFields(flags))
            {
                var fieldType = typeof(Field<>).MakeGenericType(property.FieldType);
                var v = (Field)Activator.CreateInstance(fieldType);

                v.Name = property.Name;
                _fields.Add(v.Name, v);
            }
        }

        public void Link(int programHandle, string istanceName)
        {
            foreach (var item in _fields.Values)
            {
                var s = $"{istanceName}.{item.Name}";
                item.Location = GL.GetUniformLocation(programHandle, s);
            }
        }

        #region IFieldValueSetter
        public void Set(string fieldName, bool value) => SetFieldValue(fieldName, value);
        public void Set(string fieldName, int value) => SetFieldValue(fieldName, value);
        public void Set(string fieldName, float value) => SetFieldValue(fieldName, value);
        public void Set(string fieldName, Vector2 value) => SetFieldValue(fieldName, value);
        public void Set(string fieldName, Vector3 value) => SetFieldValue(fieldName, value);
        public void Set(string fieldName, Vector4 value) => SetFieldValue(fieldName, value);
        public void Set(string fieldName, Vector4h value) => SetFieldValue(fieldName, value);
        public void Set(string fieldName, Matrix2 value) => SetFieldValue(fieldName, value);
        public void Set(string fieldName, Matrix3 value) => SetFieldValue(fieldName, value);
        public void Set(string fieldName, Matrix4 value) => SetFieldValue(fieldName, value);
        #endregion

        private void SetFieldValue<V>(string name, V value) => ((_fields[name]) as Field<V>).Value = value;
    }
}
