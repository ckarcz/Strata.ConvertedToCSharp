using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.amount
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.Guavate.toImmutableList;


	using Bean = org.joda.beans.Bean;
	using BeanBuilder = org.joda.beans.BeanBuilder;
	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using MetaProperty = org.joda.beans.MetaProperty;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;
	using DirectPrivateBeanBuilder = org.joda.beans.impl.direct.DirectPrivateBeanBuilder;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using Ordering = com.google.common.collect.Ordering;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using FxConvertible = com.opengamma.strata.basics.currency.FxConvertible;
	using FxRateProvider = com.opengamma.strata.basics.currency.FxRateProvider;

	/// <summary>
	/// A collection of cash flows.
	/// <para>
	/// Contains a list of <seealso cref="CashFlow cash flow"/> objects, each referring to a single cash flow.
	/// The list is can be <seealso cref="#sorted() sorted"/> by date and value if desired.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private") public final class CashFlows implements com.opengamma.strata.basics.currency.FxConvertible<CashFlows>, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class CashFlows : FxConvertible<CashFlows>, ImmutableBean
	{

	  /// <summary>
	  /// A cash flows instance to be used when there is no cash flow.
	  /// </summary>
	  public static readonly CashFlows NONE = new CashFlows(ImmutableList.of());

	  /// <summary>
	  /// The cash flows.
	  /// <para>
	  /// Each entry includes details of a single cash flow.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.google.common.collect.ImmutableList<CashFlow> cashFlows;
	  private readonly ImmutableList<CashFlow> cashFlows;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance from a single cash flow.
	  /// </summary>
	  /// <param name="cashFlow"> The cash flow </param>
	  /// <returns> the cash flows instance </returns>
	  public static CashFlows of(CashFlow cashFlow)
	  {
		return CashFlows.of(ImmutableList.of(cashFlow));
	  }

	  /// <summary>
	  /// Obtains an instance from a list of cash flows.
	  /// </summary>
	  /// <param name="cashFlows"> the list of cash flows </param>
	  /// <returns> the cash flows instance </returns>
	  public static CashFlows of(IList<CashFlow> cashFlows)
	  {
		return new CashFlows(cashFlows);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the cash flow by index.
	  /// </summary>
	  /// <param name="index">  the index to get </param>
	  /// <returns> the cash flow </returns>
	  public CashFlow getCashFlow(int index)
	  {
		return cashFlows.get(index);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Combines this cash flows instance with another cash flow.
	  /// <para>
	  /// This returns a new cash flows instance with a combined list of cash flow instances.
	  /// This instance is immutable and unaffected by this method.
	  /// The result may contain duplicate cash flows.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="cashFlow">  the additional single cash flow </param>
	  /// <returns> the new instance of {@code CashFlows} based on this instance, with the additional single cash flow added </returns>
	  public CashFlows combinedWith(CashFlow cashFlow)
	  {
		return new CashFlows(ImmutableList.builder<CashFlow>().addAll(cashFlows).add(cashFlow).build());
	  }

	  /// <summary>
	  /// Combines this cash flows instance with another one.
	  /// <para>
	  /// This returns a new cash flows instance with a combined list of cash flow instances.
	  /// This instance is immutable and unaffected by this method.
	  /// The result may contain duplicate cash flows.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="other">  the other cash flows </param>
	  /// <returns> the new instance of {@code CashFlows} based on this instance, with the other instance added </returns>
	  public CashFlows combinedWith(CashFlows other)
	  {
		return new CashFlows(ImmutableList.builder<CashFlow>().addAll(cashFlows).addAll(other.cashFlows).build());
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns an instance that is sorted.
	  /// <para>
	  /// The sort is by date, then value.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the sorted instance </returns>
	  public CashFlows sorted()
	  {
		if (Ordering.natural().isOrdered(cashFlows))
		{
		  return this;
		}
		else
		{
		  return new CashFlows(Ordering.natural().immutableSortedCopy(cashFlows));
		}
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Converts this collection of cash flows to an equivalent amount in the specified currency.
	  /// <para>
	  /// This ensures that the result will have all currency amounts expressed in terms of the given currency.
	  /// If conversion is needed, the provider will be used to supply the FX rate.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="resultCurrency">  the currency of the result </param>
	  /// <param name="rateProvider">  the provider of FX rates </param>
	  /// <returns> the converted instance, in the specified currency </returns>
	  /// <exception cref="RuntimeException"> if no FX rate could be found </exception>
	  public CashFlows convertedTo(Currency resultCurrency, FxRateProvider rateProvider)
	  {
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		return CashFlows.of(cashFlows.Select(cf => cf.convertedTo(resultCurrency, rateProvider)).collect(toImmutableList()));
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code CashFlows}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static CashFlows.Meta meta()
	  {
		return CashFlows.Meta.INSTANCE;
	  }

	  static CashFlows()
	  {
		MetaBean.register(CashFlows.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  private CashFlows(IList<CashFlow> cashFlows)
	  {
		JodaBeanUtils.notNull(cashFlows, "cashFlows");
		this.cashFlows = ImmutableList.copyOf(cashFlows);
	  }

	  public override CashFlows.Meta metaBean()
	  {
		return CashFlows.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the cash flows.
	  /// <para>
	  /// Each entry includes details of a single cash flow.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ImmutableList<CashFlow> getCashFlows()
	  {
		return cashFlows;
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
		  CashFlows other = (CashFlows) obj;
		  return JodaBeanUtils.equal(cashFlows, other.cashFlows);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(cashFlows);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(64);
		buf.Append("CashFlows{");
		buf.Append("cashFlows").Append('=').Append(JodaBeanUtils.ToString(cashFlows));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code CashFlows}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  cashFlows_Renamed = DirectMetaProperty.ofImmutable(this, "cashFlows", typeof(CashFlows), (Type) typeof(ImmutableList));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "cashFlows");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code cashFlows} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<com.google.common.collect.ImmutableList<CashFlow>> cashFlows = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "cashFlows", CashFlows.class, (Class) com.google.common.collect.ImmutableList.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ImmutableList<CashFlow>> cashFlows_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "cashFlows");
		internal IDictionary<string, MetaProperty<object>> metaPropertyMap$;

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Meta()
		{
			if (!InstanceFieldsInitialized)
			{
				InitializeInstanceFields();
				InstanceFieldsInitialized = true;
			}
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override protected org.joda.beans.MetaProperty<?> metaPropertyGet(String propertyName)
		protected internal override MetaProperty<object> metaPropertyGet(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 733659538: // cashFlows
			  return cashFlows_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends CashFlows> builder()
		public override BeanBuilder<CashFlows> builder()
		{
		  return new CashFlows.Builder();
		}

		public override Type beanType()
		{
		  return typeof(CashFlows);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code cashFlows} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ImmutableList<CashFlow>> cashFlows()
		{
		  return cashFlows_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 733659538: // cashFlows
			  return ((CashFlows) bean).getCashFlows();
		  }
		  return base.propertyGet(bean, propertyName, quiet);
		}

		protected internal override void propertySet(Bean bean, string propertyName, object newValue, bool quiet)
		{
		  metaProperty(propertyName);
		  if (quiet)
		  {
			return;
		  }
		  throw new System.NotSupportedException("Property cannot be written: " + propertyName);
		}

	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The bean-builder for {@code CashFlows}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<CashFlows>
	  {

		internal IList<CashFlow> cashFlows = ImmutableList.of();

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Builder()
		{
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 733659538: // cashFlows
			  return cashFlows;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") @Override public Builder set(String propertyName, Object newValue)
		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 733659538: // cashFlows
			  this.cashFlows = (IList<CashFlow>) newValue;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override CashFlows build()
		{
		  return new CashFlows(cashFlows);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(64);
		  buf.Append("CashFlows.Builder{");
		  buf.Append("cashFlows").Append('=').Append(JodaBeanUtils.ToString(cashFlows));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}