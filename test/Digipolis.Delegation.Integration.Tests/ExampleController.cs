using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Digipolis.Delegation.Integration.Tests.TestClasses;
using Xunit;

namespace Digipolis.Delegation.Integration.Tests
{
    public class ExampleControllerTests : BaseTestController
    {
        public ExampleControllerTests(ExampleAppFactory factory) : base(factory)
        {
        }
        
        [Fact]
        public async Task ShouldGetTokenFromInnerCall()
        {
            //first get a test otken
            //var result = await GetData<string>("/Example/GetJwtToken");

            string token = "eyJ4NXUiOiJodHRwczpcL1wvYXBpLWd3LWEuYW50d2VycGVuLmJlXC9rZXlzXC9wdWIiLCJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYmYiOjE2MDc5MzU3NjcsImlhdCI6MTYwNzkzNTc2NywiaXNzIjoiaHR0cHM6XC9cL2FwaS1ndy1hLmFudHdlcnBlbi5iZSIsImF1ZCI6ImF1dGh6LWFwcDEtYS5hbnR3ZXJwZW4uYmUiLCJYLUF1dGhlbnRpY2F0ZWQtVXNlcmlkIjoicmMwMTcwNEBkaWdhbnQuYW50d2VycGVuLmxvY2FsIiwiWC1Db25zdW1lci1JRCI6IjQ2N2Q2ZmJkLThlMjAtNDg2Ny1iMTY2LTcxMTBmZmNjZjQxZiIsIlgtQ29uc3VtZXItVXNlcm5hbWUiOiJpbnQtdGVzdG5ldHRvb2xib3gudGVzdGF1dGgudjEiLCJYLUNvbnN1bWVyLUN1c3RvbS1JRCI6ImludC10ZXN0bmV0dG9vbGJveC50ZXN0YXV0aC52MSIsImV4cCI6MTYwODE1MjA2NywiWC1KV1QtSXNzdWVyIjoiaHR0cHM6XC9cL2FwaS1ndy1hLmFudHdlcnBlbi5iZSIsImp0aSI6IlNaVWN1dlhhMU5nRjhHblVEMTh3VWFxdEpNbVFQZmxsIiwiWC1Db25zdW1lci1Hcm91cHMiOiJhY3BhYXMubWVhdXRoei52MiwgYWNwYWFzLmF1dGh6LnY0In0.hM54ZXFP6a0WZBJGiHLTL2G1gKJ3CEXMbO0j83ybdgpJwy1I8Wxc4-2rGbJ4D_7FmTR9rN7-4ThF0j_aqoCynIde7fT9zmc64s-EuWZhZlcDmwBuwzzUIbIaNIxrehN2fCYxArzamoWUCM5gvMscezbQKfARZR1ve4aSR4ThfP8jqCpK0B9rqfnGOW8EYpoNPIensEMFFnBcXc7aoby1_DYOUes7Ob5ykEGrNTvTjAUbBHox0LKP7UTbSQ5BdoOxQPktJpoUiGnZkgTyUzpUAvBu0hqfBdKmngZgysBunDzy06gmp8zBAvOOkl-wrl3YN9Q4MVyG_lpP-IOLk3AlLjgr3WJlcxSrZOgjEbN0_2Ca6ZqtXmvBRKizmCJhE93s6tb2V7s5cNi2e3pBxeZ4C6TYlklpDJfNf_70IOlpZhyVYqQG3yE8gn0-MBr-ZK8WABC4vCBe4jLEN8Q07ErJ2eji9fHtEe2KBFNYduDPYwQbn5COS3FbPJ7gFyNpgCjErj6jUy8ZmwOT-Mf9V_pMR5-A_v353_mvcwIV2Zp1QZ43maWZdi9KcsSyJeheiQ-8aw0j7zoifVV2Yz978GUzM-lRtiJ-eDw6-hEAnfPhofqUQIKV2Pp2ZxDSYaiWZURR8i0r1uAgzZzacDrp05qACns6oX20S904mO0fjY2z5W4";
            
            var innerToken = await GetData<string>("/Example/GetUserInfo", new Dictionary<string, string>
            {
                {"dgp-authorization-for", $"Bearer {token}" }
            });
        }

        
    }
}