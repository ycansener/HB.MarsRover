using System;
using System.Collections.Generic;
using System.Text;

namespace HB.MarsRover.Domain.Models
{
    // For environment, domain should only knows the contract of the Environment, since the specs of the environment are not set in stone
    public interface IEnvironment
    {
        /// <summary>
        /// Gets the size of the X axis - starts from 1
        /// </summary>
        /// <returns>size of the x axis of the environment</returns>
        int GetSizeX();
        /// <summary>
        /// Gets the size of the Y axis - starts from 1
        /// </summary>
        /// <returns>size of the y axis of the environment</returns>
        int GetSizeY();
    }
}
