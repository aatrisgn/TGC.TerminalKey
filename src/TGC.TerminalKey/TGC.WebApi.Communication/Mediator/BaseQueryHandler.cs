namespace TGC.HomeAutomation.Application.Abstractions;

public abstract class BaseQueryHandler<TQuery> where TQuery : class, IQuery
{
	public virtual bool Accepts(IQuery query) => query is TQuery;
	protected TQuery GetTypedQuery(IQuery query) => query as TQuery;
}