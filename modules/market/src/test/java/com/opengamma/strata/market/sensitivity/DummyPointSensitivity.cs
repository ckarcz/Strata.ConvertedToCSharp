using System;
using System.Text;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.sensitivity
{

	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using TypedMetaBean = org.joda.beans.TypedMetaBean;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using LightMetaBean = org.joda.beans.impl.light.LightMetaBean;

	using ComparisonChain = com.google.common.collect.ComparisonChain;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using FxRateProvider = com.opengamma.strata.basics.currency.FxRateProvider;

	/// <summary>
	/// Dummy point sensitivity implementation.
	/// Based on zero-rate sensitivity.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(style = "light") public final class DummyPointSensitivity implements PointSensitivity, PointSensitivityBuilder, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class DummyPointSensitivity : PointSensitivity, PointSensitivityBuilder, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.currency.Currency curveCurrency;
		private readonly Currency curveCurrency;
	  /// <summary>
	  /// The date that was looked up on the curve.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final java.time.LocalDate date;
	  private readonly LocalDate date;
	  /// <summary>
	  /// The currency of the sensitivity.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final com.opengamma.strata.basics.currency.Currency currency;
	  private readonly Currency currency;
	  /// <summary>
	  /// The value of the sensitivity.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(overrideGet = true) private final double sensitivity;
	  private readonly double sensitivity;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance from the curve currency, date and value.
	  /// <para>
	  /// The currency representing the curve is used also for the sensitivity currency.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="currency">  the currency of the curve and sensitivity </param>
	  /// <param name="date">  the date that was looked up on the curve </param>
	  /// <param name="sensitivity">  the value of the sensitivity </param>
	  /// <returns> the point sensitivity object </returns>
	  public static DummyPointSensitivity of(Currency currency, LocalDate date, double sensitivity)
	  {
		return new DummyPointSensitivity(currency, date, currency, sensitivity);
	  }

	  /// <summary>
	  /// Obtains an instance from the curve currency, date, sensitivity currency and value.
	  /// <para>
	  /// The currency representing the curve is used also for the sensitivity currency.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="curveCurrency">  the currency of the curve </param>
	  /// <param name="date">  the date that was looked up on the curve </param>
	  /// <param name="sensitivityCurrency">  the currency of the sensitivity </param>
	  /// <param name="sensitivity">  the value of the sensitivity </param>
	  /// <returns> the point sensitivity object </returns>
	  public static DummyPointSensitivity of(Currency curveCurrency, LocalDate date, Currency sensitivityCurrency, double sensitivity)
	  {
		return new DummyPointSensitivity(curveCurrency, date, sensitivityCurrency, sensitivity);
	  }

	  //-------------------------------------------------------------------------
	  public DummyPointSensitivity withCurrency(Currency currency)
	  {
		if (this.currency.Equals(currency))
		{
		  return this;
		}
		return new DummyPointSensitivity(curveCurrency, date, currency, sensitivity);
	  }

	  public DummyPointSensitivity withSensitivity(double sensitivity)
	  {
		return new DummyPointSensitivity(curveCurrency, date, currency, sensitivity);
	  }

	  public int compareKey(PointSensitivity other)
	  {
		if (other is DummyPointSensitivity)
		{
		  DummyPointSensitivity otherZero = (DummyPointSensitivity) other;
		  return ComparisonChain.start().compare(curveCurrency, otherZero.curveCurrency).compare(currency, otherZero.currency).compare(date, otherZero.date).result();
		}
		return this.GetType().Name.CompareTo(other.GetType().Name);
	  }

	  public override DummyPointSensitivity convertedTo(Currency resultCurrency, FxRateProvider rateProvider)
	  {
		return (DummyPointSensitivity) PointSensitivity.this.convertedTo(resultCurrency, rateProvider);
	  }

	  //-------------------------------------------------------------------------
	  public override DummyPointSensitivity multipliedBy(double factor)
	  {
		return new DummyPointSensitivity(curveCurrency, date, currency, sensitivity * factor);
	  }

	  public DummyPointSensitivity mapSensitivity(System.Func<double, double> @operator)
	  {
		return new DummyPointSensitivity(curveCurrency, date, currency, @operator(sensitivity));
	  }

	  public DummyPointSensitivity normalize()
	  {
		return this;
	  }

	  public MutablePointSensitivities buildInto(MutablePointSensitivities combination)
	  {
		return combination.add(this);
	  }

	  public DummyPointSensitivity cloned()
	  {
		return this;
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code DummyPointSensitivity}.
	  /// </summary>
	  private static readonly TypedMetaBean<DummyPointSensitivity> META_BEAN = LightMetaBean.of(typeof(DummyPointSensitivity), MethodHandles.lookup(), new string[] {"curveCurrency", "date", "currency", "sensitivity"}, new object[0]);

	  /// <summary>
	  /// The meta-bean for {@code DummyPointSensitivity}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static TypedMetaBean<DummyPointSensitivity> meta()
	  {
		return META_BEAN;
	  }

	  static DummyPointSensitivity()
	  {
		MetaBean.register(META_BEAN);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  private DummyPointSensitivity(Currency curveCurrency, LocalDate date, Currency currency, double sensitivity)
	  {
		JodaBeanUtils.notNull(curveCurrency, "curveCurrency");
		JodaBeanUtils.notNull(date, "date");
		JodaBeanUtils.notNull(currency, "currency");
		this.curveCurrency = curveCurrency;
		this.date = date;
		this.currency = currency;
		this.sensitivity = sensitivity;
	  }

	  public override TypedMetaBean<DummyPointSensitivity> metaBean()
	  {
		return META_BEAN;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the currency of the curve for which the sensitivity is computed. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public Currency CurveCurrency
	  {
		  get
		  {
			return curveCurrency;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the date that was looked up on the curve. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public LocalDate Date
	  {
		  get
		  {
			return date;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the currency of the sensitivity. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public Currency Currency
	  {
		  get
		  {
			return currency;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the value of the sensitivity. </summary>
	  /// <returns> the value of the property </returns>
	  public double Sensitivity
	  {
		  get
		  {
			return sensitivity;
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
		  DummyPointSensitivity other = (DummyPointSensitivity) obj;
		  return JodaBeanUtils.equal(curveCurrency, other.curveCurrency) && JodaBeanUtils.equal(date, other.date) && JodaBeanUtils.equal(currency, other.currency) && JodaBeanUtils.equal(sensitivity, other.sensitivity);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(curveCurrency);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(date);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(currency);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(sensitivity);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(160);
		buf.Append("DummyPointSensitivity{");
		buf.Append("curveCurrency").Append('=').Append(curveCurrency).Append(',').Append(' ');
		buf.Append("date").Append('=').Append(date).Append(',').Append(' ');
		buf.Append("currency").Append('=').Append(currency).Append(',').Append(' ');
		buf.Append("sensitivity").Append('=').Append(JodaBeanUtils.ToString(sensitivity));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}