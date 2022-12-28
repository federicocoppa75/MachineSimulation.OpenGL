using Machine._3D.Views.Programs;
using M3DVG = Machine._3D.Views.Geometries;
using OTKM = OpenTK.Mathematics;
using MVMME = Machine.ViewModels.MachineElements;
using M3DVH = Machine._3D.Views.Helpers;

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
            var t = Element as MVMME.ToolViewModel;

            M3DVH.MaterialHelper.SetMaterial(program, t.ConeColor);
            OTKM.Matrix4 model = GetChainTransformation();
            program.ModelViewProjectionMatrix.Set(model * view * projection);

            ConeGeometry.Draw();
        }
    }
}
