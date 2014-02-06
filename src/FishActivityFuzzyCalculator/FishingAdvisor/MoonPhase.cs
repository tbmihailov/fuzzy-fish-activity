using FuzzyFramework.Dimensions;
using FuzzyFramework.Members;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SampleProject
{
    public class MoonPhase : DiscreteMember
    {
        /// <summary>
        /// MoonPhase constructur
        /// </summary>
        /// <param name="dimension">Dimension of the set. In this case, it is "moon"</param>
        /// <param name="caption"></param>
        public MoonPhase(IDiscreteDimension dimension, string caption)
            : base(dimension, caption)
        {
        }

    }
}
