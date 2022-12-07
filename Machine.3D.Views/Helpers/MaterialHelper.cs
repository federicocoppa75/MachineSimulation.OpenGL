﻿using Machine._3D.Views.Programs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OTKM = OpenTK.Mathematics;
using MDB = Machine.Data.Base;

namespace Machine._3D.Views.Helpers
{
    internal static class MaterialHelper
    {
        public static Material Convert(MDB.Color color)
        {
            return new Material()
            {
                ambient = new OTKM.Vector3(color.R / 255f, color.G / 255f, color.B / 255f),
                diffuse = new OTKM.Vector3(color.R / 255f, color.G / 255f, color.B / 255f),
                specular = new OTKM.Vector3(0.5f, 0.5f, 0.5f),
                shininess = 32
            };
        }

        public static void SetMaterial(BaseProgram program, MDB.Color color)
        {
            var m = Convert(color);

            program.MaterialAmbient.Set(m.ambient);
            program.MaterialDiffuse.Set(m.diffuse);
            program.MaterialSpecular.Set(m.specular);
            program.MaterialShininess.Set(m.shininess);
        }
    }
}
