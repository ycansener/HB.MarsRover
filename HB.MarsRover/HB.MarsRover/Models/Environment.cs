using System;
using System.Collections.Generic;
using System.Text;
using HB.MarsRover.Domain.Models;

namespace HB.MarsRover.Models
{
    public class MarsEnvironment : IEnvironment
    {
        private readonly int[,] _environment;

        public MarsEnvironment(int sizeX, int sizeY)
        {
            _environment = new int[sizeX, sizeY];
        }

        public int GetSizeX()
        {
            return _environment.GetLength(0);
        }

        public int GetSizeY()
        {
            return _environment.GetLength(1);
        }
    }
}
