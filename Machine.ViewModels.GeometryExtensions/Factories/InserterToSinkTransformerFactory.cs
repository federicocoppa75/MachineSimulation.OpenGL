using Machine.ViewModels.GeometryExtensions.Helpers;
using Machine.ViewModels.Interfaces.Insertions;
using Machine.ViewModels.Interfaces.MachineElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machine.ViewModels.GeometryExtensions.Factories
{
    public class InserterToSinkTransformerFactory : IInserterToSinkTransformerFactory
    {
        public IInserterToSinkTransformer GetTransformer(IInsertionsSink sink, IInjectorElement injector) => new InserterToSinkTransformer(sink, injector);
    }
}
