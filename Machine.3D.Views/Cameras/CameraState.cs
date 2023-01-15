using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machine._3D.Views.Cameras
{

    public class CameraState
    {
        public Vector3 Position;
        public Vector3 LookAt;
        public Vector3 Up;

        public CameraState()
        {
            LookAt = -Vector3.UnitZ;
            Up = Vector3.UnitY;
        }

        public override string ToString()
        {
            return string.Format((string)"({0},{1},{2})", (object)Position, (object)LookAt, (object)Up);
        }
    }

}
