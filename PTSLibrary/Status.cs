using System;
using System.Collections.Generic;
using System.Text;

namespace PTSLibrary
{
    public enum Status
    {
        ReadyToStart = 1,
        InProgress = 2,
        Completed = 3,
        WaitingForPredecessor = 4
    }
}
