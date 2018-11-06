using System;
using System.Text;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.impl.volatility.smile
{

	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using TypedMetaBean = org.joda.beans.TypedMetaBean;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using ImmutableValidator = org.joda.beans.gen.ImmutableValidator;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using LightMetaBean = org.joda.beans.impl.light.LightMetaBean;

	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;

	/// <summary>
	/// The data bundle for SSVI smile formula.
	/// <para>
	/// The bundle contains the SSVI model parameters, ATM volatility, rho and eta.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(style = "light") public final class SsviFormulaData implements SmileModelData, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class SsviFormulaData : SmileModelData, ImmutableBean
	{

	  /// <summary>
	  /// The number of model parameters.
	  /// </summary>
	  private const int NUM_PARAMETERS = 3;

	  /// <summary>
	  /// The model parameters.
	  /// <para>
	  /// This must be an array of length 3.
	  /// The parameters in the array are in the order of sigma (ATM volatility), rho and eta.
	  /// The constraints for the parameters are defined in <seealso cref="#isAllowed(int, double)"/>.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.collect.array.DoubleArray parameters;
	  private readonly DoubleArray parameters;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance of the SSVI formula data.
	  /// </summary>
	  /// <param name="sigma">  the sigma parameter, ATM volatility </param>
	  /// <param name="rho">  the rho parameter </param>
	  /// <param name="eta">  the eta parameter </param>
	  /// <returns> the instance </returns>
	  public static SsviFormulaData of(double sigma, double rho, double eta)
	  {
		return new SsviFormulaData(DoubleArray.of(sigma, rho, eta));
	  }

	  /// <summary>
	  /// Obtains an instance of the SSVI formula data.
	  /// <para>
	  /// The parameters in the input array should be in the order of sigma (ATM volatility), rho and eta.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="parameters">  the parameters </param>
	  /// <returns> the instance </returns>
	  public static SsviFormulaData of(double[] parameters)
	  {
		ArgChecker.notNull(parameters, "parameters");
		ArgChecker.isTrue(parameters.Length == NUM_PARAMETERS, "the number of parameters should be 3");
		return new SsviFormulaData(DoubleArray.copyOf(parameters));
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableValidator private void validate()
	  private void validate()
	  {
		for (int i = 0; i < NUM_PARAMETERS; ++i)
		{
		  ArgChecker.isTrue(isAllowed(i, parameters.get(i)), "the {}-th parameter is not allowed", i);
		}
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the sigma parameter.
	  /// </summary>
	  /// <returns> the sigma parameter </returns>
	  public double Sigma
	  {
		  get
		  {
			return parameters.get(0);
		  }
	  }

	  /// <summary>
	  /// Gets the rho parameter.
	  /// </summary>
	  /// <returns> the rho parameter </returns>
	  public double Rho
	  {
		  get
		  {
			return parameters.get(1);
		  }
	  }

	  /// <summary>
	  /// Gets the eta parameters.
	  /// </summary>
	  /// <returns> the eta parameter </returns>
	  public double Eta
	  {
		  get
		  {
			return parameters.get(2);
		  }
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns a copy of this instance with sigma replaced.
	  /// </summary>
	  /// <param name="sigma">  the new sigma </param>
	  /// <returns> the new data instance </returns>
	  public SsviFormulaData withSigma(double sigma)
	  {
		return of(sigma, Rho, Eta);
	  }

	  /// <summary>
	  /// Returns a copy of this instance with rho replaced.
	  /// </summary>
	  /// <param name="rho">  the new rho </param>
	  /// <returns> the new data instance </returns>
	  public SsviFormulaData withRho(double rho)
	  {
		return of(Sigma, rho, Eta);
	  }

	  /// <summary>
	  /// Returns a copy of this instance with eta replaced.
	  /// </summary>
	  /// <param name="eta">  the new eta </param>
	  /// <returns> the new data instance </returns>
	  public SsviFormulaData withEta(double eta)
	  {
		return of(Sigma, Rho, eta);
	  }

	  //-------------------------------------------------------------------------
	  public int NumberOfParameters
	  {
		  get
		  {
			return NUM_PARAMETERS;
		  }
	  }

	  public double getParameter(int index)
	  {
		ArgChecker.inRangeExclusive(index, -1, NUM_PARAMETERS, "index");
		return parameters.get(index);
	  }

	  public bool isAllowed(int index, double value)
	  {
		switch (index)
		{
		  case 0:
			return value > 0;
		  case 1:
			return value >= -1 && value <= 1;
		  case 2:
			return value > 0;
		  default:
			throw new System.ArgumentException("index " + index + " outside range");
		}
	  }

	  public SsviFormulaData with(int index, double value)
	  {
		ArgChecker.inRange(index, 0, NUM_PARAMETERS, "index");
		double[] paramsCp = parameters.toArray();
		paramsCp[index] = value;
		return of(paramsCp);
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code SsviFormulaData}.
	  /// </summary>
	  private static readonly TypedMetaBean<SsviFormulaData> META_BEAN = LightMetaBean.of(typeof(SsviFormulaData), MethodHandles.lookup(), new string[] {"parameters"}, new object[0]);

	  /// <summary>
	  /// The meta-bean for {@code SsviFormulaData}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static TypedMetaBean<SsviFormulaData> meta()
	  {
		return META_BEAN;
	  }

	  static SsviFormulaData()
	  {
		MetaBean.register(META_BEAN);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  private SsviFormulaData(DoubleArray parameters)
	  {
		JodaBeanUtils.notNull(parameters, "parameters");
		this.parameters = parameters;
		validate();
	  }

	  public override TypedMetaBean<SsviFormulaData> metaBean()
	  {
		return META_BEAN;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the model parameters.
	  /// <para>
	  /// This must be an array of length 3.
	  /// The parameters in the array are in the order of sigma (ATM volatility), rho and eta.
	  /// The constraints for the parameters are defined in <seealso cref="#isAllowed(int, double)"/>.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public DoubleArray Parameters
	  {
		  get
		  {
			return parameters;
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
		  SsviFormulaData other = (SsviFormulaData) obj;
		  return JodaBeanUtils.equal(parameters, other.parameters);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(parameters);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(64);
		buf.Append("SsviFormulaData{");
		buf.Append("parameters").Append('=').Append(JodaBeanUtils.ToString(parameters));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}