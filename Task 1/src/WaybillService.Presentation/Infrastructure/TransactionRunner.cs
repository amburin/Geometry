using System;
using System.Threading.Tasks;
using System.Transactions;

namespace WaybillService.Presentation.Infrastructure
{
    public static class TransactionRunner
    {
        public static Task<TResult> RunWithTransactionAsync<TResult>(Func<Task<TResult>> func)
        {
            return RunWithTransactionAsync(func, IsolationLevel.ReadCommitted, TransactionScopeOption.Required);
        }

        public static async Task<TResult> RunWithTransactionAsync<TResult>(Func<Task<TResult>> func, IsolationLevel il,
            TransactionScopeOption scopeOption)
        {
            using (var transaction = new TransactionScope(
                scopeOption,
                new TransactionOptions
                {
                    IsolationLevel = il
                }, 
                TransactionScopeAsyncFlowOption.Enabled)
            )
            {
                var result = await func();
                transaction.Complete();
                return result;
            }
        }

        public static Task RunWithTransactionAsync(Func<Task> func)
        {
            return RunWithTransactionAsync(func, IsolationLevel.ReadCommitted, TransactionScopeOption.Required);
        }
    
        public static async Task RunWithTransactionAsync(Func<Task> func, IsolationLevel il,
                TransactionScopeOption scopeOption)
            {
                using (var transaction = new TransactionScope(
                    scopeOption,
                    new TransactionOptions
                    {
                        IsolationLevel = il
                    },
                    TransactionScopeAsyncFlowOption.Enabled)
                )
                {
                    await func();
                    transaction.Complete();
                }
            }
        }
}
