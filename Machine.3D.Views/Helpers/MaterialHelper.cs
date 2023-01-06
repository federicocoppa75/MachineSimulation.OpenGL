using Machine._3D.Views.Programs;
using OTKM = OpenTK.Mathematics;
using MDB = Machine.Data.Base;
using MVMGEM = Machine.ViewModels.GeometryExtensions.Materials;

namespace Machine._3D.Views.Helpers
{
    internal static class MaterialHelper
    {
        public static Material Convert(MDB.Color color)
        {
            return new Material()
            {
                ambient = new OTKM.Vector4(color.R / 255f, color.G / 255f, color.B / 255f, color.A / 255f),
                diffuse = new OTKM.Vector4(color.R / 255f, color.G / 255f, color.B / 255f, color.A / 255f),
                specular = new OTKM.Vector4(0.5f, 0.5f, 0.5f, 1),
                shininess = 32
            };
        }

        public static void SetMaterial(BaseProgram program, MDB.Color color)
        {
            var m = Convert(color);

            program.material.Set(new Material()
            {
                ambient = m.ambient,
                diffuse = m.diffuse,
                specular = m.specular,
                shininess = m.shininess
            });
        }

        public static void SetMaterial(BaseProgram program, MVMGEM.Material material)
        {
            program.material.Set(new Material()
            {
                ambient = material.Ambient,
                diffuse = material.Diffuse,
                specular = material.Ambient,
                shininess = material.Shininess
            });
        }
    }
}
