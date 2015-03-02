﻿// ======================================================== IDataLink.cs
namespace Kerosene.ORM.Core
{
	using Kerosene.Tools;
	using System;
	using System.Linq;

	// ==================================================== 
	/// <summary>
	/// Represents an abstract connection against an underlying database, also acting as a
	/// factory to create objects adapted to it.
	/// </summary>
	public interface IDataLink : IDisposableEx, ICloneable
	{
		/// <summary>
		/// Returns a new instance that is a copy of the original one.
		/// </summary>
		/// <returns>A new instance.</returns>
		new IDataLink Clone();

		/// <summary>
		/// The engine this link is associated with, that maintains the main characteristics
		/// of the underlying database engine.
		/// </summary>
		IDataEngine Engine { get; }

		/// <summary>
		/// The parser instance this link maintains.
		/// </summary>
		IParser Parser { get; }

		/// <summary>
		/// The nestable transaction this link maintains.
		/// </summary>
		INestableTransaction Transaction { get; }

		/// <summary>
		/// Gets or sets the default transaction mode to use when creating a new transaction
		/// for this instance.
		/// </summary>
		NestableTransactionMode DefaultTransactionMode { get; set; }

		/// <summary>
		/// Opens the connection against the underlying database-alike service.
		/// <para>Note that this method is called automatically by the framework when needed.</para>
		/// </summary>
		void Open();

		/// <summary>
		/// Closes the connection that might be opened against the underlying database-alike
		/// service.
		/// <para>Note that this method is called automatically by the framework when needed.</para>
		/// </summary>
		void Close();

		/// <summary>
		/// Whether the connection against the underlying database-alike service can be considered
		/// to be opened or not.
		/// </summary>
		bool IsOpen { get; }

		/// <summary>
		/// Factory method invoked to create an enumerator to execute the given enumerable
		/// command.
		/// </summary>
		/// <param name="command">The command to execute.</param>
		/// <returns>An enumerator able to execute de command.</returns>
		IEnumerableExecutor CreateEnumerableExecutor(IEnumerableCommand command);

		/// <summary>
		/// Factory method invoked to create an executor to execute the given scalar command.
		/// </summary>
		/// <param name="command">The command to execute.</param>
		/// <returns>An executor able to execute de command.</returns>
		IScalarExecutor CreateScalarExecutor(IScalarCommand command);

		/// <summary>
		/// Creates a new raw command for this link.
		/// </summary>
		/// <returns>The new command.</returns>
		IRawCommand Raw();

		/// <summary>
		/// Creates a new raw command for this link, and sets its initial contents.
		/// </summary>
		/// <param name="text">The text of the command. Embedded arguments are specified using
		/// the standard positional '{n}' format.</param>
		/// <param name="args">An optional collection containing the arguments to be used by
		/// this command.</param>
		/// <returns>The new command.</returns>
		IRawCommand Raw(string text, params object[] args);

		/// <summary>
		/// Creates a new raw command for this link, and sets its initial contents.
		/// </summary>
		/// <param name="spec">A dynamic lambda expression that when parsed specified the new
		/// contents of this command.</param>
		/// <returns>The new command.</returns>
		IRawCommand Raw(Func<dynamic, object> spec);

		/// <summary>
		/// Creates a new query command for this link.
		/// </summary>
		/// <returns>The new command.</returns>
		IQueryCommand Query();

		/// <summary>
		/// Creates a new query command for this link, and sets the initial contents of its
		/// FROM clause.
		/// </summary>
		/// <param name="froms">The collection of lambda expressions that resolve into the
		/// elements to include in this clause:
		/// <para>- A string, as in 'x => "name AS alias"', where the alias part is optional.</para>
		/// <para>- A table specification, as in 'x => x.Table.As(alias)', where both the alias part
		/// is optional.</para>
		/// <para>- Any expression that can be parsed into a valid SQL sentence for this clause.</para>
		/// </param>
		/// <returns>The new command.</returns>
		IQueryCommand From(params Func<dynamic, object>[] froms);

		/// <summary>
		/// Creates a new query command for this link, and sets the initial contents of its
		/// SELECT clause.
		/// </summary>
		/// <param name="selects">The collection of lambda expressions that resolve into the
		/// elements to include into this clause:
		/// <para>- A string, as in 'x => "name AS alias"', where the alias part is optional.</para>
		/// <para>- A table and column specification, as in 'x => x.Table.Column.As(alias)', where
		/// both the table and alias parts are optional.</para>
		/// <para>- A specification for all columns of a table using the 'x => x.Table.All()' syntax.</para>
		/// <para>- Any expression that can be parsed into a valid SQL sentence for this clause.</para>
		/// </param>
		/// <returns>The new command.</returns>
		IQueryCommand Select(params Func<dynamic, object>[] selects);

		/// <summary>
		/// Creates a new insert command for this link.
		/// </summary>
		/// <param name="table">A dynamic lambda expression that resolves into the table the new
		/// command will refer to.</param>
		/// <returns>The new command.</returns>
		IInsertCommand Insert(Func<dynamic, object> table);

		/// <summary>
		/// Creates a new delete command for this link.
		/// </summary>
		/// <param name="table">A dynamic lambda expression that resolves into the table the new
		/// command will refer to.</param>
		/// <returns>The new command.</returns>
		IDeleteCommand Delete(Func<dynamic, object> table);

		/// <summary>
		/// Creates a new update command for this link.
		/// </summary>
		/// <param name="table">A dynamic lambda expression that resolves into the table the new
		/// command will refer to.</param>
		/// <returns>The new command.</returns>
		IUpdateCommand Update(Func<dynamic, object> table);
	}
}
// ======================================================== 