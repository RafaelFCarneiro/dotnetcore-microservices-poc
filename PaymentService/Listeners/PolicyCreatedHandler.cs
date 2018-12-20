﻿using MediatR;
using PaymentService.Domain;
using PolicyService.Api.Events;
using System.Threading;
using System.Threading.Tasks;

namespace PaymentService.Listeners
{
    public class PolicyCreatedHandler : INotificationHandler<PolicyCreated>
    {
        private readonly IDataStore dataStore;
        private readonly PolicyAccountNumberGenerator policyAccountNumberGenerator;

        public PolicyCreatedHandler(IDataStore dataStore, PolicyAccountNumberGenerator policyAccountNumberGenerator)
        {
            this.dataStore = dataStore;
            this.policyAccountNumberGenerator = policyAccountNumberGenerator;
        }

        public async Task Handle(PolicyCreated notification, CancellationToken cancellationToken)
        {
            var policy = new PolicyAccount(notification.PolicyNumber, policyAccountNumberGenerator.Generate());

            using (dataStore)
            {
                dataStore.PolicyAccounts.Add(policy);
                dataStore.CommitChanges();
            }
        }
    }
}
