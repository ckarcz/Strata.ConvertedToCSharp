using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.value
{

	using Bean = org.joda.beans.Bean;
	using BeanBuilder = org.joda.beans.BeanBuilder;
	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using MetaProperty = org.joda.beans.MetaProperty;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using ImmutableConstructor = org.joda.beans.gen.ImmutableConstructor;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;
	using DirectPrivateBeanBuilder = org.joda.beans.impl.direct.DirectPrivateBeanBuilder;

	using ArgChecker = com.opengamma.strata.collect.ArgChecker;

	/// <summary>
	/// Standard implementation of {@code Rounding} that uses the half-up convention.
	/// <para>
	/// This class implements <seealso cref="Rounding"/> to provide the ability to round a number.
	/// Rounding follows the normal <seealso cref="RoundingMode#HALF_UP"/> convention.
	/// For example, this could be used to round a price to the appropriate market convention.
	/// </para>
	/// <para>
	/// Note that rounding a {@code double} is not straightforward as floating point
	/// numbers are based on a binary representation, not a decimal one.
	/// For example, the value 0.1 cannot be exactly represented in a {@code double}.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private") final class HalfUpRounding implements Rounding, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	internal sealed class HalfUpRounding : Rounding, ImmutableBean
	{

	  /// <summary>
	  /// Cache common roundings.
	  /// Roundings will be commonly used in trades, which are relatively long-lived,
	  /// so some limited caching makes sense.
	  /// </summary>
	  private static readonly HalfUpRounding[] CACHE = new HalfUpRounding[16];
	  static HalfUpRounding()
	  {
		for (int i = 0; i < 16; i++)
		{
		  CACHE[i] = new HalfUpRounding(i, 0);
		}
		MetaBean.register(HalfUpRounding.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The number of decimal places to round to.
	  /// <para>
	  /// Rounding follows the normal <seealso cref="RoundingMode#HALF_UP"/> convention.
	  /// </para>
	  /// <para>
	  /// The value must be from 0 to 255 inclusive.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition private final int decimalPlaces;
	  private readonly int decimalPlaces;
	  /// <summary>
	  /// The fraction of the smallest decimal place to round to.
	  /// <para>
	  /// If used, this allows the rounding point to be set as a fraction of the smallest decimal place.
	  /// For example, setting this field to 32 will round to the nearest 1/32nd of the last decimal place.
	  /// </para>
	  /// <para>
	  /// This will not be present if rounding is to an exact number of decimal places and there is no fraction.
	  /// The value must be from 2 to 256 inclusive, 0 is used to indicate no fractional part.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition private final int fraction;
	  private readonly int fraction;
	  /// <summary>
	  /// The fraction, as a {@code BigDecimal}.
	  /// Not a Joda-Beans property.
	  /// </summary>
	  [NonSerialized]
	  private readonly decimal fractionDecimal;
	  /// <summary>
	  /// The hash code.
	  /// Uniquely identifies the state of the object.
	  /// Not a Joda-Beans property.
	  /// </summary>
	  [NonSerialized]
	  private readonly int uniqueHashCode;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance that rounds to the specified number of decimal places.
	  /// <para>
	  /// This returns a convention that rounds to the specified number of decimal places.
	  /// Rounding follows the normal <seealso cref="RoundingMode#HALF_UP"/> convention.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="decimalPlaces">  the number of decimal places to round to, from 0 to 255 inclusive </param>
	  /// <returns> the rounding convention </returns>
	  /// <exception cref="IllegalArgumentException"> if the decimal places is invalid </exception>
	  public static HalfUpRounding ofDecimalPlaces(int decimalPlaces)
	  {
		if (decimalPlaces >= 0 && decimalPlaces < 16)
		{
		  return CACHE[decimalPlaces];
		}
		return new HalfUpRounding(decimalPlaces, 1);
	  }

	  /// <summary>
	  /// Obtains an instance from the number of decimal places and fraction.
	  /// <para>
	  /// This returns a convention that rounds to a fraction of the specified number of decimal places.
	  /// Rounding follows the normal <seealso cref="RoundingMode#HALF_UP"/> convention.
	  /// </para>
	  /// <para>
	  /// For example, to round to the nearest 1/32nd of the 4th decimal place, call
	  /// this method with the arguments 4 and 32.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="decimalPlaces">  the number of decimal places to round to, from 0 to 255 inclusive </param>
	  /// <param name="fraction">  the fraction of the last decimal place, such as 32 for 1/32, from 0 to 256 inclusive </param>
	  /// <returns> the rounding convention </returns>
	  /// <exception cref="IllegalArgumentException"> if the decimal places or fraction is invalid </exception>
	  public static HalfUpRounding ofFractionalDecimalPlaces(int decimalPlaces, int fraction)
	  {
		return new HalfUpRounding(decimalPlaces, fraction);
	  }

	  //-------------------------------------------------------------------------
	  // constructor
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableConstructor private HalfUpRounding(int decimalPlaces, int fraction)
	  private HalfUpRounding(int decimalPlaces, int fraction)
	  {

		if (decimalPlaces < 0 || decimalPlaces > 255)
		{
		  throw new System.ArgumentException("Invalid decimal places, must be from 0 to 255 inclusive");
		}
		if (fraction < 0 || fraction > 256)
		{
		  throw new System.ArgumentException("Invalid fraction, must be from 0 to 256 inclusive");
		}
		this.decimalPlaces = ArgChecker.notNegative(decimalPlaces, "decimalPlaces");
		this.fraction = (fraction <= 1 ? 0 : fraction);
		this.fractionDecimal = (fraction <= 1 ? null : decimal.valueOf(this.fraction));
		this.uniqueHashCode = (this.decimalPlaces << 16) + this.fraction;
	  }

	  // deserialize transient
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: private Object readResolve() throws java.io.ObjectStreamException
	  private object readResolve()
	  {
		return new HalfUpRounding(decimalPlaces, fraction);
	  }

	  //-------------------------------------------------------------------------
	  public double round(double value)
	  {
		return Rounding.this.round(value);
	  }

	  public decimal round(decimal value)
	  {
		if (fraction > 1)
		{
		  return value * fractionDecimal.setScale(decimalPlaces, RoundingMode.HALF_UP).divide(fractionDecimal);
		}
		return value.setScale(decimalPlaces, RoundingMode.HALF_UP);
	  }

	  //-----------------------------------------------------------------------
	  public override bool Equals(object obj)
	  {
		if (obj == this)
		{
		  return true;
		}
		if (obj is HalfUpRounding)
		{
		  // hash code is unique so can be used to compare
		  return (uniqueHashCode == ((HalfUpRounding) obj).uniqueHashCode);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		return uniqueHashCode;
	  }

	  //-------------------------------------------------------------------------
	  public override string ToString()
	  {
		return "Round to " + (fraction > 1 ? "1/" + fraction + " of " : "") + decimalPlaces + "dp";
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code HalfUpRounding}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static HalfUpRounding.Meta meta()
	  {
		return HalfUpRounding.Meta.INSTANCE;
	  }


	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  public override HalfUpRounding.Meta metaBean()
	  {
		return HalfUpRounding.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the number of decimal places to round to.
	  /// <para>
	  /// Rounding follows the normal <seealso cref="RoundingMode#HALF_UP"/> convention.
	  /// </para>
	  /// <para>
	  /// The value must be from 0 to 255 inclusive.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property </returns>
	  public int DecimalPlaces
	  {
		  get
		  {
			return decimalPlaces;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the fraction of the smallest decimal place to round to.
	  /// <para>
	  /// If used, this allows the rounding point to be set as a fraction of the smallest decimal place.
	  /// For example, setting this field to 32 will round to the nearest 1/32nd of the last decimal place.
	  /// </para>
	  /// <para>
	  /// This will not be present if rounding is to an exact number of decimal places and there is no fraction.
	  /// The value must be from 2 to 256 inclusive, 0 is used to indicate no fractional part.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property </returns>
	  public int Fraction
	  {
		  get
		  {
			return fraction;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code HalfUpRounding}.
	  /// </summary>
	  internal sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  decimalPlaces_Renamed = DirectMetaProperty.ofImmutable(this, "decimalPlaces", typeof(HalfUpRounding), Integer.TYPE);
			  fraction_Renamed = DirectMetaProperty.ofImmutable(this, "fraction", typeof(HalfUpRounding), Integer.TYPE);
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "decimalPlaces", "fraction");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code decimalPlaces} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<int> decimalPlaces_Renamed;
		/// <summary>
		/// The meta-property for the {@code fraction} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<int> fraction_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "decimalPlaces", "fraction");
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
			case 1477363453: // decimalPlaces
			  return decimalPlaces_Renamed;
			case -1653751294: // fraction
			  return fraction_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends HalfUpRounding> builder()
		public override BeanBuilder<HalfUpRounding> builder()
		{
		  return new HalfUpRounding.Builder();
		}

		public override Type beanType()
		{
		  return typeof(HalfUpRounding);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code decimalPlaces} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<int> decimalPlaces()
		{
		  return decimalPlaces_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code fraction} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<int> fraction()
		{
		  return fraction_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 1477363453: // decimalPlaces
			  return ((HalfUpRounding) bean).DecimalPlaces;
			case -1653751294: // fraction
			  return ((HalfUpRounding) bean).Fraction;
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
	  /// The bean-builder for {@code HalfUpRounding}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<HalfUpRounding>
	  {

		internal int decimalPlaces;
		internal int fraction;

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
			case 1477363453: // decimalPlaces
			  return decimalPlaces;
			case -1653751294: // fraction
			  return fraction;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 1477363453: // decimalPlaces
			  this.decimalPlaces = (int?) newValue.Value;
			  break;
			case -1653751294: // fraction
			  this.fraction = (int?) newValue.Value;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override HalfUpRounding build()
		{
		  return new HalfUpRounding(decimalPlaces, fraction);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(96);
		  buf.Append("HalfUpRounding.Builder{");
		  buf.Append("decimalPlaces").Append('=').Append(JodaBeanUtils.ToString(decimalPlaces)).Append(',').Append(' ');
		  buf.Append("fraction").Append('=').Append(JodaBeanUtils.ToString(fraction));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}