using Machine._3D.Views.Programs;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machine._3D.Views.Elements.SectionedPanel
{
    internal class PanelViewModel : ElementViewModel
    {
        private InnerPanelViewModel _innerPanel = new InnerPanelViewModel();
        private OuterPanelViewModel _outerPanel = new OuterPanelViewModel();

        public override bool IsVisible => IsVisibleBase();

        public override void Draw(IProgram program, Matrix4 projection, Matrix4 view)
        {
            _innerPanel.Draw(program, projection, view);
            _outerPanel.Draw(program, projection, view);
        }

        public void Initialize()
        {
            _innerPanel.Element = Element;
            _outerPanel.Element = Element;
            _innerPanel.Initialize();
            _outerPanel.Initialize();
        }
    }
}
