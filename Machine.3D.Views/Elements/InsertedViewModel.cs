using Machine._3D.Views.Programs;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machine._3D.Views.Elements
{
    internal class InsertedViewModel : ElementViewModel
    {
        public override bool IsVisible => IsVisibleBase();

        public override void Draw(BaseProgram program, Matrix4 projection, Matrix4 view)
        {
            base.Draw(program, projection, view);
        }
    }
}
