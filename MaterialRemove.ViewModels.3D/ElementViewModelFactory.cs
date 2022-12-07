using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MRVMI = MaterialRemove.ViewModels.Interfaces;
using MRVM = MaterialRemove.ViewModels;

namespace MaterialRemove.ViewModels._3D
{
    public class ElementViewModelFactory : MRVMI.IElementViewModelFactory
    {
        public MRVM.SectionFaceViewModel CreateSectionFaceViewModel() => new SectionFaceViewModel();

        public MRVM.SectionVolumeViewModel CreateSectionVolumeViewModel() => new SectionVolumeViewModel();
    }
}
