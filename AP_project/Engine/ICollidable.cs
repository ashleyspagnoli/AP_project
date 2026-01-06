using System;
using System.Collections.Generic;
using System.Text;

namespace AP_project.Engine
{
    public interface ICollidable
    {
        void OnCollide(Entity other);
    }
}
