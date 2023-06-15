﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDK.Core.Attributes
{
    public class NdkPrimaryKey:System.Attribute
    {
        public bool AutoGenerated { get; set;}

        public NdkPrimaryKey(bool autoGenerated) 
        {
            this.AutoGenerated = autoGenerated;
        }
    }
}
