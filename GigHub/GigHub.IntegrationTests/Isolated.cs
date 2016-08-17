using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework.Interfaces;
using System.Transactions;

namespace GigHub.IntegrationTests
{
    public class Isolated : Attribute, ITestAction
    {
        private TransactionScope _transaction;

        public ActionTargets Targets
        {
            get
            {
                return ActionTargets.Test;
            }
        }

        public void AfterTest(ITest test)
        {
            _transaction.Dispose();
        }

        public void BeforeTest(ITest test)
        {
            _transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        }
    }
}
