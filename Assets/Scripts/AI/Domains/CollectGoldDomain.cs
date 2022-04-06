﻿using System;
using FluidHTN;

public static class CollectGoldDomain
{
    public static Domain<AIContext, int> Create()
    {
        /// DO NOT USE SPLICING OF COMPLEX DOMAINS
        /// Only splice primitive actions, in order to avoid infinite loops
        return new AIDomainBuilder("Collect Gold Domain")
            .Select("Collect Gold")
                .CanCollect("Gold")

                .Splice(PrimitiveActions.CollectGoldAction)

                .Sequence("Recruit worker and collect gold")
                    .Splice(RecruitWorkerDomain.Create())   
                    .Splice(PrimitiveActions.CollectGoldAction)     
                .End()

                .Sequence("Re-assign worker to collect gold")
                    .UnassignWorker()
                    .Splice(PrimitiveActions.CollectGoldAction)
                .End()
            .End()
            .Build();
    }

}

