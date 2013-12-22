﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Concordion.Api.Listener
{
    public class AssertSuccessEvent
    {
        #region Properties

        public Element Element
        {
            get;
            private set;
        }

        #endregion

        #region Constructors

        public AssertSuccessEvent(Element element) {
            this.Element = element;
        }

        #endregion
    }
}
