using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machine._3D.Views.Programs
{
    internal interface IFieldValueSetter
    {
        void Set(string fieldName, bool value);
        void Set(string fieldName, int value);
        void Set(string fieldName, float value);
        void Set(string fieldName, Vector2 value);
        void Set(string fieldName, Vector3 value);
        void Set(string fieldName, Vector4 value);
        void Set(string fieldName, Vector4h value);
        void Set(string fieldName, Matrix2 value);
        void Set(string fieldName, Matrix3 value);
        void Set(string fieldName, Matrix4 value);
    }
}
