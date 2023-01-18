using Machine._3D.Views.Programs;
using M3DVG = Machine._3D.Views.Geometries;
using OTKM = OpenTK.Mathematics;
using M3DVH = Machine._3D.Views.Helpers;
using MDB = Machine.Data.Base;

namespace Machine._3D.Views.Elements
{
    internal class ToolViewModel : ElementViewModel
    {
        private static MDB.Color _coneColor = new MDB.Color() { A = 255, B = 128, G = 128, R = 128 };

        public M3DVG.Mesh ConeGeometry { get; set; }

        public override bool IsVisible => IsVisibleBase();

        public override void Draw(IProgram program, OTKM.Matrix4 projection, OTKM.Matrix4 view)
        {
            base.Draw(program, projection, view);

            if (ConeGeometry!= null) DrawCone(program, projection, view);
        }

        protected void DrawCone(IProgram program, OTKM.Matrix4 projection, OTKM.Matrix4 view)
        {

            M3DVH.MaterialHelper.SetMaterial(program, _coneColor);
            OTKM.Matrix4 model = GetChainTransformation();
            program.ModelViewProjectionMatrix.Set(model * view * projection);

            ConeGeometry.Draw();
        }
    }
}
