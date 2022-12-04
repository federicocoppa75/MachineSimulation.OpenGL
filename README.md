# MachineSimulation.OpenGL
This project is an alternative of [**MachineSimulation.NET**](https://github.com/federicocoppa75/MachineSimulation.NET): the 3D view is based on OpenGL wrapped by OpenTK.
The struct of the application il based on MVVM pattern; the models, view models and views, except 3D view, are the same of [**MachineSimulation.NET**](https://github.com/federicocoppa75/MachineSimulation.NET), the diffent is the implementation of the 3D view.

## Machine.Viewer
Application for view the machine with relative tooling.

## Machine.3D.Views
View of the machine implemented on OpenGL. The use of OpenGL is made by these packages:
* [**OpenTK**](https://github.com/opentk/opentk) - the C# library that bind OpenGL;
* [**ObjectTK**](https://github.com/opentk/ObjectTK) - a thin abstraction layer on top of OpenTK to provide OpenGL features in an object-oriented;
* [**GLWpfControl**](https://github.com/opentk/GLWpfControl) - a native control for WPF in OpenTK.

## Machine.ViewModels.GeometryExtensions
Class library for factories, builders and helpers.