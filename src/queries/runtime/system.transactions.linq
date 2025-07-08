<Query Kind="Program" />

void Main()
{
	var transaction = new CommittableTransaction(new TransactionOptions()
	{
		IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
	});
	
	TransactionManager.ImplicitDistributedTransactions = true;

	transaction.EnlistDurable(Guid.NewGuid(), new Test(), new()
	{

	});

	try
	{
		transaction.Commit();
	}
	catch (Exception exception)
	{
		transaction.Rollback();
	}


}


public class Test : IEnlistmentNotification
{
	public void Commit(Enlistment enlistment)
	{

		throw new NotImplementedException();
	}

	public void InDoubt(Enlistment enlistment)
	{
		throw new NotImplementedException();
	}

	public void Prepare(PreparingEnlistment preparingEnlistment)
	{
		throw new NotImplementedException();
	}

	public void Rollback(Enlistment enlistment)
	{
		throw new NotImplementedException();
	}
}

