using Machine.ViewModels.Interfaces;
using Machine.ViewModels.Interfaces.Links;
using Machine.ViewModels.Interfaces.MachineElements;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using MDB = Machine.Data.Base;

namespace Machine.ViewModels.GeometryExtensions.Helpers
{
    public static class ChainTransformtionHelper
    {
        public static Matrix4 GetChainTransformation(IMachineElement element)
        {
            if (element.Parent == null)
            {
                return Convert(element.Transformation);
            }
            else
            {
                if (element is IAngularTransmission at) return GetChainTransformation(at);
                else if (element is IToolElement te) return GetChainTransformation(te);
                else if (element is IPanelElement pe) return GetChainTransformation(pe);
                else return GetBaseChainTransformation(element);
            }
        }

        private static Matrix4 GetChainTransformation(IAngularTransmission element) => GetChainTransformationFromToolholder(element.Parent);

        private static Matrix4 GetChainTransformation(IToolElement element) => GetChainTransformationFromToolholder(element.Parent);

        private static Matrix4 GetChainTransformationFromToolholder(IMachineElement element)
        {
            var m = GetChainTransformation(element);
            var th = element as IToolholderBase;

            if (th == null) throw new InvalidOperationException("Tool is not attacced on tool holder!");

            var mth = GetToolholderTransformation(th);

            return mth * m;
        }

        private static Matrix4 GetChainTransformation(IPanelElement element)
        {
            var m = GetChainTransformation(element.Parent);
            var mp = element as IMovablePanel;
            var pt = Convert(element.Transformation);
            var ct = Matrix4.CreateTranslation((float)element.CenterX, (float)element.CenterY, (float)element.CenterZ);
            var mpt = Matrix4.Identity;

            if (mp != null) mpt = Matrix4.CreateTranslation((float)mp.OffsetX, 0, 0);

            return mpt * ct * pt * m;
        }

        private static Matrix4 GetBaseChainTransformation(IMachineElement element)
        {
            var t = GetChainTransformation(element.Parent);
            var et = Convert(element.Transformation);
            var lt = GetLinkTransformation(element.LinkToParent);

            return lt * et * t;
        }

        public static Matrix4 GetToolholderTransformation(IToolholderBase th)
        {
            var p = th.Position;
            var d = th.Direction;
            var tp = Matrix4.CreateTranslation((float)p.X, (float)p.Y, (float)p.Z);
            var td = GetDirectionTransformation(new Vector3((float)d.X, (float)d.Y, (float)d.Z));

            return td * tp;
        }

        public static Matrix4 GetDirectionTransformation(MDB.Vector v) => GetDirectionTransformation(new Vector3((float)v.X, (float)v.Y, (float)v.Z));

        public static Matrix4 GetDirectionTransformation(Vector3 v)
        {
            var t = new Vector3(0, 0, -1);
            var s = (v.X * t.X) + (v.Y * t.Y) + (v.Z * t.Z);

            if (s == 1.0)
            {
                return Matrix4.Identity;
            }
            else if (s == -1.0)
            {
                return new Matrix4() { M11 = -1, M22 = -1, M33 = -1, M44 = 1 };
            }
            else
            {
                return CreateRotatioMatrix(t, v);
            }
        }

        private static Matrix4 CreateRotatioMatrix(Vector3 start, Vector3 target)
        {
            var n = Vector3.Cross(start, target);
            var a = Vector3.CalculateAngle(start, target);

            n.Normalize();

            return Matrix4.CreateFromAxisAngle(n, a);
        }

        public static Matrix4 GetLinkTransformation(ILinkViewModel linkToParent)
        {
            if (linkToParent != null)
            {
                switch (linkToParent.Type)
                {
                    case Data.Enums.LinkType.Linear:
                        return GetLinearLinkTransformation(linkToParent);
                    case Data.Enums.LinkType.Rotary:
                        return GetRotaryLinkTransformation(linkToParent);
                    default:
                        throw new ArgumentOutOfRangeException($"Link type {linkToParent.Type} not supported!");
                }
            }
            else
            {
                return Matrix4.Identity;
            }
        }

        private static Matrix4 GetLinearLinkTransformation(ILinkViewModel linkToParent)
        {
            float x = 0.0f, y = 0.0f, z = 0.0f;
            float v = (float)linkToParent.Value;

            if (linkToParent is ILinearLinkViewModel llvm) v -= (float)llvm.Pos;

            switch (linkToParent.Direction)
            {
                case Data.Enums.LinkDirection.X:
                    x = v;
                    break;
                case Data.Enums.LinkDirection.Y:
                    y = v;
                    break;
                case Data.Enums.LinkDirection.Z:
                    z = v;
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"Direction {linkToParent.Direction} not managed!");
            }

            return Matrix4.CreateTranslation(x, y, z);
        }

        private static Matrix4 GetRotaryLinkTransformation(ILinkViewModel linkToParent)
        {
            float a = (float)(linkToParent.Value * MathHelper.Pi / 180.0);
            Matrix4 matrix;

            switch (linkToParent.Direction)
            {
                case Data.Enums.LinkDirection.X:
                    Matrix4.CreateRotationX(a, out matrix);
                    break;
                case Data.Enums.LinkDirection.Y:
                    Matrix4.CreateRotationY(a, out matrix);
                    break;
                case Data.Enums.LinkDirection.Z:
                    Matrix4.CreateRotationZ(a, out matrix);
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"Direction {linkToParent.Direction} not managed!");
            }

            return matrix;
        }

        public static Matrix4 Convert(MDB.Matrix m)
        {
            if (m == null) return Matrix4.Identity;

            var result = new Matrix4((float)m.M11, (float)m.M12, (float)m.M13, (float)m.OffsetX,
                                        (float)m.M21, (float)m.M22, (float)m.M23, (float)m.OffsetY,
                                        (float)m.M31, (float)m.M32, (float)m.M33, (float)m.OffsetZ,
                                        0.0f, 0.0f, 0.0f, 1.0f);

            result.Transpose();
            return result;
        }

        public static Matrix4 ConvertToTranslationMatrix(MDB.Point p) => Matrix4.CreateTranslation((float)p.X, (float)p.Y, (float)p.Z);
    }

}
