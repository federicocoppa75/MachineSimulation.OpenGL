using Machine._3D.Views.Programs;
using Machine.ViewModels.Interfaces.MachineElements;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using M3DVH = Machine._3D.Views.Helpers;

namespace Machine._3D.Views.Elements
{
    internal class PanelViewModel : ElementViewModel
    {
        public override bool IsVisible => IsVisibleBase();

        public override void Draw(BaseProgram program, Matrix4 projection, Matrix4 view)
        {
            M3DVH.MaterialHelper.SetMaterial(program, new Data.Base.Color() { R = 253, G = 131, B = 0, A = 255 });
            Matrix4 model = GetChainTransformation();
            program.ModelViewProjectionMatrix.Set(model * view * projection);

            Geometry.Draw();
        }
    }
}
