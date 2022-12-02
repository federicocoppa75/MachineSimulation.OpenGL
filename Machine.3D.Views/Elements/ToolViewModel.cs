﻿using Machine._3D.Views.Programs;
using Machine.ViewModels.Interfaces.MachineElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using M3DVG = Machine._3D.Views.Geometries;
using OTKM = OpenTK.Mathematics;

namespace Machine._3D.Views.Elements
{
    internal class ToolViewModel : ElementViewModel
    {
        public M3DVG.Mesh ConeGeometry { get; set; }

        public override bool IsVisible => IsVisibleBase();

        public override void Draw(BaseProgram program, OTKM.Matrix4 projection, OTKM.Matrix4 view)
        {
            base.Draw(program, projection, view);

            if (ConeGeometry!= null) DrawCone(program, projection, view);
        }

        protected void DrawCone(BaseProgram program, OTKM.Matrix4 projection, OTKM.Matrix4 view)
        {
            var t = Element as ViewModels.MachineElements.ToolViewModel;

            SetMaterial(program, t.ConeColor);
            OTKM.Matrix4 model = GetChainTransformation();
            program.ModelViewProjectionMatrix.Set(model * view * projection);

            ConeGeometry.Draw(program);
        }
    }
}
