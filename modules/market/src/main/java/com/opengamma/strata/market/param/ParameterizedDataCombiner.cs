using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.param
{

	using Preconditions = com.google.common.@base.Preconditions;
	using ImmutableList = com.google.common.collect.ImmutableList;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;

	/// <summary>
	/// Helper that can be used to combine two or more underlying instances of {@code ParameterizedData}.
	/// <para>
	/// This is used by implementations of <seealso cref="ParameterizedData"/> that are based on more
	/// than one underlying {@code ParameterizedData} instance.
	/// </para>
	/// <para>
	/// This helper should be created in the constructor of the combined instance.
	/// In each of the five {@code ParameterizedData} methods of the combined instance,
	/// this helper should be invoked. See {@code DiscountFxForwardRates} for sample usage.
	/// </para>
	/// </summary>
	public sealed class ParameterizedDataCombiner
	{

	  /// <summary>
	  /// The underlying instances.
	  /// </summary>
	  private readonly ParameterizedData[] underlyings;
	  /// <summary>
	  /// The lookup array.
	  /// </summary>
	  private readonly int[] lookup;
	  /// <summary>
	  /// The count of parameters.
	  /// </summary>
	  private readonly int paramCount;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance that can combine the specified underlying instances.
	  /// </summary>
	  /// <param name="instances">  the underlying instances to combine </param>
	  /// <returns> the combiner </returns>
	  public static ParameterizedDataCombiner of(params ParameterizedData[] instances)
	  {
		return new ParameterizedDataCombiner(instances);
	  }

	  /// <summary>
	  /// Obtains an instance that can combine the specified underlying instances.
	  /// </summary>
	  /// <param name="instances">  the underlying instances to combine </param>
	  /// <returns> the combiner </returns>
	  public static ParameterizedDataCombiner of<T1>(IList<T1> instances) where T1 : ParameterizedData
	  {
		return new ParameterizedDataCombiner((ParameterizedData[]) instances.ToArray());
	  }

	  //------------------------------------------------------------------------- 
	  // creates an instance
	  private ParameterizedDataCombiner(ParameterizedData[] underlyings)
	  {
		ArgChecker.notEmpty(underlyings, "underlyings");
		int size = underlyings.Length;
		this.underlyings = underlyings;
		int[] lookup = new int[size];
		for (int i = 1; i < size; i++)
		{
		  lookup[i] = lookup[i - 1] + underlyings[i - 1].ParameterCount;
		}
		this.lookup = lookup;
		this.paramCount = lookup[size - 1] + underlyings[size - 1].ParameterCount;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the number of parameters.
	  /// <para>
	  /// This returns the total parameter count of all the instances.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the number of parameters </returns>
	  public int ParameterCount
	  {
		  get
		  {
			return paramCount;
		  }
	  }

	  /// <summary>
	  /// Gets the value of the parameter at the specified index.
	  /// <para>
	  /// This gets the parameter from the correct instance.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="parameterIndex">  the zero-based index of the parameter to get </param>
	  /// <returns> the value of the parameter </returns>
	  /// <exception cref="IndexOutOfBoundsException"> if the index is invalid </exception>
	  public double getParameter(int parameterIndex)
	  {
		int underlyingIndex = findUnderlyingIndex(parameterIndex);
		int adjustment = lookup[underlyingIndex];
		return underlyings[underlyingIndex].getParameter(parameterIndex - adjustment);
	  }

	  /// <summary>
	  /// Gets the metadata of the parameter at the specified index.
	  /// <para>
	  /// This gets the parameter metadata from the correct instance.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="parameterIndex">  the zero-based index of the parameter to get </param>
	  /// <returns> the metadata of the parameter </returns>
	  /// <exception cref="IndexOutOfBoundsException"> if the index is invalid </exception>
	  public ParameterMetadata getParameterMetadata(int parameterIndex)
	  {
		int underlyingIndex = findUnderlyingIndex(parameterIndex);
		int adjustment = lookup[underlyingIndex];
		return underlyings[underlyingIndex].getParameterMetadata(parameterIndex - adjustment);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Updates a parameter on the specified underlying.
	  /// <para>
	  /// This should be invoked once for each of the underlying instances.
	  /// It is intended to be used to pass the result of each invocation to the
	  /// constructor of the combined instance.
	  /// </para>
	  /// <para>
	  /// If the parameter index applies to the underlying, it is updated.
	  /// If the parameter index does not apply to the underlying, no error occurs.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <R>  the type of the underlying </param>
	  /// <param name="underlyingIndex">  the index of the underlying instance </param>
	  /// <param name="underlyingType">  the type of the parameterized data at the specified index </param>
	  /// <param name="parameterIndex">  the zero-based index of the parameter to change </param>
	  /// <param name="newValue">  the new value for the specified parameter </param>
	  /// <returns> a parameterized data instance based on this with the specified parameter altered </returns>
	  /// <exception cref="IndexOutOfBoundsException"> if the index is invalid </exception>
	  public R underlyingWithParameter<R>(int underlyingIndex, Type<R> underlyingType, int parameterIndex, double newValue) where R : ParameterizedData
	  {

		ParameterizedData perturbed = underlyings[underlyingIndex];
		if (findUnderlyingIndex(parameterIndex) == underlyingIndex)
		{
		  int adjustment = lookup[underlyingIndex];
		  perturbed = perturbed.withParameter(parameterIndex - adjustment, newValue);
		}
		return underlyingType.cast(perturbed);
	  }

	  /// <summary>
	  /// Applies a perturbation to the specified underlying.
	  /// <para>
	  /// This should be invoked once for each of the underlying instances.
	  /// It is intended to be used to pass the result of each invocation to the
	  /// constructor of the combined instance.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <R>  the type of the underlying </param>
	  /// <param name="underlyingIndex">  the index of the underlying instance </param>
	  /// <param name="underlyingType">  the type of the parameterized data at the specified index </param>
	  /// <param name="perturbation">  the perturbation to apply </param>
	  /// <returns> a parameterized data instance based on this with the specified perturbation applied </returns>
	  public R underlyingWithPerturbation<R>(int underlyingIndex, Type<R> underlyingType, ParameterPerturbation perturbation) where R : ParameterizedData
	  {

		ParameterizedData underlying = underlyings[underlyingIndex];
		// perturb using a derived perturbation that adjusts the index
		int adjustment = lookup[underlyingIndex];
		ParameterizedData perturbed = underlying.withPerturbation((idx, value, meta) => perturbation(idx + adjustment, value, meta));
		return underlyingType.cast(perturbed);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Updates a parameter on the specified list of underlying instances.
	  /// <para>
	  /// The correct underlying is identified and updated, with the list returned.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <R>  the type of the underlying </param>
	  /// <param name="underlyingType">  the type of the parameterized data at the specified index </param>
	  /// <param name="parameterIndex">  the zero-based index of the parameter to change </param>
	  /// <param name="newValue">  the new value for the specified parameter </param>
	  /// <returns> a parameterized data instance based on this with the specified parameter altered </returns>
	  /// <exception cref="IndexOutOfBoundsException"> if the index is invalid </exception>
	  public IList<R> withParameter<R>(Type<R> underlyingType, int parameterIndex, double newValue) where R : ParameterizedData
	  {

		int underlyingIndex = findUnderlyingIndex(parameterIndex);
		ImmutableList.Builder<R> builder = ImmutableList.builder();
		for (int i = 0; i < underlyings.Length; i++)
		{
		  ParameterizedData underlying = underlyings[i];
		  if (i == underlyingIndex)
		  {
			int adjustment = lookup[underlyingIndex];
			ParameterizedData perturbed = underlying.withParameter(parameterIndex - adjustment, newValue);
			builder.add(underlyingType.cast(perturbed));
		  }
		  else
		  {
			builder.add(underlyingType.cast(underlying));
		  }
		}
		return builder.build();
	  }

	  /// <summary>
	  /// Applies a perturbation to each underlying.
	  /// <para>
	  /// The updated list of underlying instances is returned.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <R>  the type of the underlying </param>
	  /// <param name="underlyingType">  the type of the parameterized data at the specified index </param>
	  /// <param name="perturbation">  the perturbation to apply </param>
	  /// <returns> a parameterized data instance based on this with the specified parameter altered </returns>
	  /// <exception cref="IndexOutOfBoundsException"> if the index is invalid </exception>
	  public IList<R> withPerturbation<R>(Type<R> underlyingType, ParameterPerturbation perturbation) where R : ParameterizedData
	  {

		ImmutableList.Builder<R> builder = ImmutableList.builder();
		for (int i = 0; i < underlyings.Length; i++)
		{
		  builder.add(underlyingWithPerturbation(i, underlyingType, perturbation));
		}
		return builder.build();
	  }

	  //-------------------------------------------------------------------------
	  // convert parameter index to underlying index
	  private int findUnderlyingIndex(int parameterIndex)
	  {
		Preconditions.checkElementIndex(parameterIndex, paramCount);
		for (int i = 1; i < lookup.Length; i++)
		{
		  if (parameterIndex < lookup[i])
		  {
			return i - 1;
		  }
		}
		return lookup.Length - 1;
	  }

	}

}