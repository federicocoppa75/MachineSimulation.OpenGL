using Machine.ViewModels.Interfaces;
using Machine.ViewModels.Interfaces.MachineElements;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Machine.ViewModels.GeometryExtensions.Math;
using Machine.ViewModels.GeometryExtensions.Helpers;
using System.Runtime.InteropServices.ComTypes;

namespace Machine.ViewModels.GeometryExtensions
{
    internal static class ElementViewModelTransformExtension
    {
        public static Matrix4 GetChainTransformation(this IMachineElement endOfChain, bool fromRoot = false)
        {
            IMachineElement p = endOfChain;
            var list = new List<IMachineElement>();
            var matrix = Matrix4.Identity;

            while (p != null)
            {
                list.Add(p);
                p = p.Parent;
            }

            if (!fromRoot)
            {
                for (int i = (list.Count - 1); i >= 0; i--) matrix.Append(GetElementTransformation(list[i]));
            }
            else
            {
                for (int i = 0; i < list.Count; i++) matrix.Append(GetElementTransformation(list[i]));
            }

            return matrix;
        }

        private static Matrix4 GetElementTransformation(IMachineElement e)
        {
            if (e is IPanelElement pe)
            {
                return pe.GetTransformation();
            }
            else if (e == null)
            //else if ((e == null) || (e.Transformation == null))
            //if ((e == null) || (e.Transformation == null))
            {
                return Matrix4.Identity;
            }
            else
            {
                var ts = (e.Transformation != null) ? ChainTransformtionHelper.Convert(e.Transformation) : Matrix4.Identity;

                if (e.LinkToParent != null) ts.Append(ChainTransformtionHelper.GetLinkTransformation(e.LinkToParent));

                if (e.Parent is IToolholderBase th)
                {
                    var dt = ChainTransformtionHelper.GetDirectionTransformation(th.Direction);
                    var pt = ChainTransformtionHelper.ConvertToTranslationMatrix(th.Position);

                    ts = dt * pt * ts;
                }


                return ts;
            }
        }

        private static Matrix4 GetTransformation(this IPanelElement panel)
        {
            var matrix = Matrix4.Identity;

            if (panel is IMovablePanel mp)
            {
                var m = Matrix4.Identity;

                m.M41 = (float)mp.OffsetX;
                matrix.Append(m);
            }

            var mc = Matrix4.Identity;

            mc.M41 = (float)panel.CenterX;
            mc.M42 = (float)panel.CenterY;
            mc.M43 = (float)panel.CenterZ;

            matrix.Append(mc);

            return matrix;
        }

    }
}
