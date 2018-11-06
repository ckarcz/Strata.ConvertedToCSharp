using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.sensitivity
{

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
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using FxConvertible = com.opengamma.strata.basics.currency.FxConvertible;
	using FxRateProvider = com.opengamma.strata.basics.currency.FxRateProvider;
	using Guavate = com.opengamma.strata.collect.Guavate;

	/// <summary>
	/// A collection of point sensitivities.
	/// <para>
	/// Contains a list of <seealso cref="PointSensitivity point sensitivity"/> objects,
	/// each referring to a specific point on a curve that was queried.
	/// The order of the list has no specific meaning, but does allow duplicates.
	/// </para>
	/// <para>
	/// For example, the point sensitivity for present value on a FRA might contain
	/// two entries, one for the Ibor forward curve and one for the discount curve.
	/// Each entry identifies the date that the curve was queried and the resulting multiplier.
	/// </para>
	/// <para>
	/// When creating an instance, consider using <seealso cref="MutablePointSensitivities"/>.
	/// </para>
	/// <para>
	/// One way of viewing this class is as a {@code Map} from a specific sensitivity
	/// key to a {@code double} sensitivity value. However, instead or being structured
	/// as a {@code Map}, the data is structured as a {@code List}, with the key and
	/// value in each entry.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private") public final class PointSensitivities implements com.opengamma.strata.basics.currency.FxConvertible<PointSensitivities>, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class PointSensitivities : FxConvertible<PointSensitivities>, ImmutableBean
	{

	  /// <summary>
	  /// An empty instance.
	  /// </summary>
	  private static readonly PointSensitivities EMPTY = new PointSensitivities(ImmutableList.of());

	  /// <summary>
	  /// The point sensitivities.
	  /// <para>
	  /// Each entry includes details of the market data query it relates to.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.google.common.collect.ImmutableList<PointSensitivity> sensitivities;
	  private readonly ImmutableList<PointSensitivity> sensitivities;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// An empty sensitivity instance.
	  /// </summary>
	  /// <returns> the empty instance </returns>
	  public static PointSensitivities empty()
	  {
		return EMPTY;
	  }

	  /// <summary>
	  /// Obtains an instance from an array of sensitivity entries.
	  /// </summary>
	  /// <param name="sensitivity">  the sensitivity entry </param>
	  /// <returns> the sensitivities instance </returns>
	  public static PointSensitivities of(params PointSensitivity[] sensitivity)
	  {
		return PointSensitivities.of(ImmutableList.copyOf(sensitivity));
	  }

	  /// <summary>
	  /// Obtains an instance from a list of sensitivity entries.
	  /// </summary>
	  /// <param name="sensitivities">  the list of sensitivity entries </param>
	  /// <returns> the sensitivities instance </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") public static PointSensitivities of(java.util.List<? extends PointSensitivity> sensitivities)
	  public static PointSensitivities of<T1>(IList<T1> sensitivities)
	  {
		return new PointSensitivities((IList<PointSensitivity>) sensitivities);
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the number of sensitivity entries.
	  /// </summary>
	  /// <returns> the size of the internal list of point sensitivities </returns>
	  public int size()
	  {
		return sensitivities.size();
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Combines this point sensitivities with another instance.
	  /// <para>
	  /// This returns a new sensitivity instance with a combined list of point sensitivities.
	  /// This instance is immutable and unaffected by this method.
	  /// The result may contain duplicate point sensitivities.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="other">  the other point sensitivities </param>
	  /// <returns> a {@code PointSensitivities} based on this one, with the other instance added </returns>
	  public PointSensitivities combinedWith(PointSensitivities other)
	  {
		return new PointSensitivities(ImmutableList.builder<PointSensitivity>().addAll(sensitivities).addAll(other.sensitivities).build());
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Multiplies the sensitivities in this instance by the specified factor.
	  /// <para>
	  /// The result will consist of the same entries, but with each sensitivity value multiplied.
	  /// This instance is immutable and unaffected by this method.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="factor">  the multiplicative factor </param>
	  /// <returns> a {@code PointSensitivities} based on this one, with each sensitivity multiplied by the factor </returns>
	  public PointSensitivities multipliedBy(double factor)
	  {
		return mapSensitivities(s => s * factor);
	  }

	  /// <summary>
	  /// Applies an operation to the sensitivities in this instance.
	  /// <para>
	  /// The result will consist of the same entries, but with the operator applied to each sensitivity value.
	  /// This instance is immutable and unaffected by this method.
	  /// </para>
	  /// <para>
	  /// This is used to apply a mathematical operation to the sensitivity values.
	  /// For example, the operator could multiply the sensitivities by a constant, or take the inverse.
	  /// <pre>
	  ///   inverse = base.mapSensitivities(value -> 1 / value);
	  /// </pre>
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="operator">  the operator to be applied to the sensitivities </param>
	  /// <returns> a {@code PointSensitivities} based on this one, with the operator applied to the sensitivity values </returns>
	  public PointSensitivities mapSensitivities(System.Func<double, double> @operator)
	  {
//JAVA TO C# CONVERTER TODO TASK: Method reference constructor syntax is not converted by Java to C# Converter:
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		return sensitivities.Select(s => s.withSensitivity(@operator(s.Sensitivity))).collect(Collectors.collectingAndThen(Guavate.toImmutableList(), PointSensitivities::new));
	  }

	  /// <summary>
	  /// Normalizes the point sensitivities by sorting and merging.
	  /// <para>
	  /// The list of sensitivities is sorted and then merged.
	  /// Any two entries that represent the same curve query are merged.
	  /// For example, if there are two point sensitivities that were created based on the same curve,
	  /// currency and fixing date, then the entries are combined, summing the sensitivity value.
	  /// </para>
	  /// <para>
	  /// The intention is that normalization occurs after gathering all the point sensitivities.
	  /// </para>
	  /// <para>
	  /// This instance is immutable and unaffected by this method.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> a {@code PointSensitivities} based on this one, with the sensitivities normalized </returns>
	  public PointSensitivities normalized()
	  {
		if (sensitivities.Empty)
		{
		  return this;
		}
		IList<PointSensitivity> mutable = new List<PointSensitivity>();
		foreach (PointSensitivity sensi in sensitivities)
		{
		  insert(mutable, sensi);
		}
		return new PointSensitivities(mutable);
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Returns a mutable version of this object.
	  /// <para>
	  /// The result is an instance of the mutable <seealso cref="MutablePointSensitivities"/>.
	  /// It will contain the same individual sensitivity entries.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the mutable sensitivity instance, not null </returns>
	  public MutablePointSensitivities toMutable()
	  {
		return new MutablePointSensitivities(sensitivities);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Checks if this sensitivity equals another within the specified tolerance.
	  /// <para>
	  /// This returns true if the two instances have the list of {@code PointSensitivity},
	  /// where the sensitivity {@code double} values are compared within the specified tolerance.
	  /// It is expected that this comparator will be used on the normalized version of the sensitivity.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="other">  the other sensitivity </param>
	  /// <param name="tolerance">  the tolerance </param>
	  /// <returns> true if equal up to the tolerance </returns>
	  public bool equalWithTolerance(PointSensitivities other, double tolerance)
	  {
		ImmutableList<PointSensitivity> list1 = this.Sensitivities;
		ImmutableList<PointSensitivity> list2 = other.Sensitivities;
		int nbList1 = list1.size();
		int nbList2 = list2.size();
		if (nbList1 != nbList2)
		{
		  return false;
		}
		for (int i1 = 0; i1 < nbList1; i1++)
		{
		  if (list1.get(i1).compareKey(list2.get(i1)) == 0)
		  {
			if (Math.Abs(list1.get(i1).Sensitivity - list2.get(i1).Sensitivity) > tolerance)
			{
			  return false;
			}
		  }
		  else
		  {
			return false;
		  }
		}
		return true;
	  }

	  //-------------------------------------------------------------------------
	  public PointSensitivities convertedTo(Currency resultCurrency, FxRateProvider rateProvider)
	  {
		IList<PointSensitivity> mutable = new List<PointSensitivity>();
		foreach (PointSensitivity sensi in sensitivities)
		{
		  insert(mutable, sensi.convertedTo(resultCurrency, rateProvider));
		}
		return new PointSensitivities(mutable);
	  }

	  // inserts a sensitivity into the mutable list in the right location
	  // merges the entry with an existing entry if the key matches
	  private static void insert(IList<PointSensitivity> mutable, PointSensitivity addition)
	  {
//JAVA TO C# CONVERTER TODO TASK: Method reference arbitrary object instance method syntax is not converted by Java to C# Converter:
		int index = Collections.binarySearch(mutable, addition, PointSensitivity::compareKey);
		if (index >= 0)
		{
		  PointSensitivity @base = mutable[index];
		  double combined = @base.Sensitivity + addition.Sensitivity;
		  mutable[index] = @base.withSensitivity(combined);
		}
		else
		{
		  int insertionPoint = -(index + 1);
		  mutable.Insert(insertionPoint, addition);
		}
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code PointSensitivities}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static PointSensitivities.Meta meta()
	  {
		return PointSensitivities.Meta.INSTANCE;
	  }

	  static PointSensitivities()
	  {
		MetaBean.register(PointSensitivities.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  private PointSensitivities(IList<PointSensitivity> sensitivities)
	  {
		JodaBeanUtils.notNull(sensitivities, "sensitivities");
		this.sensitivities = ImmutableList.copyOf(sensitivities);
	  }

	  public override PointSensitivities.Meta metaBean()
	  {
		return PointSensitivities.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the point sensitivities.
	  /// <para>
	  /// Each entry includes details of the market data query it relates to.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ImmutableList<PointSensitivity> Sensitivities
	  {
		  get
		  {
			return sensitivities;
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
		  PointSensitivities other = (PointSensitivities) obj;
		  return JodaBeanUtils.equal(sensitivities, other.sensitivities);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(sensitivities);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(64);
		buf.Append("PointSensitivities{");
		buf.Append("sensitivities").Append('=').Append(JodaBeanUtils.ToString(sensitivities));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code PointSensitivities}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  sensitivities_Renamed = DirectMetaProperty.ofImmutable(this, "sensitivities", typeof(PointSensitivities), (Type) typeof(ImmutableList));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "sensitivities");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code sensitivities} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<com.google.common.collect.ImmutableList<PointSensitivity>> sensitivities = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "sensitivities", PointSensitivities.class, (Class) com.google.common.collect.ImmutableList.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ImmutableList<PointSensitivity>> sensitivities_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "sensitivities");
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
			case 1226228605: // sensitivities
			  return sensitivities_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends PointSensitivities> builder()
		public override BeanBuilder<PointSensitivities> builder()
		{
		  return new PointSensitivities.Builder();
		}

		public override Type beanType()
		{
		  return typeof(PointSensitivities);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code sensitivities} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ImmutableList<PointSensitivity>> sensitivities()
		{
		  return sensitivities_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 1226228605: // sensitivities
			  return ((PointSensitivities) bean).Sensitivities;
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
	  /// The bean-builder for {@code PointSensitivities}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<PointSensitivities>
	  {

		internal IList<PointSensitivity> sensitivities = ImmutableList.of();

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
			case 1226228605: // sensitivities
			  return sensitivities;
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
			case 1226228605: // sensitivities
			  this.sensitivities = (IList<PointSensitivity>) newValue;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override PointSensitivities build()
		{
		  return new PointSensitivities(sensitivities);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(64);
		  buf.Append("PointSensitivities.Builder{");
		  buf.Append("sensitivities").Append('=').Append(JodaBeanUtils.ToString(sensitivities));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}