using System;
using System.Text;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.calc.runner
{

	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using TypedMetaBean = org.joda.beans.TypedMetaBean;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using LightMetaBean = org.joda.beans.impl.light.LightMetaBean;

	using Messages = com.opengamma.strata.collect.Messages;
	using Result = com.opengamma.strata.collect.result.Result;
	using ScenarioArray = com.opengamma.strata.data.scenario.ScenarioArray;

	/// <summary>
	/// The result of a single calculation.
	/// <para>
	/// This stores the calculated result for a single cell in the output grid.
	/// A set of related results for a single target can be stored in a <seealso cref="CalculationResults"/> instance.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(style = "light") public final class CalculationResult implements org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class CalculationResult : ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition private final int rowIndex;
		private readonly int rowIndex;
	  /// <summary>
	  /// The column index of the value in the results grid.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition private final int columnIndex;
	  private readonly int columnIndex;
	  /// <summary>
	  /// The result of the calculation.
	  /// <para>
	  /// The result may be a single value or a multi-scenario value.
	  /// A multi-scenario value will implement <seealso cref="ScenarioArray"/> unless it has been aggregated.
	  /// </para>
	  /// <para>
	  /// If the calculation did not complete successfully, a failure result will be returned
	  /// explaining the problem. Callers must check whether the result is a success or failure
	  /// before examining the result value.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.collect.result.Result<?> result;
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
	  private readonly Result<object> result;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance for the specified row and column index in the output grid.
	  /// <para>
	  /// The <seealso cref="Result"/> object captures the result value, or the failure that
	  /// prevented the result from being calculated.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="rowIndex">  the row index of the value in the results grid </param>
	  /// <param name="columnIndex">  the column index of the value in the results grid </param>
	  /// <param name="result">  the result of the calculation </param>
	  /// <returns> a calculation result containing the row index, column index and result object </returns>
	  public static CalculationResult of<T1>(int rowIndex, int columnIndex, Result<T1> result)
	  {
		return new CalculationResult(rowIndex, columnIndex, result);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the result of the calculation, casting the result to a known type.
	  /// <para>
	  /// The result may be a single value or a multi-scenario value.
	  /// A multi-scenario value will implement <seealso cref="ScenarioArray"/> unless it has been aggregated.
	  /// </para>
	  /// <para>
	  /// If the calculation did not complete successfully, a failure result will be returned
	  /// explaining the problem. Callers must check whether the result is a success or failure
	  /// before examining the result value.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <T>  the result type </param>
	  /// <param name="type">  the result type </param>
	  /// <returns> the result, cast to the specified type </returns>
	  /// <exception cref="ClassCastException"> if the result is not of the specified type </exception>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") public <T> com.opengamma.strata.collect.result.Result<T> getResult(Class<T> type)
	  public Result<T> getResult<T>(Type<T> type)
	  {
		// cannot use result.map() as we want the exception to be thrown
		if (result.Failure || type.IsInstanceOfType(result.Value))
		{
		  return (Result<T>) result;
		}
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		throw new System.InvalidCastException(Messages.format("Result queried with type '{}' but was '{}'", type.FullName, result.Value.GetType().FullName));
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns a copy of this result with the underlying result updated.
	  /// </summary>
	  /// <param name="underlyingResult">  the new underlying result </param>
	  /// <returns> a new instance with the result updated </returns>
	  public CalculationResult withResult<T1>(Result<T1> underlyingResult)
	  {
		return new CalculationResult(rowIndex, columnIndex, underlyingResult);
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code CalculationResult}.
	  /// </summary>
	  private static readonly TypedMetaBean<CalculationResult> META_BEAN = LightMetaBean.of(typeof(CalculationResult), MethodHandles.lookup(), new string[] {"rowIndex", "columnIndex", "result"}, new object[0]);

	  /// <summary>
	  /// The meta-bean for {@code CalculationResult}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static TypedMetaBean<CalculationResult> meta()
	  {
		return META_BEAN;
	  }

	  static CalculationResult()
	  {
		MetaBean.register(META_BEAN);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  private CalculationResult<T1>(int rowIndex, int columnIndex, Result<T1> result)
	  {
		JodaBeanUtils.notNull(result, "result");
		this.rowIndex = rowIndex;
		this.columnIndex = columnIndex;
		this.result = result;
	  }

	  public override TypedMetaBean<CalculationResult> metaBean()
	  {
		return META_BEAN;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the row index of the value in the results grid. </summary>
	  /// <returns> the value of the property </returns>
	  public int RowIndex
	  {
		  get
		  {
			return rowIndex;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the column index of the value in the results grid. </summary>
	  /// <returns> the value of the property </returns>
	  public int ColumnIndex
	  {
		  get
		  {
			return columnIndex;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the result of the calculation.
	  /// <para>
	  /// The result may be a single value or a multi-scenario value.
	  /// A multi-scenario value will implement <seealso cref="ScenarioArray"/> unless it has been aggregated.
	  /// </para>
	  /// <para>
	  /// If the calculation did not complete successfully, a failure result will be returned
	  /// explaining the problem. Callers must check whether the result is a success or failure
	  /// before examining the result value.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: public com.opengamma.strata.collect.result.Result<?> getResult()
	  public Result<object> Result
	  {
		  get
		  {
			return result;
		  }
	  }

	  //-----------------------------------------------------------------------
	  public override bool Equals(object obj)
	  {
		if (obj == this)
		{
		  return true;
		}
		if (obj != null && obj.GetType() == this.GetType())
		{
		  CalculationResult other = (CalculationResult) obj;
		  return (rowIndex == other.rowIndex) && (columnIndex == other.columnIndex) && JodaBeanUtils.equal(result, other.result);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(rowIndex);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(columnIndex);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(result);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(128);
		buf.Append("CalculationResult{");
		buf.Append("rowIndex").Append('=').Append(rowIndex).Append(',').Append(' ');
		buf.Append("columnIndex").Append('=').Append(columnIndex).Append(',').Append(' ');
		buf.Append("result").Append('=').Append(JodaBeanUtils.ToString(result));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}