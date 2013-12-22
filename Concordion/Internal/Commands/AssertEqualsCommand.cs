﻿// Copyright 2009 Jeffrey Cameron
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//   http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordion.Api;
using Concordion.Api.Listener;
using Concordion.Internal.Util;

namespace Concordion.Internal.Commands
{
    public class AssertEqualsCommand : AbstractCommand
    {
        #region Fields

        private IComparer<object> m_comparer;
        private readonly List<IAssertEqualsListener> m_Listeners = new List<IAssertEqualsListener>();

        #endregion

        #region Constructors

        public AssertEqualsCommand()
            : this(new BrowserStyleWhitespaceComparer())
        {
        }

        public AssertEqualsCommand(IComparer<object> comparer)
        {
            m_comparer = comparer;
        }

        #endregion

        #region Methods

        public void addAssertEqualsListener(IAssertEqualsListener listener)
        {
            this.m_Listeners.Add(listener);
        }

        public void removeAssertEqualsListener(IAssertEqualsListener listener)
        {
            this.m_Listeners.Add(listener);
        }

        private void AnnounceSuccess(Element element)
        {
            foreach (var assertEqualsListener in m_Listeners)
            {
                assertEqualsListener.SuccessReported(new AssertSuccessEvent(element));
            }
        }

        private void AnnounceFailure(Element element, String expected, Object actual)
        {
            foreach (var assertEqualsListener in m_Listeners)
            {
                assertEqualsListener.FailureReported(new AssertFailureEvent(element, expected, actual));
            }
        }

        private void OnSuccessReported(Element element)
        {
            if (SuccessReported != null)
            {
                SuccessReported(this, new SuccessReportedEventArgs { Element = element });
            }
        }

        private void OnFailureReported(Element element, object actual, string expected)
        {
            if (FailureReported != null)
            {
                FailureReported(this, new FailureReportedEventArgs { Element = element, Actual = actual, Expected = expected });
            }
        }

        #endregion

        #region ICommand Members

        public override void Verify(CommandCall commandCall, IEvaluator evaluator, IResultRecorder resultRecorder)
        {
            Check.IsFalse(commandCall.HasChildCommands, "Nesting commands inside an 'assertEquals' is not supported");

            Element element = commandCall.Element;

            object actual = evaluator.Evaluate(commandCall.Expression);
            string expected = element.Text;

            if (m_comparer.Compare(actual, expected) == 0)
            {
                resultRecorder.Record(Result.Success);
                AnnounceSuccess(element);
            }
            else
            {
                resultRecorder.Record(Result.Failure);
                AnnounceFailure(element, expected, actual);
            }
        }

        #endregion

        #region IResultReporter Members

        public event EventHandler<SuccessReportedEventArgs> SuccessReported;
        public event EventHandler<FailureReportedEventArgs> FailureReported;

        #endregion
    }
}
